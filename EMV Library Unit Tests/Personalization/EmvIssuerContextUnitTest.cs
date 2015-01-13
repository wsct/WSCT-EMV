using System.Linq;
using NUnit.Framework;
using WSCT.EMV.Security;
using WSCT.Helpers;

namespace WSCT.EMV.Personalization
{
    [TestFixture]
    public class EmvIssuerContextUnitTest
    {
        [Test]
        public void BuildTlvData_All()
        {
            var emvIssuerContext = new EmvIssuerContext();
            emvIssuerContext.CaPublicKeyIndex = "01";
            emvIssuerContext.IssuerPublicKeyCertificate = "0203";
            emvIssuerContext.IssuerPublicKeyRemainder = "040506";
            emvIssuerContext.IssuerPrivateKey = new PrivateKey { PublicExponent = "07080900" };
            var tlv = emvIssuerContext.BuildTlvData();

            Assert.AreEqual(4, tlv.Count);
            Assert.AreEqual("8F 01 01", tlv.First(t => t.Tag == 0x8F).ToByteArray().ToHexa());
            Assert.AreEqual("90 02 02 03", tlv.First(t => t.Tag == 0x90).ToByteArray().ToHexa());
            Assert.AreEqual("92 03 04 05 06", tlv.First(t => t.Tag == 0x92).ToByteArray().ToHexa());
            Assert.AreEqual("9F 32 04 07 08 09 00", tlv.First(t => t.Tag == 0x9F32).ToByteArray().ToHexa());
        }

        [Test]
        public void BuildTlvData_Partial()
        {
            var emvIssuerContext = new EmvIssuerContext();
            emvIssuerContext.CaPublicKeyIndex = "01";
            var tlv = emvIssuerContext.BuildTlvData();

            Assert.AreEqual(1, tlv.Count);
            Assert.AreEqual("8F 01 01", tlv[0].ToByteArray().ToHexa());
        }
    }
}