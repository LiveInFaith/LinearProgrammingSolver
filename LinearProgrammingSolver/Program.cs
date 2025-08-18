using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LinearProgrammingSolver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileHandler fh = new FileHandler("input.txt");

            Table table = fh.GetTable();

            fh.WriteTableToFile(table);

            Console.WriteLine();
            Console.WriteLine("Table written to output file.");

        }
    }
}
