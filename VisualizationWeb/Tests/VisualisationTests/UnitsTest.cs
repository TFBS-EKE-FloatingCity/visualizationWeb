using Application.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VisualizationTests
{
   [TestClass]
   public class UnitsTest
   {
      #region PrefixToNumber

      [TestMethod]
      public void PrefixToNr100()
      {
         double result = UnitConversion.PrefixToNumber("100");
         Assert.AreEqual(100d, result);
      }

      [TestMethod]
      public void PrefixToNr999()
      {
         double result = UnitConversion.PrefixToNumber("999");
         Assert.AreEqual(999d, result);
      }

      [TestMethod]
      public void PrefixToNr1k()
      {
         double result = UnitConversion.PrefixToNumber("1kw");
         Assert.AreEqual(1000d, result);
      }

      [TestMethod]
      public void PrefixToNr1k5()
      {
         double result = UnitConversion.PrefixToNumber("1,5kw");
         Assert.AreEqual(1500d, result);
      }

      [TestMethod]
      public void PrefixToNr1M5W()
      {
         double result = UnitConversion.PrefixToNumber("1,5mw");
         Assert.AreEqual(1500000d, result);
      }

      [TestMethod]
      public void PrefixToNr2G7M()
      {
         double result = UnitConversion.PrefixToNumber("2,7gw");
         Assert.AreEqual(2700000000d, result);
      }

      #endregion PrefixToNumber

      #region NumberToPrefix

      [TestMethod]
      public void NrToPrefix05()
      {
         string result = UnitConversion.NumberToPrefix(0.5);
         Assert.AreEqual("0,5W", result);
      }

      [TestMethod]
      public void NrToPrefix1()
      {
         string result = UnitConversion.NumberToPrefix(1);
         Assert.AreEqual("1W", result);
      }

      [TestMethod]
      public void NrToPrefix1000()
      {
         string result = UnitConversion.NumberToPrefix(1000);
         Assert.AreEqual("1kW", result);
      }

      [TestMethod]
      public void NrToPrefix1000000()
      {
         string result = UnitConversion.NumberToPrefix(1000000);
         Assert.AreEqual("1MW", result);
      }

      [TestMethod]
      public void NrToPrefix999999()
      {
         string result = UnitConversion.NumberToPrefix(999999);
         Assert.AreEqual("0,999999MW", result);
      }

      [TestMethod]
      public void NrToPrefix1000000000()
      {
         string result = UnitConversion.NumberToPrefix(1000000000);
         Assert.AreEqual("1GW", result);
      }

      [TestMethod]
      public void NrToPrefix1000000000000()
      {
         string result = UnitConversion.NumberToPrefix(1000000000000);
         Assert.AreEqual("1TW", result);
      }

      [TestMethod]
      public void NrToPrefix1500000000000()
      {
         string result = UnitConversion.NumberToPrefix(1500000000000);
         Assert.AreEqual("1,5TW", result);
      }

      #endregion NumberToPrefix
   }
}