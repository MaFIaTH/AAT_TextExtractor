using System.Collections.Generic;

namespace AAT_TextExtractor
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
            public static string inputPath { get; set; }
            public static string outputPath { get; set; }
            public static string originalPath { get; set; }
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