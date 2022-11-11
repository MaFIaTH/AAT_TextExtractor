using System;
using static AAT_TextExtractor.GetInput;

namespace AAT_TextExtractor
{
    internal class Program
    {
        public static string version = "1.1.1";
        public static void Main(string[] args)
        {
            GetFileMode();
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
            int action;
            while (!int.TryParse(Console.ReadLine(), out action) || action < 1 || action > 5)
            {
                Console.WriteLine("Invalid input, please try again");
                Console.Write("\nSelect your action (1 - 5): ");
            }

            switch (action)
            {
                case 1:
                    Process.Initialization();
                    break;
                case 2:
                    GetDirectory();
                    break;
                case 3:
                    Storage.ProcessMode.extractMode = !Storage.ProcessMode.extractMode;
                    GetDirectory();
                    break;
                case 4:
                    GetFileMode();
                    break;
                case 5:
                    Environment.Exit(0);
                    return;
            }
        }
    }
}