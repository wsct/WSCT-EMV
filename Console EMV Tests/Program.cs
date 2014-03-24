using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using WSCT.Core;
using WSCT.EMV.Card;
using WSCT.EMV.Objects;
using WSCT.ISO7816;
using WSCT.Wrapper;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.ConsoleEMVTests
{
    class Program
    {
        TLVDictionary tagsManager;
        List<EMVApplication> emvApplications;
        XmlDocument xmlDoc;
        XmlElement xmlRoot;

        static void Main(string[] args)
        {
            try
            {
                Console.WindowWidth = 132;
                Console.WindowHeight = 50;
                Console.SetBufferSize(132, 1000);
            }
            catch (Exception)
            { }
            Console.ForegroundColor = ConsoleColor.Gray;
            new Program().run(args);
        }

        #region >> EMV Event Handlers

        private void beforePSESelection(EMVDefinitionFile df)
        {
            Console.WriteLine();
            Console.WriteLine("=========== P S E   S e l e c t i o n");
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("PSESelection"));
        }

        private void afterPSESelection(EMVDefinitionFile df)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = P S E   S e l e c t i o n");
            Console.WriteLine();

            if (df.TlvFci != null)
            {
                xmlRoot.LastChild.AppendChild(df.TlvFci.toXmlNode(xmlDoc));
                Console.WriteLine("  >> TLV: " + df.TlvFci);
                foreach (TLVData tlv in df.TlvFci.getTags())
                {
                    WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }
        }

        private void beforePSERead(PaymentSystemEnvironment pse)
        {
            Console.WriteLine();
            Console.WriteLine("=========== P S E   R e a d");
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("PSERead"));
        }

        private void afterPSERead(PaymentSystemEnvironment pse)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = P S E   R e a d");
            Console.WriteLine();

            foreach (TLVData record in pse.TlvRecords)
            {
                foreach (TLVData tlv in record.getTags())
                {
                    xmlRoot.LastChild.AppendChild(tlv.toXmlNode(xmlDoc));
                    WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }

            Console.WriteLine();
            Console.WriteLine("=========== P S E   A p p l i c a t i o n s   f o u n d");
            Console.WriteLine();

            foreach (EMVApplication emvFound in pse.GetApplications())
            {
                Console.WriteLine("  >> Application '{0}' [ {1} ] found", emvFound.Aid, tagsManager.createInstance(emvFound.TlvFromPSE.getTag(0x50)));
            }
        }

        private void beforeApplicationSelection(EMVDefinitionFile df)
        {
            Console.WriteLine();
            Console.WriteLine("=========== E M V   A I D   S e l e c t i o n   {0}", df.Aid);
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("ApplicationSelection"));
        }

        private void afterApplicationSelection(EMVDefinitionFile df)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   A I D   S e l e c t i o n   {0}", df.Aid);
            Console.WriteLine();

            if (df.TlvFci != null)
            {
                Console.WriteLine("  >> TLV: " + df.TlvFci);
                foreach (TLVData tlv in df.TlvFci.getTags())
                {
                    xmlRoot.LastChild.AppendChild(tlv.toXmlNode(xmlDoc));
                    WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }
        }

        private void beforeGetProcessingOptions(EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("=========== E M V   G e t P r o c e s s i n g O p t i o n s   {0}", emv.Aid);
            Console.WriteLine();

            xmlRoot.LastChild.AppendChild(xmlDoc.CreateElement("GetProcessingOptions"));
        }

        private void afterGetProcessingOptions(EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   G e t P r o c e s s i n g O p t i o n s   {0}", emv.Aid);
            Console.WriteLine();

            foreach (TLVData tlv in emv.TlvProcessingOptions.getTags())
            {
                xmlRoot.LastChild.AppendChild(tlv.toXmlNode(xmlDoc));
                WriteTLV(tlv.tag, tlv, tagsManager);
            }

            Console.WriteLine("    >> {0}", emv.Aip);
            foreach (ApplicationFileLocator.FileIdentification file in emv.Afl.GetFiles())
            {
                Console.WriteLine("    >> {0}", file);
            }
        }

        private void beforeReadApplicationData(EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("=========== E M V   R e a d A p p l i c a t i o n D a t a   {0}", emv.Aid);
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("ReadApplicationData"));
        }

        private void afterReadApplicationData(EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   R e a d A p p l i c a t i o n D a t a   {0}", emv.Aid);
            Console.WriteLine();

            foreach (TLVData record in emv.TlvRecords)
            {
                foreach (TLVData tlv in record.getTags())
                {
                    xmlRoot.LastChild.AppendChild(tlv.toXmlNode(xmlDoc));
                    WriteTLV(tlv.tag, tlv, tagsManager);
                }
            }
        }

        private void beforeGetData(EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("=========== E M V   G e t D a t a   {0}", emv.Aid);
            Console.WriteLine();

            xmlRoot.AppendChild(xmlDoc.CreateElement("GetData"));
        }

        private void afterGetData(EMVApplication emv)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   G e t D a t a   {0}", emv.Aid);
            Console.WriteLine();

            if (emv.TlvATC != null)
            {
                xmlRoot.LastChild.AppendChild(emv.TlvATC.toXmlNode(xmlDoc));
                WriteTLV(emv.TlvATC.tag, emv.TlvATC, tagsManager);
            }
            if (emv.TlvLastOnlineATCRegister != null)
            {
                xmlRoot.LastChild.AppendChild(emv.TlvLastOnlineATCRegister.toXmlNode(xmlDoc));
                WriteTLV(emv.TlvLastOnlineATCRegister.tag, emv.TlvLastOnlineATCRegister, tagsManager);
            }
            if (emv.TlvPINTryCounter != null)
            {
                xmlRoot.LastChild.AppendChild(emv.TlvPINTryCounter.toXmlNode(xmlDoc));
                WriteTLV(emv.TlvPINTryCounter.tag, emv.TlvPINTryCounter, tagsManager);
            }
            if (emv.TlvLogFormat != null)
            {
                xmlRoot.LastChild.AppendChild(emv.TlvLogFormat.toXmlNode(xmlDoc));
                WriteTLV(emv.TlvLogFormat.tag, emv.TlvLogFormat, tagsManager);
            }
        }

        #endregion

        private void run(string[] args)
        {
            Console.WriteLine("=========== S o m e   T L V D a t a   e x a m p l e s");

            #region >> TLV Example

            Console.WriteLine("88 01 02".toTLVData());
            TLVData tlv1 = "88 01 02".toTLVData();
            TLVData tlv2 = "5F 2D 03 01 02 03".toTLVData();
            List<TLVData> ltlv = new List<TLVData>();
            ltlv.Add(tlv1);
            ltlv.Add(tlv2);
            Console.WriteLine(ltlv.toTLVData(0x20));
            Console.WriteLine(ltlv.toTLVData(0x20).getTag(0x88));
            Console.WriteLine(ltlv.toTLVData(0x20).getTag(0x5F2D));

            #endregion

            Console.WriteLine();
            Console.WriteLine("=========== I n i t i a l i z i n g   P C / S C");

            xmlDoc = new XmlDocument();
            xmlRoot = xmlDoc.CreateElement("WinSCard");
            xmlDoc.AppendChild(xmlRoot);

            #region >> ConsoleObserver

            ConsoleObserver logger = new ConsoleObserver();

            #endregion

            #region >> CardContext

            ICardContext context = new CardContext();
            logger.observeContext((CardContextObservable)context);

            if (context.establish() != ErrorCode.Success)
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

            Console.WriteLine();
            Console.WriteLine("=========== C a r d   i n s e r t i o n   d e t e c t i o n");

            #region >> StatusChangeMonitor

            StatusChangeMonitor monitor = new StatusChangeMonitor(context);

            logger.observeMonitor(monitor);

            AbstractReaderState readerState = monitor.WaitForCardPresence(0);
            if (readerState == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(">> Insert a card in one of the {0} readers (time out in 15s)", context.readersCount);
                readerState = monitor.WaitForCardPresence(15000);
            }

            if (readerState == null)
            {
                Console.WriteLine(">> Time Out! No card found");
                return;
            }

            #endregion

            #region >> CardChannel

            ICardChannel rawCardChannel = new CardChannel(context, readerState.ReaderName);
            logger.observeChannel((CardChannelObservable)rawCardChannel);

            CardChannelISO7816 cardChannel = new CardChannelISO7816(rawCardChannel);

            if (cardChannel.connect(ShareMode.Shared, Protocol.Any) != ErrorCode.Success)
            {
                Console.WriteLine("Erreur: connect() failed");
                return;
            }

            Byte[] buffer = new Byte[0];
            cardChannel.getAttrib(Attrib.AtrString, ref buffer);

            if (cardChannel.reconnect(ShareMode.Shared, Protocol.Any, Disposition.ResetCard) != ErrorCode.Success)
            {
                Console.WriteLine("Erreur: reconnect() failed");
                return;
            }

            cardChannel.getAttrib(Attrib.AtrString, ref buffer);

            #endregion

            #region >> TagsManager

            tagsManager = SerializedObject<TLVDictionary>.loadFromXml(@"Dictionary.EMVTag.xml");

            #endregion

            #region >> PSE Analysis

            PaymentSystemEnvironment pse = new PaymentSystemEnvironment(cardChannel);

            pse.BeforeSelectEvent += beforePSESelection;
            pse.AfterSelectEvent += afterPSESelection;
            pse.BeforeReadEvent += beforePSERead;
            pse.AfterReadEvent += afterPSERead;

            if (pse.Select() == 0x9000)
            {
                if (pse.TlvFci.hasTag(0x88))
                {
                    pse.Read();
                }
            }

            #endregion

            emvApplications = new List<EMVApplication>();
            foreach (EMVApplication emvFound in pse.GetApplications())
            {
                emvApplications.Add(emvFound);
            }

            #region >> AID selection

            foreach (EMVApplication emv in emvApplications)
            {
                emv.BeforeSelectEvent += beforeApplicationSelection;
                emv.AfterSelectEvent += afterApplicationSelection;
                emv.BeforeGetProcessingOptionsEvent += beforeGetProcessingOptions;
                emv.AfterGetProcessingOptionsEvent += afterGetProcessingOptions;
                emv.BeforeReadApplicationDataEvent += beforeReadApplicationData;
                emv.AfterReadApplicationDataEvent += afterReadApplicationData;
                emv.BeforeGetDataEvent += beforeGetData;
                emv.AfterGetDataEvent += afterGetData;

                if (emv.Select() == 0x9000)
                {
                    if (emv.GetProcessingOptions() == 0x9000)
                    {
                        emv.ReadApplicationData();
                        emv.GetData();
                    }
                }
            }

            #endregion

            Console.WriteLine();
            Console.WriteLine("=========== T e r m i n a t i n g");

            cardChannel.disconnect(Disposition.UnpowerCard);

            xmlDoc.Save(new FileStream("EMV.TLV.xml", FileMode.Create, FileAccess.ReadWrite));

            Console.WriteLine();
            Console.WriteLine("=========== C a r d   r e m o v a l   d e t e c t i o n");

            #region >> StatusChangeMonitor

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">> Waiting for a change since last call (time out in 10s)");
            // "unpower" change should be fired for the previously used reader
            monitor.waitForChange(10000);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">> Remove the card in one of the readers {0} (time out in 10s)", readerState.ReaderName);
            // Wait for another change
            monitor.waitForChange(10000);

            #endregion

            Console.WriteLine();
            context.release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="tlv"></param>
        /// <param name="tagsManager"></param>
        static void WriteTLV(UInt32 tagId, TLVData tlv, TLVDictionary tagsManager)
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