using System;
using System.Collections.Generic;
using System.Text;

using WSCT;
using WSCT.Wrapper;
using WSCT.Core;
using WSCT.Core.APDU;
using WSCT.ISO7816;
using WSCT.ISO7816.Commands;
using WSCT.Stack;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// Internal wrapper that allows APDU adaptation for T=0 smartcards
    /// </summary>
    /// <remarks>
    /// TODO use a ICardChannelStack/Layer instead of this wrapper
    /// </remarks>
    class CardChannelTerminalTransportLayer : ICardChannel, ICardChannelObservable
    {
        #region >> Fields

        ICardChannel _cardChannel;

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardChannelTerminalTransportLayer(ICardChannel cardChannel)
        {
            _cardChannel = cardChannel;
        }

        #endregion

        #region >> ICardChannel Membres

        /// <inheritdoc />
        public Protocol protocol
        {
            get { return _cardChannel.protocol; }
        }

        /// <inheritdoc />
        public string readerName
        {
            get { return _cardChannel.readerName; }
        }

        /// <inheritdoc />
        public void attach(ICardContext context, string readerName)
        {
            _cardChannel.attach(context, readerName);
        }

        /// <inheritdoc />
        public ErrorCode connect(ShareMode shareMode, Protocol preferedProtocol)
        {
            if (beforeConnectEvent != null) beforeConnectEvent(this, shareMode, preferedProtocol);

            ErrorCode ret = _cardChannel.connect(shareMode, preferedProtocol);

            if (afterConnectEvent != null) afterConnectEvent(this, shareMode, preferedProtocol, ret);

            return ret;
        }

        /// <inheritdoc />
        public ErrorCode disconnect(Disposition disposition)
        {
            if (beforeDisconnectEvent != null) beforeDisconnectEvent(this, disposition);

            ErrorCode ret = _cardChannel.disconnect(disposition);

            if (afterDisconnectEvent != null) afterDisconnectEvent(this, disposition, ret);

            return ret;
        }

        /// <inheritdoc />
        public ErrorCode getAttrib(Attrib attrib, ref byte[] buffer)
        {
            if (beforeGetAttribEvent != null) beforeGetAttribEvent(this, attrib, buffer);

            ErrorCode ret = _cardChannel.getAttrib(attrib, ref buffer);

            if (afterGetAttribEvent != null) afterGetAttribEvent(this, attrib, buffer, ret);

            return ret;
        }

        /// <inheritdoc />
        public State getStatus()
        {
            if (beforeGetStatusEvent != null) beforeGetStatusEvent(this);

            State ret = _cardChannel.getStatus();

            if (afterGetStatusEvent != null) afterGetStatusEvent(this, ret);

            return ret;
        }

        /// <inheritdoc />
        public ErrorCode reconnect(ShareMode shareMode, Protocol preferedProtocol, Disposition initialization)
        {
            if (beforeReconnectEvent != null) beforeReconnectEvent(this, shareMode, preferedProtocol, initialization);

            ErrorCode ret = _cardChannel.reconnect(shareMode, preferedProtocol, initialization);

            if (afterReconnectEvent != null) afterReconnectEvent(this, shareMode, preferedProtocol, initialization, ret);

            return ret;
        }

        /// <inheritdoc />
        public ErrorCode transmit(ICardCommand command, ICardResponse response)
        {
            if (beforeTransmitEvent != null) beforeTransmitEvent(this, command, response);

            ErrorCode ret;

            // Adapt APDU for T=0 smartcards
            if (protocol == Protocol.SCARD_PROTOCOL_T0)
            {
                ret = transmitT0((CommandAPDU)command, (ResponseAPDU)response);
            }
            // T=1 smartcards
            else
            {
                ret = _cardChannel.transmit(command, response);
            }

            if (afterTransmitEvent != null) afterTransmitEvent(this, command, response, ret);

            return ret;
        }

        #endregion

        #region >> ICardChannelObservable Membres

        /// <inheritdoc />
        public event beforeConnect beforeConnectEvent;

        /// <inheritdoc />
        public event afterConnect afterConnectEvent;

        /// <inheritdoc />
        public event beforeDisconnect beforeDisconnectEvent;

        /// <inheritdoc />
        public event afterDisconnect afterDisconnectEvent;

        /// <inheritdoc />
        public event beforeGetAttrib beforeGetAttribEvent;

        /// <inheritdoc />
        public event afterGetAttrib afterGetAttribEvent;

        /// <inheritdoc />
        public event beforeGetStatus beforeGetStatusEvent;

        /// <inheritdoc />
        public event afterGetStatus afterGetStatusEvent;

        /// <inheritdoc />
        public event beforeReconnect beforeReconnectEvent;

        /// <inheritdoc />
        public event afterReconnect afterReconnectEvent;

        /// <inheritdoc />
        public event beforeTransmit beforeTransmitEvent;

        /// <inheritdoc />
        public event afterTransmit afterTransmitEvent;

        #endregion

        #region >> Methods

        /// <inheritdoc />
        public ErrorCode transmitT0(CommandAPDU cAPDU, ResponseAPDU rAPDU)
        {
            ErrorCode ret;

            // Adapt APDU for T=0 smartcards
            // If C-APDU is CC1: add P3=0
            if (cAPDU.isCC1)
            {
                CommandResponsePair crp = new CommandResponsePair(cAPDU);
                crp.cAPDU.le = 0;
                crp.rAPDU = rAPDU;
                ret = crp.transmit(_cardChannel);
            }
            // If C-APDU is CC2: test SW1=61/6C
            else if (cAPDU.isCC2)
            {
                CommandResponsePair crp = new CommandResponsePair(cAPDU);
                crp.rAPDU = rAPDU;
                ret = crp.transmit(_cardChannel);
                if (ret == ErrorCode.SCARD_S_SUCCESS && crp.rAPDU.sw1 == 0x61)
                {
                    CommandResponsePair crpGetResponse = new CommandResponsePair(new GetResponseCommand(crp.rAPDU.sw2));
                    crpGetResponse.rAPDU = rAPDU;
                    ret = crpGetResponse.transmit(_cardChannel);
                }
                else if (ret == ErrorCode.SCARD_S_SUCCESS && crp.rAPDU.sw1 == 0x6C)
                {
                    CommandResponsePair crpWithLe = new CommandResponsePair();
                    crpWithLe.cAPDU = crp.cAPDU;
                    crpWithLe.cAPDU.le = crp.rAPDU.sw2;
                    crpWithLe.rAPDU = rAPDU;
                    ret = crpWithLe.transmit(_cardChannel);
                }
                else
                {
                    // last rAPDU must be returned as is
                }
            }
            // If C-APDU is CC3: nothing to do
            else if (cAPDU.isCC3)
            {
                CommandResponsePair crp = new CommandResponsePair(cAPDU);
                crp.rAPDU = rAPDU;
                ret = crp.transmit(_cardChannel);
            }
            // If C-APDU is CC4: first CC3 then CC2 GET RESPONSE
            else
            {
                UInt32 le = cAPDU.le;
                cAPDU.hasLe = false;
                CommandResponsePair crp = new CommandResponsePair(cAPDU);
                crp.rAPDU = rAPDU;
                ret = crp.transmit(_cardChannel);
                if (ret == ErrorCode.SCARD_S_SUCCESS && crp.rAPDU.sw1 == 0x61)
                {
                    CommandResponsePair crpGetResponse = new CommandResponsePair(new GetResponseCommand(crp.rAPDU.sw2));
                    crpGetResponse.rAPDU = rAPDU;
                    ret = crpGetResponse.transmit(_cardChannel);
                }
                else if (ret == ErrorCode.SCARD_S_SUCCESS && crp.rAPDU.sw1 == 0x6C)
                {
                    CommandResponsePair crpWithLe = new CommandResponsePair();
                    crpWithLe.cAPDU = crp.cAPDU;
                    crpWithLe.cAPDU.le = crp.rAPDU.sw2;
                    crpWithLe.rAPDU = rAPDU;
                    ret = crpWithLe.transmit(_cardChannel);
                }
                else
                {
                    // last rAPDU must be returned as is
                }
                // Restore initial cAPDU for logs
                cAPDU.hasLe = true;
            }

            return ret;
        }

        #endregion
    }
}
