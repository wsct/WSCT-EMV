using System;
using WSCT.Core;
using WSCT.Core.APDU;
using WSCT.Helpers;
using WSCT.Wrapper;
using WSCT.Wrapper.Desktop.Core;

namespace WSCT.ConsoleEMVTests
{
    internal class ConsoleObserver
    {
        internal string header;
        internal ConsoleColor highlightColor = ConsoleColor.White;
        internal ConsoleColor standardColor = ConsoleColor.Gray;

        #region >> Constructors

        public ConsoleObserver()
            : this("[{0,7}] Core ")
        {
        }

        public ConsoleObserver(string _header)
        {
            header = _header;
            __start();
        }

        #endregion

        internal virtual void __start()
        {
            Console.WriteLine(header + "ConsoleObserver started", LogLevel.Info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void observeContext(CardContextObservable context)
        {
            context.AfterEstablishEvent += notifyEstablish;
            context.AfterGetStatusChangeEvent += notifyGetStatusChange;
            context.AfterListReaderGroupsEvent += notifyListReaderGroups;
            context.AfterListReadersEvent += notifyListReaders;
            context.AfterReleaseEvent += notifyRelease;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        public void observeChannel(CardChannelObservable channel)
        {
            channel.BeforeConnectEvent += beforeConnect;
            channel.AfterConnectEvent += notifyConnect;

            channel.BeforeDisconnectEvent += beforeDisconnect;
            channel.AfterDisconnectEvent += notifyDisconnect;

            channel.BeforeGetAttribEvent += beforeGetAttrib;
            channel.AfterGetAttribEvent += notifyGetAttrib;

            channel.BeforeReconnectEvent += beforeReconnect;
            channel.AfterReconnectEvent += notifyReconnect;

            channel.BeforeTransmitEvent += beforeTransmit;
            channel.AfterTransmitEvent += notifyTransmit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monitor"></param>
        public void observeMonitor(StatusChangeMonitor monitor)
        {
            monitor.OnCardInsertionEvent += onCardInsertionEvent;
            monitor.OnCardRemovalEvent += onCardRemovalEvent;
        }

        #region >> CardChannelObservable delegates

        public void notifyConnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.Success)
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Info, errorCode);
            }
            else
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Warning, errorCode);
            }
        }

        public void notifyDisconnect(ICardChannel cardChannel, Disposition disposition, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.Success)
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Info, errorCode);
            }
            else
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Warning, errorCode);
            }
        }

        public void notifyGetAttrib(ICardChannel cardChannel, Attrib attrib, byte[] buffer, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.Success)
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Info, errorCode);
                Console.WriteLine(header + ">> byte[]: [{1}]", LogLevel.Info, buffer.ToHexa());
                Console.WriteLine(header + ">> string: \"{1}\"", LogLevel.Info, buffer.ToAsciiString());
            }
            else
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Warning, errorCode);
            }
        }

        public void notifyReconnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, Disposition initialization, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.Success)
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Info, errorCode);
            }
            else
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Warning, errorCode);
            }
        }

        public void notifyTransmit(ICardChannel cardChannel, ICardCommand cardCommand, ICardResponse cardResponse, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.Success)
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Info, errorCode);
                Console.WriteLine(header + ">> RAPDU: [{1}]", LogLevel.Info, cardResponse);
            }
            else
            {
                Console.WriteLine(header + ">> Error: {1}", LogLevel.Warning, errorCode);
            }
        }

        public void beforeConnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "connect(\"{1}\",{2},{3})", LogLevel.Info, cardChannel.ReaderName, shareMode, preferedProtocol);
            Console.ForegroundColor = standardColor;
        }

        public void beforeDisconnect(ICardChannel cardChannel, Disposition disposition)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "disconnect({1})", LogLevel.Info, disposition);
            Console.ForegroundColor = standardColor;
        }

        public void beforeGetAttrib(ICardChannel channel, Attrib attrib, byte[] buffer)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "getAttrib({1})", LogLevel.Info, attrib);
            Console.ForegroundColor = standardColor;
        }

        public void beforeReconnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, Disposition initialization)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "reconnect({1},{2},{3})", LogLevel.Info, shareMode, preferedProtocol, initialization);
            Console.ForegroundColor = standardColor;
        }

        public void beforeTransmit(ICardChannel cardChannel, ICardCommand cardCommand, ICardResponse cardResponse)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "transmit({1})", LogLevel.Info, cardCommand);
            Console.ForegroundColor = standardColor;
        }

        #endregion

        #region >> CardContextObservable delegates

        public void notifyEstablish(ICardContext cardContext, ErrorCode errorCode)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "establish(): {1}", LogLevel.Info, errorCode);
            Console.ForegroundColor = standardColor;
        }

        private void notifyGetStatusChange(ICardContext cardContext, UInt32 timeout, AbstractReaderState[] readerStates, ErrorCode errorCode)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "getStatusChange(): {1}", LogLevel.Info, errorCode);
            Console.ForegroundColor = standardColor;
            if (errorCode == ErrorCode.Success)
            {
                foreach (var readerState in readerStates)
                {
                    Console.WriteLine(header + ">> {2}", LogLevel.Info, readerState.EventState, readerState);
                }
            }
        }

        public void notifyListReaders(ICardContext cardContext, string group, ErrorCode errorCode)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "listReaders({2}): {1}", LogLevel.Info, errorCode, @group);
            Console.ForegroundColor = standardColor;
            if (errorCode == ErrorCode.Success)
            {
                foreach (var reader in cardContext.Readers)
                {
                    Console.WriteLine(header + ">> Reader found: {1}", LogLevel.Info, reader);
                }
            }
        }

        public void notifyListReaderGroups(ICardContext cardContext, ErrorCode errorCode)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "listReaderGroups(): {1}", LogLevel.Info, errorCode);
            Console.ForegroundColor = standardColor;
            if (errorCode == ErrorCode.Success)
            {
                foreach (var group in cardContext.Groups)
                {
                    Console.WriteLine(header + ">> Reader groups found: {1}", LogLevel.Info, @group);
                }
            }
        }

        public void notifyRelease(ICardContext cardContext, ErrorCode errorCode)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(header + "release(): {1}", LogLevel.Info, errorCode);
            Console.ForegroundColor = standardColor;
        }

        #endregion

        #region >> StatusChangeMonitor delegates

        private void onCardInsertionEvent(AbstractReaderState readerState)
        {
            Console.ForegroundColor = standardColor;
            Console.WriteLine(header + ">> Card insertion detected on reader {1}", LogLevel.Info, readerState.ReaderName);
        }

        private void onCardRemovalEvent(AbstractReaderState readerState)
        {
            Console.ForegroundColor = standardColor;
            Console.WriteLine(header + ">> Card removal detected on reader {1}", LogLevel.Info, readerState.ReaderName);
        }

        #endregion
    }
}