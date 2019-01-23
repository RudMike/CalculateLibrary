// -----------------------------------------------------------------------
// <copyright file="SimpleExpression.cs" company="Mike Rudnikov">
//   Absolutely free for training
// </copyright>
// <author>Mike Rudnikov</author>
// -----------------------------------------------------------------------
namespace CalculateLibrary
{
    #region usings
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// Class for working with simple expressions.
    /// </summary>
    /// <remarks>Simple expression is an expression of the form "operand, operator, operand".</remarks>
    internal class SimpleExpression
    {
        /// <summary>
        /// Dictionary, which contains operation mark and method, which match this mark.
        /// </summary>
        private Dictionary<string, Operations> operation;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleExpression" /> class.
        /// </summary>
        /// <param name="operatr">Operator mark.</param>
        /// <param name="secondOperand">Second operand.</param>
        /// <param name="firstOperand">First operand.</param>
        internal SimpleExpression(string operatr, string secondOperand, string firstOperand)
        {
            this.FirstOperand = Convert.ToDouble(firstOperand);
            this.SecondOperand = Convert.ToDouble(secondOperand);
            this.Operator = operatr;

            this.operation = new Dictionary<string, Operations>
            {
                { "+", this.Sum },
                { "-", this.Substract },
                { "*", this.Multiply },
                { "/", this.Divide }
            };
        }   
        
        /// <summary>
        /// Delegate for methods of calculate expression.
        /// </summary>
        /// <returns>Returns result of corresponding operation.</returns>      
        private delegate string Operations();

        /// <summary>
        /// Gets or sets the first operand of expression (numerator of dividing).
        /// </summary>
        private double FirstOperand { get; set; }

        /// <summary>
        /// Gets or sets the second operand of expression (denominator of dividing).
        /// </summary>
        private double SecondOperand { get; set; }

        /// <summary>
        /// Gets or sets the operator +-*/.
        /// </summary>
        private string Operator { get; set; }

        /// <summary>
        /// Does the corresponding operation.
        /// </summary>
        /// <returns>Returns the result of corresponding math function.</returns>
        internal string DoMathOperation()
        {
            return this.operation[this.Operator]();
        }

        /// <summary>
        /// Sum operation.
        /// </summary>
        /// <returns>Returns result of sum.</returns>
        private string Sum()
        {
            return (this.FirstOperand + this.SecondOperand).ToString();
        }

        /// <summary>
        /// Substraction operation.
        /// </summary>
        /// <returns>Returns result of substraction.</returns>
        private string Substract()
        {
            return (this.FirstOperand - this.SecondOperand).ToString();
        }

        /// <summary>
        /// Multiplication operation.
        /// </summary>
        /// <returns>Returns result of multiplication.</returns>
        private string Multiply()
        {
            return (this.FirstOperand * this.SecondOperand).ToString();
        }

        /// <summary>
        /// Divide operation.
        /// </summary>
        /// <returns>Returns result of dividing.</returns>
        private string Divide()
        {
            if (this.SecondOperand == 0)
            {
                throw new DivideByZeroException("Exception. Trying to divide by zero.");
            }
            else
            {
                return (this.FirstOperand / this.SecondOperand).ToString();
            }
        }
    }
}
