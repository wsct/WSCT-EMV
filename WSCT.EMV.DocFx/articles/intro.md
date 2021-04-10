# A basic EMV transaction example
This example targets the home made EMV applet (AID F04341454E4201).

No success check is done in this code for more readability.

```csharp
var context = new CardContext();
context.Establish();

context.ListReaders("");

var rawChannel = new CardChannel(context, context.Readers.First());
var channel = new CardChannelIso7816(rawChannel);
channel.Connect(ShareMode.Exclusive, Protocol.Any);

var emv = new EmvApplication(channel);

var certificateAuthorities = new CertificationAuthorityRepository();
certificateAuthorities.Add("F0 43 41 45 4E", "01", new PublicKey("AC 1D C3 9D 33 7C 5A BD DF 3A 9D F5 C7 A0 0B 0F 28 22 F8 1D B7 E6 60 64 F1 42 03 50 E5 DF F4 46 8E 2F DA 95 32 EF E3 69 EE BC DA 30 C4 62 F2 54B0 D7 5F CB 68 82 A8 A4 73 F8 C1 AA 36 5D A1 3E 61 11 C0 17 0B 83 F6 33 3C D5 16 D7 FC CB F9 F9 B6 E9 56 74 89 3D 5D BD 93 43 40 56 43 37 D5 F33C D6 5A 01 07 33 32 56 7D C2 FA FD E5 9B AD 2D AD 67 DD E8 DD 63 08 F1 FC 09 D8 76 C4 F1 89 79", "010001"));
emv.CertificationAuthorityRepository = certificateAuthorities;

emv.Aid = "F0 43 41 45 4E 42 01";
emv.Select();

emv.GetProcessingOptions();

emv.ReadApplicationData();

emv.GetData();

emv.InternalAuthenticate("01020304".FromHexa());

var pinBlock = new PlaintextPINBlock();
pinBlock.ClearPIN = "1234".FromBcd();
emv.VerifyPin(pinBlock);

emv.TlvTerminalData.Add(new TlvData("9F02 06 000000001000")); // Amount, Authorised: Authorised amount of the transaction (excluding adjustments)
emv.TlvTerminalData.Add(new TlvData("9F03 06 000000000000")); // Amount, Other: Secondary amount associated with the transaction representing a cashback amount
emv.TlvTerminalData.Add(new TlvData("9F1A 02 0250")); // Terminal Country Code: Indicates the country of the terminal, represented according to ISO 3166
emv.TlvTerminalData.Add(new TlvData("5F2A 02 0978")); // Transaction Currency Code: Indicates the currency code of the transaction according to ISO 4217
emv.TlvTerminalData.Add(new TlvData("9A 03 210131")); // Transaction Date: Local date that the transaction was authorised
emv.TlvTerminalData.Add(new TlvData("9C 01 00")); // Transaction Type: Indicates the type of financial transaction, represented by the first two digits of the ISO 8583:1987 Processing Code. The actual values to be used for the Transaction Type data element are defined by the relevant payment system. 00: Purchase of goods and services - 01: Cash(ATM) - 20: Purchase return

emv.GenerateAc1(CryptogramType.TC, "31323334".FromHexa());

// ...

channel.Disconnect(Disposition.UnpowerCard);

context.Release();
```