// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Mike Rudnikov">
//   Absolutely free for training
// </copyright>
// <author>Mike Rudnikov</author>
// -----------------------------------------------------------------------
namespace CalculateLibraryChecker
{
    #region usings
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using CalculateLibrary;
    #endregion

    /// <summary>
    /// Main class of program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method of program.
        /// </summary>
        /// <param name="args">Array of arguments.</param>
        private static void Main(string[] args)
        {
            // TryingMultiThread.TryingThreads();
            Console.WriteLine("Input expression\n");
            string expression = Console.ReadLine();

            string variablesList;

            string variableValueFromConsole;

            double variableValueForDictionary;

            Dictionary<char, double> variables = new Dictionary<char, double>();
            Regex regex = new Regex(@"[^a-zA-Z]");

            variablesList = regex.Replace(expression, string.Empty);

            foreach (char var in variablesList)
            {
                if (!variables.ContainsKey(var))
                {
                    while (true)
                    {
                        Console.WriteLine("Input variable " + var);
                        variableValueFromConsole = Console.ReadLine();
                        if (double.TryParse(variableValueFromConsole, out variableValueForDictionary))
                        {
                            variables.Add(var, variableValueForDictionary);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect value " + variableValueFromConsole + ",try more");
                        }
                    }
                }
            }

            Calculate calc = new Calculate();

            try
            {
                Console.WriteLine(expression + " = " + calc.CalculateExpression(expression, variables));
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (VariablesException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (CalculateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (QuotesCountException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Main(args);
        }
    }
}
