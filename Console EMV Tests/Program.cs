using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using WSCT.Core;
using WSCT.EMV.Card;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;
using WSCT.Helpers.Desktop;
using WSCT.ISO7816;
using WSCT.Wrapper;
using WSCT.Wrapper.Desktop.Core;

namespace WSCT.ConsoleEMVTests
{
    internal class Program
    {
        private List<EmvApplication> emvApplications;
        private TlvDictionary tagsManager;
        private XDocument xmlDoc;
        private XElement xmlRoot;

        private static void Main(string[] args)
        {
            try
            {
                Console.WindowWidth = 132;
                Console.WindowHeight = 50;
                Console.SetBufferSize(132, 1000);
            }
            catch (Exception)
            {
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            RegisterPcl.Register();

            new Program().Run();
        }

        private void Run()
        {
            Console.WriteLine("=========== S o m e   T L V D a t a   e x a m p l e s");

            #region >> TLV Example

            Console.WriteLine("88 01 02".ToTlvData());
            var tlv1 = "88 01 02".ToTlvData();
            var tlv2 = "5F 2D 03 01 02 03".ToTlvData();
            var ltlv = new List<TlvData>();
            ltlv.Add(tlv1);
            ltlv.Add(tlv2);
            Console.WriteLine(ltlv.ToTlvData(0x20));
            Console.WriteLine(ltlv.ToTlvData(0x20).GetTag(0x88));
            Console.WriteLine(ltlv.ToTlvData(0x20).GetTag(0x5F2D));

            #endregion

            Console.WriteLine();
            Console.WriteLine("=========== I n i t i a l i z i n g   P C / S C");

            xmlRoot = new XElement("WinSCard");
            xmlDoc = new XDocument(xmlRoot);

            #region >> ConsoleObserver

            var logger = new ConsoleObserver();

            #endregion

            #region >> CardContext

            ICardContext context = new CardContext();
            logger.ObserveContext((CardContextObservable)context);

            if (context.Establish() != ErrorCode.Success)
            {
                Console.WriteLine("Erreur: establish() failed");
                return;
            }
            context.ListReaderGroups();
            if (context.GroupsCount == 0)
            {
                Console.WriteLine("Error: no reader group found");
                context.Release();
                return;
            }

            context.ListReaders(context.Groups[0]);
            if (context.ReadersCount == 0)
            {
                Console.WriteLine("Error: no reader found");
                context.Release();
                return;
            }

            #endregion

            Console.WriteLine();
            Console.WriteLine("=========== C a r d   i n s e r t i o n   d e t e c t i o n");

            #region >> StatusChangeMonitor

            var monitor = new StatusChangeMonitor(context);

            logger.ObserveMonitor(monitor);

            var readerState = monitor.WaitForCardPresence(0);
            if (readerState == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(">> Insert a card in one of the {0} readers (time out in 15s)", context.ReadersCount);
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
            logger.ObserveChannel((CardChannelObservable)rawCardChannel);

            var cardChannel = new CardChannelIso7816(rawCardChannel);

            if (cardChannel.Connect(ShareMode.Shared, Protocol.Any) != ErrorCode.Success)
            {
                Console.WriteLine("Erreur: connect() failed");
                return;
            }

            var buffer = new byte[0];
            cardChannel.GetAttrib(Attrib.AtrString, ref buffer);

            if (cardChannel.Reconnect(ShareMode.Shared, Protocol.Any, Disposition.ResetCard) != ErrorCode.Success)
            {
                Console.WriteLine("Erreur: reconnect() failed");
                return;
            }

            cardChannel.GetAttrib(Attrib.AtrString, ref buffer);

            #endregion

            #region >> TagsManager

            tagsManager = SerializedObject<TlvDictionary>.LoadFromXml(@"Dictionary.EMVTag.xml");

            #endregion

            #region >> PSE Analysis

            var pse = new PaymentSystemEnvironment(cardChannel);

            pse.BeforeSelectEvent += BeforePseSelection;
            pse.AfterSelectEvent += AfterPseSelection;
            pse.BeforeReadEvent += BeforePseRead;
            pse.AfterReadEvent += AfterPseRead;

            if (pse.Select() == 0x9000)
            {
                if (pse.TlvFci.HasTag(0x88))
                {
                    pse.Read();
                }
            }

            #endregion

            emvApplications = new List<EmvApplication>();
            foreach (var emvFound in pse.GetApplications())
            {
                emvApplications.Add(emvFound);
            }

            #region >> AID selection

            foreach (var emv in emvApplications)
            {
                emv.BeforeSelectEvent += BeforeApplicationSelection;
                emv.AfterSelectEvent += AfterApplicationSelection;
                emv.BeforeGetProcessingOptionsEvent += BeforeGetProcessingOptions;
                emv.AfterGetProcessingOptionsEvent += AfterGetProcessingOptions;
                emv.BeforeReadApplicationDataEvent += BeforeReadApplicationData;
                emv.AfterReadApplicationDataEvent += AfterReadApplicationData;
                emv.BeforeGetDataEvent += BeforeGetData;
                emv.AfterGetDataEvent += AfterGetData;

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

            cardChannel.Disconnect(Disposition.UnpowerCard);

            xmlDoc.Save(new FileStream("EMV.TLV.xml", FileMode.Create, FileAccess.ReadWrite));

            Console.WriteLine();
            Console.WriteLine("=========== C a r d   r e m o v a l   d e t e c t i o n");

            #region >> StatusChangeMonitor

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">> Waiting for a change since last call (time out in 10s)");
            // "unpower" change should be fired for the previously used reader
            monitor.WaitForChange(10000);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">> Remove the card in one of the readers {0} (time out in 10s)", readerState.ReaderName);
            // Wait for another change
            monitor.WaitForChange(10000);

            #endregion

            Console.WriteLine();
            context.Release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="tlv"></param>
        /// <param name="tagsManager"></param>
        private static void WriteTlv(UInt32 tagId, TlvData tlv, TlvDictionary tagsManager)
        {
            Console.WriteLine("  >> Contains tag {0:X2}: {1} [ {2} ]", tagId, tlv.HasTag(tagId), tlv.GetTag(tagId));
            if (tlv.HasTag(tagId) && (tagsManager.CreateInstance(tlv.GetTag(tagId)) != null))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("     >> {0:N}: {0}", tagsManager.CreateInstance(tlv.GetTag(tagId)));
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        #region >> EMV Event Handlers

        private void BeforePseSelection(Object sender, EmvEventArgs eventArgs)
        {
            Console.WriteLine();
            Console.WriteLine("=========== P S E   S e l e c t i o n");
            Console.WriteLine();

            xmlRoot.Add(new XElement("PSESelection"));
        }

        private void AfterPseSelection(Object sender, EmvEventArgs eventArgs)
        {
            Console.WriteLine();
            Console.WriteLine("= = = = = = P S E   S e l e c t i o n");
            Console.WriteLine();

            var df = sender as EmvDefinitionFile;
            if (df == null)
            {
                throw new ArgumentException("sender is not an EmvDefinitionFile");
            }

            if (df.TlvFci != null)
            {
                xmlRoot.Elements().Last().Add(df.TlvFci.ToXmlNode(xmlDoc));
                Console.WriteLine("  >> TLV: " + df.TlvFci);
                foreach (TlvData tlv in df.TlvFci.GetTags())
                {
                    WriteTlv(tlv.Tag, tlv, tagsManager);
                }
            }
        }

        private void BeforePseRead(Object sender, EmvEventArgs eventArgs)
        {
            Console.WriteLine();
            Console.WriteLine("=========== P S E   R e a d");
            Console.WriteLine();

            xmlRoot.Add(new XElement("PSERead"));
        }

        private void AfterPseRead(Object sender, EmvEventArgs eventArgs)
        {
            var pse = sender as PaymentSystemEnvironment;
            if (pse == null)
            {
                throw new ArgumentException("sender is not a PaymentSystemEnvironment");
            }

            Console.WriteLine();
            Console.WriteLine("= = = = = = P S E   R e a d");
            Console.WriteLine();

            foreach (var record in pse.TlvRecords)
            {
                foreach (TlvData tlv in record.GetTags())
                {
                    xmlRoot.Elements().Last().Add(tlv.ToXmlNode(xmlDoc));
                    WriteTlv(tlv.Tag, tlv, tagsManager);
                }
            }

            Console.WriteLine();
            Console.WriteLine("=========== P S E   A p p l i c a t i o n s   f o u n d");
            Console.WriteLine();

            foreach (var emvFound in pse.GetApplications())
            {
                Console.WriteLine("  >> Application '{0}' [ {1} ] found", emvFound.Aid, tagsManager.CreateInstance(emvFound.TlvFromPSE.GetTag(0x50)));
            }
        }

        private void BeforeApplicationSelection(Object sender, EmvEventArgs eventArgs)
        {
            var df = sender as EmvDefinitionFile;
            if (df == null)
            {
                throw new ArgumentException("sender is not an EmvDefinitionFile");
            }

            Console.WriteLine();
            Console.WriteLine("=========== E M V   A I D   S e l e c t i o n   {0}", df.Aid);
            Console.WriteLine();

            xmlRoot.Add(new XElement("ApplicationSelection"));
        }

        private void AfterApplicationSelection(Object sender, EmvEventArgs eventArgs)
        {
            var df = sender as EmvDefinitionFile;
            if (df == null)
            {
                throw new ArgumentException("sender is not an EmvDefinitionFile");
            }

            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   A I D   S e l e c t i o n   {0}", df.Aid);
            Console.WriteLine();

            if (df.TlvFci != null)
            {
                Console.WriteLine("  >> TLV: " + df.TlvFci);
                foreach (TlvData tlv in df.TlvFci.GetTags())
                {
                    xmlRoot.Elements().Last().Add(tlv.ToXmlNode(xmlDoc));
                    WriteTlv(tlv.Tag, tlv, tagsManager);
                }
            }
        }

        private void BeforeGetProcessingOptions(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            Console.WriteLine();
            Console.WriteLine("=========== E M V   G e t P r o c e s s i n g O p t i o n s   {0}", emv.Aid);
            Console.WriteLine();

            xmlRoot.Elements().Last().Add(new XElement("GetProcessingOptions"));
        }

        private void AfterGetProcessingOptions(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   G e t P r o c e s s i n g O p t i o n s   {0}", emv.Aid);
            Console.WriteLine();

            foreach (TlvData tlv in emv.TlvProcessingOptions.GetTags())
            {
                xmlRoot.Elements().Last().Add(tlv.ToXmlNode(xmlDoc));
                WriteTlv(tlv.Tag, tlv, tagsManager);
            }

            Console.WriteLine("    >> {0}", emv.Aip);
            foreach (var file in emv.Afl.Files)
            {
                Console.WriteLine("    >> {0}", file);
            }
        }

        private void BeforeReadApplicationData(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            Console.WriteLine();
            Console.WriteLine("=========== E M V   R e a d A p p l i c a t i o n D a t a   {0}", emv.Aid);
            Console.WriteLine();

            xmlRoot.Add(new XElement("ReadApplicationData"));
        }

        private void AfterReadApplicationData(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   R e a d A p p l i c a t i o n D a t a   {0}", emv.Aid);
            Console.WriteLine();

            foreach (var record in emv.TlvRecords)
            {
                foreach (TlvData tlv in record.GetTags())
                {
                    xmlRoot.Elements().Last().Add(tlv.ToXmlNode(xmlDoc));
                    WriteTlv(tlv.Tag, tlv, tagsManager);
                }
            }
        }

        private void BeforeGetData(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            Console.WriteLine();
            Console.WriteLine("=========== E M V   G e t D a t a   {0}", emv.Aid);
            Console.WriteLine();

            xmlRoot.Add(new XElement("GetData"));
        }

        private void AfterGetData(Object sender, EmvEventArgs eventArgs)
        {
            var emv = sender as EmvApplication;
            if (emv == null)
            {
                throw new ArgumentException("sender is not an EMVApplication");
            }

            Console.WriteLine();
            Console.WriteLine("= = = = = = E M V   G e t D a t a   {0}", emv.Aid);
            Console.WriteLine();

            if (emv.TlvATC != null)
            {
                xmlRoot.Elements().Last().Add(emv.TlvATC.ToXmlNode(xmlDoc));
                WriteTlv(emv.TlvATC.Tag, emv.TlvATC, tagsManager);
            }
            if (emv.TlvLastOnlineATCRegister != null)
            {
                xmlRoot.Elements().Last().Add(emv.TlvLastOnlineATCRegister.ToXmlNode(xmlDoc));
                WriteTlv(emv.TlvLastOnlineATCRegister.Tag, emv.TlvLastOnlineATCRegister, tagsManager);
            }
            if (emv.TlvPINTryCounter != null)
            {
                xmlRoot.Elements().Last().Add(emv.TlvPINTryCounter.ToXmlNode(xmlDoc));
                WriteTlv(emv.TlvPINTryCounter.Tag, emv.TlvPINTryCounter, tagsManager);
            }
            if (emv.TlvLogFormat != null)
            {
                xmlRoot.Elements().Last().Add(emv.TlvLogFormat.ToXmlNode(xmlDoc));
                WriteTlv(emv.TlvLogFormat.Tag, emv.TlvLogFormat, tagsManager);
            }
        }

        #endregion
    }
}