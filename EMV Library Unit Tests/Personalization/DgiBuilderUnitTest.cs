using NUnit.Framework;
using WSCT.Helpers.Desktop;
using WSCT.Helpers.Json;

namespace WSCT.EMV.Personalization
{
    [TestFixture]
    public class DgiBuilderUnitTest
    {
        private readonly RecordModel recordModel;
        private readonly EmvPersonalizationData data;
        private readonly TagModel tagModel;

        #region >> Const

        private const string ModelJson = @"{
            ""sfi"": 2,
            ""index"": 3,
            ""signed"": true,
            ""fields"": [ ""5A"", ""5F25"" ]
        }";

        private const string DataJson = @"{
            ""5A"": ""4970000000"",
            ""5F25"": ""0714"",
            ""84"": ""F0 43 41 45 4E 42 01"",
            ""50"": ""45 4E 53 49 42 41 4E 4B"",
            ""87"": ""01"",
            ""5F2D"": ""66 72 65 6E"",
            ""9F11"": ""01"",
            ""9F12"": ""42 41 4E 4B 20 4F 46 20 45 4E 53 49 43 41 45 4E"",
            ""9F4D"": ""9F 02 06 9F 27 01 9F 1A 02 5F 2A 02 9A 03 9C 01""
        }";

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
            RegisterPcl.Register();

            recordModel = ModelJson.CreateFromJsonString<RecordModel>();
            data = DataJson.CreateFromJsonString<EmvPersonalizationData>();
            tagModel = TagJson.CreateFromJsonString<TagModel>();
        }

        [Test]
        public void GetCommandForRecordModel()
        {
            var builder = new DgiBuilder(data);
            var command = builder.GetCommand(recordModel);

            Assert.AreEqual("02030E700C5A0549700000005F25020714", command);
        }

        [Test]
        public void GetCommandForTagModel()
        {
            var builder = new DgiBuilder(data);
            var command = builder.GetCommand(tagModel);

            Assert.AreEqual("6F4C8407F04341454E4201A5415008454E534942414E4B8701015F2D046672656E9F1101019F121042414E4B204F4620454E53494341454EBF0C139F4D109F02069F27019F1A025F2A029A039C01", command);
        }
    }
}
