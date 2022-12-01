using System;
using System.Collections.Generic;

namespace AAT_TextExtractor_GUI
{
    public class Storage
    {
        public class ProcessMode
        {
            public static bool extractMode { get; set; }
            public static bool singleFileMode { get; set; }
        }
        public class DirectoryPath
        {
            public static string translatedPath = String.Empty;
            public static string outputPath = String.Empty;
            public static string originalPath = String.Empty;
            public static string recentDirectory { get; set; }
        }

        public class ExtractOptions
        {
            public static bool includeWait { get; set; }
            public static bool includeNewLine { get; set; }
            public static bool includeNewTextBox { get; set; }
        }

        public class ListData
        {
            public static List<string> lineNumbers = new List<string>();
            public static List<string> stringLines = new List<string>();
            public static List<string> originalTextData = new List<string>();
            public static List<string> newTextData = new List<string>();
            public static List<int> metaData = new List<int>();
        }
    }
}