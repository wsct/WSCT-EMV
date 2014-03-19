using NUnit.Framework;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    public class DataObjectListUnitTest
    {
        [Test]
        public void Constructor()
        {
            var dol = new DataObjectList();

            Assert.IsNull(dol.tlv);
        }

        [Test]
        public void Parse()
        {
            const string value = "88 01 5F 2D 02";
            var dol = new DataObjectList(value.fromHexa());

            Assert.IsNotNull(dol.tlv);
            Assert.AreEqual(value, dol.tlv.value.toHexa());

            var tlvs = dol.ParseRawData("03 66 72".fromHexa());

            Assert.AreEqual(2, tlvs.Count);

            Assert.AreEqual(0x88, tlvs[0].tag);
            Assert.AreEqual(1, tlvs[0].length);
            Assert.AreEqual("03", tlvs[0].value.toHexa());

            Assert.AreEqual(0x5F2D, tlvs[1].tag);
            Assert.AreEqual(2, tlvs[1].length);
            Assert.AreEqual("66 72", tlvs[1].value.toHexa());
        }

        [Test]
        public void BuildData()
        {
            var tlvs = new[] { new TLVData("88 01 03".fromHexa()), new TLVData("5F 2D 02 66 72".fromHexa()) };
            const string value = "88 01 5F 2D 02";
            var dol = new DataObjectList(value.fromHexa());
            var result = dol.BuildData(tlvs);

            Assert.AreEqual("03 66 72".fromHexa(), result);
        }
    }
}
