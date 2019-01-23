// -----------------------------------------------------------------------
// <copyright file="Calculate.cs" company="Mike Rudnikov">
//   Absolutely free for training
// </copyright>
// <author>Mike Rudnikov</author>
// -----------------------------------------------------------------------
namespace CalculateLibrary
{
    #region usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CalculateLibrary;
    #endregion

    /// <summary>
    /// Main class of program.
    /// </summary>
    public class Calculate : ICalculate
    {
        /// <summary>
        /// Calculates string, which contains mathematical expression without variables.
        /// </summary>
        /// <param name="expression">Input expression. </param>
        /// <exception cref="VariablesException">Exception VariablesException when trying to send into this method expression with variables.</exception>
        /// <exception cref="QuotesCountException">Exception QuotesCountException with the incorrect count of opening/closing quotes.</exception>
        /// <exception cref="DivideByZeroException">Exception DivideByZeroException when trying to divide by zero.</exception>
        /// <exception cref="CalculateException">Exception CalculateException when input expression is incorrect.</exception>
        /// <exception cref="ArgumentNullException">Exception ArgumentNullException when input expression is empty.</exception>
        /// <returns>Returns result of expression calculate.</returns>
        public string CalculateExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException("Exception. Expression is empty.");
            }

            expression = DoExpressionFormatCorrect(expression);

            Regex regex = new Regex(@"[a-zA-Z]");

            if (regex.Match(expression).Success)
            {
                throw new VariablesException("Exception. Trying to calculate the expression with variables without declaration it.\nUse another method or change input expression.");
            }

            // calculates the expression and returns result
            return this.Calculating(expression);
        }

        /// <summary>
        /// Calculates string, which contains mathematical expression with variables.
        /// </summary>
        /// <param name="expression">Input expression.</param>
        /// <param name="variables">Dictionary which contains variables and value of it.</param>
        /// <exception cref="QuotesCountException">Exception QuotesCountException with the incorrect count of opening/closing quotes.</exception>
        /// <exception cref="DivideByZeroException">Exception DivideByZeroException when trying to divide by zero.</exception>
        /// <exception cref="CalculateException">Exception CalculateException when input expression is incorrect.</exception>
        /// <exception cref="ArgumentNullException">Exception ArgumentNullException when input expression is empty.</exception>
        /// <returns>Returns result of expression calculate.</returns>
        public string CalculateExpression(string expression, Dictionary<char, double> variables)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException("Exception. Expression is empty.");
            }        

            expression = DoExpressionFormatCorrect(expression);

            List<Variables> listOfVariables = new List<Variables>();

            for (int index = 0; index < variables.Count(); index++)
            {
                string temp;
                listOfVariables.Add(new Variables(variables.ElementAt(index).Key, variables.ElementAt(index).Value));

                // it need because expression like "-A*5", where A=-1
                // became expression "--1*5", which will be incorrect for calculating
                // for correct calculation expression must be like -(0-1)*5
                if (listOfVariables[index].VariableValue < 0)
                {
                    temp = string.Concat("(0", listOfVariables[index].VariableValue.ToString(), ")");
                }
                else
                {
                    temp = listOfVariables[index].VariableValue.ToString();
                }
                
                expression = expression.Replace(listOfVariables[index].VariableName.ToString(), temp);                   
            }

            return this.Calculating(expression);
        }

        /// <summary>
        /// Method for checking input string on correct and trying to do it correct
        /// (delete tabulation symbols, line transfer, put symbol "*" and "+" where necessary, check quotes correct).
        /// </summary>
        /// <param name="expression">Expression, given to the correct form.</param>
        /// <returns>Returns expression to the correct form.</returns>
        private static string DoExpressionFormatCorrect(string expression)
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
        private static bool IsCorrectQuotes(string expression)
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
        private static bool IsVariable(char checkSymbol)
        {
            return char.IsLetter(checkSymbol) ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is number.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is number, otherwise "false".</returns>
        private static bool IsNumber(char checkSymbol)
        {
            return char.IsNumber(checkSymbol) ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is letter or is number.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is letter or number, otherwise "false".</returns>
        private static bool IsVariableOrNumber(char checkSymbol)
        {
            return (char.IsNumber(checkSymbol) || char.IsLetter(checkSymbol)) ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is opening quote.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is opening quote, otherwise "false".</returns>
        private static bool IsOpeningQuote(char checkSymbol)
        {
            return checkSymbol.Equals('(') ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is opening quote.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is opening quote, otherwise "false".</returns>
        private static bool IsOpeningQuote(string checkSymbol)
        {
            return checkSymbol.Equals("(") ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is closing quote.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is closing quote, otherwise "false".</returns>
        private static bool IsClosingQuote(char checkSymbol)
        {
            return checkSymbol.Equals(')') ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is closing quote.
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is closing quote, otherwise "false".</returns>
        private static bool IsClosingQuote(string checkSymbol)
        {
            return checkSymbol.Equals(")") ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is operator (+-*/).
        /// </summary>
        /// <param name="checkSymbol">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is operator, otherwise "false".</returns>
        private static bool IsOperator(char checkSymbol)
        {
            string operators = "+-*/";
            return operators.IndexOf(checkSymbol) != -1 ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is low priority operator (+-).
        /// </summary>
        /// <param name="checkOperator">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is low priority operator, otherwise "false".</returns>
        private static bool IsLowPriorityOperator(char checkOperator)
        {
            string operators = "+-";
            return operators.IndexOf(checkOperator) != -1 ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is high priority operator (*/).
        /// </summary>
        /// <param name="checkOperator">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is low priority operator, otherwise "false".</returns>
        private static bool IsHighPriorityOperator(char checkOperator)
        {
            string operators = "*/";
            return operators.IndexOf(checkOperator) != -1 ? true : false;
        }

        /// <summary>
        /// Method for determine, is the symbol is point (.).
        /// </summary>
        /// <param name="checkOperator">Check symbol.</param>
        /// <returns>Returns "true" if the symbol is point, otherwise "false".</returns>
        private static bool IsPoint(char checkOperator)
        {
            string operators = ".";
            return operators.IndexOf(checkOperator) != -1 ? true : false;
        }

        /// <summary>
        /// Method for getting operator priority.
        /// </summary>
        /// <param name="checkOperator">Checked operator.</param>
        /// <returns>Returns operator priority: "0" if operator is "()", "1" if operator is "+-", "2" if operator is "*/", "-1" if operator is unknown.</returns>
        private static int GetPriorityOperator(char checkOperator)
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
        private static int GetPriorityOperator(string checkOperator)
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
        private static bool IsCorrectFirstExpressionSymbol(string expression)
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
        private static bool IsCorrectLastExpressionSymbol(string expression)
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

        /// <summary>
        /// Calculates the expression.
        /// </summary>
        /// <param name="expression">Input expression.</param>
        /// <returns>Returns result of calculated.</returns>
        private string Calculating(string expression)
        {
            SimpleExpression simpleExpession;
            Stack<string> stackOutput = new Stack<string>();
            Stack<string> stackOperators = new Stack<string>();

            // current number in which we'll be step by step write a number
            // which contains from numbers and points one after another
            StringBuilder currentNumber = new StringBuilder();

            int expressionCount = expression.Length;

            // delegate for checking number on readiness
            // if number is ready write it on output stack and clear it to write new number
            Action checkNumberOnReadinessAndAddToStack = () =>
            {               
                if (currentNumber.Length != 0)
                {
                    stackOutput.Push(currentNumber.ToString());
                    currentNumber.Clear();
                }
            };

            foreach (char currentSymbol in expression)
            {
                if (IsOpeningQuote(currentSymbol))
                {
                    checkNumberOnReadinessAndAddToStack();
                    stackOperators.Push(currentSymbol.ToString());
                }
                else if (IsClosingQuote(currentSymbol))
                {
                    checkNumberOnReadinessAndAddToStack();
                    while (!IsOpeningQuote(stackOperators.Peek()))
                    {
                        simpleExpession = new SimpleExpression(stackOperators.Pop(), stackOutput.Pop(), stackOutput.Pop());
                        stackOutput.Push(simpleExpession.DoMathOperation());
                    }

                    // push out the leftover opening quote
                    stackOperators.Pop();
                }
                else if (IsOperator(currentSymbol))
                {
                    checkNumberOnReadinessAndAddToStack();

                    if (stackOperators.Count == 0)
                    {
                        stackOperators.Push(currentSymbol.ToString());
                    }
                    else if (GetPriorityOperator(stackOperators.Peek()) == GetPriorityOperator(currentSymbol))
                    {
                        simpleExpession = new SimpleExpression(stackOperators.Pop(), stackOutput.Pop(), stackOutput.Pop());
                        stackOutput.Push(simpleExpession.DoMathOperation());                      
                        stackOperators.Push(currentSymbol.ToString());
                    }
                    else if (GetPriorityOperator(stackOperators.Peek()) > GetPriorityOperator(currentSymbol))
                    {
                        while (stackOperators.Count != 0 && !IsOpeningQuote(stackOperators.Peek()))
                        {
                            simpleExpession = new SimpleExpression(stackOperators.Pop(), stackOutput.Pop(), stackOutput.Pop());
                            stackOutput.Push(simpleExpession.DoMathOperation());
                        }

                        stackOperators.Push(currentSymbol.ToString());
                    }
                    else if (GetPriorityOperator(stackOperators.Peek()) < GetPriorityOperator(currentSymbol))
                    {
                        stackOperators.Push(currentSymbol.ToString());
                    }
                }
                else if (IsNumber(currentSymbol) || IsPoint(currentSymbol))
                {
                    currentNumber.Append(currentSymbol);
                }
                else
                {
                    throw new CalculateException("Exception. Unknown symbol " + currentSymbol.ToString());
                }
            }

            checkNumberOnReadinessAndAddToStack();
            checkNumberOnReadinessAndAddToStack = null;

            while (stackOperators.Count != 0)
            {
                simpleExpession = new SimpleExpression(stackOperators.Pop(), stackOutput.Pop(), stackOutput.Pop());
                stackOutput.Push(simpleExpession.DoMathOperation());
            }

            // leftover number in output stack is result
            return stackOutput.Peek();
        }
    }
}
