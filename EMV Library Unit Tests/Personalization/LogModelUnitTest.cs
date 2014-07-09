using NUnit.Framework;

namespace WSCT.EMV.Personalization
{
    [TestFixture]
    public class LogModelUnitTest
    {
        [Test]
        public void LogFormat()
        {
            var logModel = new LogModel
            {
                Columns = new[]
                {
                    new LogColumnModel { Tag = "9F02", Size = 6 },
                    new LogColumnModel { Tag = "9F27", Size = 1 },
                    new LogColumnModel { Tag = "9A", Size = 3 }
                }
            };

            Assert.AreEqual("9F02069F27019A03", logModel.LogFormat);
        }
    }
}