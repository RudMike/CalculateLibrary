// -----------------------------------------------------------------------
// <copyright file="TryingMultiThread.cs" company="Mike Rudnikov">
//   Absolutely free for training
// </copyright>
// <author>Mike Rudnikov</author>
// -----------------------------------------------------------------------
namespace CalculateLibraryChecker
{
    #region usings
    using System;
    using System.Threading;
    using CalculateLibrary;
    #endregion

    /// <summary>
    /// Class with method for multithreading.
    /// </summary>
    internal static class TryingMultiThread
    {
        /// <summary>
        /// Method, which run 5 threads for calculating various  expressions.
        /// </summary>
        [Obsolete("Only for the checking opportunity of launch with few threads")]
        internal static void TryingThreads()
        {
            Thread thread1;
            Thread thread2;
            Thread thread3;
            Thread thread4;
            Thread thread5;

            string expression1;
            string expression2;
            string expression3;
            string expression4;
            string expression5;

            expression1 = "-1.45(-12)(12+3)"; // answer is 261
            expression2 = "(12+(12+5(7*8)))(12+3)"; // answer is 4560
            expression3 = "( 8 + 2 * 5 ) / ( 1 + 3 * 2 - 4 )"; // answer is 6
            expression4 = "( 1 + 2 ) * 4 + 3"; // answer is 15
            expression5 = "( 15 / 3 + 11 - 3 * 5 ) / 3.2 * ( 5.6 - 10 )"; // answer is -1.375 

            Calculate calc = new Calculate();

            thread1 = new Thread(() => Console.WriteLine(expression1 + " = " + calc.CalculateExpression(expression1)));
            thread2 = new Thread(() => Console.WriteLine(expression2 + " = " + calc.CalculateExpression(expression2)));
            thread3 = new Thread(() => Console.WriteLine(expression3 + " = " + calc.CalculateExpression(expression3)));
            thread4 = new Thread(() => Console.WriteLine(expression4 + " = " + calc.CalculateExpression(expression4)));
            thread5 = new Thread(() => Console.WriteLine(expression5 + " = " + calc.CalculateExpression(expression5)));

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();
        }
    }
}
