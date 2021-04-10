# WSCT EMV documentation.

## What is WSCT EMV?
WSCT EMV is made of a library and several tools based on [WSCT][] framework allowing communication with a contact EMV smart card (see [EMV specifications][] for more details).

The project is hosted on [Github](https://github.com/wsct/WSCT-EMV) but may be private for you as I don't want the code to be public. Only the API documentation is public.

Developed by S.Vernois @ [ENSICAEN][] / [GREYC][] with the help of some students for proof of concepts.

> [!CAUTION]
> Disclaimer: The EMV library is not "production ready" nor does it target this state.
>
> Its purpose is to allow to follow or not follow EMV transaction steps (as described by [EMV specifications][]) for teaching and experimenting purposes only.
> 
> EMV Cryptography requirements are only partially supported (no ciphered PIN for example). If the public keys of the card AC is set then issuer and ICC certificates can be checked and deciphered, data integrity can be verified.

## The projects

### EMV Library
Defines the implementation of the EMV specification.

### Plugin EMV Explorer
Defines a plugin allowing to execute an EMV transaction through the [WSCT GUI][] graphic tool.

### Plugin EMV Personalization
Defines a plugin allowing to personalize PSE and EMV applets, based on the [EMV Card Personalization Specification 1.1][].

> [!WARNING]
> It targets the home made PSE and EMV applets and may not work with applets made by other vendors.

### Command line tools
This tools are to prepare the personalization data, from the Certification Authority to the the DGI.

#### EMV RSA Key Generation
Generates a new RSA key (size: 1024).

#### EMV Issuer Certificate Generation
Generates the issuer public key certificate.

#### EMV ICC Certificate Generation
Generates the ICC public key certificate.

#### EMV Card Personalization
Generates the DGI to be send with STORE DATA commands to the card.

[WSCT]: https://github.com/wsct/WSCT-Core/
[EMV specifications]: https://www.emvco.com/emv-technologies/contact/
[WSCT GUI]: https://github.com/wsct/WSCT-GUI
[EMV Card Personalization Specification 1.1]: https://www.emvco.com/emv-technologies/contact/
[ENSICAEN]: https://www.ensicaen.fr/
[GREYC]: https://www.greyc.fr/en/equipes/safe-2/
