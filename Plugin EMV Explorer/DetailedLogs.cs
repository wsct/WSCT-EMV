using System;
using System.Drawing;
using WSCT.Core;
using WSCT.Core.APDU;
using WSCT.EMV.Card;
using WSCT.EMV.Objects;
using WSCT.Wrapper;

using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    class DetailedLogs
    {
        readonly String header;
        readonly Color highlightColor = Color.DarkBlue;
        readonly Color standardColor = Color.Black;

        readonly GUI gui;

        public TLVDictionary TlvDictionary;

        #region >> Constructors

        public DetailedLogs(GUI guiInstance)
            : this(guiInstance, "[{0,7}] Core ")
        {
        }

        public DetailedLogs(GUI guiInstance, String header)
        {
            gui = guiInstance;
            this.header = header ?? string.Empty;
        }

        #endregion

        #region >> observe*

        public void ObserveEMV(EMVApplication emv)
        {
            emv.BeforeSelectEvent += BeforeApplicationSelection;
            emv.AfterSelectEvent += AfterApplicationSelection;

            emv.BeforeGetProcessingOptionsEvent += BeforeGetProcessingOptions;
            emv.AfterGetProcessingOptionsEvent += AfterGetProcessingOptions;

            emv.BeforeReadApplicationDataEvent += BeforeReadApplicationData;
            emv.AfterReadApplicationDataEvent += AfterReadApplicationData;

            emv.BeforeGetDataEvent += BeforeGetData;
            emv.AfterGetDataEvent += AfterGetData;

            emv.BeforeReadLogFileEvent += BeforeReadLogFile;
            emv.AfterReadLogFileEvent += AfterReadLogFile;

            emv.BeforeVerifyPinEvent += BeforeVerifyPin;
            emv.AfterVerifyPinEvent += AfterVerifyPin;

            emv.BeforeInternalAuthenticateEvent += BeforeInternalAuthenticate;
            emv.AfterInternalAuthenticateEvent += AfterInternalAuthenticate;

            emv.BeforeGetChallengeEvent += BeforeGetChallenge;
            emv.AfterGetChallengeEvent += AfterGetChallenge;

            emv.BeforeGenerateAC1Event += BeforeGenerateAC1;
            emv.AfterGenerateAC1Event += AfterGenerateAC1;
        }

        public void ObservePSE(PaymentSystemEnvironment pse)
        {
            pse.BeforeSelectEvent += BeforePSESelection;
            pse.AfterSelectEvent += AfterPSESelection;

            pse.BeforeReadEvent += BeforePSERead;
            pse.AfterReadEvent += AfterPSERead;
        }

        #endregion

        #region >> CardChannelObservable Event Handlers

        public void NotifyDisconnect(ICardChannel cardChannel, Disposition disposition, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            else
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
        }

        public void NotifyReconnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, Disposition initialization, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            else
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
        }

        public void NotifyTransmit(ICardChannel cardChannel, ICardCommand cardCommand, Byte[] recvBuffer, UInt32 recvSize, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
            {

                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> RAPDU: [{0}]\n", recvBuffer.toHexa((int)recvSize)));
            }
            else
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
        }

        public void NotifyTransmit(ICardChannel cardChannel, ICardCommand cardCommand, ICardResponse cardResponse, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> RAPDU: [{0}]\n", cardResponse));
            }
            else
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
        }

        public void BeforeDisconnect(ICardChannel cardChannel, Disposition disposition)
        {
            gui.guiDetailedLogs.SelectionColor = highlightColor;
            gui.guiDetailedLogs.AppendText(String.Format(header + "disconnect({0})\n", disposition));
            gui.guiDetailedLogs.SelectionColor = standardColor;
        }

        public void BeforeReconnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, Disposition initialization)
        {
            gui.guiDetailedLogs.SelectionColor = highlightColor;
            gui.guiDetailedLogs.AppendText(String.Format(header + "reconnect({0},{1},{2})\n", shareMode, preferedProtocol, initialization));
            gui.guiDetailedLogs.SelectionColor = standardColor;
        }

        public void BeforeTransmit(ICardChannel cardChannel, ICardCommand cardCommand, Byte[] recvBuffer, UInt32 recvSize)
        {
            gui.guiDetailedLogs.SelectionColor = highlightColor;
            gui.guiDetailedLogs.AppendText(String.Format(header + "transmit({0})\n", cardCommand));
            gui.guiDetailedLogs.SelectionColor = standardColor;
        }

        public void BeforeTransmit(ICardChannel cardChannel, ICardCommand cardCommand, ICardResponse cardResponse)
        {
            gui.guiDetailedLogs.SelectionColor = highlightColor;
            gui.guiDetailedLogs.AppendText(String.Format(header + "transmit({0})\n", cardCommand));
            gui.guiDetailedLogs.SelectionColor = standardColor;
        }

        #endregion

        #region >> EMV Event Handlers

        public void BeforePSESelection(EMVDefinitionFile df)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText("P S E   S e l e c t i o n\n");
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterPSESelection(EMVDefinitionFile df)
        {
            if (df.TlvFci != null)
            {
                gui.guiDetailedLogs.AppendText("  >> TLV: " + df.TlvFci + "\n");
                foreach (TLVData tlv in df.TlvFci.getTags())
                {
                    WriteTlv(tlv.tag, tlv, TlvDictionary);
                }
            }
            else
            {
                gui.guiDetailedLogs.AppendText("  >> No PSE found\n");
            }
        }

        public void BeforePSERead(PaymentSystemEnvironment pse)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText("P S E   R e a d\n");
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterPSERead(PaymentSystemEnvironment pse)
        {
            foreach (TLVData record in pse.TlvRecords)
            {
                foreach (TLVData tlv in record.getTags())
                {
                    WriteTlv(tlv.tag, tlv, TlvDictionary);
                }
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText("PSE Applications found:\n");

            foreach (EMVApplication emvFound in pse.GetApplications())
            {
                gui.guiDetailedLogs.AppendText(String.Format("  >> Application '{0}' [ {1} ] found\n", emvFound.Aid, TlvDictionary.createInstance(emvFound.TlvFromPSE.getTag(0x50))));
            }
        }

        public void BeforeApplicationSelection(EMVDefinitionFile df)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   A I D   S e l e c t i o n   {0}\n", df.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterApplicationSelection(EMVDefinitionFile df)
        {
            if (df.TlvFci != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("  >> TLV: {0}\n", df.TlvFci));
                foreach (TLVData tlv in df.TlvFci.getTags())
                {
                    WriteTlv(tlv.tag, tlv, TlvDictionary);
                }
            }
            else
            {
                gui.guiDetailedLogs.AppendText("  >> AID not found\n");
            }
        }

        public void BeforeGetProcessingOptions(EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e t P r o c e s s i n g O p t i o n s   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterGetProcessingOptions(EMVApplication emv)
        {
            if (emv.TlvProcessingOptions == null)
                return;
            foreach (TLVData tlv in emv.TlvProcessingOptions.getTags())
            {
                WriteTlv(tlv.tag, tlv, TlvDictionary);
            }

            gui.guiDetailedLogs.AppendText(String.Format("    >> {0}\n", emv.Aip));
            foreach (ApplicationFileLocator.FileIdentification file in emv.Afl.getFiles())
            {
                gui.guiDetailedLogs.AppendText(String.Format("    >> {0}\n", file));
            }
        }

        public void BeforeReadApplicationData(EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   R e a d A p p l i c a t i o n D a t a   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterReadApplicationData(EMVApplication emv)
        {
            // dump all records
            foreach (TLVData record in emv.TlvRecords)
            {
                foreach (TLVData tlv in record.getTags())
                {
                    WriteTlv(tlv.tag, tlv, TlvDictionary);
                }
            }

            // informations about SDA
            if (emv.Sda != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("SDA: {0}\n", emv.Sda));
            }
            else
            {
                gui.guiDetailedLogs.AppendText("SDA not supported or unknown Certification Authority\n");
            }
        }

        public void BeforeGetData(EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e t D a t a   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterGetData(EMVApplication emv)
        {
            if (emv.TlvATC != null)
            {
                WriteTlv(emv.TlvATC.tag, emv.TlvATC, TlvDictionary);
            }
            if (emv.TlvLastOnlineATCRegister != null)
            {
                WriteTlv(emv.TlvLastOnlineATCRegister.tag, emv.TlvLastOnlineATCRegister, TlvDictionary);
            }
            if (emv.TlvPINTryCounter != null)
            {
                WriteTlv(emv.TlvPINTryCounter.tag, emv.TlvPINTryCounter, TlvDictionary);
            }
            if (emv.TlvLogFormat != null)
            {
                WriteTlv(emv.TlvLogFormat.tag, emv.TlvLogFormat, TlvDictionary);
            }
        }

        public void BeforeReadLogFile(EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   R e a d L o g F i l e   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterReadLogFile(EMVApplication emv)
        {
        }

        public void BeforeVerifyPin(EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   V e r i f y P i n   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterVerifyPin(EMVApplication emv)
        {
        }

        public void BeforeInternalAuthenticate(EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   I n t e r n a l A u t h e n t i c a t e   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterInternalAuthenticate(EMVApplication emv)
        {
            if (emv.TlvSignedDynamicApplicationResponse != null)
            {
                WriteTlv(emv.TlvSignedDynamicApplicationResponse.tag, emv.TlvSignedDynamicApplicationResponse, TlvDictionary);
            }

            // informations about DDA
            if (emv.Dda != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("DDA: {0}", emv.Dda));
            }
            else
            {
                gui.guiDetailedLogs.AppendText("DDA not supported or unknown Certification Authority");
            }
        }

        public void BeforeGetChallenge(EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e t C h a l l e n g e   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterGetChallenge(EMVApplication emv)
        {
            if (emv.IccChallenge != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("    >> {0}\n", emv.IccChallenge.toHexa()));
            }
        }

        public void BeforeGenerateAC1(EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e n e r a t e A C 1   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");

            gui.guiDetailedLogs.AppendText(String.Format("    >> Requested AC: {0}\n", emv.RequestedAC1Type));
            gui.guiDetailedLogs.AppendText(String.Format("    >> Unpredictable Number: {0}\n", emv.TlvGenerateAC1UnpredictableNumber));
        }

        public void AfterGenerateAC1(EMVApplication emv)
        {
            if (emv.TlvGenerateAC1Response == null)
            {
                return;
            }

            WriteTlv(emv.TlvGenerateAC1Response.tag, emv.TlvGenerateAC1Response, TlvDictionary);
            gui.guiDetailedLogs.AppendText(String.Format("    >> Cryptogram Information Data: {0}\n", emv.Cid1));
            gui.guiDetailedLogs.AppendText(String.Format("    >> Application Cryptogram Counter: {0}\n", emv.AtcFromAC1));
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="tlv"></param>
        /// <param name="tagsManager"></param>
        void WriteTlv(UInt32 tagId, TLVData tlv, TLVDictionary tagsManager)
        {
            gui.guiDetailedLogs.AppendText(String.Format("  >> TLV {0:X2}: [ {1} ]\n", tagId, tlv.getTag(tagId)));
            if (tlv.hasTag(tagId) && ((tagsManager.createInstance(tlv.getTag(tagId))) != null))
            {
                gui.guiDetailedLogs.SelectionColor = highlightColor;
                gui.guiDetailedLogs.AppendText(String.Format("     >> {0:N}: {0}\n", tagsManager.createInstance(tlv.getTag(tagId))));
                gui.guiDetailedLogs.SelectionColor = standardColor;
            }
        }
    }
}