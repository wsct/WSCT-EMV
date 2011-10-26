using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Core;
using WSCT.Core.APDU;
using WSCT.Wrapper;

using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    class DetailedLogs
    {
        String header;
        System.Drawing.Color highlightColor = System.Drawing.Color.DarkBlue;
        System.Drawing.Color standardColor = System.Drawing.Color.Black;

        GUI gui;

        public TLVDictionary tagsManager;

        #region >> Constructors

        public DetailedLogs(GUI guiInstance)
            : this(guiInstance, "[{0,7}] Core ")
        {
        }

        public DetailedLogs(GUI guiInstance, String _header)
        {
            gui = guiInstance;
            header = _header;
        }

        #endregion

        #region >> observe*

        public void observeEMV(EMV.Card.EMVApplication emv)
        {
            emv.beforeSelectEvent += beforeApplicationSelection;
            emv.afterSelectEvent += afterApplicationSelection;

            emv.beforeGetProcessingOptionsEvent += beforeGetProcessingOptions;
            emv.afterGetProcessingOptionsEvent += afterGetProcessingOptions;

            emv.beforeReadApplicationDataEvent += beforeReadApplicationData;
            emv.afterReadApplicationDataEvent += afterReadApplicationData;

            emv.beforeGetDataEvent += beforeGetData;
            emv.afterGetDataEvent += afterGetData;

            emv.beforeReadLogFileEvent += beforeReadLogFile;
            emv.afterReadLogFileEvent += afterReadLogFile;

            emv.beforeVerifyPinEvent += beforeVerifyPin;
            emv.afterVerifyPinEvent += afterVerifyPin;

            emv.beforeInternalAuthenticateEvent += beforeInternalAuthenticate;
            emv.afterInternalAuthenticateEvent += afterInternalAuthenticate;

            emv.beforeGetChallengeEvent += beforeGetChallenge;
            emv.afterGetChallengeEvent += afterGetChallenge;

            emv.beforeGenerateAC1Event += beforeGenerateAC1;
            emv.afterGenerateAC1Event += afterGenerateAC1;
        }

        public void observePSE(EMV.Card.PaymentSystemEnvironment pse)
        {
            pse.beforeSelectEvent += beforePSESelection;
            pse.afterSelectEvent += afterPSESelection;

            pse.beforeReadEvent += beforePSERead;
            pse.afterReadEvent += afterPSERead;
        }

        #endregion

        #region >> CardChannelObservable Event Handlers

        public void notifyDisconnect(ICardChannel cardChannel, Disposition disposition, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            else
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
        }

        public void notifyReconnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, Disposition initialization, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
            else
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
        }

        public void notifyTransmit(ICardChannel cardChannel, ICardCommand cardCommand, Byte[] recvBuffer, UInt32 recvSize, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
            {

                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> RAPDU: [{0}]\n", recvBuffer.toHexa((int)recvSize)));
            }
            else
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
        }

        public void notifyTransmit(ICardChannel cardChannel, ICardCommand cardCommand, ICardResponse cardResponse, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
            {
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> RAPDU: [{0}]\n", cardResponse));
            }
            else
                gui.guiDetailedLogs.AppendText(String.Format(header + ">> Error: {0}\n", errorCode));
        }

        public void beforeDisconnect(ICardChannel cardChannel, Disposition disposition)
        {
            gui.guiDetailedLogs.SelectionColor = highlightColor;
            gui.guiDetailedLogs.AppendText(String.Format(header + "disconnect({0})\n", disposition));
            gui.guiDetailedLogs.SelectionColor = standardColor;
        }

        public void beforeReconnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, Disposition initialization)
        {
            gui.guiDetailedLogs.SelectionColor = highlightColor;
            gui.guiDetailedLogs.AppendText(String.Format(header + "reconnect({0},{1},{2})\n", shareMode, preferedProtocol, initialization));
            gui.guiDetailedLogs.SelectionColor = standardColor;
        }

        public void beforeTransmit(ICardChannel cardChannel, ICardCommand cardCommand, Byte[] recvBuffer, UInt32 recvSize)
        {
            gui.guiDetailedLogs.SelectionColor = highlightColor;
            gui.guiDetailedLogs.AppendText(String.Format(header + "transmit({0})\n", cardCommand));
            gui.guiDetailedLogs.SelectionColor = standardColor;
        }

        public void beforeTransmit(ICardChannel cardChannel, ICardCommand cardCommand, ICardResponse cardResponse)
        {
            gui.guiDetailedLogs.SelectionColor = highlightColor;
            gui.guiDetailedLogs.AppendText(String.Format(header + "transmit({0})\n", cardCommand));
            gui.guiDetailedLogs.SelectionColor = standardColor;
        }

        #endregion

        #region >> EMV Event Handlers

        public void beforePSESelection(EMV.Card.EMVDefinitionFile df)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText("P S E   S e l e c t i o n\n");
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterPSESelection(EMV.Card.EMVDefinitionFile df)
        {
            if (df.tlvFCI != null)
            {
                gui.guiDetailedLogs.AppendText("  >> TLV: " + df.tlvFCI + "\n");
                foreach (Helpers.BasicEncodingRules.TLVData tlv in df.tlvFCI.getTags())
                {
                    WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }
            else
            {
                gui.guiDetailedLogs.AppendText("  >> No PSE found\n");
            }
        }

        public void beforePSERead(EMV.Card.PaymentSystemEnvironment pse)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText("P S E   R e a d\n");
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterPSERead(EMV.Card.PaymentSystemEnvironment pse)
        {
            foreach (Helpers.BasicEncodingRules.TLVData record in pse.tlvRecords)
            {
                foreach (Helpers.BasicEncodingRules.TLVData tlv in record.getTags())
                {
                    WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }

            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText("PSE Applications found:\n");

            foreach (EMV.Card.EMVApplication emvFound in pse.getApplications())
            {
                gui.guiDetailedLogs.AppendText(String.Format("  >> Application '{0}' [ {1} ] found\n", emvFound.aid, tagsManager.createInstance(emvFound.tlvFromPSE.getTag(0x50))));
            }
        }

        public void beforeApplicationSelection(EMV.Card.EMVDefinitionFile df)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   A I D   S e l e c t i o n   {0}\n", df.aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterApplicationSelection(EMV.Card.EMVDefinitionFile df)
        {
            if (df.tlvFCI != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("  >> TLV: {0}\n", df.tlvFCI));
                foreach (Helpers.BasicEncodingRules.TLVData tlv in df.tlvFCI.getTags())
                {
                    WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }
            else
            {
                gui.guiDetailedLogs.AppendText("  >> AID not found\n");
            }
        }

        public void beforeGetProcessingOptions(EMV.Card.EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e t P r o c e s s i n g O p t i o n s   {0}\n", emv.aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterGetProcessingOptions(EMV.Card.EMVApplication emv)
        {
            if (emv.tlvProcessingOptions == null)
                return;
            foreach (Helpers.BasicEncodingRules.TLVData tlv in emv.tlvProcessingOptions.getTags())
            {
                WriteTLV(tlv.tag, tlv, tagsManager);
            }

            gui.guiDetailedLogs.AppendText(String.Format("    >> {0}\n", emv.aip));
            foreach (EMV.Objects.ApplicationFileLocator.FileIdentification file in emv.afl.getFiles())
            {
                gui.guiDetailedLogs.AppendText(String.Format("    >> {0}\n", file));
            }
        }

        public void beforeReadApplicationData(EMV.Card.EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   R e a d A p p l i c a t i o n D a t a   {0}\n", emv.aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterReadApplicationData(EMV.Card.EMVApplication emv)
        {
            // dump all records
            foreach (Helpers.BasicEncodingRules.TLVData record in emv.tlvRecords)
            {
                foreach (Helpers.BasicEncodingRules.TLVData tlv in record.getTags())
                {
                    WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }

            // informations about SDA
            if (emv.sda != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("SDA: {0}\n", emv.sda));
            }
            else
            {
                gui.guiDetailedLogs.AppendText("SDA not supported or unknown Certification Authority\n");
            }
        }

        public void beforeGetData(EMV.Card.EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e t D a t a   {0}\n", emv.aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterGetData(EMV.Card.EMVApplication emv)
        {
            if (emv.tlvATC != null)
            {
                WriteTLV(emv.tlvATC.tag, emv.tlvATC, tagsManager);
            }
            if (emv.tlvLastOnlineATCRegister != null)
            {
                WriteTLV(emv.tlvLastOnlineATCRegister.tag, emv.tlvLastOnlineATCRegister, tagsManager);
            }
            if (emv.tlvPINTryCounter != null)
            {
                WriteTLV(emv.tlvPINTryCounter.tag, emv.tlvPINTryCounter, tagsManager);
            }
            if (emv.tlvLogFormat != null)
            {
                WriteTLV(emv.tlvLogFormat.tag, emv.tlvLogFormat, tagsManager);
            }
        }

        public void beforeReadLogFile(EMV.Card.EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   R e a d L o g F i l e   {0}\n", emv.aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterReadLogFile(EMV.Card.EMVApplication emv)
        {
        }

        public void beforeVerifyPin(EMV.Card.EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   V e r i f y P i n   {0}\n", emv.aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterVerifyPin(EMV.Card.EMVApplication emv)
        {
        }

        public void beforeInternalAuthenticate(EMV.Card.EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   I n t e r n a l A u t h e n t i c a t e   {0}\n", emv.aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterInternalAuthenticate(EMV.Card.EMVApplication emv)
        {
            if (emv.tlvSignedDynamicApplicationResponse != null)
            {
                WriteTLV(emv.tlvSignedDynamicApplicationResponse.tag, emv.tlvSignedDynamicApplicationResponse, tagsManager);
            }

            // informations about DDA
            if (emv.dda != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("DDA: {0}", emv.dda));
            }
            else
            {
                gui.guiDetailedLogs.AppendText("DDA not supported or unknown Certification Authority");
            }
        }

        public void beforeGetChallenge(EMV.Card.EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e t C h a l l e n g e   {0}\n", emv.aid));
            gui.guiDetailedLogs.AppendText("\n");
        }

        public void afterGetChallenge(EMV.Card.EMVApplication emv)
        {
            if (emv.iccChallenge != null)
            {
                gui.guiDetailedLogs.AppendText(String.Format("    >> {0}\n", emv.iccChallenge.toHexa()));
            }
        }

        public void beforeGenerateAC1(EMV.Card.EMVApplication emv)
        {
            gui.guiDetailedLogs.AppendText("\n");
            gui.guiDetailedLogs.AppendText(String.Format("E M V   G e n e r a t e A C 1   {0}\n", emv.aid));
            gui.guiDetailedLogs.AppendText("\n");

            gui.guiDetailedLogs.AppendText(String.Format("    >> Requested AC: {0}\n", emv.requestedAC1Type));
            gui.guiDetailedLogs.AppendText(String.Format("    >> Unpredictable Number: {0}\n", emv.tlvGenerateAC1UnpredictableNumber));
        }

        public void afterGenerateAC1(EMV.Card.EMVApplication emv)
        {
            if (emv.tlvGenerateAC1Response != null)
            {
                WriteTLV(emv.tlvGenerateAC1Response.tag, emv.tlvGenerateAC1Response, tagsManager);
                gui.guiDetailedLogs.AppendText(String.Format("    >> Cryptogram Information Data: {0}\n", emv.cid1));
                gui.guiDetailedLogs.AppendText(String.Format("    >> Application Cryptogram Counter: {0}\n", emv.atcFromAC1));
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="tlv70"></param>
        /// <param name="tagsManager"></param>
        void WriteTLV(UInt32 tagId, TLVData tlv, TLVDictionary tagsManager)
        {
            AbstractTLVObject tagObject = null;
            gui.guiDetailedLogs.AppendText(String.Format("  >> TLV {0:X2}: [ {1} ]\n", tagId, tlv.getTag(tagId)));
            if (tlv.hasTag(tagId) && ((tagObject = tagsManager.createInstance(tlv.getTag(tagId))) != null))
            {
                gui.guiDetailedLogs.SelectionColor = highlightColor;
                gui.guiDetailedLogs.AppendText(String.Format("     >> {0:N}: {0}\n", tagsManager.createInstance(tlv.getTag(tagId))));
                gui.guiDetailedLogs.SelectionColor = standardColor;
            }
        }
    }
}