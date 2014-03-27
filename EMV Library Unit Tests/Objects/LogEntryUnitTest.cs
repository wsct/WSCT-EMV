using System;
using NUnit.Framework;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    internal class LogEntryUnitTest
    {
        [Test]
        public void Constructor()
        {
            var entry = new LogEntry();

            Assert.AreEqual(0x9F4D, entry.Tlv.Tag);
            Assert.AreEqual(2, entry.Tlv.Length);
            Assert.AreEqual(0, entry.Sfi);
            Assert.AreEqual(0, entry.CyclicFileSize);
        }

        [Test]
        public void Constructor2()
        {
            var entry = new LogEntry("9F4D 02 01 05".ToTlvData());

            Assert.AreEqual(0x9F4D, entry.Tlv.Tag);
            Assert.AreEqual(2, entry.Tlv.Length);
            Assert.AreEqual(1, entry.Sfi);
            Assert.AreEqual(5, entry.CyclicFileSize);
        }

        [Test]
        public void ConstructorException()
        {
            Assert.Throws<Exception>(() => new LogEntry("9F4D 01 01".ToTlvData()));
        }
    }
}