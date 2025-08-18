using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearProgrammingSolver
{
    internal class Temp
    {
        // -------------------------------------------------------------
// Linear Programming: Simple Table Builder & File IO
// -------------------------------------------------------------
// Expected input file format (example: "input.txt"):
//
// Line 1: objective line
//   e.g.   max +2 +3 +3 +5 +2 +4
//           ^^^  ^  ^  ^  ^  ^  ^
//           |    coefficients for x1..x6 (signs allowed)
//           objective type: "max" or "min" (lower/upper case ok)
//
// Next lines: one constraint per line (each ends with RHS):
//   e.g.   +11 +8 +6 +14 +10 +10 <= 40
//          coefficients ----------------  op  RHS
//
// Assumptions this code makes about the input:
// - Objective line starts with a word ("max"/"min") followed by numbers with leading signs (+/-).
// - Constraint lines are numbers-with-signs until an operator token (<=, >=, or =) appears.
// - The last token in a constraint line is the RHS number.
// - Coefficients and RHS parse as doubles with the current culture settings.
//
// This program reads the file, builds a simple in-memory table of:
//   - variable names (x1..xn)
//   - objective coefficients
//   - constraint coefficient matrix
//   - right-hand side values
// Then it prints a basic table to the console and writes a formatted table
// to "<original_name>_output.txt" next to your input file.
// -------------------------------------------------------------

public class Table
    {
        // ----------------------------
        // PUBLIC FIELDS (kept simple)
        // ----------------------------

        public List<string> Variables;                 // e.g., ["x1","x2",...,"xn"]
        public List<double> ObjectiveCoefficients;     // e.g., [2, 3, 3, 5, 2, 4] for "max +2 +3 ..."
        public List<List<double>> ConstraintCoefficients; // Matrix of constraints coefficients: each inner list is one constraint row
        public List<double> RightHandSide;             // RHS values (one per constraint)
        public string ObjectiveType;                   // "max" or "min" (stored lowercase)

        // ----------------------------
        // CONSTRUCTOR
        // ----------------------------
        public Table()
        {
            // Initialize all collections and strings so the object is ready to use.
            Variables = new List<string>();
            ObjectiveCoefficients = new List<double>();
            ConstraintCoefficients = new List<List<double>>();
            RightHandSide = new List<double>();
            ObjectiveType = "";
        }

        // ----------------------------
        // HIGH-LEVEL BUILD METHOD
        // ----------------------------
        public void BuildTable(string objectiveLine, List<string> constraintLines)
        {
            // Parse the objective line to fill ObjectiveType and ObjectiveCoefficients.
            ParseObjective(objectiveLine);

            // Parse each constraint line to fill ConstraintCoefficients and RightHandSide.
            ParseConstraints(constraintLines);

            // Create variable names x1..xn based on the number of objective coefficients.
            GenerateVariableNames();
        }

        // ----------------------------
        // OBJECTIVE PARSER
        // ----------------------------
        private void ParseObjective(string line)
        {
            // Split the objective line by spaces into tokens.
            // Example: "max +2 +3 +5" -> ["max", "+2", "+3", "+5"]
            string[] parts = line.Split(' ');

            // Store the objective type as lowercase ("max" or "min").
            // parts[0] is expected to be "max" or "min".
            ObjectiveType = parts[0].ToLower();

            // Iterate from token 1 to the end; tokens that start with '+' or '-' are coefficients.
            for (int i = 1; i < parts.Length; i++)
            {
                // Only accept tokens that begin with a sign (simple heuristic for this input format).
                if (parts[i].StartsWith("+") || parts[i].StartsWith("-"))
                {
                    // Convert the token to a double and add it to the objective coefficients list.
                    // Example: "+2" -> 2.0, "-3" -> -3.0
                    ObjectiveCoefficients.Add(double.Parse(parts[i]));
                }
            }
        }

        // ----------------------------
        // CONSTRAINTS PARSER
        // ----------------------------
        private void ParseConstraints(List<string> lines)
        {
            // Process each constraint line individually.
            foreach (string line in lines)
            {
                // Split the constraint line into tokens by spaces.
                // Example: "+11 +8 +6 <= 40" -> ["+11","+8","+6","<=","40"]
                string[] parts = line.Split(' ');

                // Temporary list to accumulate the coefficients for this constraint row.
                List<double> coefficients = new List<double>();

                // Index to walk through the tokens until we hit an operator (<=, >=, =).
                int i = 0;

                // While we haven't reached the operator token, collect numeric coefficients.
                while (i < parts.Length && !parts[i].Contains("<=") && !parts[i].Contains(">=") && !parts[i].Contains("="))
                {
                    // Token that begins with '+' or '-' is treated as a numeric coefficient.
                    if (parts[i].StartsWith("+") || parts[i].StartsWith("-"))
                    {
                        // Parse token to double and add to this constraint's coefficient row.
                        coefficients.Add(double.Parse(parts[i]));
                    }
                    i++; // Move to next token
                }

                // After the loop, 'coefficients' has all LHS numeric values for this constraint.
                ConstraintCoefficients.Add(coefficients);

                // The RHS is assumed to be the very last token in the line (e.g., "40" in "<= 40").
                string rhsValue = parts[parts.Length - 1];

                // Parse RHS to double and store in RightHandSide.
                RightHandSide.Add(double.Parse(rhsValue));
            }
        }

        // ----------------------------
        // VARIABLE NAME GENERATOR
        // ----------------------------
        private void GenerateVariableNames()
        {
            // Create variable names x1, x2, ..., xN based on how many objective coefficients there are.
            // If ObjectiveCoefficients.Count == 6 -> x1..x6.
            for (int i = 0; i < ObjectiveCoefficients.Count; i++)
            {
                // i starts at 0, so add 1 for human-friendly indexing: x1, x2, ...
                // If you wanted "n1, n2, ..." you could replace $"x{i + 1}" with $"n{i + 1}".
                Variables.Add($"x{i + 1}");
            }
        }

        // ----------------------------
        // CONSOLE DISPLAY (pretty-print)
        // ----------------------------
        public void DisplayTable()
        {
            Console.WriteLine("Linear Programming Table:");
            Console.WriteLine();

            // Header row: variable names plus the RHS label.
            Console.Write("Variables:\t");
            foreach (string var in Variables)
            {
                Console.Write($"{var}\t");
            }
            Console.WriteLine("RHS");

            // Objective row: shows the objective type and its coefficients.
            Console.Write($"{ObjectiveType}:\t\t");
            foreach (double coeff in ObjectiveCoefficients)
            {
                Console.Write($"{coeff}\t");
            }
            Console.WriteLine();

            // Constraint rows: each row shows its coefficients and its RHS at the end.
            for (int i = 0; i < ConstraintCoefficients.Count; i++)
            {
                Console.Write($"C{i + 1}:\t\t");
                foreach (double coeff in ConstraintCoefficients[i])
                {
                    Console.Write($"{coeff}\t");
                }
                Console.WriteLine($"{RightHandSide[i]}");
            }
        }
    }

    public class FileHandler
    {
        // ----------------------------
        // PUBLIC FIELD
        // ----------------------------

        public string FilePath; // Full or relative path to the input text file (e.g., "input.txt")

        // ----------------------------
        // CONSTRUCTOR
        // ----------------------------
        public FileHandler(string filePath)
        {
            // Store the path so other methods can use it.
            FilePath = filePath;
        }

        // ----------------------------
        // READ: Build a Table from file contents
        // ----------------------------
        public Table ReadConstraints()
        {
            // Read all lines from the input file into a string array.
            // lines[0]     -> objective line
            // lines[1..n]  -> constraint lines
            string[] lines = File.ReadAllLines(FilePath);

            // Create a fresh Table that we'll populate and return.
            Table table = new Table();

            // First line is the objective line ("max ..." or "min ...").
            string objectiveLine = lines[0];

            // Remaining lines are constraints. We'll copy them into a List<string>.
            List<string> constraintLines = new List<string>();
            for (int i = 1; i < lines.Length; i++)
            {
                constraintLines.Add(lines[i]);
            }

            // Parse and assemble the table data from the raw lines.
            table.BuildTable(objectiveLine, constraintLines);

            // Return the fully built table.
            return table;
        }

        // ----------------------------
        // WRITE: Dump the table to an output file (simple text layout)
        // ----------------------------
        public void WriteTableToFile(Table table)
        {
            // We'll accumulate lines to write and then write them all at once.
            List<string> outputLines = new List<string>();

            // Title and blank spacer line for readability.
            outputLines.Add("Linear Programming Table");
            outputLines.Add("");

            // Build the header row: "Variables:    x1   x2  ...  RHS"
            string variableHeader = "Variables:\t";
            foreach (string var in table.Variables)
            {
                variableHeader += $"{var}\t";
            }
            variableHeader += "RHS";
            outputLines.Add(variableHeader);

            // Build the objective line with its coefficients.
            string objectiveLine = $"{table.ObjectiveType}:\t\t";
            foreach (double coeff in table.ObjectiveCoefficients)
            {
                objectiveLine += $"{coeff}\t";
            }
            outputLines.Add(objectiveLine);

            // Build each constraint line: label, its coefficients, and RHS at the end.
            for (int i = 0; i < table.ConstraintCoefficients.Count; i++)
            {
                string constraintLine = $"C{i + 1}:\t\t";
                foreach (double coeff in table.ConstraintCoefficients[i])
                {
                    constraintLine += $"{coeff}\t";
                }
                constraintLine += $"{table.RightHandSide[i]}";
                outputLines.Add(constraintLine);
            }

            // Determine output file name by appending "_output" before the ".txt" extension.
            // Example: "input.txt" -> "input_output.txt"
            File.WriteAllLines(FilePath.Replace(".txt", "_output.txt"), outputLines);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a FileHandler pointing to your input file.
            // If you want to run with a different file, change "input.txt" here.
            FileHandler fileHandler = new FileHandler("input.txt");

            // Read the file and construct the Table (objective + constraints).
            Table table = fileHandler.ReadConstraints();

            // Print a readable version of the table to the console.
            table.DisplayTable();

            // Write the same information into a sibling output file (e.g., "input_output.txt").
            fileHandler.WriteTableToFile(table);

            // Let the user know the write operation completed.
            Console.WriteLine();
            Console.WriteLine("Table written to output file.");
        }
    }

}

