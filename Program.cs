/**
 * .NET Framework 4.7.2 or higher is required to run this program.
 */

using System.Drawing;
using System.Drawing.Printing;

class AccessPrinters
{
    static void Main(string[] args)
    {
        MainMenu();        
    }

    private static void MainMenu()
    {
        try
        {
            string printer = ChoosePrinter();

            Console.WriteLine("1. Print a document");
            Console.WriteLine("2. Show printer capabilities");
            Console.Write("Enter choice: (Press any other key to exit): ");
            var inputString = Console.ReadLine();
            int choice = int.Parse(inputString != null && inputString != "" ? inputString : "0");

            switch (choice)
            {
                case 1:
                    Console.WriteLine("WARNING: This will print a test document on the selected printer.");
                    Console.WriteLine("IT IS IN INITIAL STAGES OF DEVELOPMENT. SO USE IT AT YOUR OWN RISK.");
                    Console.WriteLine("Type 'YES I KNOW' to continue to print a test document.");
                    var confirmation = Console.ReadLine();
                    if (confirmation != null && confirmation == "YES I KNOW")
                    {
                        try
                        {
                            PrintDocument();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Did not get the confirmation.\n");
                    }
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Printer Capabilities:\n");
                    ShowPrinterCapabilities(printer);
                    break;
                default:
                    break;
            }

            Console.WriteLine("Try again? (y/n)");
            var tryAgain = Console.ReadLine();
            if (tryAgain != null && tryAgain.ToLower() == "y")
            {
                Console.Clear();
                MainMenu();
            }
            else
            {
                Environment.Exit(0);
            }
        }
        catch 
        {
            Console.Clear();
            Console.WriteLine("Invalid input. Press any key to try again...");
            Console.ReadLine();
            MainMenu();
        }

        
    }

    private static string ChoosePrinter()
    {
        var connectedPrinters = PrinterSettings.InstalledPrinters;
        Console.WriteLine("Choose a printer:");
        for (int i = 0; i < connectedPrinters.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {connectedPrinters[i]}");
        }
        Console.Write("Enter choice (For default printer, enter 0): ");
        var inputString = Console.ReadLine();
        int choice = int.Parse(inputString != null && inputString != "" ? inputString : "0");
        
        string chosenPrinter = choice == 0 ? GetDefaultPrinter() : connectedPrinters[choice - 1];
        Console.Clear();
        Console.WriteLine($"{chosenPrinter} selected.\n");
        return chosenPrinter;
    }

    private static void PrintDocument()
    {
        // Create a PrintDocument object
        PrintDocument doc = new PrintDocument();

        // Set printer name as the selected printer
        doc.PrinterSettings.PrinterName = GetDefaultPrinter();

        // Add PrintPage event handler
        doc.PrintPage += new PrintPageEventHandler(PrintPageHandler);

        // Print the document
        doc.Print();
    }

    private static string GetDefaultPrinter()
    {
        PrinterSettings settings = new PrinterSettings();
        return settings.PrinterName;
    }

    private static void PrintPageHandler(object sender, PrintPageEventArgs e)
    {
        // Create a font and brush
        Font font = new Font("Arial", 12);
        SolidBrush brush = new SolidBrush(Color.Black);

        // Define print area
        RectangleF printArea = e.MarginBounds;

        // Create a StringFormat object to align text
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        // Print text
        if (e.Graphics != null)
        {
            e.Graphics.DrawString("Test Document", font, brush, printArea, format);
        }
    }

    private static void ShowPrinterCapabilities(String printer)
    {
        PrinterSettings printerSettings = new PrinterSettings();
        printerSettings.PrinterName = printer;
        Console.WriteLine($"Printer Name: {printerSettings.PrinterName}\n");
        
        Console.WriteLine("Supported paper sizes:");
        foreach (PaperSize paperSize in printerSettings.PaperSizes)
        {
            Console.WriteLine($"- {paperSize.PaperName}");
        }
        Console.WriteLine($"Default paper size: {printerSettings.DefaultPageSettings.PaperSize.PaperName}\n");

        Console.WriteLine("Supported paper sources:");
        foreach (PaperSource paperSource in printerSettings.PaperSources)
        {
            Console.WriteLine($"- {paperSource.SourceName}");
        }
        Console.WriteLine($"Default paper source: {printerSettings.DefaultPageSettings.PaperSource.SourceName}\n");

        Console.WriteLine("Supported Resolutions:");
        foreach (PrinterResolution printerResolution in printerSettings.PrinterResolutions)
        {
            Console.WriteLine($"- {printerResolution.Kind}");
        }
        Console.WriteLine($"Default printer resolution: {printerSettings.DefaultPageSettings.PrinterResolution.Kind}\n");

        Console.WriteLine("Supported Color Modes:");
        foreach (PrinterResolution printerResolution in printerSettings.PrinterResolutions)
        {
            Console.WriteLine($"- {printerResolution.Kind}");
        }
        Console.WriteLine($"Default printer resolution: {printerSettings.DefaultPageSettings.PrinterResolution.Kind}\n");

        Console.WriteLine($"Duplex: {printerSettings.Duplex}");

        Console.WriteLine("Show all printer settings? (y/n)");
        var choice = Console.ReadLine();
        if (choice != null && choice.ToLower() == "y")
        {
            foreach (var property in printerSettings.DefaultPageSettings.PrinterSettings.GetType().GetProperties())
            {
                Console.WriteLine($"{property.Name}: {property.GetValue(printerSettings.DefaultPageSettings.PrinterSettings)}");
            }
        }
        Console.WriteLine();
    }
}
