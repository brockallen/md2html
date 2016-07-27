using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Md2Html
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                PrintUsage();
                return;
            }

            var path = args[0];
            if (Directory.Exists(path))
            {
                Run(path);
            }
            else
            {
                Console.WriteLine("{0} is not a directory");
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Pass the directory as a param");
        }

        private static void Run(string path)
        {
            var files = Directory.GetFiles(path, "*.md");
            foreach (var file in files)
            {
                if (!Path.GetFileName(file).StartsWith("~"))
                {
                    Process(file);
                }
            }

            var subs = Directory.GetDirectories(path);
            foreach (var sub in subs)
            {
                Run(sub);
            }
        }

        private static void Process(string file)
        {
            Console.WriteLine("Found: {0}", file);
            var idx = file.LastIndexOf(".");
            var newPath = file.Remove(idx, file.Length - idx) + ".htm";
            Convert(file, newPath);
        }

        static void Convert(string path, string newPath)
        {
            using (var reader = new StreamReader(File.OpenRead(path)))
            using (var writer = new StreamWriter(File.OpenWrite(newPath)))
            {
                CommonMark.CommonMarkConverter.Convert(reader, writer);
            }
        }
    }
}
