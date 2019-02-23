// -----------------------------------------------------------------------
// <copyright file="CalculatingHelper.cs" company="none">
//   Absolutely free for training
// </copyright>
// <author>Mike Rudnikov</author>
// -----------------------------------------------------------------------
namespace CalculateLibrary
{
    #region usings
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    #endregion

    /// <summary>
    /// Class-helper, which contains static methods, 
    /// which are used in main class.
    /// </summary>
    internal static class CalculatingHelper
    {
        /// <summary>
        /// Method for checking input string on correct and trying to do it correct
        /// (delete tabulation symbols, line transfer, put symbol "*" and "+" where necessary, check quotes correct).
        /// </summary>
        /// <param name="expression">Expression, given to the correct form.</param>
        /// <returns>Returns expression to the correct form.</returns>
        internal static string DoExpressionFormatCorrect(string expression)
        {
            if (!IsCorrectQuotes(expression))
            {
                throw new QuotesCountException("Exception. Incorrect count of quotes.");
            }

            // change regional parameters so that in Double type separator be point
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            // deletes from input string all spaces \s+ and any tabulation symbol \\\w?
            expression = Regex.Replace(expression, @"\s+|\\\w?", string.Empty);
            expression = expression.Replace(',', '.');

            List<char> expressionChars = new List<char>();

            if (!IsCorrectFirstExpressionSymbol(expression) || !IsCorrectLastExpressionSymbol(expression))
            {
                throw new CalculateException("Exception. Expression must be started and finished with number, variable or quote.");
            }

            foreach (char ch in expression)
            {
                expressionChars.Add(ch);
            }

            if (IsLowPriorityOperator(expressionChars[0]))
            {
                expressionChars.Insert(0, '0');
            }

            for (int index = 1; index < expressionChars.Count; index++)
            {
                if (IsOpeningQuote(expressionChars[index]) &&
                   (IsVariableOrNumber(expressionChars[index - 1]) || IsClosingQuote(expressionChars[index - 1])))
                {
                    expressionChars.Insert(index, '*');
                }
                else if (IsHighPriorityOperator(expressionChars[index]) &&
                        (IsOpeningQuote(expressionChars[index - 1]) || IsClosingQuote(expressionChars[index + 1])))
                {
                    throw new CalculateException("Exception. Expression is incorrect. Check symbols '*/' nearly quotes.");
                }
                else if (IsClosingQuote(expressionChars[index]) && IsLowPriorityOperator(expressionChars[index - 1]))
                {
                    expressionChars.Insert(index, '0');
                }
                else if (IsVariableOrNumber(expressionChars[index]) && IsClosingQuote(expressionChars[index - 1]))
                {
                    expressionChars.Insert(index, '*');
                }
                else if (IsVariableOrNumber(expressionChars[index]) && IsVariable(expressionChars[index - 1]))
                {
                    expressionChars.Insert(index, '*');
                }
                else if (IsVariable(expressionChars[index]) && IsNumber(expressionChars[index - 1]))
                {
                    expressionChars.Insert(index, '*');
                }
                else if (IsLowPriorityOperator(expressionChars[index]) && IsOpeningQuote(expressionChars[index - 1]))
                {
                    expressionChars.Insert(index, '0');
                }
            }

            return string.Join(string.Empty, expressionChars.ToArray());
        }

        /// <summary>
        /// Method for checking if correctly quotes position.
        /// </summary>
        /// <param name="expression">String for checking.</param>
        /// <returns>Returns "true" if quotes position is correct, otherwise "false".</returns>
        internal static bool IsCorrectQuotes(string expression)
        {
            int quotesCount = 0;

            foreach (char currentSymbol in expression)
            {
                if (quotesCount < 0)
                {
                    return false;
                }
                else
                {
                    if (IsOpeningQuote(currentSymbol))
                    {
                        quotesCount += 1;
                    }
                    else if (IsClosingQuote(currentSymbol))
                    {
                        quotesCount -= 1;
                    }
                }
            }

            if (quotesCount != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method for determine, is the symbol is letter.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is letter, otherwise "false".</returns>
        internal static bool IsVariable(char checkSymbol)
        {
            return char.IsLetter(checkSymbol) ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is number.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is number, otherwise "false".</returns>
        internal static bool IsNumber(char checkSymbol)
        {
            return char.IsNumber(checkSymbol) ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is letter or is number.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is letter or number, otherwise "false".</returns>
        internal static bool IsVariableOrNumber(char checkSymbol)
        {
            return (char.IsNumber(checkSymbol) || char.IsLetter(checkSymbol)) ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is opening quote.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is opening quote, otherwise "false".</returns>
        internal static bool IsOpeningQuote(char checkSymbol)
        {
            return checkSymbol.Equals('(') ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is opening quote.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is opening quote, otherwise "false".</returns>
        internal static bool IsOpeningQuote(string checkSymbol)
        {
            return checkSymbol.Equals("(") ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is closing quote.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is closing quote, otherwise "false".</returns>
        internal static bool IsClosingQuote(char checkSymbol)
        {
            return checkSymbol.Equals(')') ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is closing quote.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is closing quote, otherwise "false".</returns>
        internal static bool IsClosingQuote(string checkSymbol)
        {
            return checkSymbol.Equals(")") ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is operator (+-*/).
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is operator, otherwise "false".</returns>
        internal static bool IsOperator(char checkSymbol)
        {
            string operators = "+-*/";
            return operators.IndexOf(checkSymbol) != -1 ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is low priority operator (+-).
        /// </summary>
        /// <param name="checkOperator">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is low priority operator, otherwise "false".</returns>
        internal static bool IsLowPriorityOperator(char checkOperator)
        {
            string operators = "+-";
            return operators.IndexOf(checkOperator) != -1 ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is high priority operator (*/).
        /// </summary>
        /// <param name="checkOperator">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is low priority operator, otherwise "false".</returns>
        internal static bool IsHighPriorityOperator(char checkOperator)
        {
            string operators = "*/";
            return operators.IndexOf(checkOperator) != -1 ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is point (.).
        /// </summary>
        /// <param name="checkOperator">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is point, otherwise "false".</returns>
        internal static bool IsPoint(char checkOperator)
        {
            string operators = ".";
            return operators.IndexOf(checkOperator) != -1 ? true : false;
        }

        /// <summary>
        /// Method for getting operator priority.
        /// </summary>
        /// <param name="checkOperator">Checked operator.</param>
        /// <returns>Returns operator priority: "0" if operator is "()", "1" if operator is "+-", "2" if operator is "*/", "-1" if operator is unknown.</returns>
        internal static int GetPriorityOperator(char checkOperator)
        {
            string lowPriorityOperators = "()";
            string middlePriorityOperators = "+-";
            string highPriorityOperators = "*/";
            if (lowPriorityOperators.IndexOf(checkOperator) != -1)
            {
                return 0;
            }
            else if (middlePriorityOperators.IndexOf(checkOperator) != -1)
            {
                return 1;
            }
            else if (highPriorityOperators.IndexOf(checkOperator) != -1)
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Method for getting operator priority.
        /// </summary>
        /// <param name="checkOperator">Checked operator.</param>
        /// <returns>Returns operator priority: "0" if operator is "()", "1" if operator is "+-", "2" if operator is "*/", "-1" if operator is unknown.</returns>
        internal static int GetPriorityOperator(string checkOperator)
        {
            string lowPriorityOperators = "()";
            string middlePriorityOperators = "+-";
            string highPriorityOperators = "*/";
            if (lowPriorityOperators.IndexOf(checkOperator) != -1)
            {
                return 0;
            }
            else if (middlePriorityOperators.IndexOf(checkOperator) != -1)
            {
                return 1;
            }
            else if (highPriorityOperators.IndexOf(checkOperator) != -1)
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Method for checking input string on correct first symbol.
        /// </summary>
        /// <param name="expression">Input math expression.</param>
        /// <returns>Returns true if first symbol of math expression is correct.</returns>
        internal static bool IsCorrectFirstExpressionSymbol(string expression)
        {
            char firstSymbol = expression[0];

            if (IsVariableOrNumber(firstSymbol) ||
                IsOpeningQuote(firstSymbol) ||
                IsLowPriorityOperator(firstSymbol))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method for checking input string on correct last symbol.
        /// </summary>
        /// <param name="expression">Input math expression.</param>
        /// <returns>Returns true if last symbol of math expression is correct.</returns>
        internal static bool IsCorrectLastExpressionSymbol(string expression)
        {
            char lastSymbol = expression[expression.Length - 1];

            if (IsVariableOrNumber(lastSymbol) || IsClosingQuote(lastSymbol))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
