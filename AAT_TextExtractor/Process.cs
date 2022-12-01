using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AAT_TextExtractor.Storage;

namespace AAT_TextExtractor
{
    public class Process
    {
        public static string speakerIdPath = Directory.GetCurrentDirectory() + @"\SpeakerID.txt";
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

        public static void InputTextToList(string file, string version)
        {
            string newTextName = @"\" + Path.GetFileNameWithoutExtension(file);
            string inputFolder = ProcessMode.singleFileMode
                ? Path.GetDirectoryName(file)
                : DirectoryPath.inputPath;
            ListData.newTextData.Clear();
            if (version.Equals(Program.version) || version.Equals("1.1.1"))
            {
                foreach (var match in File.ReadLines(inputFolder + $"{newTextName}.txt")
                             .Where(x => x.IndexOf(@"#") == 0))
                {
                    string insertText = match.Split('#')[1];
                    ListData.newTextData.Add(insertText);
                }
            }
            else if (version.Equals("1.1.0"))
            {
                foreach (var match in File.ReadLines(inputFolder + $"{newTextName}.txt")
                             .Where(x => x.IndexOf(@"""") == 0))
                {
                    ListData.newTextData.Add(match);
                }
            }
            else if (version.Equals("1.0.0"))
            {
                foreach (string line in File.ReadAllLines(inputFolder + $"{newTextName}.txt"))
                {
                    ListData.newTextData.Add(line);
                }
            }
        }

        public static string GetVersion(string file)
        {
            foreach (var match in File.ReadLines(file).Where(x => x.IndexOf(@"{") == 0))
            {
                string version = match.Split(new[] { @"{AAT_TE version: " }, StringSplitOptions.None)[1];
                version = version.Split('}')[0];
                return version;
            }
            Console.WriteLine($"Version header in {file} is not found, please refer to the compatibility note in GitHub for solution");
            return "Error";
        }

        public static void InsertText(string file)
        {
            if (Path.GetExtension(file).Equals(".txt"))
            {
                if (!MetaCheck(file) || !OriginalCheck(file))
                {
                    return;
                }

                string fileVersion = GetVersion(file);
                if (fileVersion.Equals("Error"))
                {
                    return;
                }
                InputTextToList(file, fileVersion);
                WriteInFile(file);
            }
        }

        public static bool CheckSpeakerIdFile()
        {
            //string configPath = Directory.GetCurrentDirectory() + @"\SpeakerID.txt";
            //string configPath = @"C:\Users\ASUS\Desktop\SpeakerID.txt";
            if (!File.Exists(speakerIdPath))
            {
                Console.WriteLine($"Error: SpeakerID file in {speakerIdPath} is not found, aborting...");
                return false;
            }

            return true;
        }

        public static void ExtractText(string file)
        {
            ListData.lineNumbers.Clear();
            ListData.stringLines.Clear();
            string[] findLines = { @"Text(""", @"SetSpeakerId(", @"Wait(", @"NewLine(", @"ClearText(", @"ReadKey(", @"Op_" };
            string[] specialString = { @"ClearText(", @"ReadKey(", @"Op_", @"NewLine(", @"Wait(" };
            if (!CheckSpeakerIdFile())
            {
                return;
            }
            foreach (var match in File.ReadLines(file)
                         .Select((text, index) => new { text, lineNumber = index + 1 })
                         .Where(x => findLines.Any(x.text.Contains)))
            {
                if (match.text.Contains(findLines[0]))
                {
                    string extractedText = match.text.Split(new[] {@"Text("}, StringSplitOptions.None)[1];
                    extractedText = @"#" + extractedText.Split(new[] {@");"}, StringSplitOptions.None)[0];
                    ListData.lineNumbers.Add(match.lineNumber.ToString());
                    ListData.stringLines.Add(extractedText);
                }
                if (match.text.Contains(findLines[1]))
                {
                    string extractedText = match.text.Split(new[] {@"SetSpeakerId("}, StringSplitOptions.None)[1];
                    extractedText = extractedText.Split(new[] {@");"}, StringSplitOptions.None)[0];
                    string speakerName = "\n" + $"[{ReplaceIdWithName(extractedText)}]";
                    ListData.stringLines.Add(speakerName);
                }
                if (specialString.Any(s => match.text.Contains(s)))
                {
                    ListData.stringLines.Add($"[{match.text}]");
                }
            }
            var test = ListData.stringLines;
            CleanIrrelevantCommand(ListData.stringLines, out ListData.stringLines);
            WriteInFile(file);
        }
        
        public static void CleanIrrelevantCommand(List<string> lineList, out List<string> newLineList)
        {
            string[] splitter = { @"#""", @"Wait(", @"ClearText(", @"ReadKey(", @"Op_" };
            newLineList = lineList;
            List<int> removeIndex = new List<int>();
            bool removeWait = true;
            foreach (var match in lineList
                         .Select((text, index) => new {text, listIndex = index})
                         .Where(x => splitter.Any(x.text.Contains)))
            {
                if (match.text.Contains(@"Wait(") && removeWait)
                {
                    removeIndex.Add(match.listIndex);
                }
                else if (match.text.Contains(@"ClearText(") || match.text.Contains(@"ReadKey(") || match.text.Contains(@"Op_"))
                {
                    removeWait = true;
                    removeIndex.Add(match.listIndex);
                }
                else if (match.text.Contains(@"#"""))
                {
                    removeWait = false;
                }
            }
            foreach (int index in removeIndex.OrderByDescending(i => i))
            {
                newLineList.RemoveAt(index);
            }
        }

        public static string ReplaceIdWithName(string id)
        {
            //string configPath = @"C:\Users\ASUS\Desktop\SpeakerID.txt";
            foreach (var match in File.ReadLines(speakerIdPath)
                         .Where(x => x.Contains(id)))
            {
                if (match.Split(new[] { " =" }, StringSplitOptions.None)[0].Equals(id))
                {
                    string idName = match.Split(new[] { "= " }, StringSplitOptions.None)[1];
                    return idName;
                }
            }
            return "Error: Missing Speaker Definition";
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
                ListData.stringLines.Insert(0, @"{" + "AAT_TE version: " + Program.version + @"}");
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