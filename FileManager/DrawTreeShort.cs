using System;
using System.Text;
using System.IO;

namespace FileManager
{
    class DrawTreeShort
    {
        /// <summary>
        /// Отрисовать делево каталогов без вложенных папопок
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="page"></param>
        public static void DTS(DirectoryInfo dir, int page)
        {
            StringBuilder tree = new StringBuilder();
            GetTreeShort(tree, dir, "", true);
            Program.DrawWindow(0, 0, Program.WINDOW_WIDTH, 20);
            (int currentLeft, int currentTop) = Program.GetCursorPosition();

            int pageLines = 18;
            string[] lines = tree.ToString().Split(new char[] { '\n' });
            int pageTotal = (lines.Length + pageLines - 1) / pageLines;
            if (page > pageTotal) { page = pageTotal; }
            for (int i = (page - 1) * pageLines, counter = 0; i < page * pageLines; i++, counter++)
            {
                if (lines.Length - 1 > i)
                {
                    Console.SetCursorPosition(currentLeft + 1, currentTop + 1 + counter);
                    Console.WriteLine(lines[i]);
                }
            }
            string footer = $"╡ {page} / {pageTotal} ╞";
            Console.SetCursorPosition(Program.WINDOW_WIDTH / 2 - footer.Length / 2, 19);
            Console.WriteLine(footer);
        }

        static void GetTreeShort(StringBuilder tree, DirectoryInfo dir, string indent, bool lastDirectory)
        {
            tree.Append(indent);
            if (lastDirectory)
            {
                tree.Append("└─");
                indent += "  ";
            }
            else
            {
                tree.Append("├─");
                indent += "│ ";
            }
            tree.Append($"{dir.Name}\n");

            DirectoryInfo[] subDirs = dir.GetDirectories();
            for (int i = 0; i < subDirs.Length; i++)
            {
                if (i == subDirs.Length - 1)
                {
                    tree.Append($"{indent}└─{subDirs[i].Name}\n");
                }
                else
                {
                    tree.Append($"{indent}├─{subDirs[i].Name}\n");
                }
            }

            FileInfo[] subFiles = dir.GetFiles();
            for (int i = 0; i < subFiles.Length; i++)
            {
                if (i == subFiles.Length - 1)
                {
                    tree.Append($"{indent}└─{subFiles[i].Name}\n");
                }
                else
                {
                    tree.Append($"{indent}├─{subFiles[i].Name}\n");
                }
            }
        }
    }
}