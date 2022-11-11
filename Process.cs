using System;
using System.IO;
using System.Linq;
using static AAT_TextExtractor.Storage;

namespace AAT_TextExtractor
{
    public class Process
    {
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
            Program.AfterProcess();
        }

        public static bool OriginalCheck(string file)
        {
            ListData.originalTextData.Clear();
            if (ProcessMode.singleFileMode)
            {
                foreach (string line in File.ReadAllLines(DirectoryPath.originalPath))
                {
                    ListData.originalTextData.Add(line);
                }
                return true;
            }

            string originalName = @"\" + Path.GetFileNameWithoutExtension(file);
            if (!File.Exists(DirectoryPath.originalPath + $"{originalName}.txt"))
            {
                Console.WriteLine($"Original file for {file} doesn't exist, skipping...");
                return false;
            }
            foreach (string line in File.ReadAllLines(DirectoryPath.originalPath + $"{originalName}.txt"))
            {
                ListData.originalTextData.Add(line);
            }
            
            return true;

        }
        
        public static bool MetaCheck(string file)
        {
            string metaName = @"\" + Path.GetFileNameWithoutExtension(file);
            string inputFolder = ProcessMode.singleFileMode
                ? Path.GetDirectoryName(file)
                : DirectoryPath.inputPath;
            if (!File.Exists(inputFolder + $"{metaName}.meta"))
            {
                Console.WriteLine($"Meta file for {file} doesn't exist, skipping...");
                return false;
            }
            ListData.metaData.Clear();
            foreach (string line in File.ReadAllLines(inputFolder + $"{metaName}.meta"))
            {
                ListData.metaData.Add(Convert.ToInt32(line));
            }
            return true;
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
                if (!MetaCheck(file) || !OriginalCheck(file))
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
                string name = ProcessMode.singleFileMode
                    ? Path.GetFileNameWithoutExtension(DirectoryPath.originalPath)
                    : Path.GetFileNameWithoutExtension(file);
                string insertOutput = DirectoryPath.outputPath + $"\\{name}.txt";
                int lineCounter = 0;
                if (CheckForDuplicate(insertOutput))
                {
                    return;
                }
                foreach (int j in ListData.metaData)
                {
                    ListData.originalTextData[j - 1] = @"Text(" + ListData.newTextData[lineCounter] + @");";
                    lineCounter++;
                }
                File.WriteAllLines(insertOutput, ListData.originalTextData);
                Console.WriteLine($"Inserting new text data in {insertOutput}... DONE!");
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
                    }

                    Console.WriteLine("Input is invalid, please try again.");
                }
            }
            return false;
        }
    }
}