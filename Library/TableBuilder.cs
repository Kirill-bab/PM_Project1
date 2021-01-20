using System;

namespace Library
{
    public static class TableBuilder
    {
        private static int _tableWidth = 73;

        public static void DrawTable(string[] headers, string[][] input)
        {
            PrintLine();
            PrintRow(headers);
            PrintLine();
            foreach (var row in input)
            {
                PrintRow(row);
            }
            PrintLine();
        }

        private static void PrintLine()
        {
            Console.WriteLine(new string('-', _tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (_tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        public static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
