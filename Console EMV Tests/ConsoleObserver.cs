using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Core;
using WSCT.Core.APDU;
using WSCT.Wrapper;

using WSCT.Helpers;

namespace WSCT.ConsoleEMVTests
{
    class ConsoleObserver
    {
        internal String header;
        internal ConsoleColor highlightColor = ConsoleColor.White;
        internal ConsoleColor standardColor = ConsoleColor.Gray;

        #region Constructors

        public ConsoleObserver()
            : this("[{0,7}] Core ")
        {
        }

        public ConsoleObserver(String _header)
        {
            header = _header;
            __start();
        }

        #endregion

        internal virtual void __start()
        {
            Console.WriteLine(String.Format(header + "ConsoleObserver started", LogLevel.Info));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void observeContext(Core.CardContextObservable context)
        {
            context.afterEstablishEvent += notifyEstablish;
            context.afterListReaderGroupsEvent += notifyListReaderGroups;
            context.afterListReadersEvent += notifyListReaders;
            context.afterReleaseEvent += notifyRelease;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        public void observeChannel(Core.CardChannelObservable channel)
        {
            channel.beforeConnectEvent += beforeConnect;
            channel.afterConnectEvent += notifyConnect;

            channel.beforeDisconnectEvent += beforeDisconnect;
            channel.afterDisconnectEvent += notifyDisconnect;

            channel.beforeGetAttribEvent += beforeGetAttrib;
            channel.afterGetAttribEvent += notifyGetAttrib;

            channel.beforeReconnectEvent += beforeReconnect;
            channel.afterReconnectEvent += notifyReconnect;

            channel.beforeTransmitEvent += beforeTransmit;
            channel.afterTransmitEvent += notifyTransmit;
        }

        #region >> CardChannelObservable delegates

        public void notifyConnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Info, errorCode));
            else
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Warning, errorCode));
        }

        public void notifyDisconnect(ICardChannel cardChannel, Disposition disposition, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Info, errorCode));
            else
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Warning, errorCode));
        }

        public void notifyGetAttrib(ICardChannel cardChannel, Attrib attrib, Byte[] buffer, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
            {
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Info, errorCode));
                Console.WriteLine(String.Format(header + ">> Byte[]: [{1}]", LogLevel.Info, buffer.toHexa()));
                Console.WriteLine(String.Format(header + ">> String: \"{1}\"", LogLevel.Info, buffer.toString()));
            }
            else
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Warning, errorCode));
        }

        public void notifyReconnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, Disposition initialization, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Info, errorCode));
            else
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Warning, errorCode));
        }

        public void notifyTransmit(ICardChannel cardChannel, ICardCommand cardCommand, Byte[] recvBuffer, UInt32 recvSize, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
            {
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Info, errorCode));
                Console.WriteLine(String.Format(header + ">> RAPDU: [{1}]", LogLevel.Info, recvBuffer.toHexa((int)recvSize)));
            }
            else
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Warning, errorCode));
        }

        public void notifyTransmit(ICardChannel cardChannel, ICardCommand cardCommand, ICardResponse cardResponse, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
            {
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Info, errorCode));
                Console.WriteLine(String.Format(header + ">> RAPDU: [{1}]", LogLevel.Info, cardResponse));
            }
            else
                Console.WriteLine(String.Format(header + ">> Error: {1}", LogLevel.Warning, errorCode));
        }

        public void beforeConnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(String.Format(header + "connect(\"{1}\",{2},{3})", LogLevel.Info, cardChannel.readerName, shareMode, preferedProtocol));
            Console.ForegroundColor = standardColor;
        }

        public void beforeDisconnect(ICardChannel cardChannel, Disposition disposition)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(String.Format(header + "disconnect({1})", LogLevel.Info, disposition));
            Console.ForegroundColor = standardColor;
        }

        public void beforeGetAttrib(ICardChannel channel, Attrib attrib, Byte[] buffer)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(String.Format(header + "getAttrib({1})", LogLevel.Info, attrib));
            Console.ForegroundColor = standardColor;
        }

        public void beforeReconnect(ICardChannel cardChannel, ShareMode shareMode, Protocol preferedProtocol, Disposition initialization)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(String.Format(header + "reconnect({1},{2},{3})", LogLevel.Info, shareMode, preferedProtocol, initialization));
            Console.ForegroundColor = standardColor;
        }

        public void beforeTransmit(ICardChannel cardChannel, ICardCommand cardCommand, ICardResponse cardResponse)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(String.Format(header + "transmit({1})", LogLevel.Info, cardCommand));
            Console.ForegroundColor = standardColor;
        }

        #endregion

        #region >> CardContextObservable delegates

        public void notifyEstablish(ICardContext cardContext, ErrorCode errorCode)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(String.Format(header + "establish(): {1}", LogLevel.Info, errorCode));
            Console.ForegroundColor = standardColor;
        }

        public void notifyListReaders(ICardContext cardContext, string group, ErrorCode errorCode)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(String.Format(header + "listReaders({2}): {1}", LogLevel.Info, errorCode, group));
            Console.ForegroundColor = standardColor;
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
                foreach (String reader in cardContext.readers)
                    Console.WriteLine(String.Format(header + ">> Reader found: {1}", LogLevel.Info, reader));
        }

        public void notifyListReaderGroups(ICardContext cardContext, ErrorCode errorCode)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(String.Format(header + "listReaderGroups(): {1}", LogLevel.Info, errorCode));
            Console.ForegroundColor = standardColor;
            if (errorCode == ErrorCode.SCARD_S_SUCCESS)
                foreach (String group in cardContext.groups)
                    Console.WriteLine(String.Format(header + ">> Reader groups found: {1}", LogLevel.Info, group));
        }

        public void notifyRelease(ICardContext cardContext, ErrorCode errorCode)
        {
            Console.ForegroundColor = highlightColor;
            Console.WriteLine(String.Format(header + "release(): {1}", LogLevel.Info, errorCode));
            Console.ForegroundColor = standardColor;
        }

        #endregion
    }
}