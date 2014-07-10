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
        private readonly GpoModel gpoModel;
        private readonly EmvPersonalizationModel model;

        #region >> Const

        private const string RecordModelJson = @"{
            ""sfi"": 2,
            ""index"": 3,
            ""signed"": true,
            ""fields"": [ ""5A"", ""5F25"" ]
        }";

        private const string DataJson = @"{
            ""5A"": ""4970000000"",
            ""5F25"": ""0714"",
            ""84"": ""F0 43 41 45 4E 42 01"",
            ""50"": ""ENSIBANK"",
            ""82"": [ ""sda"", ""issuer-authentication"" ],
            ""87"": ""01"",
            ""5F2D"": [ ""fr"", ""en"" ],
            ""9F11"": ""01"",
            ""9F12"": ""BANK OF ENSICAEN"",
            ""9F4D"": {
                ""sfi"": 11,
                ""size"": 64
            }
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

        private const string GpoJson = @"{
            ""dgi"": ""9104"",
            ""fields"": [ ""82"", ""94"" ]
        }";

        private const string RecordsJson = @"[
            {
                ""sfi"": 1,
                ""index"": 1,
                ""signed"": true,
                ""fields"": [ ""57"", ""5F20"", ""8C"", ""8D"" ]
            },
            {
                ""sfi"": 2,
                ""index"": 1,
                ""signed"": false,
                ""fields"": [ ""5A"", ""5F34"", ""5F25"", ""5F24"", ""5F28"" ]
            },
            {
                ""sfi"": 3,
                ""index"": 1,
                ""signed"": true,
                ""fields"": [ ""9F07"", ""8E"", ""9F0D"", ""9F0E"", ""9F0F"" ]
            },
            {
                ""sfi"": 3,
                ""index"": 2,
                ""signed"": false,
                ""fields"": [ ""93"", ""9F4A"", ""9F44"", ""9F08"" ]
            },
            {
                ""sfi"": 3,
                ""index"": 3,
                ""signed"": false,
                ""fields"": [ ""90"", ""8F"", ""9F32"", ""92"" ]
            }
        ]";

        #endregion

        public DgiBuilderUnitTest()
        {
            RegisterPcl.Register();

            recordModel = RecordModelJson.CreateFromJsonString<RecordModel>();
            data = DataJson.CreateFromJsonString<EmvPersonalizationData>();
            tagModel = TagJson.CreateFromJsonString<TagModel>();
            gpoModel = GpoJson.CreateFromJsonString<GpoModel>();

            var recordsModel = RecordsJson.CreateFromJsonString<RecordModel[]>();
            model = new EmvPersonalizationModel { Gpo = gpoModel, Records = recordsModel };
        }

        [Test]
        public void GetCommandForRecordModel()
        {
            var builder = new DgiBuilder(model, data);
            var command = builder.GetCommand(recordModel);

            Assert.AreEqual("02030E700C5A0549700000005F25020714", command);
        }

        [Test]
        public void GetCommandForTagModel()
        {
            var builder = new DgiBuilder(model, data);
            var command = builder.GetCommand(tagModel);

            Assert.AreEqual("6F3E8407F04341454E4201A5335008454E534942414E4B8701015F2D046672656E9F1101019F121042414E4B204F4620454E53494341454EBF0C059F4D020B40", command);
        }

        [Test]
        public void GetCommandForGpoModel()
        {
            var builder = new DgiBuilder(model, data);
            var command = builder.GetCommand(gpoModel);

            Assert.AreEqual("91041282024400940C080101011001010018010301", command);
        }
    }
}
