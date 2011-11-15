using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Core;
using WSCT.Core.APDU;
using WSCT.Wrapper;
using WSCT;
using WSCT.EMV;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.ConsoleEMVTests
{
    class Program
    {
        TLVDictionary tagsManager;
        List<EMV.Card.EMVApplication> emvApplications;
        System.Xml.XmlDocument xmlDoc;
        System.Xml.XmlElement xmlRoot;

        static void Main(string[] args)
        {
            Console.WindowWidth = 132;
            Console.WindowHeight = 50;
            Console.SetBufferSize(132, 1000);
            Console.ForegroundColor = ConsoleColor.Gray;
            new Program().run(args);
        }

        #region >> EMV Event Handlers

        private void beforePSESelection(EMV.Card.EMVDefinitionFile df)
        {
            Console.WriteLine();
            Console.WriteLine("=========== P S E   S e l e c t i o n");
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("PSESelection"));
        }

        private void afterPSESelection(EMV.Card.EMVDefinitionFile df)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = P S E   S e l e c t i o n");
            Console.WriteLine();

            if (df.tlvFCI != null)
            {
                xmlRoot.LastChild.AppendChild(df.tlvFCI.toXmlNode(xmlDoc));
                Console.WriteLine("  >> TLV: " + df.tlvFCI);
                foreach (Helpers.BasicEncodingRules.TLVData tlv in df.tlvFCI.getTags())
                {
                    Program.WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }
        }

        private void beforePSERead(EMV.Card.PaymentSystemEnvironment pse)
        {
            Console.WriteLine();
            Console.WriteLine("=========== P S E   R e a d");
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("PSERead"));
        }

        private void afterPSERead(EMV.Card.PaymentSystemEnvironment pse)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = P S E   R e a d");
            Console.WriteLine();

            foreach (Helpers.BasicEncodingRules.TLVData record in pse.tlvRecords)
            {
                foreach (Helpers.BasicEncodingRules.TLVData tlv in record.getTags())
                {
                    xmlRoot.LastChild.AppendChild(tlv.toXmlNode(xmlDoc));
                    Program.WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }

            Console.WriteLine();
            Console.WriteLine("=========== P S E   A p p l i c a t i o n s   f o u n d");
            Console.WriteLine();

            foreach (EMV.Card.EMVApplication emvFound in pse.getApplications())
            {
                Console.WriteLine("  >> Application '{0}' [ {1} ] found", emvFound.aid, tagsManager.createInstance(emvFound.tlvFromPSE.getTag(0x50)));
            }
        }

        private void beforeApplicationSelection(EMV.Card.EMVDefinitionFile df)
        {
            Console.WriteLine();
            Console.WriteLine("=========== E M V   A I D   S e l e c t i o n   {0}", df.aid);
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("ApplicationSelection"));
        }

        private void afterApplicationSelection(EMV.Card.EMVDefinitionFile df)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   A I D   S e l e c t i o n   {0}", df.aid);
            Console.WriteLine();

            if (df.tlvFCI != null)
            {
                Console.WriteLine("  >> TLV: " + df.tlvFCI);
                foreach (Helpers.BasicEncodingRules.TLVData tlv in df.tlvFCI.getTags())
                {
                    xmlRoot.LastChild.AppendChild(tlv.toXmlNode(xmlDoc));
                    Program.WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }
        }

        private void beforeGetProcessingOptions(EMV.Card.EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("=========== E M V   G e t P r o c e s s i n g O p t i o n s   {0}", emv.aid);
            Console.WriteLine();

            xmlRoot.LastChild.AppendChild(xmlDoc.CreateElement("GetProcessingOptions"));
        }

        private void afterGetProcessingOptions(EMV.Card.EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   G e t P r o c e s s i n g O p t i o n s   {0}", emv.aid);
            Console.WriteLine();

            foreach (Helpers.BasicEncodingRules.TLVData tlv in emv.tlvProcessingOptions.getTags())
            {
                xmlRoot.LastChild.AppendChild(tlv.toXmlNode(xmlDoc));
                Program.WriteTLV(tlv.tag, tlv, tagsManager);
            }

            Console.WriteLine("    >> {0}", emv.aip);
            foreach (EMV.Objects.ApplicationFileLocator.FileIdentification file in emv.afl.getFiles())
            {
                Console.WriteLine("    >> {0}", file);
            }
        }

        private void beforeReadApplicationData(EMV.Card.EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("=========== E M V   R e a d A p p l i c a t i o n D a t a   {0}", emv.aid);
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("ReadApplicationData"));
        }

        private void afterReadApplicationData(EMV.Card.EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   R e a d A p p l i c a t i o n D a t a   {0}", emv.aid);
            Console.WriteLine();

            foreach (Helpers.BasicEncodingRules.TLVData record in emv.tlvRecords)
            {
                foreach (Helpers.BasicEncodingRules.TLVData tlv in record.getTags())
                {
                    xmlRoot.LastChild.AppendChild(tlv.toXmlNode(xmlDoc));
                    Program.WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }
        }

        private void beforeGetData(EMV.Card.EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("=========== E M V   G e t D a t a   {0}", emv.aid);
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("GetData"));
        }

        private void afterGetData(EMV.Card.EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   G e t D a t a   {0}", emv.aid);
            Console.WriteLine();

            if (emv.tlvATC != null)
            {
                xmlRoot.LastChild.AppendChild(emv.tlvATC.toXmlNode(xmlDoc));
                Program.WriteTLV(emv.tlvATC.tag, emv.tlvATC, tagsManager);
            }
            if (emv.tlvLastOnlineATCRegister != null)
            {
                xmlRoot.LastChild.AppendChild(emv.tlvLastOnlineATCRegister.toXmlNode(xmlDoc));
                Program.WriteTLV(emv.tlvLastOnlineATCRegister.tag, emv.tlvLastOnlineATCRegister, tagsManager);
            }
            if (emv.tlvPINTryCounter != null)
            {
                xmlRoot.LastChild.AppendChild(emv.tlvPINTryCounter.toXmlNode(xmlDoc));
                Program.WriteTLV(emv.tlvPINTryCounter.tag, emv.tlvPINTryCounter, tagsManager);
            }
            if (emv.tlvLogFormat != null)
            {
                xmlRoot.LastChild.AppendChild(emv.tlvLogFormat.toXmlNode(xmlDoc));
                Program.WriteTLV(emv.tlvLogFormat.tag, emv.tlvLogFormat, tagsManager);
            }
        }

        #endregion

        private void run(string[] args)
        {
            Console.WriteLine("=========== S o m e   T L V D a t a   e x a m p l e s");
            Console.WriteLine("88 01 02".toTLVData());
            TLVData tlv1 = "88 01 02".toTLVData();
            TLVData tlv2 = "5F 2D 03 01 02 03".toTLVData();
            List<TLVData> ltlv = new List<TLVData>();
            ltlv.Add(tlv1);
            ltlv.Add(tlv2);
            Console.WriteLine(ltlv.toTLVData(0x20));
            Console.WriteLine(ltlv.toTLVData(0x20).getTag(0x88));
            Console.WriteLine(ltlv.toTLVData(0x20).getTag(0x5F2D));

            Console.WriteLine();
            Console.WriteLine("=========== I n i t i a l i z i n g   P C / S C");

            xmlDoc = new System.Xml.XmlDocument();
            xmlRoot = xmlDoc.CreateElement("WinSCard");
            xmlDoc.AppendChild(xmlRoot);

            #region >> ConsoleObserver

            ConsoleObserver logger = new ConsoleObserver();

            #endregion

            #region >> CardContext

            ICardContext context = new Core.CardContext();
            logger.observeContext((Core.CardContextObservable)context);

            if (context.establish() != ErrorCode.SCARD_S_SUCCESS)
            {
                Console.WriteLine("Erreur: establish() failed");
                return;
            }
            context.listReaderGroups();
            if (context.groupsCount == 0)
            {
                Console.WriteLine("Error: no reader group found");
                context.release();
                return;
            }

            context.listReaders(context.groups[0]);
            if (context.readersCount == 0)
            {
                Console.WriteLine("Error: no reader found");
                context.release();
                return;
            }

            #endregion

            #region >> CardChannel

            ICardChannel rawCardChannel = new Core.CardChannel(context, context.readers[context.readersCount - 1]);
            logger.observeChannel((Core.CardChannelObservable)rawCardChannel);

            ISO7816.CardChannelISO7816 cardChannel = new ISO7816.CardChannelISO7816(rawCardChannel);

            if (cardChannel.connect(ShareMode.SCARD_SHARE_SHARED, Protocol.SCARD_PROTOCOL_ANY) != ErrorCode.SCARD_S_SUCCESS)
            {
                Console.WriteLine("Erreur: connect() failed");
                return;
            }

            if (cardChannel.reconnect(ShareMode.SCARD_SHARE_SHARED, Protocol.SCARD_PROTOCOL_ANY, Disposition.SCARD_RESET_CARD) != ErrorCode.SCARD_S_SUCCESS)
            {
                Console.WriteLine("Erreur: reconnect() failed");
                return;
            }

            #endregion

            #region >> TagsManager

            tagsManager = SerializedObject<TLVDictionary>.loadFromXml(@"Dictionary.EMVTag.xml");

            #endregion

            #region >> PSE Analysis

            EMV.Card.PaymentSystemEnvironment pse = new EMV.Card.PaymentSystemEnvironment(cardChannel);

            pse.beforeSelectEvent += beforePSESelection;
            pse.afterSelectEvent += afterPSESelection;
            pse.beforeReadEvent += beforePSERead;
            pse.afterReadEvent += afterPSERead;

            if (pse.select() == 0x9000)
            {
                if (pse.tlvFCI.hasTag(0x88))
                {
                    pse.read();
                }
            }

            #endregion

            emvApplications = new List<WSCT.EMV.Card.EMVApplication>();
            foreach (EMV.Card.EMVApplication emvFound in pse.getApplications())
            {
                emvApplications.Add(emvFound);
            }

            #region >> AID selection

            foreach (EMV.Card.EMVApplication emv in emvApplications)
            {
                emv.beforeSelectEvent += this.beforeApplicationSelection;
                emv.afterSelectEvent += this.afterApplicationSelection;
                emv.beforeGetProcessingOptionsEvent += this.beforeGetProcessingOptions;
                emv.afterGetProcessingOptionsEvent += this.afterGetProcessingOptions;
                emv.beforeReadApplicationDataEvent += this.beforeReadApplicationData;
                emv.afterReadApplicationDataEvent += this.afterReadApplicationData;
                emv.beforeGetDataEvent += this.beforeGetData;
                emv.afterGetDataEvent += this.afterGetData;

                if (emv.select() == 0x9000)
                {
                    if (emv.getProcessingOptions() == 0x9000)
                    {
                        emv.readApplicationData();
                        emv.getData();
                    }
                }
            }

            #endregion

            Console.WriteLine();
            Console.WriteLine("=========== T e r m i n a t i n g");

            cardChannel.disconnect(Disposition.SCARD_UNPOWER_CARD);

            xmlDoc.Save(new System.IO.FileStream("EMV.TLV.xml", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite));

            Console.WriteLine();
            context.release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="tlv"></param>
        /// <param name="tagsManager"></param>
        static void WriteTLV(UInt32 tagId, Helpers.BasicEncodingRules.TLVData tlv, TLVDictionary tagsManager)
        {
            AbstractTLVObject tagObject = null;
            Console.WriteLine("  >> Contains tag {0:X2}: {1} [ {2} ]", tagId, tlv.hasTag(tagId), tlv.getTag(tagId));
            if (tlv.hasTag(tagId) && ((tagObject = tagsManager.createInstance(tlv.getTag(tagId))) != null))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("     >> {0:N}: {0}", tagsManager.createInstance(tlv.getTag(tagId)));
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}