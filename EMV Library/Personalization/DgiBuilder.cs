using System;
using System.Collections.Generic;
using System.Linq;
using WSCT.EMV.Objects;
using WSCT.EMV.Security;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Personalization
{
    /// <summary>
    /// Builder of EMV DGI based on personalization models and data.
    /// </summary>
    public class DgiBuilder
    {
        private readonly EmvPersonalizationData data;
        private readonly EmvPersonalizationModel model;
        private readonly EmvIssuerContext issuerContext;
        private readonly EmvIccContext iccContext;

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="model">Card DGI model.</param>
        /// <param name="data">Card data.</param>
        /// <param name="issuerContext"></param>
        /// <param name="iccContext"></param>
        public DgiBuilder(EmvPersonalizationModel model, EmvPersonalizationData data, EmvIssuerContext issuerContext, EmvIccContext iccContext)
        {
            this.data = data;
            this.model = model;
            this.issuerContext = issuerContext;
            this.iccContext = iccContext;
        }

        #endregion

        /// <summary>
        /// Builds UDR to be used with PUT DATA command for given record .
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        public string BuildDgi(RecordModel recordModel)
        {
            var fields = new List<TagModel>();

            if (recordModel.Fields != null)
            {
                fields.AddRange(recordModel.Fields.Select(t => new TagModel { Tag = t }));
            }

            var tagModel = new TagModel { Tag = "70", Fields = fields };
            var command = BuildDgi(tagModel);

            return String.Format("{0:X2}{1:X2}{2}{3}", recordModel.Sfi, recordModel.Index, TlvDataHelper.ToBerEncodedL((uint)command.Length / 2).ToHexa('\0'), command);
        }

        /// <summary>
        /// Builds UDR to be used with STORE DATA command for given processing options.
        /// </summary>
        /// <param name="sequenceModel"></param>
        /// <returns></returns>
        public string BuildDgi(TagValuesSequenceModel sequenceModel)
        {
            var command = String.Empty;

            if (sequenceModel.Fields != null)
            {
                command = sequenceModel.Fields
                    .Select(t => BuildDgi(new TagModel { Tag = t }))
                    .Aggregate(String.Empty, (c, s) => c + s);
            }

            return String.Format("{0}{1}{2}", sequenceModel.Dgi, TlvDataHelper.ToBerEncodedL((uint)command.Length / 2).ToHexa('\0'), command);
        }

        /// <summary>
        /// Builds UDR to be used with STORE DATA command for given FCI .
        /// </summary>
        /// <param name="fciModel"></param>
        /// <returns></returns>
        public string BuildDgi(FciModel fciModel)
        {
            var fields = new List<TagModel>();

            if (fciModel.Tags != null)
            {
                fields.AddRange(fciModel.Tags);
            }

            var tagModel = new TagModel { Tag = "A5", Fields = fields };
            var command = BuildDgi(tagModel);

            return String.Format("{0}{1}{2}", fciModel.Dgi, TlvDataHelper.ToBerEncodedL((uint)command.Length / 2).ToHexa('\0'), command);
        }

        /// <summary>
        /// Builds UDR to be used with STORE DATA command for given tag.
        /// </summary>
        /// <param name="tagModel"></param>
        /// <returns></returns>
        public string BuildDgi(TagModel tagModel)
        {
            return BuildTlv(tagModel)
                .ToByteArray()
                .ToHexa('\0');
        }

        public TlvData BuildTlv(TagModel tagModel)
        {
            var tlv = new TlvData { Tag = Convert.ToUInt32(tagModel.Tag, 16) };

            if (tagModel.Fields == null)
            {
                switch (tagModel.Tag)
                {
                    case "57": // Track 2 Equivalent Data
                        tlv.Value = data.UnmanagedAttributes[tagModel.Tag]
                            .ToObject<Track2EquivalentDataModel>()
                            .Track2EqDataFormat
                            .FromHexa();
                        break;
                    case "5F2D": // Language Preference
                        tlv.Value = data.UnmanagedAttributes[tagModel.Tag]
                            .ToObject<string[]>()
                            .Aggregate(String.Empty, (c, l) => c + l)
                            .FromString();
                        break;
                    case "82": // Application Interchange Profile
                        tlv.Value = GetAipTlv(tagModel).Value;
                        break;
                    case "8F":
                        tlv.Value = issuerContext.CaPublicKeyIndex.FromHexa();
                        break;
                    case "90": // Issuer Public Key Certificate
                        tlv.Value = issuerContext.IssuerPublicKeyCertificate.FromHexa();
                        break;
                    case "92": // Issuer Public Key Remainder
                        tlv.Value = issuerContext.IssuerPublicKeyRemainder.FromHexa();
                        break;
                    case "93": // Signed Static Authentication Data
                        tlv.Value = ComputeSignedStaticApplicationData().Value;
                        break;
                    case "94": // Application File Locator
                        tlv.Value = GetAflTlv().Value;
                        break;
                    case "9F32": // Issuer Public Key Exponent
                        tlv.Value = issuerContext.IssuerPrivateKey.PublicExponent.FromHexa();
                        break;
                    case "9F46": // ICC Public Key Certificate 
                        tlv.Value = iccContext.IccPublicKeyCertificate.FromHexa();
                        break;
                    case "9F47": // ICC Public Key Exponent 
                        tlv.Value = iccContext.IccPrivateKey.PublicExponent.FromHexa();
                        break;
                    case "9F48": // ICC Public Key Remainder 
                        tlv.Value = iccContext.IccPublicKeyRemainder.FromHexa();
                        break;
                    case "8C": // CDOL1
                    case "8D": // CDOL2
                    case "9F4F": // Log Format
                        tlv.Value = data.UnmanagedAttributes[tagModel.Tag]
                            .ToObject<TagLengthModel[]>()
                            .Aggregate(String.Empty, (c, tl) => c + String.Format("{0}{1:X2}", tl.Tag, tl.Length))
                            .FromHexa();
                        break;
                    case "9F4D": // Log Entry
                        var logModel = data.UnmanagedAttributes[tagModel.Tag]
                            .ToObject<LogModel>();
                        tlv.Value = new[] { logModel.Sfi, logModel.Size };
                        break;
                    case "50": // Application Label
                    case "5F20": // Cardholder Name
                    case "9D": // Directory Definition File Name
                    case "9F12": // Application Preferred Name
                        tlv.Value = data.UnmanagedAttributes[tagModel.Tag]
                            .ToObject<string>()
                            .FromString();
                        break;
                    default:
                        tlv.Value = data.UnmanagedAttributes[tagModel.Tag]
                            .ToObject<string>()
                            .FromHexa();
                        break;
                }
            }
            else
            {
                tlv.InnerTlvs = tagModel.Fields.Select(BuildTlv).ToList();
            }

            return tlv;
        }

        private TlvData GetAflTlv()
        {
            var aflEntries = new List<AflEntry>();

            RecordModel last = null;
            byte firstRecord = 0;
            byte signedCount = 0;

            foreach (var record in model.Records)
            {
                if (last == null)
                {
                    firstRecord = record.Index;
                    signedCount = (record.Signed ? (byte)1 : (byte)0);
                }
                else
                {
                    if (record.Sfi == last.Sfi && record.Index == last.Index + 1 && (record.Signed == last.Signed || last.Signed))
                    {
                        signedCount += (record.Signed ? (byte)1 : (byte)0);
                    }
                    else
                    {
                        aflEntries.Add(new AflEntry(last.Sfi, firstRecord, last.Index, signedCount));

                        firstRecord = record.Index;
                        signedCount = (record.Signed ? (byte)1 : (byte)0);
                    }
                }
                last = record;
            }

            if (last != null)
            {
                aflEntries.Add(new AflEntry(last.Sfi, firstRecord, last.Index, signedCount));
            }

            var afl = new ApplicationFileLocator { Files = aflEntries };

            return afl.Tlv;
        }

        private TlvData GetAipTlv(TagModel tagModel)
        {
            var aipStrings = data.UnmanagedAttributes[tagModel.Tag]
                .ToObject<string[]>();

            var aip = new ApplicationInterchangeProfile();

            foreach (var aipString in aipStrings)
            {
                switch (aipString)
                {
                    case "sda":
                        aip.Sda = true;
                        break;
                    case "dda":
                        aip.Dda = true;
                        break;
                    case "cda":
                        aip.Cda = true;
                        break;
                    case "cardholder-verification":
                        aip.CardholderVerification = true;
                        break;
                    case "terminal-risk-management":
                        aip.TerminalRiskManagement = true;
                        break;
                    case "issuer-authentication":
                        aip.IssuerAuthentication = true;
                        break;
                }
            }

            return aip.Tlv;
        }

        private TlvData ComputeSignedStaticApplicationData()
        {
            var records = model.Records.Where(r => r.Signed);
            var fields = records.SelectMany(r => r.Fields);
            var tlvData = fields.Select(f => BuildTlv(new TagModel { Tag = f }));

            IEnumerable<byte> dataEnumerable = new byte[0];
            dataEnumerable = tlvData.Aggregate(dataEnumerable, (current, tlv) => current.Concat(tlv.ToByteArray()));

            var signedStaticApplicationData = new SignedStaticApplicationData
            {
                HashAlgorithmIndicator = 0x01,
                DataAuthenticationCode = data.UnmanagedAttributes["9F45"]
                    .ToObject<string>()
                    .FromHexa(),
                StaticDataToBeAuthenticated = dataEnumerable.ToArray()
            };

            var issuerPrivateKey = new PublicKey(issuerContext.IssuerPrivateKey.Modulus, issuerContext.IssuerPrivateKey.PrivateExponent);

            // 93   Signed Static Application Data (Nca)
            return new TlvData { Tag = 0x93, Value = signedStaticApplicationData.GenerateCertificate(issuerPrivateKey) };
        }
    }
}