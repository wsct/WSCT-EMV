﻿using NUnit.Framework;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    internal class ApplicationIdentifierUnitTest
    {
        [Test]
        public void Constructor()
        {
            var aid = new ApplicationIdentifier();

            Assert.IsEmpty(aid.StrRid);
            Assert.IsEmpty(aid.StrPix);
        }

        [Test]
        public void ConstructorByString()
        {
            var aid = new ApplicationIdentifier("A0 00 00 00 42 10 10");

            Assert.AreEqual("A0 00 00 00 42", aid.StrRid);
            Assert.AreEqual("10 10", aid.StrPix);
            Assert.AreEqual(0x4F, aid.Tlv.Tag);
            Assert.AreEqual(7, aid.Tlv.Length);
        }

        [Test]
        public void ConstructorByTlvData()
        {
            var aid = new ApplicationIdentifier("4F 07 A0 00 00 00 42 10 10".ToTlvData());

            Assert.AreEqual("A0 00 00 00 42", aid.StrRid);
            Assert.AreEqual("10 10", aid.StrPix);
            Assert.AreEqual(0x4F, aid.Tlv.Tag);
            Assert.AreEqual(7, aid.Tlv.Length);
        }
    }
}