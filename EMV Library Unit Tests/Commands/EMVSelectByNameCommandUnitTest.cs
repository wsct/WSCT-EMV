using NUnit.Framework;
using WSCT.Helpers;

namespace WSCT.EMV.Commands
{
    internal class EMVSelectByNameCommandUnitTest
    {
        [Test]
        public void Constructor()
        {
            var command = new EMVSelectByNameCommand();

            Assert.AreEqual("00 A4 04 00", command.BinaryCommand.ToHexa());
        }

        [Test]
        public void Constructor2()
        {
            var command = new EMVSelectByNameCommand("A0 00 00 00 42 10 10".FromHexa());

            Assert.AreEqual("00 A4 04 00 07 A0 00 00 00 42 10 10", command.BinaryCommand.ToHexa());
        }

        [Test]
        public void Constructor3()
        {
            var command = new EMVSelectByNameCommand("A0 00 00 00 42 10 10".FromHexa(), 0x20);

            Assert.AreEqual("00 A4 04 00 07 A0 00 00 00 42 10 10 20", command.BinaryCommand.ToHexa());
        }
    }
}