using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisualizationWeb.Helpers;

namespace VisualisationTests
{

   [TestClass]
    public class UnitsTest
    {   
        #region PrefixToNumber

        [TestMethod]
        public void PrefixToNr100()
        {
            double result = UnitCalc.PrefixToNumber("100");
            Assert.AreEqual(100d, result);
        }

        [TestMethod]
        public void PrefixToNr999()
        {
            double result = UnitCalc.PrefixToNumber("999");
            Assert.AreEqual(999d, result);
        }

        [TestMethod]
        public void PrefixToNr1k()
        {
            double result = UnitCalc.PrefixToNumber("1kw");
            Assert.AreEqual(1000d, result);
        }

        [TestMethod]
        public void PrefixToNr1k5()
        {
            double result = UnitCalc.PrefixToNumber("1,5kw");
            Assert.AreEqual(1500d, result);
        }

        [TestMethod]
        public void PrefixToNr1M5W()
        {
            double result = UnitCalc.PrefixToNumber("1,5mw");
            Assert.AreEqual(1500000d, result);
        }

        [TestMethod]
        public void PrefixToNr2G7M()
        {
            double result = UnitCalc.PrefixToNumber("2,7gw");
            Assert.AreEqual(2700000000d, result);
        }

        #endregion


        #region NumberToPrefix

        [TestMethod]
        public void NrToPrefix05()
        {
            string result = UnitCalc.NumberToPrefix(0.5);
            Assert.AreEqual("0,5W", result);
        }

        [TestMethod]
        public void NrToPrefix1()
        {
            string result = UnitCalc.NumberToPrefix(1);
            Assert.AreEqual("1W", result);
        }

        [TestMethod]
        public void NrToPrefix1000()
        {
            string result = UnitCalc.NumberToPrefix(1000);
            Assert.AreEqual("1kW", result);
        }

        [TestMethod]
        public void NrToPrefix1000000()
        {
            string result = UnitCalc.NumberToPrefix(1000000);
            Assert.AreEqual("1MW", result);
        }

        [TestMethod]
        public void NrToPrefix999999()
        {
            string result = UnitCalc.NumberToPrefix(999999);
            Assert.AreEqual("0,999999MW", result);
        }

        [TestMethod]
        public void NrToPrefix1000000000()
        {
            string result = UnitCalc.NumberToPrefix(1000000000);
            Assert.AreEqual("1GW", result);
        }

        [TestMethod]
        public void NrToPrefix1000000000000()
        {
            string result = UnitCalc.NumberToPrefix(1000000000000);
            Assert.AreEqual("1TW", result);
        }

        [TestMethod]
        public void NrToPrefix1500000000000()
        {
            string result = UnitCalc.NumberToPrefix(1500000000000);
            Assert.AreEqual("1,5TW", result);
        }

        #endregion
    }
}
