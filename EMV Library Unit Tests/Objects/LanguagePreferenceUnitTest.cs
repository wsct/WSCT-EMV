using NUnit.Framework;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    internal class LanguagePreferenceUnitTest
    {
        [Test]
        public void Constructor()
        {
            var label = new LanguagePreference();

            Assert.IsNotNull(label.Tlv);
            Assert.AreEqual(0x5F2D, label.Tlv.Tag);
            Assert.AreEqual(0, label.Tlv.Length);
            Assert.AreEqual(0, label.Tlv.Value.Length);
        }

        [Test]
        public void ToStringTest()
        {
            var label = new ApplicationLabel("fren");

            Assert.AreEqual(0x50, label.Tlv.Tag);
            Assert.AreEqual(4, label.Tlv.Length);
            Assert.AreEqual("fren", label.ToString());
        }
    }
}