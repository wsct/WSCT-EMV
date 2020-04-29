using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using WSCT.Helpers.Json;

namespace WSCT.EMV.Personalization
{
    [TestFixture]
    public class DgiBuilderUnitTest
    {
        private readonly EmvPersonalizationData data;
        private readonly EmvPersonalizationModel model;
        private readonly TagModel tagModel;

        #region >> Const

        private const string TagJson = @"{
            ""tag"": ""6F"",
            ""fields"": [
                { ""tag"": ""84"" },
                {
                    ""tag"": ""A5"",
                    ""fields"": [
                        { ""tag"": ""50"" },
                        { ""tag"": ""87"" },
                        { ""tag"": ""5F2D"" },
                        { ""tag"": ""9F11"" },
                        { ""tag"": ""9F12"" },
                        {
                            ""tag"": ""BF0C"",
                            ""fields"": [
                                { ""tag"": ""9F4D"" }
                            ]
                        }
                    ]
                }
            ]
        }";

        #endregion

        public DgiBuilderUnitTest()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            model = Path.Combine(path, @"Personalization\emv-card-model.json").CreateFromJsonFile<EmvPersonalizationModel>();
            data = Path.Combine(path, @"Personalization\emv-card-data.json").CreateFromJsonFile<EmvPersonalizationData>();
            tagModel = TagJson.CreateFromJsonString<TagModel>();
        }

        [Test]
        public void GetCommandForFciModel()
        {
            var builder = new DgiBuilder(model, data, null /*TODO*/, null /*TODO*/);
            var command = builder.BuildDgi(model.Fci);

            Assert.AreEqual("910235A5335008454E534942414E4B8701015F2D046672656E9F1101019F121042414E4B204F4620454E53494341454EBF0C059F4D020B05", command);
        }

        [Test]
        public void GetCommandForValuesSequenceModel()
        {
            var builder = new DgiBuilder(model, data, null /*TODO*/, null /*TODO*/);
            var command = builder.BuildDgi(model.Gpo);

            Assert.AreEqual("91041282024400940C080101011001010018010301", command);
        }

        [Test]
        public void GetCommandForRecordModel()
        {
            var builder = new DgiBuilder(model, data, null /*TODO*/, null /*TODO*/);
            var command = builder.BuildDgi(model.Records.Skip(1).First());

            Assert.AreEqual("020121701F5A0898765432105432105F3401015F25031407015F24031806305F28020250", command);
        }

        [Test]
        public void GetCommandForTagModel()
        {
            var builder = new DgiBuilder(model, data, null /*TODO*/, null /*TODO*/);
            var command = builder.BuildDgi(tagModel);

            Assert.AreEqual("6F3E8407F04341454E4201A5335008454E534942414E4B8701015F2D046672656E9F1101019F121042414E4B204F4620454E53494341454EBF0C059F4D020B05", command);
        }
    }
}