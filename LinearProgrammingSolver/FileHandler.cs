using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LinearProgrammingSolver
{
    public class FileHandler
    {
        public static void WriteTable(string path, Table table)
        {
            using (StreamWriter sw = new StreamWriter(path, append: false, encoding: Encoding.UTF8))
            {
                table.FillingTableValues(sw);
            }
        }

        public static void AdditionalTable(string path, Table table, string title)
        {
            using (StreamWriter sw = new StreamWriter(path, append: true, encoding: Encoding.UTF8))
            {
                sw.WriteLine();
                sw.WriteLine($"============= {title} =============");
                table.FillingTableValues(sw);

            }
        }

        public static string[] ReadAllLines(string path)
        {
            var lines = new System.Collections.Generic.List<string>();
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                return lines.ToArray();
            }
        }

    }
}
