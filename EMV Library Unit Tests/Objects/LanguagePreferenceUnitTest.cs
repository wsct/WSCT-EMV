using System.Linq;
using NUnit.Framework;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    public class LanguagePreferenceUnitTest
    {
        [Test]
        public void Constructor()
        {
            var languagePreference = new LanguagePreference();

            Assert.IsNotNull(languagePreference.Tlv);
            Assert.AreEqual(0x5F2D, languagePreference.Tlv.Tag);
            Assert.AreEqual(0, languagePreference.Tlv.Length);
            Assert.AreEqual(0, languagePreference.Tlv.Value.Length);
            Assert.AreEqual("", languagePreference.Text);
            Assert.AreEqual(new string[] { }, languagePreference.Languages.ToArray());
        }

        [Test]
        public void ToStringTest()
        {
            var languagePreference = new LanguagePreference("fren");

            Assert.AreEqual(0x5F2D, languagePreference.Tlv.Tag);
            Assert.AreEqual(4, languagePreference.Tlv.Length);
            Assert.AreEqual("fren", languagePreference.Text);
            Assert.AreEqual(new[] { "fr", "en" }, languagePreference.Languages.ToArray());
        }

        [Test]
        public void SetLanguages()
        {
            var languagePreference = new LanguagePreference { Languages = new[] { "fr", "en" } };

            Assert.AreEqual(0x5F2D, languagePreference.Tlv.Tag);
            Assert.AreEqual(4, languagePreference.Tlv.Length);
            Assert.AreEqual("fren", languagePreference.Text);
            Assert.AreEqual(new[] { "fr", "en" }, languagePreference.Languages.ToArray());
        }
    }
}