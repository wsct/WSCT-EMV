using NUnit.Framework;
using WSCT.Helpers;

namespace WSCT.EMV.Commands
{
    [TestFixture]
    internal class EMVReadRecordCommandUnitTest
    {
        [Test]
        public void Constructor()
        {
            var command = new EMVReadRecordCommand();

            Assert.AreEqual("00 B2 00 04", command.BinaryCommand.ToHexa());
        }

        [Test]
        public void Constructor2()
        {
            var command = new EMVReadRecordCommand(2, 3, 0x10);

            Assert.AreEqual("00 B2 02 1C 10", command.BinaryCommand.ToHexa());
        }
    }
}