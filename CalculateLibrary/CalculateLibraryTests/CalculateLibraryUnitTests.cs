// -----------------------------------------------------------------------
// <copyright file="CalculateLibraryUnitTests.cs" company="none">
//   Absolutely free for training
// </copyright>
// <author>Mike Rudnikov</author>
// -----------------------------------------------------------------------
namespace CalculateLibraryUnitTests
{
    #region usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CalculateLibrary;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    #endregion

    /// <summary>
    /// Test class for methods of dll-library CalculateLibrary.
    /// </summary>
    [TestClass]
    public class CalculateLibraryUnitTests
    {
        /// <summary>
        /// Testing on calculations correctness of CalculateExpression method,
        /// which doesn't receive variables.
        /// </summary>
        [TestMethod]
        public void CalculateExpressionWithoutVariables_CorrectnessOfCalculations_Success()
        {
            string inputExpression;
            string methodResult;
            string correctResult;

            Calculate calc = new Calculate();

            inputExpression = "(15/3+11-3*5)/3.2*(5,6-10)";
            correctResult = "-1.375";
            methodResult = calc.CalculateExpression(inputExpression);
            Assert.AreEqual(correctResult, methodResult);

            inputExpression = "-1.45(-12)(12+3)";
            correctResult = "261";
            methodResult = calc.CalculateExpression(inputExpression);
            Assert.AreEqual(correctResult, methodResult);

            inputExpression = "(5*(4/2+(12-2))/10)";
            correctResult = "6";
            methodResult = calc.CalculateExpression(inputExpression);
            Assert.AreEqual(correctResult, methodResult);
        }

        /// <summary>
        /// Testing on calculations correctness of CalculateExpression method
        /// with some variables.
        /// </summary>
        [TestMethod]
        public void CalculateExpressionWithVariables_CorrectnessOfCalculations_Success()
        {
            string inputExpression;
            string methodResult;
            string correctResult;
            Dictionary<char, double> variables = new Dictionary<char, double>();

            Calculate calc = new Calculate();

            variables.Add('a', 3);
            variables.Add('b', 10);
            variables.Add('A', -5);
            inputExpression = "(15/a+11-A*5)/3.2*(5,6-b)";
            correctResult = "-56.375";
            methodResult = calc.CalculateExpression(inputExpression, variables);
            Assert.AreEqual(correctResult, methodResult);
            variables.Clear();

            variables.Add('a', 3.0);
            inputExpression = "(2-5)*a/(5-a)/a-((5*a)+a)";
            correctResult = "-19.5";
            methodResult = calc.CalculateExpression(inputExpression, variables);
            Assert.AreEqual(correctResult, methodResult);
            variables.Clear();
        }

        /// <summary>
        /// Testing situation when input expression spaces and tabulation symbol.
        /// </summary>
        /// <example>2 +  3\n+   4 is equal 2+3+4</example>
        [TestMethod]
        public void DoExpressionFormatCorrect_ExpressionWithSpacesAndTabulation_Success()
        {
            string inputExpression;
            string methodResult;
            string correctResult;

            Calculate calc = new Calculate();

            inputExpression = "22 + 16 / 4 - 4 * (17 - 2 * 7 + 3) + 7 * (3 + 4)\n";
            correctResult = "51";
            methodResult = calc.CalculateExpression(inputExpression);
            Assert.AreEqual(correctResult, methodResult);
        }

        /// <summary>
        /// Testing situation when using commas instead points in numbers.
        /// </summary>
        /// <example>2 * 2,5</example>
        [TestMethod]
        public void DoExpressionFormatCorrect_ExpressionWithCommas_Success()
        {
            string inputExpression;
            string methodResult;
            string correctResult;

            Calculate calc = new Calculate();

            inputExpression = "10/2,5+2.5*4-100,0";
            correctResult = "-86";
            methodResult = calc.CalculateExpression(inputExpression);
            Assert.AreEqual(correctResult, methodResult);
        }

        /// <summary>
        /// Testing situation when operands "+-" doesn't have number nearly.
        /// </summary>
        /// <example>+2+3 is equal 0+2+3-0</example>
        [TestMethod]
        public void DoExpressionFormatCorrect_NoOperatorNearlyLowPriorityOperand_Success()
        {
            string inputExpression;
            string methodResult;
            string correctResult;

            Calculate calc = new Calculate();

            inputExpression = "(-10/2+)";
            correctResult = "-5";
            methodResult = calc.CalculateExpression(inputExpression);
            Assert.AreEqual(correctResult, methodResult);
        }

        /// <summary>
        /// Testing situation when between variables doesn't have "*".
        /// </summary>
        /// <example>abc is equal a*b*c</example>
        [TestMethod]
        public void DoExpressionFormatCorrect_NoMultiplyNearlyVariables_Success()
        {
            string inputExpression;
            string methodResult;
            string correctResult;

            Dictionary<char, double> variables = new Dictionary<char, double>
            {
                { 'a', 1 },
                { 'b', 2 }
            };

            Calculate calc = new Calculate();

            inputExpression = "5+ab";
            correctResult = "7";
            methodResult = calc.CalculateExpression(inputExpression, variables);
            Assert.AreEqual(correctResult, methodResult);
        }

        /// <summary>
        /// Testing situation when between variable and number doesn't have "*".
        /// </summary>
        /// <example>2b3 is equal 2*b*3</example>
        [TestMethod]
        public void DoExpressionFormatCorrect_NoMultiplyBetweenVariableAndOperand_Success()
        {
            string inputExpression;
            string methodResult;
            string correctResult;

            Dictionary<char, double> variables = new Dictionary<char, double>
            {
                { 'a', 2.5 }
            };

            Calculate calc = new Calculate();

            inputExpression = "2a+4";
            correctResult = "9";
            methodResult = calc.CalculateExpression(inputExpression, variables);
            Assert.AreEqual(correctResult, methodResult);
        }

        /// <summary>
        /// Testing situation when between quote and number/variable doesn't have "*".
        /// </summary>
        /// <example>2(a+1)3 is equal 2*(a+1)*3</example>
        [TestMethod]
        public void DoExpressionFormatCorrect_NoMultiplyNearlyQuotes_Success()
        {
            string inputExpression;
            string methodResult;
            string correctResult;

            Dictionary<char, double> variables = new Dictionary<char, double>
            {
                { 'a', 2.5 }
            };

            Calculate calc = new Calculate();

            inputExpression = "(2(10-5))a";
            correctResult = "25";
            methodResult = calc.CalculateExpression(inputExpression, variables);
            Assert.AreEqual(correctResult, methodResult);
        }

        /// <summary>
        /// Testing situation when input expression for overloaded method without variables is empty.
        /// </summary>
        /// <remarks>Must be exception.</remarks> 
        [TestMethod]
        public void CalculateExpressionWithoutVariables_EmptyExpression_Fail()
        {
            Calculate calc = new Calculate();

            try
            {
                calc.CalculateExpression(string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Exception. Expression is empty.", ex.ParamName.ToString());
            }
        }

        /// <summary>
        /// Testing situation when input expression has variables but calling wrong overloaded method.
        /// </summary>
        /// <remarks>Must be exception.</remarks>
        [TestMethod]
        public void CalculateExpressionWithoutVariables_UsingVariables_Fail()
        {
            Calculate calc = new Calculate();

            try
            {
                calc.CalculateExpression("a*2");
            }
            catch (VariablesException ex)
            {
                Assert.AreEqual("Exception. Trying to calculate the expression with variables without declaration it.\nUse another method or change input expression.", ex.Message);
            }
        }

        /// <summary>
        /// Testing situation when input expression for overloaded method with variables is empty.
        /// </summary>
        /// <remarks>Must be exception.</remarks> 
        [TestMethod]
        public void CalculateExpressionWithVariables_EmptyExpression_Fail()
        {
            Calculate calc = new Calculate();
            Dictionary<char, double> variables = new Dictionary<char, double>();

            try
            {
                calc.CalculateExpression(string.Empty, variables);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Exception. Expression is empty.", ex.ParamName.ToString());
            }
        }

        /// <summary>
        /// Testing situation when in method sending not all values of using variables.
        /// </summary>
        /// <remarks>Must be exception.</remarks> 
        [TestMethod]
        public void CalculateExpressionWithVariables_NotFullVariablesDeclaration_Fail()
        {
            Calculate calc = new Calculate();
            Dictionary<char, double> variables = new Dictionary<char, double>
            {
                { 'a', 1 }
            };

            try
            {
                calc.CalculateExpression("a+b", variables);
            }
            catch (CalculateException ex)
            {
                Assert.AreEqual("Exception. Unknown symbol b", ex.Message);
            }
        }

        /// <summary>
        /// Testing situation when in calculating arises dividing by zero.
        /// </summary>
        /// <remarks>Must be exception.</remarks>
        [TestMethod]
        public void Calculating_DividedByZero_Fail()
        {
            Calculate calc = new Calculate();

            try
            {
                calc.CalculateExpression("5/0");
            }
            catch (DivideByZeroException ex)
            {
                Assert.AreEqual("Exception. Trying to divide by zero.", ex.Message);
            }
        }

        /// <summary>
        /// Testing situation when input expression has incorrect quotes position and count.
        /// </summary>
        /// <example>(2+3)) or ((2+3) or )2+3( etc.</example>
        /// <remarks>Must be exception.</remarks>
        [TestMethod]
        public void DoExpressionFormatCorrect_IncorrectQuotes_Fail()
        {
            Calculate calc = new Calculate();

            try
            {
                calc.CalculateExpression(")2+3(");
            }
            catch (QuotesCountException ex)
            {
                Assert.AreEqual("Exception. Incorrect count of quotes.", ex.Message);
            }

            try
            {
                calc.CalculateExpression("(1+2))");
            }
            catch (QuotesCountException ex)
            {
                Assert.AreEqual("Exception. Incorrect count of quotes.", ex.Message);
            }

            try
            {
                calc.CalculateExpression("((1+2)");
            }
            catch (QuotesCountException ex)
            {
                Assert.AreEqual("Exception. Incorrect count of quotes.", ex.Message);
            }
        }

        /// <summary>
        /// Testing situation when input expression starting or ending with wrong symbols.
        /// </summary>
        /// <example>* 2 + 3 /</example>
        /// <remarks>Must be exception.</remarks>
        [TestMethod]
        public void DoExpressionFormatCorrect_IncorrectBorderOperators_Fail()
        {
            Calculate calc = new Calculate();

            try
            {
                calc.CalculateExpression("*4+2");
            }
            catch (CalculateException ex)
            {
                Assert.AreEqual("Exception. Expression must be started and finished with number, variable or quote.", ex.Message);
            }

            try
            {
                calc.CalculateExpression("4+2/");
            }
            catch (CalculateException ex)
            {
                Assert.AreEqual("Exception. Expression must be started and finished with number, variable or quote.", ex.Message);
            }
        }

        /// <summary>
        /// Testing situation when in input expression nearly quote symbols '*' and '/' on wrong position. 
        /// </summary>
        /// <example>(*2+3) or (2+3/)</example>
        /// <remarks>Must be exception.</remarks>
        [TestMethod]
        public void DoExpressionFormatCorrect_IncorrectOperatorsNearlyQuote_Fail()
        {
            Calculate calc = new Calculate();

            try
            {
                calc.CalculateExpression("(4+2/)");
            }
            catch (CalculateException ex)
            {
                Assert.AreEqual("Exception. Expression is incorrect. Check symbols '*/' nearly quotes.", ex.Message);
            }

            try
            {
                calc.CalculateExpression("(*4+2)");
            }
            catch (CalculateException ex)
            {
                Assert.AreEqual("Exception. Expression is incorrect. Check symbols '*/' nearly quotes.", ex.Message);
            }
        }

        #region ususing tests for private methods, can be deleted
        /*
        /// <summary>
        /// Test for private method, which checking if correctly quotes position.
        /// </summary>
         [TestMethod]
         public void IsCorrectQuotes()
         {
            Calculate calc = new Calculate();
            Type type = typeof(CalculatingHelper);
            MethodInfo method;

            method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).First(x => x.Name == "IsCorrectQuotes" && x.GetParameters().Length == 1);

            // method = type.getmethod("IsCorrectQuotes", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.AreEqual((bool)true, method.Invoke(calc, new string[] { "(1+1(1+1)1+1)1+1(1+1)(1+1)" }));
            Assert.AreEqual((bool)true, method.Invoke(calc, new string[] { "(1)1(1(1)1(1)1)" }));
            Assert.AreEqual((bool)true, method.Invoke(calc, new string[] { "(1(1)1)1(1)1(1)1" }));
         }

        /// <summary>
        /// Test for private method, which does view of input string to the correct format.
        /// </summary>
         [TestMethod]
         public void DoExpressionFormatCorrect()
         {
             string inputExpression1 = "(2-5)a(5)aa(5a)a";
             string outputExpression1 = "(2-5)*a*(5)*a*a*(5*a)*a";

             string inputExpression2 = "2ab-3(a4)2";
             string outputExpression2 = "2*a*b-3*(a*4)*2";

             string inputExpression3 = "(2+(-4)a5-b(-8a))/2,5";
             string outputExpression3 = "(2+(0-4)*a*5-b*(0-8*a))/2.5";

             Calculate calc = new Calculate();
             Type type = typeof(CalculatingHelper);
             MethodInfo method;
             method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).First(x => x.Name == "DoExpressionFormatCorrect" && x.GetParameters().Length == 1);

             Assert.AreEqual(outputExpression1, method.Invoke(calc, new string[] { inputExpression1 }));
             Assert.AreEqual(outputExpression2, method.Invoke(calc, new string[] { inputExpression2 }));
             Assert.AreEqual(outputExpression3, method.Invoke(calc, new string[] { inputExpression3 }));
         }
         */
        #endregion
    }
}
