using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace copyMyNas
{
    class Program
    {
        static void Main(string[] args)
        {
            string location = "\\\\192.168.0.198/admin/download/Transmission/download";
            string destination = "\\\\192.168.0.198/JMicron-Generic-0508-4";
            DirectoryInfo sourceDirectory = new DirectoryInfo(location);
            DirectoryInfo targetDirectory = new DirectoryInfo(destination);

            string[] filepath = Directory.GetDirectories(location);
            string pattern = @"\b(bdrip|x264)\b";

            foreach (var item in filepath)
            {
                try
                {
                    if (Regex.IsMatch(item.ToLower(), pattern))
                    {
                        DirectoryInfo slink = new DirectoryInfo(item);
                        string[] splittedPath = item.Split('\\');
                        DirectoryInfo tlink = new DirectoryInfo(destination + "\\" + splittedPath[1]);
                        CopySelected(slink, tlink);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            DeleteSelected();

            void CopySelected(DirectoryInfo source, DirectoryInfo target)
            {
                Directory.CreateDirectory(target.FullName);
                foreach (FileInfo fi in source.GetFiles())
                {
                    Console.WriteLine(@"Másol {0}\{1}", source.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                }
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                    CopySelected(diSourceSubDir, nextTargetSubDir);
                }
            }

            void DeleteSelected()
            {
                foreach (var item in filepath)
                {
                    if (Regex.IsMatch(item.ToLower(), pattern))
                    {
                        Console.WriteLine(@"Töröl " + sourceDirectory + item);
                        string[] splittedPath = item.Split('\\');
                        Directory.Delete(sourceDirectory + "/" + splittedPath[3], true);
                    }
                }
            }

            Console.WriteLine("kész");
            Console.ReadLine();

        }
    }
}
