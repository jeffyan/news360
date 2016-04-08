using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using News360App;
using System.Collections.Generic;

namespace News360AppTest
{
    [TestClass]
    public class EquationTest
    {
        [TestMethod]
        public void Should_Be_Valid()
        {
            var tests = new Dictionary<string, string>
            {
                { "2x + y = - y + 2xy^2", "2x+2y-2xy^2=0" },
                { "2x - y = - y + 2xy^2 +5", "2x-2xy^2-5=0" },
                { "23x + y = - y + 2xy^32", "23x+2y-2xy^32=0" },
                { "3x = - y + 2xy^32", "3x+y-2xy^32=0" },
                { "23x + y + 1 = - y + 2xy^32-555", "23x+2y+556-2xy^32=0" },
                { "5x+3das = fd^3 + d", "5x+3das-fd^3-d=0" }
            };

            foreach (var t in tests)
            {
                var equation = new Equation(t.Key);

                var canonicalForm = equation.Canonicalize();

                Assert.AreEqual(t.Value, canonicalForm);
            }
        }

        [TestMethod]
        public void Should_Throw_On_Empty_String()
        {
            try
            {
                var equation = new Equation("");
                var canonicalForm = equation.Canonicalize();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Empty equation", ex.Message);
            }
        }

        [TestMethod]
        public void Should_Throw_On_Invalid_Equation()
        {
            try
            {
                var equation = new Equation("x +2xcd");
                var canonicalForm = equation.Canonicalize();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Invalid equation", ex.Message);
            }
        }

        [TestMethod]
        public void Should_Throw_On_Invalid_Equation_LeftSide()
        {
            try
            {
                var equation = new Equation(" = x +2xcd");
                var canonicalForm = equation.Canonicalize();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Invalid equation", ex.Message);
            }
        }

        [TestMethod]
        public void Should_Throw_On_Invalid_Equation_RighttSide()
        {
            try
            {
                var equation = new Equation(" x +2xcd =");
                var canonicalForm = equation.Canonicalize();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Invalid equation", ex.Message);
            }
        }
    }
}
