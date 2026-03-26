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

    // CONSTANTS
    public const decimal DISCOUNT_RATE = 0.10m;
    public const decimal DEPRECIATION_RATE = 0.05m;

    // CATEGORY COUNTS
    static int laptopCount = 0;
    static int desktopCount = 0;
    static int otherCount = 0;

    // MOST EXPENSIVE TRACKING
    static string mostExpensiveDevice = "";
    static decimal highestCost = 0;

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Insurance calculator\n");

        char continueInput = 'y';

        while (continueInput == 'y')
        {
            OneDevice();

            // SAFE CONTINUE INPUT (prevents crash)
            string input;
            do
            {
                Console.Write("\nAdd another device? (y/n): ");
                input = Console.ReadLine().ToLower();

            } while (input != "y" && input != "n");

            continueInput = input[0];
        }

        // OUTPUT RESULTS
        Console.WriteLine("\n--- Device Slip ---");

        foreach (string item in deviceSlip)
        {
            Console.WriteLine(item);
        }

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

        // DEVICE NAME (invalid handled)
        do
        {
            Console.Write("\nEnter device name: ");
            deviceType = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(deviceType))
                Console.WriteLine("Invalid input. Device name cannot be empty.");

        } while (string.IsNullOrWhiteSpace(deviceType));

        // COST PER UNIT (expected + boundary + invalid handled)
        while (true)
        {
            Console.Write("Enter cost per unit (must be > 0): ");

            if (decimal.TryParse(Console.ReadLine(), out costPerUnit))
            {
                if (costPerUnit > 0)
                    break;

                Console.WriteLine("Value must be greater than 0.");
            }
            else
            {
                Console.WriteLine("Invalid input. Enter a number.");
            }
        }

        // NUMBER OF UNITS (expected + boundary + invalid handled)
        while (true)
        {
            Console.Write("Enter number of units (1 or more): ");

            if (int.TryParse(Console.ReadLine(), out numberOfUnits))
            {
                if (numberOfUnits >= 1)
                    break;

                Console.WriteLine("Units must be at least 1.");
            }
            else
            {
                Console.WriteLine("Invalid input. Enter a whole number.");
            }
        }

        // CATEGORY (strict boundary control)
        while (true)
        {
            Console.Write("Category (1=Laptop, 2=Desktop, 3=Other): ");

            if (int.TryParse(Console.ReadLine(), out category) &&
                category >= 1 && category <= 3)
                break;

            Console.WriteLine("Invalid input. Enter 1, 2, or 3.");
        }

        // CATEGORY NAME + COUNTING
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

        // TOTAL COST
        totalCost = costPerUnit * numberOfUnits;

        // APPLY DISCOUNT
        decimal finalCost = DiscountCost();

        // TRACK MOST EXPENSIVE
        if (finalCost > highestCost)
        {
            highestCost = finalCost;
            mostExpensiveDevice = deviceType;
        }

        // DEPRECIATION (5% over 6 months)
        decimal value = costPerUnit;
        string depreciationTable = "\nMonth\tValue";

        for (int i = 1; i <= 6; i++)
        {
            value -= value * DEPRECIATION_RATE;
            value = Math.Round(value, 2);

            depreciationTable += $"\n{i}\t{value:C}";
        }

        // STORE OUTPUT
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
        if (numberOfUnits > 5)
        {
            decimal fullPrice = 5 * costPerUnit;

            int remaining = numberOfUnits - 5;
            decimal discounted = remaining * costPerUnit * (1 - DISCOUNT_RATE);

            return fullPrice + discounted;
        }

        return totalCost;
    }
}