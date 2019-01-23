// -----------------------------------------------------------------------
// <copyright file="VariablesException.cs" company="Mike Rudnikov">
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
    /// Exception class, related with variables.
    /// </summary>
    [Serializable]
    public class VariablesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariablesException"/> class.
        /// </summary>
        /// <param name="message">String with message for user.</param>
        public VariablesException(string message)
            : base(message)
        {
        }
    }
}
