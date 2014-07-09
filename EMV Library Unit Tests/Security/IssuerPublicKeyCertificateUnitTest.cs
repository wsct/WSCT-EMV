using System;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    [TestFixture]
    internal class IssuerPublicKeyCertificateUnitTest
    {
        private const string AcModulus =
            "AC1DC39D337C5ABDDF3A9DF5C7A00B0F2822F81DB7E66064F1420350E5DFF4468E2FDA9532EFE369EEBCDA30C462F254B0D75FCB6882A8A473F8C1AA365DA13E6111C0170B83F6333CD516D7FCCBF9F9B6E95674893D5DBD934340564337D5F33CD65A01073332567DC2FAFDE59BAD2DAD67DDE8DD6308F1FC09D876C4F18979";

        private const string AcPrivateExponent =
            "A86E802D6862FB51F01026E08CB83BDA5B46CD5417D124E7E388DC41E09327339C958BEC91EA5BBC20DB4312923CDA34B2D231769711B60329527803202CC44AF161B4C90D1DA716B47534F2604632007181B0BC0F9ED1D40689B651E51345036D0FCE95449180E09A84830AF957B99E1055DB51E93DE844FC6D18C02965BC01";

        private const string AcPublicExponent = "010001";

        private const string IssuerModulus =
            "945F6277B6FC8D5379B4EC3DFAB5E78727F87D885FD9E7369592E3655E81BF96B2DE5F959DFF88FFCE4937742D80440635FC6412898AAAA2CAA5920C6F36318E84E8589175322292D2ACC69770A9BBA93ACD639A6F26C8494C6A8AEE2681BD89B2C4E24493A185ADDB88221B745D29B80EFCE94CA2DCA4F6BFFE5623EBB5CEAB";

        //private const string IssuerPrivateExponent =
        //    "008BB434771C693878FD0409DD620F9D6D79895449F056B08A88D3C22154973EDA9A991FCF34F1C3017A2F1E73D9D4083900E421260333F9EF467817C8757EC5B144C91D0C8B4C4171B2CF11BBB7194213928B6309BA0B980FFC465CD32A0D8B5A738A4753874C5197ACF55AACA18BC79E14FAC231E17DBDD9464DF398ABE88421";

        private const string IssuerPublicExponent = "010001";

        private const string IssuerPublicKeyCertificateBytes =
            "A0F3AAA25070058A3D6D7C9E341ECB001B2E134E3983F43070549D03A4920FF15D9DFB369CDE16EC42E9A27D0C6EFD78835C72872AD3DEE98F6198546F9BDA62D418A4752D47E5789A18B041206EB7003715ED8C501B1815F3C7783CE6738608A865BCD86EEAA9E0266F74A73542A1C585CA8F48E0F9E9D68C0F0EE18AE0E1BB";

        private const string Recovered =
            "6A0249000000071400000101018003945F6277B6FC8D5379B4EC3DFAB5E78727F87D885FD9E7369592E3655E81BF96B2DE5F959DFF88FFCE4937742D80440635FC6412898AAAA2CAA5920C6F36318E84E8589175322292D2ACC69770A9BBA93ACD639A6F26C8494C6A8AEE72472327071F26CFD51DBABDA1B6369E3DF56801BC";

        private const string Hash = "72472327071F26CFD51DBABDA1B6369E3DF56801";

        private const string LeftmostDigitsofthePublicKey =
            "945F6277B6FC8D5379B4EC3DFAB5E78727F87D885FD9E7369592E3655E81BF96B2DE5F959DFF88FFCE4937742D80440635FC6412898AAAA2CAA5920C6F36318E84E8589175322292D2ACC69770A9BBA93ACD639A6F26C8494C6A8AEE";

        [Test]
        public void GenerateSignature()
        {
            var certificate = new IssuerPublicKeyCertificate
            {
                HashAlgorithmIndicator = 0x01,
                IssuerIdentifier = "49000000".FromHexa(),
                CertificateExpirationDate = "0714".FromHexa(),
                CertificateSerialNumber = "000001".FromHexa(),
                PublicKeyAlgorithmIndicator = 1,
                IssuerPublicKey = new PublicKey(IssuerModulus, IssuerPublicExponent)
            };

            var bytes = certificate.GenerateCertificate(new PublicKey(AcModulus, AcPrivateExponent));

            Assert.AreEqual(IssuerPublicKeyCertificateBytes, bytes.ToHexa('\0'));
        }

        [Test]
        public void RecoverFromSignature()
        {
            var certificate = new IssuerPublicKeyCertificate();
            certificate.RecoverFromSignature(IssuerPublicKeyCertificateBytes.FromHexa(), new PublicKey(AcModulus, AcPublicExponent));

            Assert.AreEqual(0x6A, certificate.DataHeader);
            Assert.AreEqual(0xBC, certificate.DataTrailer);
            Assert.AreEqual(0x02, certificate.DataFormat);
            Assert.AreEqual(0x01, certificate.HashAlgorithmIndicator);
            Assert.AreEqual(Hash, certificate.HashResult.ToHexa('\0'));
            Assert.AreEqual(Recovered, certificate.Recovered.ToHexa('\0'));

            Assert.AreEqual(0x01, certificate.PublicKeyAlgorithmIndicator);
            Assert.AreEqual(0x03, certificate.PublicKeyExponentLength);
            Assert.AreEqual(0x80, certificate.PublicKeyLength);
            Assert.AreEqual("49000000".FromHexa(), certificate.IssuerIdentifier);
            Assert.AreEqual(LeftmostDigitsofthePublicKey, certificate.PublicKeyorLeftmostDigitsofthePublicKey.ToHexa('\0'));
        }
    }
}