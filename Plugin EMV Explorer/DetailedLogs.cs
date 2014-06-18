using System;
using System.Drawing;
using WSCT.Core;
using WSCT.Core.APDU;
using WSCT.EMV.Card;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;
using WSCT.Wrapper;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    internal class DetailedLogs
    {
        private readonly Gui gui;
        private readonly string header;
        private readonly Color highlightColor = Color.DarkBlue;
        private readonly Color standardColor = Color.Black;

        public TlvDictionary TlvDictionary;

        #region >> Constructors

        public DetailedLogs(Gui guiInstance)
            : this(guiInstance, "[{0,7}] Core ")
        {
        }

        public DetailedLogs(Gui guiInstance, string header)
        {
            gui = guiInstance;
            this.header = header ?? String.Empty;
        }

        #endregion

        #region >> observe*

        public void ObserveEmv(EmvApplication emv)
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

            emv.BeforeGenerateAC1Event += BeforeGenerateAc1;
            emv.AfterGenerateAC1Event += AfterGenerateAc1;
        }

        public void ObservePse(PaymentSystemEnvironment pse)
        {
            pse.BeforeSelectEvent += BeforePseSelection;
            pse.AfterSelectEvent += AfterPseSelection;

            pse.BeforeReadEvent += BeforePseRead;
            pse.AfterReadEvent += AfterPseRead;
        }

        #endregion

        #region >> CardChannelObservable Event Handlers

        public void NotifyDisconnect(ICardChannel cardChannel, Disposition disposition, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.Success)
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            }
            else
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            }
        }

        public void NotifyReconnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, Disposition initialization, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.Success)
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            }
            else
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            }
        }

        public void NotifyTransmit(ICardChannel cardChannel, ICardCommand cardCommand, byte[] recvBuffer, UInt32 recvSize, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.Success)
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> RAPDU: [{0}]\n", recvBuffer.ToHexa((int)recvSize)));
            }
            else
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            }
        }

        public void NotifyTransmit(ICardChannel cardChannel, ICardCommand cardCommand, ICardResponse cardResponse, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.Success)
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> RAPDU: [{0}]\n", cardResponse));
            }
            else
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            }
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

        public void BeforeTransmit(ICardChannel cardChannel, ICardCommand cardCommand, byte[] recvBuffer, UInt32 recvSize)
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

        public void BeforePseSelection(Object sender, EmvEventArgs eventArgs)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText("P S E   S e l e c t i o n\n");
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterPseSelection(Object sender, EmvEventArgs eventArgs)
        {
            var df = sender as EmvDefinitionFile;
            if (df == null)
            {
                throw new ArgumentException("sender is not an EmvDefinitionFile");
            }

            if (df.TlvFci != null)
            {
                gui.guiDetailedLogs.AppendText("  >> TLV: " + df.TlvFci + "\n");
                foreach (TlvData tlv in df.TlvFci.GetTags())
                {
                    WriteTlv(tlv.Tag, tlv, TlvDictionary);
                }
            }
            else
            {
                gui.guiDetailedLogs.AppendText("  >> No PSE found\n");
            }
        }

        public void BeforePseRead(Object sender, EmvEventArgs eventArgs)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText("P S E   R e a d\n");
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterPseRead(Object sender, EmvEventArgs eventArgs)
        {
            var pse = sender as PaymentSystemEnvironment;
            if (pse == null)
            {
                throw new ArgumentException("sender is not a PaymentSystemEnvironment");
            }

            foreach (var record in pse.TlvRecords)
            {
                foreach (TlvData tlv in record.GetTags())
                {
                    WriteTlv(tlv.Tag, tlv, TlvDictionary);
                }
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText("PSE Applications found:\n");

            foreach (var emvFound in pse.GetApplications())
            {
                gui.guiDetailedLogs.AppendText(String.Format("  >> Application '{0}' [ {1} ] found\n", emvFound.Aid, TlvDictionary.CreateInstance(emvFound.TlvFromPSE.GetTag(0x50))));
            }
        }

        public void BeforeApplicationSelection(Object sender, EmvEventArgs eventArgs)
        {
            var df = sender as EmvDefinitionFile;
            if (df == null)
            {
                throw new ArgumentException("sender is not an EmvDefinitionFile");
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   A I D   S e l e c t i o n   {0}\n", df.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterApplicationSelection(Object sender, EmvEventArgs eventArgs)
        {
            var df = sender as EmvDefinitionFile;
            if (df == null)
            {
                throw new ArgumentException("sender is not an EmvDefinitionFile");
            }

            if (df.TlvFci != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("  >> TLV: {0}\n", df.TlvFci));
                foreach (TlvData tlv in df.TlvFci.GetTags())
                {
                    WriteTlv(tlv.Tag, tlv, TlvDictionary);
                }
            }
            else
            {
                gui.guiDetailedLogs.AppendText("  >> AID not found\n");
            }
        }

        public void BeforeGetProcessingOptions(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e t P r o c e s s i n g O p t i o n s   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterGetProcessingOptions(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            if (emv.TlvProcessingOptions == null)
            {
                return;
            }
            foreach (TlvData tlv in emv.TlvProcessingOptions.GetTags())
            {
                WriteTlv(tlv.Tag, tlv, TlvDictionary);
            }

            gui.guiDetailedLogs.AppendText(String.Format("    >> {0}\n", emv.Aip));
            foreach (var file in emv.Afl.GetFiles())
            {
                gui.guiDetailedLogs.AppendText(String.Format("    >> {0}\n", file));
            }
        }

        public void BeforeReadApplicationData(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   R e a d A p p l i c a t i o n D a t a   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterReadApplicationData(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            // dump all records
            foreach (var record in emv.TlvRecords)
            {
                foreach (TlvData tlv in record.GetTags())
                {
                    WriteTlv(tlv.Tag, tlv, TlvDictionary);
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

        public void BeforeGetData(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e t D a t a   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterGetData(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            if (emv.TlvATC != null)
            {
                WriteTlv(emv.TlvATC.Tag, emv.TlvATC, TlvDictionary);
            }
            if (emv.TlvLastOnlineATCRegister != null)
            {
                WriteTlv(emv.TlvLastOnlineATCRegister.Tag, emv.TlvLastOnlineATCRegister, TlvDictionary);
            }
            if (emv.TlvPINTryCounter != null)
            {
                WriteTlv(emv.TlvPINTryCounter.Tag, emv.TlvPINTryCounter, TlvDictionary);
            }
            if (emv.TlvLogFormat != null)
            {
                WriteTlv(emv.TlvLogFormat.Tag, emv.TlvLogFormat, TlvDictionary);
            }
        }

        public void BeforeReadLogFile(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   R e a d L o g F i l e   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterReadLogFile(Object sender, EmvEventArgs eventArgs)
        {
        }

        public void BeforeVerifyPin(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   V e r i f y P i n   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterVerifyPin(Object sender, EmvEventArgs eventArgs)
        {
        }

        public void BeforeInternalAuthenticate(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   I n t e r n a l A u t h e n t i c a t e   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterInternalAuthenticate(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            if (emv.TlvSignedDynamicApplicationResponse != null)
            {
                WriteTlv(emv.TlvSignedDynamicApplicationResponse.Tag, emv.TlvSignedDynamicApplicationResponse, TlvDictionary);
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

        public void BeforeGetChallenge(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e t C h a l l e n g e   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void AfterGetChallenge(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            if (emv.IccChallenge != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("    >> {0}\n", emv.IccChallenge.ToHexa()));
            }
        }

        public void BeforeGenerateAc1(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e n e r a t e A C 1   {0}\n", emv.Aid));
            gui.guiDetailedLogs.AppendText("\n");

            gui.guiDetailedLogs.AppendText(String.Format("    >> Requested AC: {0}\n", emv.RequestedAC1Type));
            gui.guiDetailedLogs.AppendText(String.Format("    >> Unpredictable Number: {0}\n", emv.TlvGenerateAC1UnpredictableNumber));
        }

        public void AfterGenerateAc1(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            if (emv.TlvGenerateAC1Response == null)
            {
                return;
            }

            WriteTlv(emv.TlvGenerateAC1Response.Tag, emv.TlvGenerateAC1Response, TlvDictionary);
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
        private void WriteTlv(UInt32 tagId, TlvData tlv, TlvDictionary tagsManager)
        {
            gui.guiDetailedLogs.AppendText(String.Format("  >> TLV {0:X2}: [ {1} ]\n", tagId, tlv.GetTag(tagId)));

            if (!tlv.HasTag(tagId) || (tagsManager.CreateInstance(tlv.GetTag(tagId)) == null))
            {
                return;
            }

            gui.guiDetailedLogs.SelectionColor = highlightColor;
            gui.guiDetailedLogs.AppendText(String.Format("     >> {0:N}: {0}\n", tagsManager.CreateInstance(tlv.GetTag(tagId))));
            gui.guiDetailedLogs.SelectionColor = standardColor;
        }
    }
}