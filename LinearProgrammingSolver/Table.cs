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

        private readonly double[,] _Array;
        public int Rows { get; }
        public int Columns { get; }
        public string[] RowNames { get; }
        public string[] ColNames { get; }

        public Table(int rows, int cols, string[] rownames, string[] colnames)
        {
            if (rows <= 0 || cols <= 0) throw new Exception("rows and cols must be > 0");
            if (rownames == null) throw new Exception("row names cannot be null");
            if (colnames == null) throw new Exception("column names cannot be null");
            if (rownames.Length != rows) throw new Exception("row names count must equal rows");
            if (colnames.Length != cols) throw new Exception("column names count must equal cols");

            Rows = rows;
            Columns = cols;
            _Array = new double[rows, cols];

            RowNames = (string[])rownames.Clone();
            ColNames = (string[])colnames.Clone();
        }
        public void Set(int row, int col, double value) { _Array[row, col] = value; }
        public double Get(int row, int col) { return _Array[row, col]; }

        public void FillingTableValues(TextWriter txtW)
        {
            txtW.Write("\t");

            for (int c = 0; c < ColNames.Length; c++)
            {
                if (c > 0) txtW.Write("\t");
                txtW.Write(ColNames[c]);
            }
            txtW.WriteLine();

            for (int r  = 0; r < RowNames.Length; r++)
            {
                txtW.Write(RowNames[r]);
                for (int c = 0;c < ColNames.Length; c++)
                {
                    txtW.Write("\t");
                    double round = Math.Round(_Array[r, c], 3);
                    txtW.Write(round.ToString("F3", CultureInfo.InvariantCulture));
                }
                txtW.WriteLine();
            }
        }
    }
}
