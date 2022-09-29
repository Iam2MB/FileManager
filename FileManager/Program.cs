using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace FileManager
{
    internal class Program
    {
        public static int WINDOW_HEIGHT = 35;
        public static int WINDOW_WIDTH = 120;
        private static string currentDir = Directory.GetCurrentDirectory();
        private static List<string> historyComands = new List<string>();
        private static int pointHistory;

        static void Main(string[] args)
        {
            {
                Console.Title = Properties.Settings.Default.Title;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
                Console.SetBufferSize(WINDOW_WIDTH, WINDOW_HEIGHT);

                DrawWindow(0, 0, WINDOW_WIDTH, 20);
                DrawWindow(0, 20, WINDOW_WIDTH, 10);
                UpdateConsole();
            }
            Console.ReadKey(true);
        }

        static void UpdateConsole()
        {
            InformPanel.IPanel();
            DrawConsole(currentDir, 0, 30, WINDOW_WIDTH, 3);
            ProcessEnterCommand(WINDOW_WIDTH);
        }

        /// <summary>
        /// Позиция курсора
        /// </summary>
        /// <returns></returns>
        public static (int left, int top) GetCursorPosition()
        {
            return (Console.CursorLeft, Console.CursorTop);
        }

        /// <summary>
        /// Ввод команд в консоль
        /// </summary>
        /// <param name="width"></param>
        static void ProcessEnterCommand(int width)
        {
            (int left, int top) = GetCursorPosition();
            StringBuilder command = new StringBuilder();
            ConsoleKeyInfo Key;
            char key;
            do
            {
                Key = Console.ReadKey();
                key = Key.KeyChar;

                if (Key.Key != ConsoleKey.Backspace &&
                    Key.Key != ConsoleKey.Enter &&
                    Key.Key != ConsoleKey.UpArrow &&
                    Key.Key != ConsoleKey.DownArrow &&
                    Key.Key != ConsoleKey.LeftArrow &&
                    Key.Key != ConsoleKey.RightArrow)
                {
                    command.Append(key);
                }

                (int currentLeft, int currentTop) = GetCursorPosition();

                if (currentLeft == width - 2)
                {
                    Console.SetCursorPosition(currentLeft - 1, top);
                    Console.Write(" ");
                    Console.SetCursorPosition(currentLeft - 1, top);
                }
                if (Key.Key == ConsoleKey.Backspace)
                {
                    if (command.Length > 0)
                    {
                        command.Remove(command.Length - 1, 1);
                    }
                    if (currentLeft >= left)
                    {
                        Console.SetCursorPosition(currentLeft, top);
                        Console.Write(" ");
                        Console.SetCursorPosition(currentLeft, top);
                    }
                    else
                    {
                        Console.SetCursorPosition(left, top);
                    }
                }

                if (Key.Key == ConsoleKey.DownArrow)
                {
                    try
                    {
                        DrawConsole(currentDir, 0, 30, WINDOW_WIDTH, 3);
                        Console.Write(historyComands[pointHistory]);
                        pointHistory++;
                        if (pointHistory == historyComands.Count)
                        {
                            pointHistory = historyComands.Count - 1;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (Key.Key == ConsoleKey.UpArrow)
                {
                    try
                    {
                        DrawConsole(currentDir, 0, 30, WINDOW_WIDTH, 3);
                        Console.Write(historyComands[pointHistory - 1]);
                        pointHistory--;
                        if (pointHistory == 0)
                        {
                            pointHistory = 1;
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }

                if (Key.Key == ConsoleKey.LeftArrow) { Console.SetCursorPosition(currentLeft - 1, top); }
                if (Key.Key == ConsoleKey.RightArrow) { Console.SetCursorPosition(currentLeft + 1, top); }

            }
            while (key != 13);
            historyComands.Add(command.ToString());
            pointHistory = historyComands.Count;
            //HistoryComands(command.ToString());
            ParseCommandString(command.ToString());
        }

        static void ParseCommandString(string command)
        {
            string[] commandParam = command.ToLower().Split(' ');
            if (commandParam.Length > 0)
            {
                switch (commandParam[0])
                {
                    case "cd":
                        if (commandParam.Length > 1 && Directory.Exists(commandParam[1]))
                        {
                            currentDir = commandParam[1];

                            DrawTreeShort.DTS(new DirectoryInfo(commandParam[1]), 1);
                        }
                        break;
                    case "ls":
                        if (commandParam.Length > 1 && Directory.Exists(commandParam[1]))
                        {
                            if (commandParam.Length > 3 && commandParam[2] == "-p" && int.TryParse(commandParam[3], out int n))
                            {
                                DrawTree.DT(new DirectoryInfo(commandParam[1]), n);
                            }
                            else
                            {
                                DrawTree.DT(new DirectoryInfo(commandParam[1]), 1);
                            }
                        }
                        break;
                    case "del":
                        if (commandParam.Length > 1 && Directory.Exists(commandParam[1]) || File.Exists(commandParam[1]))
                        {
                            if (File.Exists(commandParam[1])) File.Delete(commandParam[1]);
                            if (Directory.Exists(commandParam[1])) Directory.Delete(commandParam[1]);

                        }
                        break;
                }
            }
            UpdateConsole();
        }

        /// <summary>
        /// Отрисовать консоль
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Width"></param>
        /// <param name="height"></param>
        static void DrawConsole(string dir, int x, int y, int Width, int height)
        {
            DrawWindow(x, y, Width, height);
            Console.SetCursorPosition(x + 1, y + height / 2);
            Console.Write($"{dir}>");
        }

        /// <summary>
        /// Отрисовать окно
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Width"></param>
        /// <param name="height"></param>
        public static void DrawWindow(int x, int y, int Width, int height)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("╔");
            for (int i = 0; i < Width - 2; i++)
            {
                Console.Write("═");
            }
            Console.Write("╗");

            Console.SetCursorPosition(x, y + 1);
            for (int i = 0; i < height - 2; i++)
            {
                Console.Write("║");
                for (int j = x + 1; j < x + Width - 1; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("║");
            }

            Console.Write("╚");
            for (int i = 0; i < Width - 2; i++)
            {
                Console.Write("═");
            }
            Console.Write("╝");
            Console.SetCursorPosition(x, y);
        }

    }
}