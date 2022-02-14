using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ShadowLogCleaner
{
    internal class Program
    {

        private static String AsciiArt =
            "╭━━━┳╮╱╱╱╱╱╱╭╮╱╱╱╱╱╱╱╱╭╮╱╱╱╱╱╱╱╱╱╭━━━┳╮\n" +
            "┃╭━╮┃┃╱╱╱╱╱╱┃┃╱╱╱╱╱╱╱╱┃┃╱╱╱╱╱╱╱╱╱┃╭━╮┃┃\n" +
            "┃╰━━┫╰━┳━━┳━╯┣━━┳╮╭╮╭╮┃┃╱╱╭━━┳━━╮┃┃╱╰┫┃╭━━┳━━┳━╮╭━━┳━╮\n" +
            "╰━━╮┃╭╮┃╭╮┃╭╮┃╭╮┃╰╯╰╯┃┃┃╱╭┫╭╮┃╭╮┃┃┃╱╭┫┃┃┃━┫╭╮┃╭╮┫┃━┫╭╯\n" +
            "┃╰━╯┃┃┃┃╭╮┃╰╯┃╰╯┣╮╭╮╭╯┃╰━╯┃╰╯┃╰╯┃┃╰━╯┃╰┫┃━┫╭╮┃┃┃┃┃━┫┃\n" +
            "╰━━━┻╯╰┻╯╰┻━━┻━━╯╰╯╰╯╱╰━━━┻━━┻━╮┃╰━━━┻━┻━━┻╯╰┻╯╰┻━━┻╯\n" +
            "╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╭━╯┃\n" +
            "╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╱╰━━╯\n";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            WriteLine(Program.AsciiArt, ConsoleColor.Red);
            bool deleteConfig = Confirm("Do you wanna clear config too? (recomm1ended)");
            ClearLogs(deleteConfig);
            WriteLine("Press any key to exit...", ConsoleColor.Cyan);
            Console.ReadKey(false);
        }

        private static void ClearLogs(bool deleteConfig)
        {
            WriteLine("[-] Stopping all Riot Games processes...", ConsoleColor.Green);
            Execute("taskkill -f -im riot*");
            Execute("taskkill -f -im league*");
            Thread.Sleep(1000);
            WriteLine("[+] Riot Processes stopped", ConsoleColor.DarkGreen);

            WriteLine("[-] Deleting %programdata%\\machine.cfg...", ConsoleColor.Green);
            DeleteFileIfExists("C:\\ProgramData\\Riot Games\\machine.cfg");
            WriteLine("[+] Deleted %programdata%\\machine.cfg", ConsoleColor.DarkGreen);

            WriteLine("[-] Deleting Riot Games LocalAppData...", ConsoleColor.Green);
            String localRiotGames = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Riot Games";
            DeleteDirectoryIfExists(localRiotGames, true);
            WriteLine("[+] Deleted " + localRiotGames, ConsoleColor.DarkGreen);

            WriteLine("[-] Deleting Logs...", ConsoleColor.Green);
            String logs = "C:\\Riot Games\\League of Legends\\Logs";
            DeleteDirectoryIfExists(logs, true);
            WriteLine("[+] Deleted " + logs, ConsoleColor.DarkGreen);

            if (deleteConfig)
            {
                WriteLine("[-] Deleting Config...", ConsoleColor.Green);
                String config = "C:\\Riot Games\\League of Legends\\Config";
                DeleteDirectoryIfExists(config, true);
                WriteLine("[+] Deleted " + config, ConsoleColor.DarkGreen);
            }
        }

        private static void DeleteFileIfExists(String path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                WriteLine("[!] File " + path + " does not exists!", ConsoleColor.Red);
            }
        }

        private static void DeleteDirectoryIfExists(String path, bool recursive)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive);
            }
            else
            {
                WriteLine("[!] Directory " + path + " does not exists!", ConsoleColor.Red);
            }
        }

        private static void Execute(String command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + command
            };
            process.StartInfo = startInfo;
            process.Start();
        }

        public static void WriteLine(String text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void Write(String text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static bool Confirm(string title)
        {
            ConsoleKey response;
            do
            {
                Write($"{ title } [y/n] ", ConsoleColor.Green);
                response = Console.ReadKey(false).Key;
                if (response != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return (response == ConsoleKey.Y);
        }
    }
}
