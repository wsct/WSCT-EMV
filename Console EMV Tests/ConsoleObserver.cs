using System;
using WSCT.Core;
using WSCT.Core.Events;
using WSCT.Helpers;
using WSCT.Stack;
using WSCT.Wrapper;
using WSCT.Wrapper.Desktop.Core;

namespace WSCT.EMV.ConsoleTests
{
    internal class ConsoleObserver
    {
        internal string Header;
        internal ConsoleColor HighlightColor = ConsoleColor.White;
        internal ConsoleColor StandardColor = ConsoleColor.Gray;

        #region >> Constructors

        public ConsoleObserver()
        {
            Header = "[{0,7}] [{1,7}] {2}";
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ObserveContext(CardContextObservable context)
        {
            context.AfterEstablishEvent += NotifyEstablish;
            context.AfterGetStatusChangeEvent += NotifyGetStatusChange;
            context.AfterListReaderGroupsEvent += NotifyListReaderGroups;
            context.AfterListReadersEvent += NotifyListReaders;
            context.AfterReleaseEvent += NotifyRelease;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        public void ObserveChannel(CardChannelObservable channel)
        {
            channel.BeforeConnectEvent += BeforeConnect;
            channel.AfterConnectEvent += NotifyConnect;

            channel.BeforeDisconnectEvent += BeforeDisconnect;
            channel.AfterDisconnectEvent += NotifyDisconnect;

            channel.BeforeGetAttribEvent += BeforeGetAttrib;
            channel.AfterGetAttribEvent += NotifyGetAttrib;

            channel.BeforeReconnectEvent += BeforeReconnect;
            channel.AfterReconnectEvent += NotifyReconnect;

            channel.BeforeTransmitEvent += BeforeTransmit;
            channel.AfterTransmitEvent += NotifyTransmit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monitor"></param>
        public void ObserveMonitor(StatusChangeMonitor monitor)
        {
            monitor.OnCardInsertionEvent += OnCardInsertionEvent;
            monitor.OnCardRemovalEvent += OnCardRemovalEvent;
        }

        void WriteWarning(object sender, string message)
        {
            WriteLine(LogLevel.Warning, sender, message);
        }

        void WriteInfo(object sender, string message)
        {
            WriteLine(LogLevel.Info, sender, message);
        }

        void WriteError(object sender, string message)
        {
            WriteLine(LogLevel.Error, sender, message);
        }

        void WriteLine(LogLevel level, object sender, string message)
        {
            if (sender is ICardChannelLayerObservable channelLayer)
            {
                Console.WriteLine(Header, level, channelLayer.LayerId, message);
                return;
            }

            if (sender is ICardChannelObservable)
            {
                Console.WriteLine(Header, level, "", message);
            }

            if (sender is ICardContextLayerObservable contextLayer)
            {
                Console.WriteLine(Header, level, contextLayer.LayerId, message);
                return;
            }

            if (sender is ICardContextObservable)
            {
                Console.WriteLine(Header, level, "", message);
            }
        }

        #region >> CardChannelObservable delegates

        public void NotifyConnect(object sender, AfterConnectEventArgs eventArgs)
        {
            if (eventArgs.ReturnValue == ErrorCode.Success)
            {
                WriteInfo(sender, String.Format(">> Error: {0}", eventArgs.ReturnValue));
            }
            else
            {
                WriteWarning(sender, String.Format(">> Error: {0}", eventArgs.ReturnValue));
            }
        }

        public void NotifyDisconnect(object sender, AfterDisconnectEventArgs eventArgs)
        {
            if (eventArgs.ReturnValue == ErrorCode.Success)
            {
                WriteInfo(sender, String.Format(">> Error: {0}", eventArgs.ReturnValue));
            }
            else
            {
                WriteWarning(sender, String.Format(">> Error: {0}", eventArgs.ReturnValue));
            }
        }

        public void NotifyGetAttrib(object sender, AfterGetAttribEventArgs eventArgs)
        {
            if (eventArgs.ReturnValue == ErrorCode.Success)
            {
                WriteInfo(sender, String.Format(">> Error: {0}", eventArgs.ReturnValue));
                WriteInfo(sender, String.Format(">> byte[]: [{0}]", eventArgs.Buffer.ToHexa()));
                WriteInfo(sender, String.Format(">> string: \"{0}\"", eventArgs.Buffer.ToAsciiString()));
            }
            else
            {
                WriteWarning(sender, String.Format(Header + ">> Error: {0}", LogLevel.Warning, eventArgs.ReturnValue));
            }
        }

        public void NotifyReconnect(object sender, AfterReconnectEventArgs eventArgs)
        {
            if (eventArgs.ReturnValue == ErrorCode.Success)
            {
                WriteInfo(sender, String.Format(">> Error: {0}", eventArgs.ReturnValue));
            }
            else
            {
                WriteWarning(sender, String.Format(">> Error: {0}", eventArgs.ReturnValue));
            }
        }

        public void NotifyTransmit(object sender, AfterTransmitEventArgs eventArgs)
        {
            if (eventArgs.ReturnValue == ErrorCode.Success)
            {
                WriteInfo(sender, String.Format(">> Error: {0}", eventArgs.ReturnValue));
                WriteInfo(sender, String.Format(">> RAPDU: [{0}]", eventArgs.Response));
            }
            else
            {
                WriteWarning(sender, String.Format(">> Error: {0}", eventArgs.ReturnValue));
            }
        }

        public void BeforeConnect(object sender, BeforeConnectEventArgs eventArgs)
        {
            var cardChannel = (ICardChannel)sender;
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("Connect(\"{0}\",{1},{2})", cardChannel.ReaderName, eventArgs.ShareMode, eventArgs.PreferedProtocol));
            Console.ForegroundColor = StandardColor;
        }

        public void BeforeDisconnect(object sender, BeforeDisconnectEventArgs eventArgs)
        {
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("Disconnect({0})", eventArgs.Disposition));
            Console.ForegroundColor = StandardColor;
        }

        public void BeforeGetAttrib(object sender, BeforeGetAttribEventArgs eventArgs)
        {
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("GetAttrib({0})", eventArgs.Attrib));
            Console.ForegroundColor = StandardColor;
        }

        public void BeforeReconnect(object sender, BeforeReconnectEventArgs eventArgs)
        {
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("Reconnect({0},{1},{2})", eventArgs.ShareMode, eventArgs.PreferedProtocol, eventArgs.Initialization));
            Console.ForegroundColor = StandardColor;
        }

        public void BeforeTransmit(object sender, BeforeTransmitEventArgs eventArgs)
        {
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("Transmit({0})", eventArgs.Command));
            Console.ForegroundColor = StandardColor;
        }

        #endregion

        #region >> CardContextObservable delegates

        public void NotifyEstablish(object sender, AfterEstablishEventArgs eventArgs)
        {
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("Establish(): {0}", eventArgs.ReturnValue));
            Console.ForegroundColor = StandardColor;
        }

        private void NotifyGetStatusChange(object sender, AfterGetStatusChangeEventArgs eventArgs)
        {
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("GetStatusChange(): {0}", eventArgs.ReturnValue));
            Console.ForegroundColor = StandardColor;
            if (eventArgs.ReturnValue == ErrorCode.Success)
            {
                foreach (var readerState in eventArgs.ReaderStates)
                {
                    WriteInfo(sender, String.Format(">> {0}", readerState));
                }
            }
        }

        public void NotifyListReaders(object sender, AfterListReadersEventArgs eventArgs)
        {
            var cardContext = (ICardContext)sender;
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("ListReaders({1}): {0}", eventArgs.ReturnValue, eventArgs.Group));
            Console.ForegroundColor = StandardColor;
            if (eventArgs.ReturnValue == ErrorCode.Success)
            {
                foreach (var reader in cardContext.Readers)
                {
                    WriteInfo(sender, String.Format(">> Reader found: {0}", reader));
                }
            }
        }

        public void NotifyListReaderGroups(object sender, AfterListReaderGroupsEventArgs eventArgs)
        {
            var cardContext = (ICardContext)sender;
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("ListReaderGroups(): {0}", eventArgs.ReturnValue));
            Console.ForegroundColor = StandardColor;
            if (eventArgs.ReturnValue == ErrorCode.Success)
            {
                foreach (var group in cardContext.Groups)
                {
                    WriteInfo(sender, String.Format(">> Reader groups found: {0}", group));
                }
            }
        }

        public void NotifyRelease(object sender, AfterReleaseEventArgs eventArgs)
        {
            Console.ForegroundColor = HighlightColor;
            WriteInfo(sender, String.Format("Release(): {0}", eventArgs.ReturnValue));
            Console.ForegroundColor = StandardColor;
        }

        #endregion

        #region >> StatusChangeMonitor delegates

        private void OnCardInsertionEvent(object sender, OnCardInsertionEventArgs eventArgs)
        {
            Console.ForegroundColor = StandardColor;
            WriteInfo(sender, String.Format(">> Card insertion detected on reader {0}", eventArgs.ReaderState.ReaderName));
        }

        private void OnCardRemovalEvent(object sender, OnCardRemovalEventArgs eventArgs)
        {
            Console.ForegroundColor = StandardColor;
            WriteInfo(sender, String.Format(">> Card removal detected on reader {0}", eventArgs.ReaderState.ReaderName));
        }

        #endregion
    }
}