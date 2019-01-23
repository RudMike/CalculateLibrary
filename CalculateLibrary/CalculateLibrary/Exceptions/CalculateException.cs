// -----------------------------------------------------------------------
// <copyright file="CalculateException.cs" company="Mike Rudnikov">
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
    /// Exception class, related with incorrect input expression.
    /// </summary>
    [Serializable]
    public class CalculateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculateException"/> class.
        /// </summary>
        /// <param name="message">String with message for user.</param>
        public CalculateException(string message)
            : base(message)
        {
        }
    }
}
