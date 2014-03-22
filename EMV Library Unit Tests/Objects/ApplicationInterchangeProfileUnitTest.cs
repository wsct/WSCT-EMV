﻿using NUnit.Framework;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    class ApplicationInterchangeProfileUnitTest
    {
        [Test]
        public void Constructor()
        {
            var aip = new ApplicationInterchangeProfile();

            Assert.AreEqual(0x82, aip.tlv.tag);
            Assert.IsFalse(aip.Sda);
            Assert.IsFalse(aip.Dda);
            Assert.IsFalse(aip.Cda);
            Assert.IsFalse(aip.CardholderVerification);
            Assert.IsFalse(aip.IssuerAuthentication);
            Assert.IsFalse(aip.TerminalRiskManagement);
        }

        [Test]
        public void ConstructorByteArray()
        {
            var aip = new ApplicationInterchangeProfile(new byte[] { 0x5C, 0x00 });

            Assert.AreEqual(0x82, aip.tlv.tag);
            Assert.IsTrue(aip.Sda);
            Assert.IsFalse(aip.Dda);
            Assert.IsFalse(aip.Cda);
            Assert.IsTrue(aip.CardholderVerification);
            Assert.IsTrue(aip.IssuerAuthentication);
            Assert.IsTrue(aip.TerminalRiskManagement);
        }

        [Test]
        public void ConstructorTwoBytes()
        {
            var aip = new ApplicationInterchangeProfile(0x3C, 0x00);

            Assert.AreEqual(0x82, aip.tlv.tag);
            Assert.IsFalse(aip.Sda);
            Assert.IsTrue(aip.Dda);
            Assert.IsFalse(aip.Cda);
            Assert.IsTrue(aip.CardholderVerification);
            Assert.IsTrue(aip.IssuerAuthentication);
            Assert.IsTrue(aip.TerminalRiskManagement);
        }
    }
}
