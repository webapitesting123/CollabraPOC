using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpellTheNumber;
namespace SpellTheNumber_TDD
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestZero()
        {
            int zero = 0;
            Assert.AreEqual(NumericSpelling.ToVerbal(zero), "zero");
        }
        [TestMethod]
        public void TestNegative()
        {
            int testNegative = -1095;
            Assert.AreEqual(NumericSpelling.ToVerbal(testNegative), "negative one thousand and ninety five");
        }
        [TestMethod]
        public void TestHundred()
        {
            int hundred = 100;
            Assert.AreEqual(NumericSpelling.ToVerbal(hundred), "one hundred");
        }

        [TestMethod]
        public void TestThousand()
        {
            int thousand = 10004;
            Assert.AreEqual(NumericSpelling.ToVerbal(thousand), "ten thousand and four");
        }
        [TestMethod]
        public void Testtrillion()
        {
            long Trillian = 10576749323475;
            Assert.AreEqual(NumericSpelling.ToVerbal(Trillian), "ten trillion, five hundred and seventy six billion, seven hundred and forty nine million, three hundred and twenty three thousand, four hundred and seventy five");
        }
    }
}
