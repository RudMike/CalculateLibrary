// -----------------------------------------------------------------------
// <copyright file="Variables.cs" company="none">
//   Absolutely free for training
// </copyright>
// <author>Mike Rudnikov</author>
// -----------------------------------------------------------------------
namespace CalculateLibrary
{
    #region usings
    using System;
    #endregion

    /// <summary>
    /// Class for working with variables.
    /// </summary>
    internal class Variables
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Variables"/> class.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="variableValue">Variable value.</param>
        internal Variables(char variableName, double variableValue)
        {
            this.VariableName = variableName;
            this.VariableValue = variableValue;
            VariableCount++;
        }

        /// <summary>
        /// Gets or sets of variable count.
        /// </summary>
        internal static int VariableCount { get; set; }

        /// <summary>
        /// Gets or sets of variable name.
        /// </summary>
        internal char VariableName { get; set; }

        /// <summary>
        /// Gets or sets of variable value.
        /// </summary>
        internal double VariableValue { get; set; }
    }
}
