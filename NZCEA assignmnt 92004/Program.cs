using System;
using System.Collections.Generic;

internal class Program
{
    // GLOBAL VARIABLES
    static decimal overallCost = 0;

    public static List<string> deviceSlip = new List<string>();

    public static decimal costPerUnit;
    public static int numberOfUnits;
    public static decimal totalCost;

    // CONSTANTS (better practice than hardcoding values)
    public const decimal DISCOUNT_RATE = 0.10m;
    public const decimal DEPRECIATION_RATE = 0.05m;

    // device counters
    static int laptopCount = 0;
    static int desktopCount = 0;
    static int otherCount = 0;

    // most expensive tracking
    static string mostExpensiveDevice = "";
    static decimal highestCost = 0;

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Insurance calculator\n");

        char continueInput = 'y';

        // LOOP for multiple devices
        while (continueInput == 'y')
        {
            OneDevice();

            Console.WriteLine("\nAdd another device? (y/n)");
            continueInput = Console.ReadLine().ToLower()[0];
        }

        // OUTPUT DEVICE SLIP
        Console.WriteLine("\n--- Device Slip ---");
        foreach (string item in deviceSlip)
        {
            Console.WriteLine(item);
        }

        // FINAL SUMMARY OUTPUT
        Console.WriteLine($"\nNumber of laptops: {laptopCount}");
        Console.WriteLine($"Number of desktops: {desktopCount}");
        Console.WriteLine($"Number of other devices: {otherCount}");

        Console.WriteLine($"\nTotal insurance value: {overallCost:C}");

        Console.WriteLine($"Most expensive device: {mostExpensiveDevice} @ {highestCost:C}");
    }

    public static void OneDevice()
    {
        string deviceType;
        int category;
        string categoryName = "";

        // INPUT
        Console.Write("\nEnter device name: ");
        deviceType = Console.ReadLine();

        Console.Write("Enter cost per unit: ");
        costPerUnit = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Enter number of units: ");
        numberOfUnits = Convert.ToInt32(Console.ReadLine());

        // VALIDATE CATEGORY INPUT
        while (true)
        {
            Console.Write("Category (1=Laptop, 2=Desktop, 3=Other): ");

            if (int.TryParse(Console.ReadLine(), out category) && category >= 1 && category <= 3)
                break;

            Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
        }

        // CATEGORY LOGIC + COUNTING
        if (category == 1)
        {
            categoryName = "Laptop";
            laptopCount += numberOfUnits;
        }
        else if (category == 2)
        {
            categoryName = "Desktop";
            desktopCount += numberOfUnits;
        }
        else
        {
            categoryName = "Other";
            otherCount += numberOfUnits;
        }

        // CALCULATE TOTAL COST BEFORE DISCOUNT
        totalCost = costPerUnit * numberOfUnits;

        // CALCULATE DISCOUNTED COST
        decimal finalCost = DiscountCost();

        // TRACK MOST EXPENSIVE DEVICE (based on total cost)
        if (finalCost > highestCost)
        {
            highestCost = finalCost;
            mostExpensiveDevice = deviceType;
        }

        // DEPRECIATION (5% LOSS OVER 6 MONTHS)
        decimal value = costPerUnit;

        string depreciationTable = "\nMonth\tValue";

        for (int i = 1; i <= 6; i++)
        {
            value = value - (value * DEPRECIATION_RATE);
            value = Math.Round(value, 2);

            depreciationTable += $"\n{i}\t{value:C}";
        }

        // ADD TO SLIP
        deviceSlip.Add(
            $"{deviceType}\nTotal cost for {numberOfUnits} x {deviceType} = {finalCost:C}" +
            $"\n{depreciationTable}\nCATEGORY: {categoryName}\n"
        );

        // ADD TO TOTAL
        overallCost += finalCost;
    }

    // DISCOUNT FUNCTION
    static decimal DiscountCost()
    {
        decimal cost = 0;

        if (numberOfUnits > 5)
        {
            // first 5 full price
            decimal fullPrice = 5 * costPerUnit;

            // remaining with discount
            int remaining = numberOfUnits - 5;
            decimal discounted = remaining * costPerUnit * (1 - DISCOUNT_RATE);

            cost = fullPrice + discounted;
        }
        else
        {
            cost = totalCost;
        }

        return cost;
    }
}