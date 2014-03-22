using NUnit.Framework;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    class LanguagePreferenceUnitTest
    {
        [Test]
        public void Constructor()
        {
            var label = new LanguagePreference();

            Assert.IsNotNull(label.tlv);
            Assert.AreEqual(0x5F2D, label.tlv.tag);
            Assert.AreEqual(0, label.tlv.length);
            Assert.AreEqual(0, label.tlv.value.Length);
        }

        [Test]
        public void ToStringTest()
        {
            var label = new ApplicationLabel("fren");

            Assert.AreEqual(0x50, label.tlv.tag);
            Assert.AreEqual(4, label.tlv.length);
            Assert.AreEqual("fren", label.ToString());
        }
    }
}
