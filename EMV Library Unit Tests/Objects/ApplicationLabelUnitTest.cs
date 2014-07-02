using NUnit.Framework;

namespace WSCT.EMV.Objects
{
    [TestFixture]
    public class ApplicationLabelUnitTest
    {
        [Test]
        public void Constructor()
        {
            var label = new ApplicationLabel();

            Assert.IsNotNull(label.Tlv);
            Assert.AreEqual(0x50, label.Tlv.Tag);
            Assert.AreEqual(0, label.Tlv.Length);
            Assert.AreEqual(0, label.Tlv.Value.Length);
            Assert.AreEqual("", label.Text);
        }

        [Test]
        public void ConstructorString()
        {
            var label = new ApplicationLabel("ENSIBANK");

            Assert.AreEqual(0x50, label.Tlv.Tag);
            Assert.AreEqual(8, label.Tlv.Length);

            Assert.AreEqual("ENSIBANK", label.Text);
        }

        [Test]
        public void SetText()
        {
            var label = new ApplicationLabel { Text = "ENSIBANK" };

            Assert.AreEqual(0x50, label.Tlv.Tag);
            Assert.AreEqual(8, label.Tlv.Length);

            Assert.AreEqual("ENSIBANK", label.Text);
        }
    }
}