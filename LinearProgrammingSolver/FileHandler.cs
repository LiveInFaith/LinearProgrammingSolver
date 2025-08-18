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


        public void WriteTableToFile(Table table)
        {
            List<string> lines = new List<string>();

            string VariableHeader = "Variable:\t";

            foreach(string head in table.VariableNames)
            {
                VariableHeader += $"{head}\t";
            }
            VariableHeader += "RHS";
            lines.Add(VariableHeader);

            string ObjLine = $"{table.MinOrMaxQuestion}:\t\t";
            foreach (double val in table.ObjectiveValues)
            {
                ObjLine += $"{val}\t";
            }
            lines.Add(ObjLine);

            for (int i = 0; i < table.ConstraintValues.Count; i++)
            {
                string constraintLine = $"C{i + 1}:\t\t";
                foreach (double coeff in table.ConstraintValues[i])
                {
                    constraintLine += $"{coeff}\t";
                }
                constraintLine += $"{table.RHSValues[i]}";
                lines.Add(constraintLine);
            }

            File.WriteAllLines(FilePath.Replace(".txt", "_output.txt"), lines);
        }
    }
}
