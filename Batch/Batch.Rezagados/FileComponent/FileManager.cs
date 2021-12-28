using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Batch.Correo.FileComponent
{
    public static class FileManager
    {
        public static void DeleteFiles(string[] files)
        {
            foreach (var file in files)
            {
                if (FileExists(file))
                    File.Delete(file);
            }
        }

        public static bool FileExists(string file)
        {
            return File.Exists(file);
        }

        public static void CopyFile(string source, string dest)
        {
            File.Copy(source, dest, true);
        }

        public static void CreateFile(string fileName, string content)
        {
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.WriteLine(content);
            }
        }
    }
}
