// -----------------------------------------------------------------------
// <copyright file="Calculate.cs" company="none">
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

            expression = CalculatingHelper.DoExpressionFormatCorrect(expression);

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

            expression = CalculatingHelper.DoExpressionFormatCorrect(expression);

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
                if (CalculatingHelper.IsOpeningQuote(currentSymbol))
                {
                    checkNumberOnReadinessAndAddToStack();
                    stackOperators.Push(currentSymbol.ToString());
                }
                else if (CalculatingHelper.IsClosingQuote(currentSymbol))
                {
                    checkNumberOnReadinessAndAddToStack();
                    while (!CalculatingHelper.IsOpeningQuote(stackOperators.Peek()))
                    {
                        simpleExpession = new SimpleExpression(stackOperators.Pop(), stackOutput.Pop(), stackOutput.Pop());
                        stackOutput.Push(simpleExpession.DoMathOperation());
                    }

                    // push out the leftover opening quote
                    stackOperators.Pop();
                }
                else if (CalculatingHelper.IsOperator(currentSymbol))
                {
                    checkNumberOnReadinessAndAddToStack();

                    if (stackOperators.Count == 0)
                    {
                        stackOperators.Push(currentSymbol.ToString());
                    }
                    else if (CalculatingHelper.GetPriorityOperator(stackOperators.Peek()) == CalculatingHelper.GetPriorityOperator(currentSymbol))
                    {
                        simpleExpession = new SimpleExpression(stackOperators.Pop(), stackOutput.Pop(), stackOutput.Pop());
                        stackOutput.Push(simpleExpession.DoMathOperation());                      
                        stackOperators.Push(currentSymbol.ToString());
                    }
                    else if (CalculatingHelper.GetPriorityOperator(stackOperators.Peek()) > CalculatingHelper.GetPriorityOperator(currentSymbol))
                    {
                        while (stackOperators.Count != 0 && !CalculatingHelper.IsOpeningQuote(stackOperators.Peek()))
                        {
                            simpleExpession = new SimpleExpression(stackOperators.Pop(), stackOutput.Pop(), stackOutput.Pop());
                            stackOutput.Push(simpleExpession.DoMathOperation());
                        }

                        stackOperators.Push(currentSymbol.ToString());
                    }
                    else if (CalculatingHelper.GetPriorityOperator(stackOperators.Peek()) < CalculatingHelper.GetPriorityOperator(currentSymbol))
                    {
                        stackOperators.Push(currentSymbol.ToString());
                    }
                }
                else if (CalculatingHelper.IsNumber(currentSymbol) || CalculatingHelper.IsPoint(currentSymbol))
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
