using WSCT.Core;
using WSCT.Core.APDU;
using WSCT.ISO7816;
using WSCT.ISO7816.Commands;
using WSCT.Wrapper;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// Internal wrapper that allows APDU adaptation for T=0 smartcards.
    /// </summary>
    /// <remarks>
    /// TODO use a ICardChannelStack/Layer instead of this wrapper
    /// </remarks>
    internal class CardChannelTerminalTransportLayer : ICardChannel
    {
        #region >> Fields

        private readonly ICardChannel _cardChannel;

        #endregion

        #region >> Properties

        /// <summary>
        /// Set it to <c>false</c> for a TTL conforming the EMV specification, or <c>true</c> for a T=1 permissive mode.
        /// </summary>
        public bool StrictT1 { get; set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="CardChannelTerminalTransportLayer"/> instance.
        /// </summary>
        /// <param name="cardChannel"></param>
        public CardChannelTerminalTransportLayer(ICardChannel cardChannel)
        {
            _cardChannel = cardChannel;
        }

        #endregion

        #region >> ICardChannel Membres

        /// <inheritdoc />
        public Protocol Protocol
        {
            get { return _cardChannel.Protocol; }
        }

        /// <inheritdoc />
        public string ReaderName
        {
            get { return _cardChannel.ReaderName; }
        }

        /// <inheritdoc />
        public void Attach(ICardContext context, string readerName)
        {
            _cardChannel.Attach(context, readerName);
        }

        /// <inheritdoc />
        public ErrorCode Connect(ShareMode shareMode, Protocol preferedProtocol)
        {
            var ret = _cardChannel.Connect(shareMode, preferedProtocol);

            return ret;
        }

        /// <inheritdoc />
        public ErrorCode Disconnect(Disposition disposition)
        {
            var ret = _cardChannel.Disconnect(disposition);

            return ret;
        }

        /// <inheritdoc />
        public ErrorCode GetAttrib(Attrib attrib, ref byte[] buffer)
        {
            var ret = _cardChannel.GetAttrib(attrib, ref buffer);

            return ret;
        }

        /// <inheritdoc />
        public State GetStatus()
        {
            var ret = _cardChannel.GetStatus();

            return ret;
        }

        /// <inheritdoc />
        public ErrorCode Reconnect(ShareMode shareMode, Protocol preferedProtocol, Disposition initialization)
        {
            var ret = _cardChannel.Reconnect(shareMode, preferedProtocol, initialization);

            return ret;
        }

        /// <inheritdoc />
        public ErrorCode Transmit(ICardCommand command, ICardResponse response)
        {
            ErrorCode ret;

            // Adapt APDU for T=0 smartcards
            if (Protocol == Protocol.T0)
            {
                ret = TransmitT0((CommandAPDU)command, (ResponseAPDU)response);
            }
            // T=1 smartcards
            else
            {
                ret = _cardChannel.Transmit(command, response);
            }

            return ret;
        }

        #endregion

        #region >> Methods

        private ErrorCode TransmitT0(CommandAPDU cAPDU, ResponseAPDU rAPDU)
        {
            ErrorCode ret;

            // Adapt APDU for T=0 smartcards
            // If C-APDU is CC1: add P3=0
            if (cAPDU.IsCc1)
            {
                var crp = new CommandResponsePair(cAPDU) { CApdu = { Le = 0 }, RApdu = rAPDU };
                ret = crp.Transmit(_cardChannel);
            }
            // If C-APDU is CC2: test SW1=61/6C
            else if (cAPDU.IsCc2)
            {
                var crp = new CommandResponsePair(cAPDU);
                // Let the crp create a new crp.rAPDU
                ret = crp.Transmit(_cardChannel);
                if (ret == ErrorCode.Success && crp.RApdu.Sw1 == 0x61)
                {
                    var crpGetResponse = new CommandResponsePair(new GetResponseCommand(crp.RApdu.Sw2)) { RApdu = rAPDU };
                    ret = crpGetResponse.Transmit(_cardChannel);
                }
                else if (ret == ErrorCode.Success && crp.RApdu.Sw1 == 0x6C)
                {
                    var crpWithLe = new CommandResponsePair { CApdu = crp.CApdu };
                    crpWithLe.CApdu.Le = crp.RApdu.Sw2;
                    crpWithLe.RApdu = rAPDU;
                    ret = crpWithLe.Transmit(_cardChannel);
                }
                else
                {
                    // last rAPDU must be returned as is
                    rAPDU.Udr = crp.RApdu.Udr;
                    rAPDU.Sw1 = crp.RApdu.Sw1;
                    rAPDU.Sw2 = crp.RApdu.Sw2;
                }
            }
            // If C-APDU is CC3: nothing to do
            else if (cAPDU.IsCc3)
            {
                var crp = new CommandResponsePair(cAPDU) { RApdu = rAPDU };
                ret = crp.Transmit(_cardChannel);
            }
            // If C-APDU is CC4: first CC3 then CC2 GET RESPONSE
            else
            {
                cAPDU.HasLe = false;
                var crp = new CommandResponsePair(cAPDU);
                // Let the crp create a new crp.rAPDU
                ret = crp.Transmit(_cardChannel);
                if (ret == ErrorCode.Success && crp.RApdu.Sw1 == 0x61)
                {
                    var crpGetResponse = new CommandResponsePair(new GetResponseCommand(crp.RApdu.Sw2)) { RApdu = rAPDU };
                    ret = crpGetResponse.Transmit(_cardChannel);
                }
                else if (ret == ErrorCode.Success && crp.RApdu.Sw1 == 0x6C)
                {
                    var crpWithLe = new CommandResponsePair { CApdu = crp.CApdu };
                    crpWithLe.CApdu.Le = crp.RApdu.Sw2;
                    crpWithLe.RApdu = rAPDU;
                    ret = crpWithLe.Transmit(_cardChannel);
                }
                else
                {
                    // last rAPDU must be returned as is
                    rAPDU.Udr = crp.RApdu.Udr;
                    rAPDU.Sw1 = crp.RApdu.Sw1;
                    rAPDU.Sw2 = crp.RApdu.Sw2;
                }
                // Restore initial cAPDU for logs
                cAPDU.HasLe = true;
            }

            return ret;
        }

        #endregion
    }
}