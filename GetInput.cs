using System;
using System.IO;
using static AAT_TextExtractor.Storage;

namespace AAT_TextExtractor
{
    public class GetInput
    {
        public static void GetFileMode()
        {
            Console.WriteLine("AAT Text Line Extractor Tools\n");
            while (true)
            {
                Console.Write("Single File Mode? (Y/N): ");
                string mode = Console.ReadLine().ToUpper();
                if (mode.Equals("Y") || mode.Equals("N"))
                {
                    switch (mode)
                    {
                        case "Y":
                            ProcessMode.singleFileMode = true;
                            break;
                        case "N":
                            ProcessMode.singleFileMode = false;
                            break;
                    }
                    GetProcessMode();
                    return;
                }
                Console.WriteLine("Input is incorrect, please try again");
            }
        }

        public static void GetProcessMode()
        {
            while (true)
            {
                Console.Write("Select process, Extract or Insert (E/I): ");
                string mode = Console.ReadLine().ToUpper();
                if (mode.Equals("E") || mode.Equals("I"))
                {
                    switch (mode)
                    {
                        case "E":
                            ProcessMode.extractMode = true;
                            GetDirectory();
                            break;
                        case "I":
                            ProcessMode.extractMode = false;
                            GetDirectory();
                            break;
                    }
                    return;
                }
                Console.WriteLine("Input is incorrect, please try again");
            }
        }

        public static void GetDirectory()
        {
            //Get Input
            string inputExample = ProcessMode.singleFileMode ? @"C:\Input\Filename.extension" : @"C:\Input";
            Console.WriteLine("\n==== Input Directory ===");
            Console.WriteLine($"Example: {inputExample}");
            Console.Write("Please enter input directory: ");
            if (ProcessMode.singleFileMode)
            {
                while (!File.Exists(DirectoryPath.inputPath = Console.ReadLine()))
                {
                    Console.WriteLine("File doesn't exist! Please try again");
                    Console.Write("Please enter input directory: ");
                }
            }
            else
            {
                while (!Directory.Exists(DirectoryPath.inputPath = Console.ReadLine()))
                {
                    Console.WriteLine("Directory doesn't exist! Please try again");
                    Console.Write("Please enter input directory: ");
                }
            }

            //Get Original
            if (!ProcessMode.extractMode)
            {
                string originalExample =
                    ProcessMode.singleFileMode ? @"C:\Original\Filename.extension" : @"C:\Original";
                Console.WriteLine("\n==== Original Directory ===");
                Console.WriteLine($"Example: {originalExample}");
                Console.Write("Please enter original directory: ");
                if (ProcessMode.singleFileMode)
                {
                    while (!File.Exists(DirectoryPath.originalPath = Console.ReadLine()))
                    {
                        Console.WriteLine("File doesn't exist! Please try again");
                        Console.Write("Please enter input directory: ");
                    }
                }
                else
                {
                    while (!Directory.Exists(DirectoryPath.originalPath = Console.ReadLine()))
                    {
                        Console.WriteLine("Directory doesn't exist! Please try again");
                        Console.Write("Please enter input directory: ");
                    }
                }
            }

            //Get Output
            string outputExample = @"C:\Output";
            Console.WriteLine("\n==== Output Directory ===");
            Console.WriteLine($"Example: {outputExample}");
            Console.Write("Please enter output directory: ");
            while (!Directory.Exists(DirectoryPath.outputPath = Console.ReadLine()))
            {
                Console.WriteLine("Directory doesn't exist! Please try again");
                Console.Write("Please enter output directory: ");
            }

            Process.Initialization();

        }
    }
}