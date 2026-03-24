namespace NZCEA_assignmnt_92004
{
    internal class Program
    {
        //global variables
        static decimal overallCost = 0;
        public static List<string> deviceSlip = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Insurance calculator \n\n");


            char continueInput = 'y';
            while (continueInput.Equals('y'))
            {
                OneDevice();
                Console.WriteLine("\n\nWould you like to enter another device? y/n");
                continueInput = Console.ReadLine()[0];

            }

            Console.WriteLine("\n--- Device Slip ---");

            foreach (string item in deviceSlip)
            {
                Console.WriteLine(item);
            }

            



        }


        public static void OneDevice()
        {
            // variables
            string deviceType;
            decimal costPerUnit;
            int category;
            int numberOfUnits;
            decimal totalCost;

            //code

            Console.WriteLine("Please enter device type");
            deviceType = Console.ReadLine();
            

            Console.WriteLine("Please enter cost for a single unit");
            costPerUnit = Convert.ToDecimal(Console.ReadLine());
           

            Console.WriteLine("Please enter the amount of units");
            numberOfUnits = Convert.ToInt32(Console.ReadLine());
            
            while (true)
            {
                Console.Write("Category (1=Laptop, 2=Desktop, 3=Other): ");

                if (int.TryParse(Console.ReadLine(), out category) && category >= 1 && category <= 3)
                {
                    break;
                }

                Console.WriteLine("Invalid category. Please enter 1, 2, or 3.");
            }
            

            totalCost = costPerUnit * numberOfUnits;

            // convert category number → name
            string categoryName = "";

            if (category == 1) categoryName = "Laptop";
            else if (category == 2) categoryName = "Desktop";
            else categoryName = "Other";

            // add ONE clean line to the list
            deviceSlip.Add(
                $"Device: {deviceType} | Units: {numberOfUnits} | Cost/unit: {costPerUnit:C} | Category: {categoryName} | Total: {totalCost:C}"
            );

            overallCost += totalCost;


        }
    }
}
