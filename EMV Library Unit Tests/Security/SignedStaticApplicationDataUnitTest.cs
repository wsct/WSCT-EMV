using System;
using System.Linq;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    [TestFixture]
    internal class SignedStaticApplicationDataUnitTest
    {
        private const string IssuerModulus =
            "00945F6277B6FC8D5379B4EC3DFAB5E78727F87D885FD9E7369592E3655E81BF96B2DE5F959DFF88FFCE4937742D80440635FC6412898AAAA2CAA5920C6F36318E84E8589175322292D2ACC69770A9BBA93ACD639A6F26C8494C6A8AEE2681BD89B2C4E24493A185ADDB88221B745D29B80EFCE94CA2DCA4F6BFFE5623EBB5CEAB";

        private const string IssuerPrivateExponent =
            "008BB434771C693878FD0409DD620F9D6D79895449F056B08A88D3C22154973EDA9A991FCF34F1C3017A2F1E73D9D4083900E421260333F9EF467817C8757EC5B144C91D0C8B4C4171B2CF11BBB7194213928B6309BA0B980FFC465CD32A0D8B5A738A4753874C5197ACF55AACA18BC79E14FAC231E17DBDD9464DF398ABE88421";

        private const string IssuerPublicExponent = "010001";

        private const string StaticDataAuthenticationCertificateBytes =
            "82D027BF3B68D67835547008475DD074B5758DA3B002CCCC39FE099EE786776657ADCCA00448F27A1CD8951F0442A77EAE6824D1A1B7EC4F3A6031069B06E8F611EA393F3E769638A5F77E79BB3BC0E7D9547372EA1C864527E3BA0E37969C1A71EE285F06DF1668BA050493B11472771BBD29956C7C55DE13E6F40791F60EC9";

        private const string Recovered =
            "6A03012014BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBECC6B9BF722A3645482243F072D15BBBF17D44BCBC";

        private const string Hash = "ECC6B9BF722A3645482243F072D15BBBF17D44BC";

        [Test]
        public void GenerateSignature()
        {
            var certificate = new SignedStaticApplicationData
            {
                HashAlgorithmIndicator = 0x01,
                DataAuthenticationCode = "2014".FromHexa(),
                StaticDataToBeAuthenticated = "F04341454E 01".FromHexa()
            };

            var bytes = certificate.GenerateCertificate(new PublicKey(IssuerModulus, IssuerPrivateExponent));

            Console.WriteLine(bytes.ToHexa('\0'));

            Assert.AreEqual(StaticDataAuthenticationCertificateBytes, bytes.ToHexa('\0'));
        }

        [Test]
        public void RecoverFromSignature()
        {
            var certificate = new SignedStaticApplicationData();
            certificate.RecoverFromSignature(StaticDataAuthenticationCertificateBytes.FromHexa(), new PublicKey(IssuerModulus, IssuerPublicExponent));

            Assert.AreEqual(0x6A, certificate.DataHeader);
            Assert.AreEqual(0xBC, certificate.DataTrailer);
            Assert.AreEqual(0x03, certificate.DataFormat);
            Assert.AreEqual(0x01, certificate.HashAlgorithmIndicator);
            Assert.AreEqual(Hash, certificate.HashResult.ToHexa('\0'));
            Assert.AreEqual(Recovered, certificate.Recovered.ToHexa('\0'));

            Assert.AreEqual("2014", certificate.DataAuthenticationCode.ToHexa('\0'));
            Assert.AreEqual(Enumerable.Repeat((byte)0xBB, (IssuerModulus.Length - 2) / 2 - 26).ToArray(), certificate.PadPattern);
        }
    }
}