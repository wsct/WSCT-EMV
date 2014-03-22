using NUnit.Framework;
using WSCT.Helpers;

namespace WSCT.EMV.Commands
{
    [TestFixture]
    class EMVReadRecordCommandUnitTest
    {
        [Test]
        public void Constructor()
        {
            var command = new EMVReadRecordCommand();

            Assert.AreEqual("00 B2 00 04", command.binaryCommand.toHexa());
        }

        [Test]
        public void Constructor2()
        {
            var command = new EMVReadRecordCommand(2, 3, 0x10);

            Assert.AreEqual("00 B2 02 1C 10", command.binaryCommand.toHexa());
        }
    }
}
