using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        const string prepend_html = @"
<!DOCTYPE html>
<html><head>
<style>
body {
    width:800px;
    margin:auto;
    padding:10px;
    font-sie:14pt;  
    line-height:140%;
    font-family:sans-serif;
}
pre {
    padding:8px;
    line-height:140%;
    background-color:#ddd;
}
code {
    padding:2px;
    background-color:#ddd;
}
</style>
</head><body>
";
        const string prepend_md = @"
![logo](http://foo/logo.png 'Name')
---
";
        const string append_html = @"
</body></html>
";
        const string append_md = @"
---
Copyright © 2016 [Company Name](https://www.url.com). All rights reserved.
";

        static void Convert(string path, string newPath)
        {
            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }

            var html = "";

            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                var md = prepend_md + reader.ReadToEnd() + append_md;
                html = prepend_html + CommonMark.CommonMarkConverter.Convert(md) + append_html;
            }

            using (var newFile = File.OpenWrite(newPath))
            using (var writer = new StreamWriter(newFile, Encoding.UTF8))
            {
                writer.Write(html);
                writer.Flush();
            }
        }
    }
}
