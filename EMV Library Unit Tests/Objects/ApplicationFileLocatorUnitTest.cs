using System.Linq;
using NUnit.Framework;
using WSCT.Helpers;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    public class ApplicationFileLocatorUnitTest
    {
        [Test]
        public void Constructor()
        {
            var afl = new ApplicationFileLocator();

            var files = afl.Files;
            Assert.AreEqual(0, files.Count());
            Assert.AreEqual(0x94, afl.Tlv.Tag);
        }

        [Test]
        public void Files1()
        {
            var afl = new ApplicationFileLocator("08 02 03 04".FromHexa());

            Assert.AreEqual(0x94, afl.Tlv.Tag);

            var files = afl.Files.ToList();
            Assert.AreEqual(1, files.Count());

            var file0 = files[0];
            Assert.AreEqual(1, file0.Sfi);
            Assert.AreEqual(2, file0.FirstRecord);
            Assert.AreEqual(3, file0.LastRecord);
            Assert.AreEqual(4, file0.OfflineNumberOfRecords);
        }

        [Test]
        public void Files2()
        {
            var afl = new ApplicationFileLocator("08 02 03 04 18 03 04 01".FromHexa());

            Assert.AreEqual(0x94, afl.Tlv.Tag);

            var files = afl.Files.ToList();
            Assert.AreEqual(2, files.Count());

            var file0 = files[0];
            Assert.AreEqual(1, file0.Sfi);
            Assert.AreEqual(2, file0.FirstRecord);
            Assert.AreEqual(3, file0.LastRecord);
            Assert.AreEqual(4, file0.OfflineNumberOfRecords);

            var file1 = files[1];
            Assert.AreEqual(3, file1.Sfi);
            Assert.AreEqual(3, file1.FirstRecord);
            Assert.AreEqual(4, file1.LastRecord);
            Assert.AreEqual(1, file1.OfflineNumberOfRecords);
        }

        [Test]
        public void SetFiles()
        {
            var afl = new ApplicationFileLocator { Files = new[] { new AflEntry(1, 2, 3, 4), new AflEntry(3, 3, 4, 1) } };

            Assert.AreEqual(0x94, afl.Tlv.Tag);

            var files = afl.Files.ToList();
            Assert.AreEqual(2, files.Count());

            var file0 = files[0];
            Assert.AreEqual(1, file0.Sfi);
            Assert.AreEqual(2, file0.FirstRecord);
            Assert.AreEqual(3, file0.LastRecord);
            Assert.AreEqual(4, file0.OfflineNumberOfRecords);

            var file1 = files[1];
            Assert.AreEqual(3, file1.Sfi);
            Assert.AreEqual(3, file1.FirstRecord);
            Assert.AreEqual(4, file1.LastRecord);
            Assert.AreEqual(1, file1.OfflineNumberOfRecords);
        }
    }
}