using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearProgrammingSolver
{
    public class FileHandler
    {
        public static void WriteTable(string path, Table table)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                table.FillingTableValues(sw);
            }
        }

        public static void AdditionalTable(string path, Table table, string title)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine($"============= {title} =============");
                table.FillingTableValues(sw);

            }
        }

        public static string[] ReadAllLines(string path)
        {

        }

    }
}
