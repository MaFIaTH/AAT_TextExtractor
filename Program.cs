using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AAT_TextExtractor
{
    internal class Program
    {
        class ProcessMode
        {
            public static bool extractMode { get; set; }
            public static bool singleFileMode { get; set; }
        }
        class DirectoryPath
        {
            public static string inputPath { get; set; }
            public static string outputPath { get; set; }
            
            public static string originalPath { get; set; }
        }

        class ListData
        {
            public static List<string> lineNumbers = new List<string>();
            public static List<string> stringLines = new List<string>();
            public static List<string> originalTextData = new List<string>();
            public static List<string> newTextData = new List<string>();
            public static List<int> metaData = new List<int>();
        }
        public static void Main(string[] args)
        {
            GetFileMode();
        }

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
                string originalExample = ProcessMode.singleFileMode ? @"C:\Original\Filename.extension" : @"C:\Original";
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
            Initialization();

        }

        public static void Initialization()
        {
            if (ProcessMode.extractMode)
            {
                Console.WriteLine("\n==== Processing ====");
                Console.WriteLine("Mode: Extract\n");
                if (ProcessMode.singleFileMode)
                {
                    ExtractText(DirectoryPath.inputPath);
                }
                else
                {
                    foreach (string file in Directory.EnumerateFiles(DirectoryPath.inputPath))
                    {
                        ExtractText(file);
                    }
                }
            }
            else
            {
                Console.WriteLine("\n==== Processing ====");
                Console.WriteLine("Mode: Insert\n");
                if (ProcessMode.singleFileMode)
                {
                    InsertText(DirectoryPath.inputPath);
                }
                else
                {
                    foreach (string file in Directory.EnumerateFiles(DirectoryPath.inputPath))
                    {
                        InsertText(file);
                    }
                }
            }
            AfterProcess();
        }

        public static int OriginalCheck(string file)
        {
            ListData.originalTextData.Clear();
            if (ProcessMode.singleFileMode)
            {
                foreach (string line in File.ReadAllLines(DirectoryPath.originalPath))
                {
                    ListData.originalTextData.Add(line);
                }
                return 0;
            }

            string originalName = @"\" + Path.GetFileNameWithoutExtension(file);
            if (!File.Exists(DirectoryPath.originalPath + $"{originalName}.txt"))
            {
                Console.WriteLine($"Original file for {file} doesn't exist, skipping...");
                return -1;
            }
            foreach (string line in File.ReadAllLines(DirectoryPath.originalPath + $"{originalName}.txt"))
            {
                ListData.originalTextData.Add(line);
            }
            
            return 0;

        }
        
        public static int MetaCheck(string file)
        {
            string metaName = @"\" + Path.GetFileNameWithoutExtension(file);
            string inputFolder = ProcessMode.singleFileMode
                ? Path.GetDirectoryName(file)
                : DirectoryPath.inputPath;
            if (!File.Exists(inputFolder + $"{metaName}.meta"))
            {
                Console.WriteLine($"Meta file for {file} doesn't exist, skipping...");
                return -1;
            }
            ListData.metaData.Clear();
            foreach (string line in File.ReadAllLines(inputFolder + $"{metaName}.meta"))
            {
                ListData.metaData.Add(Convert.ToInt32(line));
            }
            return 0;
        }

        public static void InputTextToList(string file)
        {
            string newTextName = @"\" + Path.GetFileNameWithoutExtension(file);
            string inputFolder = ProcessMode.singleFileMode
                ? Path.GetDirectoryName(file)
                : DirectoryPath.inputPath;
            ListData.newTextData.Clear();
            foreach (string line in File.ReadAllLines(inputFolder + $"{newTextName}.txt"))
            {
                ListData.newTextData.Add(line);
            }
        }

        public static void InsertText(string file)
        {
            if (Path.GetExtension(file).Equals(".txt"))
            {
                if (MetaCheck(file) < 0 || OriginalCheck(file) < 0)
                {
                    return;
                }

                InputTextToList(file);
                WriteInFile(file);
            }
        }

        public static void ExtractText(string file)
        {
            ListData.lineNumbers.Clear();
            ListData.stringLines.Clear();
            foreach (var match in File.ReadLines(file)
                         .Select((text, index) => new { text, lineNumber = index + 1 })
                         .Where(x => x.text.Contains(@"Text(""")))
            {
                if (match.text.Contains(@"Text("""))
                {
                    string extractedText = match.text.Split(new[] {@"Text("}, StringSplitOptions.None)[1];
                    extractedText = extractedText.Split(new[] {@");"}, StringSplitOptions.None)[0];
                    ListData.lineNumbers.Add(match.lineNumber.ToString());
                    ListData.stringLines.Add(extractedText);
                    //Console.WriteLine(extractedText);
                }
            }
            WriteInFile(file);
        }
        
        public static void WriteInFile(string file)
        {
            if (ProcessMode.extractMode)
            {

                string name = ProcessMode.singleFileMode
                    ? Path.GetFileNameWithoutExtension(DirectoryPath.inputPath)
                    : Path.GetFileNameWithoutExtension(file);
                //name = name.Split('.')[0];
                
                //string name = Path.GetFileNameWithoutExtension(DirectoryPath.inputPath);
                string textOutput = DirectoryPath.outputPath + $"\\{name}.txt";
                string metaOutput = DirectoryPath.outputPath + $"\\{name}.meta";
                if (!CheckForDuplicate(textOutput))
                {
                    File.WriteAllLines(textOutput, ListData.stringLines);
                    Console.WriteLine($"Creating {textOutput}... DONE!");
                }
                if (!CheckForDuplicate(metaOutput))
                {
                    File.WriteAllLines(metaOutput, ListData.lineNumbers);
                    Console.WriteLine($"Creating {metaOutput}... DONE!");
                }
            }
            else
            {
                /*
                string name = ProcessMode.singleFileMode ? Path.GetFileName(DirectoryPath.originalPath)
                    : file.Split(new[] {DirectoryPath.inputPath + "\\"}, StringSplitOptions.None)[1];
                name = name.Split('.')[0];
                */
                string name = ProcessMode.singleFileMode
                    ? Path.GetFileNameWithoutExtension(DirectoryPath.originalPath)
                    : Path.GetFileNameWithoutExtension(file);
                string insertOutput = DirectoryPath.outputPath + $"\\{name}.txt";
                int lineCounter = 0;
                foreach (int j in ListData.metaData)
                {
                    ListData.originalTextData[j - 1] = @"Text(" + ListData.newTextData[lineCounter] + @");";
                    lineCounter++;
                }
                if (!CheckForDuplicate(insertOutput))
                {
                    File.WriteAllLines(insertOutput, ListData.originalTextData);
                    Console.WriteLine($"Inserting new text data in {insertOutput}... DONE!");
                }
            }
        }

        public static bool CheckForDuplicate(string file)
        {
            if (File.Exists(file))
            {
                Console.WriteLine($"\n{file} is already exist");
                while (true)
                {
                    Console.Write("Would you like to overwrite? (Y/N): ");
                    string answer = Console.ReadLine().ToUpperInvariant();
                    if (answer.Equals("Y") || answer.Equals("N"))
                    {
                        if (answer.Equals("N"))
                        {
                            return true;
                        }
                        return false;

                        break;
                    }

                    Console.WriteLine("Input is invalid, please try again.");
                }
            }
            return false;
        }

        public static void AfterProcess()
        {
            Console.WriteLine("\nWould you like to:" +
                              "\nRepeat (1): repeat the process with the same input." +
                              "\nRepeat w/ new path (2): repeat the process but the user will have to specify new input & output path." +
                              "\nReverse w/ new path (3): reverse extract/insert process but the user will have to specify new input & output path." +
                              "\nRestart (4): restart and select mode again." +
                              "\nExit (5): exit this program.");
            Console.Write("\nSelect your action (1 - 5): ");
            int action = 1;
            while (!int.TryParse(Console.ReadLine(), out action) || action < 1 || action > 5)
            {
                Console.WriteLine("Invalid input, please try again");
                Console.Write("\nSelect your action (1 - 5): ");
            }

            switch (action)
            {
                case 1:
                    Initialization();
                    break;
                case 2:
                    GetDirectory();
                    break;
                case 3:
                    ProcessMode.extractMode = !ProcessMode.extractMode;
                    GetDirectory();
                    break;
                case 4:
                    GetFileMode();
                    break;
                case 5:
                    return;
            }
        }
    }
}