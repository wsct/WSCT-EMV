using NUnit.Framework;
using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    [TestFixture]
    internal class StaticDataAuthenticationUnitTest
    {
        private const string SignedStaticAuthenticationData =
            "69 08 84 98 A6 40 78 29 B3 5D 5C EC CE 2D 82 0E A4 FB 2E 02 8B 66 B4 82 D8 1C 84 DF 27 FD 7F 84 06 30 70 3B F2 19 BB 0F D1 30 A8 5D D1 06 DE 5C 0F 32 0C 16 B2 6B 47 F5 97 31 9D 4B F8 43 E8 63 EA 0A C2 F7 E6 7E 49 99 1C BD F3 B1 A3 DC B9 B3 05 61 15 9B 62 CD 18 B7 E2 2F E3 CE B3 B5 DC 74 08 B9 A2 37 09 25 3C B7 49 54 51 4B B7 96 45 0B 64 64 29 E2 2C 7F DA 4B 9B C6 DF 37 A0 20 6B";

        private const string PublicKey =
            "CDD00B896DFF262422F64DFF00E0FE5D06EEC8F287CE55E6B1FB6BB958B4A22FEEB405333531676532E074C5AEA0430469822E6A7F2E6D1AB10B180ADDD717095638579379C96AFFDD0AFC7546838806C0FFB57122DD8912B367ADAAF820DB481FC4746DB70654C46A632BCE8FE6FE21CF5FBD7B8355CD2B7CEC7C2F3A6683";

        private const string Recovered =
            "6A0301DAC1BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB06EA8FF5962D399689DDDC6CF8A95BAA9682C9C3BC";
        private const string Hash = "06 EA 8F F5 96 2D 39 96 89 DD DC 6C F8 A9 5B AA 96 82 C9 C3";

        [Test]
        public void RecoverFromSignature()
        {
            var certificate = new StaticDataAuthentication();
            certificate.RecoverFromSignature(SignedStaticAuthenticationData.FromHexa(), new PublicKey(PublicKey, "03"));

            Assert.AreEqual(0x6A, certificate.DataHeader);
            Assert.AreEqual(0xBC, certificate.DataTrailer);
            Assert.AreEqual(0x03, certificate.DataFormat);
            Assert.AreEqual(0x01, certificate.HashAlgorithmIndicator);
            Assert.AreEqual(Hash.FromHexa(), certificate.HashResult);
            Assert.AreEqual(Recovered.FromHexa(), certificate.Recovered);

            Assert.AreEqual("01 DA".FromHexa(), certificate.DataAuthenticationCode);
        }
    }
}