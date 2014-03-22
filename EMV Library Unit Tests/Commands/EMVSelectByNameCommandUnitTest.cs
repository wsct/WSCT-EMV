using NUnit.Framework;
using WSCT.Helpers;

namespace WSCT.EMV.Commands
{
    class EMVSelectByNameCommandUnitTest
    {
        [Test]
        public void Constructor()
        {
            var command = new EMVSelectByNameCommand();

            Assert.AreEqual("00 A4 04 00", command.binaryCommand.toHexa());
        }

        [Test]
        public void Constructor2()
        {
            var command = new EMVSelectByNameCommand("A0 00 00 00 42 10 10".fromHexa());

            Assert.AreEqual("00 A4 04 00 07 A0 00 00 00 42 10 10", command.binaryCommand.toHexa());
        }

        [Test]
        public void Constructor3()
        {
            var command = new EMVSelectByNameCommand("A0 00 00 00 42 10 10".fromHexa(), 0x20);

            Assert.AreEqual("00 A4 04 00 07 A0 00 00 00 42 10 10 20", command.binaryCommand.toHexa());
        }
    }
}
