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
            MinOrMaxQuestion = " ";
        }


    }
}
