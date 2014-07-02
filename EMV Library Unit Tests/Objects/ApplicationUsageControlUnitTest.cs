using NUnit.Framework;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    public class ApplicationUsageControlUnitTest
    {
        [Test]
        public void Constructor()
        {
            var aip = new ApplicationUsageControl();

            Assert.AreEqual(0x9F07, aip.Tlv.Tag);

            Assert.IsFalse(aip.Atm);
            Assert.IsFalse(aip.DomesticCash);
            Assert.IsFalse(aip.DomesticCashback);
            Assert.IsFalse(aip.DomesticGoods);
            Assert.IsFalse(aip.DomesticServices);
            Assert.IsFalse(aip.InternationalCash);
            Assert.IsFalse(aip.InternationalCashBack);
            Assert.IsFalse(aip.InternationalGoods);
            Assert.IsFalse(aip.InternationalServices);
            Assert.IsFalse(aip.OtherThanAtm);
        }

        [Test]
        public void SetViaProperties()
        {
            var aip = new ApplicationUsageControl
            {
                Atm = true,
                DomesticCash = true,
                DomesticCashback = false,
                DomesticGoods = true,
                DomesticServices = true,
                InternationalCash = true,
                InternationalCashBack = false,
                InternationalGoods = true,
                InternationalServices = true,
                OtherThanAtm = false
            };

            Assert.AreEqual(0x9F07, aip.Tlv.Tag);

            Assert.IsTrue(aip.Atm);
            Assert.IsTrue(aip.DomesticCash);
            Assert.IsFalse(aip.DomesticCashback);
            Assert.IsTrue(aip.DomesticGoods);
            Assert.IsTrue(aip.DomesticServices);
            Assert.IsTrue(aip.InternationalCash);
            Assert.IsFalse(aip.InternationalCashBack);
            Assert.IsTrue(aip.InternationalGoods);
            Assert.IsTrue(aip.InternationalServices);
            Assert.IsFalse(aip.OtherThanAtm);
        }
    }
}