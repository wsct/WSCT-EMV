using NUnit.Framework;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    internal class ApplicationTransactionCounterUnitTest
    {
        [Test]
        public void Constructor()
        {
            var atc = new ApplicationTransactionCounter();

            Assert.AreEqual(0x9F36, atc.Tlv.Tag);
            Assert.AreEqual(2, atc.Tlv.Length);
            Assert.AreEqual(0, atc.Counter);
        }

        [Test]
        public void Constructor2()
        {
            var atc = new ApplicationTransactionCounter("9F36 02 F1A2".ToTlvData());

            Assert.AreEqual(0x9F36, atc.Tlv.Tag);
            Assert.AreEqual(2, atc.Tlv.Length);
            Assert.AreEqual(0xF1A2, atc.Counter);
        }
    }
}