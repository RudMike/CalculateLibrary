// -----------------------------------------------------------------------
// <copyright file="ICalculate.cs" company="none">
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
    /// Interface of dll-library.
    /// </summary>
    public interface ICalculate
    {
        /// <summary>
        /// Calculates string, which contains mathematical expression with variables.
        /// </summary>
        /// <param name="expression">Input expression.</param>
        /// <param name="variables">Dictionary which contains variables and value of it.</param>
        /// <returns>Returns result of expression calculate.</returns>
        string CalculateExpression(string expression, Dictionary<char, double> variables);

        /// <summary>
        /// Calculates string, which contains mathematical expression without variables.
        /// </summary>
        /// <param name="expression">Input expression.</param>
        /// <returns>Returns result of expression calculate.</returns>
        string CalculateExpression(string expression);
    }
}