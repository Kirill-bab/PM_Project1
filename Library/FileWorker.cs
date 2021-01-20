using System;
using System.Diagnostics;
using System.IO;

namespace Library
{
    public static class FileWorker
    {
        public static void Write(string path, string data, bool append = false)
        {
            if (append == false)
            {
                File.WriteAllText(path, data);
            }
            else
            {
                using (var writer = File.AppendText(path))
                {
                    writer.Write(data + Environment.NewLine);
                    writer.Flush();
                }
            }
        }

        public static string Read(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            return File.ReadAllText(path);
        }
    }
}
