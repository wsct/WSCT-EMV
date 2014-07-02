using NUnit.Framework;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    public class ApplicationIdentifierUnitTest
    {
        [Test]
        public void Constructor()
        {
            var aid = new ApplicationIdentifier();

            Assert.IsEmpty(aid.Rid);
            Assert.IsEmpty(aid.Pix);
        }

        [Test]
        public void ConstructorByString()
        {
            var aid = new ApplicationIdentifier("F0 43 41 45 4E 42 01");

            Assert.AreEqual(0x4F, aid.Tlv.Tag);
            Assert.AreEqual(7, aid.Tlv.Length);

            Assert.AreEqual("F0 43 41 45 4E", aid.Rid);
            Assert.AreEqual("42 01", aid.Pix);
        }

        [Test]
        public void ConstructorByTlvData()
        {
            var aid = new ApplicationIdentifier("4F 07 F0 43 41 45 4E 42 01".ToTlvData());

            Assert.AreEqual(0x4F, aid.Tlv.Tag);
            Assert.AreEqual(7, aid.Tlv.Length);

            Assert.AreEqual("F0 43 41 45 4E", aid.Rid);
            Assert.AreEqual("42 01", aid.Pix);
        }

        [Test]
        public void SetRidAndPix()
        {
            var aid = new ApplicationIdentifier { Rid = "F04341454E", Pix = "4201" };

            Assert.AreEqual(0x4F, aid.Tlv.Tag);
            Assert.AreEqual(7, aid.Tlv.Length);

            Assert.AreEqual("F0 43 41 45 4E", aid.Rid);
            Assert.AreEqual("42 01", aid.Pix);
        }
    }
}