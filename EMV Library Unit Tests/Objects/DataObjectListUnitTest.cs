using NUnit.Framework;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    public class DataObjectListUnitTest
    {
        [Test]
        public void BuildData()
        {
            var tlvs = new[] { new TlvData("88 01 03".FromHexa()), new TlvData("5F 2D 02 66 72".FromHexa()) };
            const string value = "88 01 5F 2D 02";
            var dol = new DataObjectList(value.FromHexa());
            var result = dol.BuildData(tlvs);

            Assert.AreEqual("03 66 72".FromHexa(), result);
        }

        [Test]
        public void Constructor()
        {
            var dol = new DataObjectList();

            Assert.IsNull(dol.Tlv);
        }

        [Test]
        public void Parse()
        {
            const string value = "88 01 5F 2D 02";
            var dol = new DataObjectList(value.FromHexa());

            Assert.IsNotNull(dol.Tlv);
            Assert.AreEqual(value, dol.Tlv.Value.ToHexa());

            var tlvs = dol.ParseRawData("03 66 72".FromHexa());

            Assert.AreEqual(2, tlvs.Count);

            Assert.AreEqual(0x88, tlvs[0].Tag);
            Assert.AreEqual(1, tlvs[0].Length);
            Assert.AreEqual("03", tlvs[0].Value.ToHexa());

            Assert.AreEqual(0x5F2D, tlvs[1].Tag);
            Assert.AreEqual(2, tlvs[1].Length);
            Assert.AreEqual("66 72", tlvs[1].Value.ToHexa());
        }
    }
}