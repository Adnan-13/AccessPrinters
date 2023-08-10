/**
 * .NET Framework 4.7.2 or higher is required to run this program.
 */

using System.Drawing;
using System.Drawing.Printing;

class AccessPrinters
{
    private static string PRINTER_NAME = "";

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
                    Console.Clear();
                    Console.WriteLine("1. Print a local file");
                    Console.WriteLine("2. Print a dummy test document");

                    Console.Write("Enter choice: ");
                    var inputString2 = Console.ReadLine();
                    int choice2 = int.Parse(
                        inputString2 != null && inputString2 != "" ? inputString2 : "0"
                    );

                    Console.WriteLine(
                        "WARNING: This will print a test document on the selected printer."
                    );
                    Console.WriteLine(
                        "IT IS IN INITIAL STAGES OF DEVELOPMENT. SO USE IT AT YOUR OWN RISK."
                    );
                    Console.WriteLine("Type 'YES I KNOW' to continue to print a test document.");
                    var confirmation = Console.ReadLine();
                    if (confirmation != null && confirmation == "YES I KNOW")
                    //if (true)
                    {
                        try
                        {
                            switch (choice2)
                            {
                                case 1:
                                    Console.Clear();
                                    //Console.Write("Enter file path: ");
                                    //var filePath = Console.ReadLine();
                                    Console.WriteLine("1. Show files in current directory");
                                    Console.WriteLine("2. Enter file path");
                                    Console.Write("Enter choice: ");
                                    var inputString3 = Console.ReadLine();
                                    int choice3 = int.Parse(
                                        inputString3 != null && inputString3 != ""
                                            ? inputString3
                                            : "0"
                                    );
                                    string filePath = "";
                                    switch (choice3)
                                    {
                                        case 1:
                                            string[] strings = Directory.GetFiles(
                                                Directory.GetCurrentDirectory()
                                            );
                                            Console.WriteLine("Files in current directory:");
                                            for (int i = 0; i < strings.Length; i++)
                                            {
                                                //Console.WriteLine($"{i + 1}. {strings[i]}");
                                                Console.WriteLine(
                                                    $"{i + 1}. {Path.GetFileName(strings[i])}"
                                                );
                                            }
                                            Console.Write("Enter choice: ");
                                            var inputString4 = Console.ReadLine();
                                            if (inputString4 == null || inputString4 == "")
                                            {
                                                throw new Exception("Invalid input.");
                                            }
                                            int choice4 = int.Parse(inputString4);
                                            filePath = strings[choice4 - 1];
                                            break;
                                        case 2:
                                            Console.Write("Enter file path: ");
                                            filePath = Console.ReadLine() ?? "";
                                            break;
                                        default:
                                            break;
                                    }
                                    PrintFile(filePath);
                                    break;
                                case 2:
                                    PrintDummyDocument();
                                    break;
                                default:
                                    break;
                            }
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
        PRINTER_NAME = chosenPrinter;
        return chosenPrinter;
    }

    private static void PrintDummyDocument()
    {
        // Create a PrintDocument object
        PrintDocument pd = new();

        // Set printer name as the selected printer
        pd.PrinterSettings.PrinterName = GetDefaultPrinter();

        // Add PrintPage event handler
        pd.PrintPage += new PrintPageEventHandler((sender, e) => PrintPageHandler(sender, e, ""));

        // Print the document
        pd.Print();
        pd.Dispose();
    }

    private static string GetDefaultPrinter()
    {
        PrinterSettings settings = new();
        return settings.PrinterName;
    }

    private static void PrintFile(string filePath)
    {
        PrintDocument pd = new();
        pd.PrinterSettings.PrinterName = PRINTER_NAME != "" ? PRINTER_NAME : ChoosePrinter();
        pd.PrintPage += (sender, e) => PrintPageHandler(sender, e, filePath);
        pd.Print();
        pd.Dispose();
    }

    private static void PrintPageHandler(object sender, PrintPageEventArgs e, string filePath)
    {
        if (filePath != "")
        {
            // if file is not type of txt then throw exception
            if (Path.GetExtension(filePath) != ".txt")
            {
                throw new Exception("Only .txt files are supported.");
            }

            Console.WriteLine($"Printing {Path.GetFileName(filePath)}...");

            string content = File.ReadAllText(filePath);
            Console.Clear();
            Console.WriteLine("Content of file:\n");
            Console.WriteLine(content);

            // Select Paper Size
            Console.WriteLine("Select paper size:");
            for (int i = 0; i < e.PageSettings.PrinterSettings.PaperSizes.Count; i++)
            {
                if (
                    e.PageSettings.PrinterSettings.PaperSizes[i].PaperName != "Custom"
                    && e.PageSettings.PrinterSettings.PaperSizes[i].PaperName != ""
                )
                {
                    Console.WriteLine(
                        $"{i + 1}. {e.PageSettings.PrinterSettings.PaperSizes[i].PaperName}"
                    );
                }
            }
            Console.Write("Enter choice(For default paper size, enter 0): ");
            var inputString = Console.ReadLine();
            int choice = int.Parse(inputString != null && inputString != "" ? inputString : "0");
            e.PageSettings.PaperSize =
                choice == 0
                    ? e.PageSettings.PrinterSettings.DefaultPageSettings.PaperSize
                    : e.PageSettings.PrinterSettings.PaperSizes[choice - 1];
            Console.WriteLine($"Paper size: {e.PageSettings.PaperSize.PaperName}");

            // Get font size from user
            Console.Write("Enter font size (default is 12): ");
            inputString = Console.ReadLine();
            int fontSize = int.Parse(inputString != null && inputString != "" ? inputString : "12");
            Font font = new("Arial", fontSize);
            Console.WriteLine($"Font size: {fontSize}");

            // Get Resolution from user
            Console.WriteLine("Select resolution:");
            for (int i = 0; i < e.PageSettings.PrinterSettings.PrinterResolutions.Count; i++)
            {
                if (
                    e.PageSettings.PrinterSettings.PrinterResolutions[i].Kind
                    != PrinterResolutionKind.Custom
                )
                {
                    Console.WriteLine(
                        $"{i + 1}. {e.PageSettings.PrinterSettings.PrinterResolutions[i].Kind}"
                    );
                }
            }
            Console.Write("Enter choice(For default resolution, enter 0): ");
            inputString = Console.ReadLine();
            choice = int.Parse(inputString != null && inputString != "" ? inputString : "0");
            e.PageSettings.PrinterResolution =
                choice == 0
                    ? e.PageSettings.PrinterSettings.DefaultPageSettings.PrinterResolution
                    : e.PageSettings.PrinterSettings.PrinterResolutions[choice - 1];
            Console.WriteLine($"Resolution: {e.PageSettings.PrinterResolution.Kind}");

            // Get orientation from user
            Console.WriteLine("Select orientation:");
            Console.WriteLine("1. Portrait(default)");
            Console.WriteLine("2. Landscape");
            Console.Write("Enter choice: ");
            inputString = Console.ReadLine();
            choice = int.Parse(inputString != null && inputString != "" ? inputString : "1");
            e.PageSettings.Landscape = choice == 2;
            Console.WriteLine($"Orientation: Landscape: {e.PageSettings.Landscape}");

            // Get number of copies from user
            Console.Write("Enter number of copies (default is 1): ");
            inputString = Console.ReadLine();
            int copies = int.Parse(inputString != null && inputString != "" ? inputString : "1");
            e.PageSettings.PrinterSettings.Copies = (short)copies;

            // Get alignment from user
            Console.WriteLine("Select alignment:");
            Console.WriteLine("1. Left(default)");
            Console.WriteLine("2. Center");
            Console.WriteLine("3. Right");
            Console.Write("Enter choice: ");
            inputString = Console.ReadLine();
            choice = int.Parse(inputString != null && inputString != "" ? inputString : "1");
            StringFormat format =
                new()
                {
                    Alignment =
                        choice == 2
                            ? StringAlignment.Center
                            : choice == 3
                                ? StringAlignment.Far
                                : StringAlignment.Near
                };

            e.Graphics?.DrawString(content, font, Brushes.Black, e.MarginBounds, format);
        }
        else
        {
            // Create a font and brush
            Font font = new("Arial", 12);
            SolidBrush brush = new(Color.Black);

            // Define print area
            RectangleF printArea = e.MarginBounds;

            // Create a StringFormat object to align text
            StringFormat format =
                new()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

            // Print text
            e.Graphics?.DrawString("Test Document", font, brush, printArea, format);
        }
    }

    private static void ShowPrinterCapabilities(string printer)
    {
        PrinterSettings printerSettings = new() { PrinterName = printer };
        Console.WriteLine($"Printer Name: {printerSettings.PrinterName}\n");

        Console.WriteLine("Supported paper sizes:");
        foreach (PaperSize paperSize in printerSettings.PaperSizes)
        {
            Console.WriteLine($"- {paperSize.PaperName}");
        }
        Console.WriteLine(
            $"Default paper size: {printerSettings.DefaultPageSettings.PaperSize.PaperName}\n"
        );

        Console.WriteLine("Supported paper sources:");
        foreach (PaperSource paperSource in printerSettings.PaperSources)
        {
            Console.WriteLine($"- {paperSource.SourceName}");
        }
        Console.WriteLine(
            $"Default paper source: {printerSettings.DefaultPageSettings.PaperSource.SourceName}\n"
        );

        Console.WriteLine("Supported Resolutions:");
        foreach (PrinterResolution printerResolution in printerSettings.PrinterResolutions)
        {
            Console.WriteLine($"- {printerResolution.Kind}");
        }
        Console.WriteLine(
            $"Default printer resolution: {printerSettings.DefaultPageSettings.PrinterResolution.Kind}\n"
        );

        Console.WriteLine("Supported Color Modes:");
        foreach (PrinterResolution printerResolution in printerSettings.PrinterResolutions)
        {
            Console.WriteLine($"- {printerResolution.Kind}");
        }
        Console.WriteLine(
            $"Default printer resolution: {printerSettings.DefaultPageSettings.PrinterResolution.Kind}\n"
        );

        Console.WriteLine($"Duplex: {printerSettings.Duplex}");

        Console.WriteLine("Show all printer settings? (y/n)");
        var choice = Console.ReadLine();
        if (choice != null && choice.ToLower() == "y")
        {
            foreach (
                var property in printerSettings.DefaultPageSettings.PrinterSettings
                    .GetType()
                    .GetProperties()
            )
            {
                Console.WriteLine(
                    $"{property.Name}: {property.GetValue(printerSettings.DefaultPageSettings.PrinterSettings)}"
                );
            }
        }
        Console.WriteLine();
    }
}
