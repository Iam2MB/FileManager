using System;
using System.Text;

namespace FileManager
{
    internal class InformPanel
    {
        public static void IPanel()
        {
            Console.SetCursorPosition(2, 21);
            Console.WriteLine("Наименование команд");
            Console.SetCursorPosition(2, 22);
            for (int i = 1; i < Program.WINDOW_WIDTH / 2; i++) Console.Write("-");
            Console.WriteLine();
            Console.SetCursorPosition(2, Console.CursorTop);
            Console.WriteLine("cd\tпереход к выбранному каталогу");
            Console.SetCursorPosition(2, Console.CursorTop);
            Console.WriteLine("ls\tвывод древа каталогов");
            Console.SetCursorPosition(2, Console.CursorTop);
            Console.WriteLine("ls -p\tвывод древа каталогов постранично");
            Console.SetCursorPosition(2, Console.CursorTop);
            Console.WriteLine("del\tудаление указанного каталога или файла");
            Console.SetCursorPosition(2, Console.CursorTop);
            Console.WriteLine("Стрелки вверх/вниз - история ввода");
        }
    }
}