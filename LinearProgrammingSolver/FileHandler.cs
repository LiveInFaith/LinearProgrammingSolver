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
        public string FilePath;
        public FileHandler(string FP)
        {
            FilePath = FP;
        }

        public Table GetTable()
        {
            Table table = new Table();

            string[] lines = File.ReadAllLines(FilePath);

            string objLine = lines[0];

            List<string> constraintLines = new List<string>();

            for (int i = 1; i < lines.Length; i++)
            {
                constraintLines.Add(lines[i]);
            }

            table.BuildTable(objLine, constraintLines);

            return table;
        }


         


    }
}
