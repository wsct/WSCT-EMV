using System.Linq;
using NUnit.Framework;
using WSCT.Helpers;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    internal class ApplicationFileLocatorUnitTest
    {
        [Test]
        public void Constructor()
        {
            var afl = new ApplicationFileLocator();

            var files = afl.GetFiles();
            Assert.AreEqual(0, files.Count());
        }

        [Test]
        public void GetFiles()
        {
            var afl = new ApplicationFileLocator("08 02 03 04".FromHexa());

            var files = afl.GetFiles().ToList();
            Assert.AreEqual(1, files.Count());
            var file = files[0];
            Assert.AreEqual(1, file.Sfi);
            Assert.AreEqual(2, file.FirstRecord);
            Assert.AreEqual(3, file.LastRecord);
            Assert.AreEqual(4, file.OfflineNumberOfRecords);
        }
    }
}