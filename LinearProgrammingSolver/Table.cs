using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LinearProgrammingSolver
{
    public sealed class Table
    {
        public List<string> VariableNames;
        public List<double> ObjectiveValues;
        public List<List<double>> ConstraintValues;
        public List<double> RHSValues;
        public string MinOrMaxQuestion;

        public Table()
        {
            VariableNames = new List<string>();
            ObjectiveValues = new List<double>();
            ConstraintValues = new List<List<double>>();
            RHSValues = new List<double>();
            MinOrMaxQuestion = "";
        }

        public void BuildTable(string objLine, List<string> constLine)
        {
            ParseObjective(objLine);
            ParseConstraint(constLine);
            GenerateVariableNames();
        }
        
        public void ParseObjective(string line)
        {
            string[] obj = line.Split(" ");

            MinOrMaxQuestion = obj[0].ToLower();

            for (int i = 1; i < obj.Length; i++)
            {
                if (obj[i].StartsWith("+") || obj[i].StartsWith("-"))
                {
                    ObjectiveValues.Add(double.Parse(obj[i]));
                } else
                {
                    Console.WriteLine("All variable must start with a + or a -");
                    break;
                }
            }
        }

        public void ParseConstraint(List<string> lines)
        {
            foreach (string line in lines)
            {
                string[] constraint = line.Split(" ");

                List<double> values = new List<double>();

                int i = 0;

                while (i < constraint.Length && !constraint[i].Contains("<=") && !constraint[i].Contains(">=") && !constraint[i].Contains("="))
                {
                    if(constraint[i].StartsWith("+") || constraint[i].StartsWith("-") )
                    {
                        values.Add(double.Parse(constraint[i]));
                    }
                    else
                    {
                        Console.WriteLine("All variable must start with a + or a -");
                        break;
                    }
                    i++;
                }
                ConstraintValues.Add(values);

                for (int j = 0; j < constraint.Length; j++)
                {
                    if (constraint[j].Contains("<=") || constraint[j].Contains(">=") || constraint[j].Contains("="))
                    {
                        
                        if (j + 1 < constraint.Length)
                        {
                            RHSValues.Add(double.Parse(constraint[j + 1]));
                        }
                        
                        else if (constraint[j].Length > 2)
                        {
                            string rhsPart = constraint[j].Substring(2); // Remove <= or >=
                          RHSValues.Add(double.Parse(rhsPart));
                        }
                        break;
                    }
                }
            }
            
        }

        public void GenerateVariableNames()
        {
            for (int i = 0; i < ObjectiveValues.Count; i++)
            {
                VariableNames.Add($"x{i+1}");
            }
        }

    }
}
