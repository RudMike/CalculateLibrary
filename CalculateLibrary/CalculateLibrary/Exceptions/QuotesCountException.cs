// -----------------------------------------------------------------------
// <copyright file="QuotesCountException.cs" company="none">
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
    /// Exception class, related with opening/closing quotes.
    /// </summary>
    [Serializable]
    public class QuotesCountException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuotesCountException"/> class.
        /// </summary>
        /// <param name="message">String with message for user.</param>
        public QuotesCountException(string message)
            : base(message)
        {
        }
    }
}
