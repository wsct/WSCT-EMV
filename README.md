WSCT-EMV
=========

Public repository for WSCT EMV project.

Developed by S.Vernois @ ENSICAEN / GREYC, with the help of some students.

# EMV Library

**Disclaimer**: The EMV library is **not "production ready"** nor does it target this state.

Its purpose is to allow to follow or not follow EMV transaction steps (as described by EMVCo specifications) for teaching and experimenting purposes only.

EMV Cryptography requirements are only partially supported (no ciphered PIN for example).
If the public keys of the card AC is set then issuer and ICC certificates can be checked and deciphered, data integrity can be verified.

# WSCT Plugin

The plugin "EMV Explorer" allows visual interactions (commands and responses interpretation) with an EMV card.

**Warning**: Some features can be dangerous for the card. For example, a wrong PIN presentation may block the card or ARQC send with high amount may prevent the use of the card in offline mode.

# Command line tools for EMV card personalization

**Disclaimer**: these tools aren't professional tools but allow easy personalization of our home made test EMV cards.

These tools use predefined input and output files (name and format). No implicit parameters are supported.

## RSA key generation : 
```
wsct-emvrsa.exe
```
> Generates a new RSA key (size: 1024).

*Input*: none

*Output*:
* `emv-rsa.json`: contains the generated key (private & public).

## EMV Issuer key generation
```
wsct-emvissuer.exe
```
> Generates the issuer public key certificate.

*Input*:
* `certificate-authority.json`: public key of the certification authority.
* `issuer-certificate-data.json`: issuer's data and key.

*Output*:
* `emv-issuer-context.json`: issuer's public key certificate and its private key.

## EMV ICC key generation
```
wsct-emvicc
```
> Generates the ICC public key certificate.

*Input*: 
* `issuer-certificate-data.json`
* `icc-certificate-data.json`: ICC's data and key.

*Output*:
* `emv-icc-context.json`: ICC's public key certificate and its private key.

## EMV DGI creation
```
wsct-emvcp
```
> Generates the DGI to be send with STORE DATA commands to the card:
* FCI (answer to SELECT command)
* GPO (answer to GET PROCESSING OPTIONS command)
  * AIP is defined in the card data
  * AFL is computed from the defined records
* RECORDS (answers to READ RECORD commands)

*Input*: 
* `emv-card-model.json`: structure of the data in the card (records in files and some specific data).
* `emv-card-data.json`: data values for the card being personalized.
* `emv-issuer-context.json`
* `emv-icc-context.json`

*Output*:
* `emv-cardpersonalisation-dgi.json`: generated DGI for the card (ACID, FCI, GPO, Records).

## Files format

### RSA key format
 Files: `emv-rsa.json`, `certificate-authority.json`.
```json
{
  "Modulus": "00...99",
  "PrivateExponent": "15...BD",
  "PublicExponent": "010001"
}
```

### Issuer certificate format
Files: `issuer-certificate-data.json`.
```json
{
    "CaPublicKeyIndex": "01",

    "IssuerPrivateKey": {
        "PrivateExponent": "8B...21",
        "Modulus": "94...AB",
        "PublicExponent": "010001"
    },

    "HashAlgorithmIndicator": "01",
    "IssuerIdentifier": "925042",
    "ExpirationDate": "0720",
    "SerialNumber": "000001",
    "PublicKeyAlgorithmIndicator": "01"
}
```

### Issuer context format
Files: `emv-issuer-context`.
```json
{
  "CaPublicKeyIndex": "01",
  "IssuerPrivateKey": {
    "Modulus": "94...AB",
    "PrivateExponent": "8B...21",
    "PublicExponent": "01 00 01"
  },
  "IssuerPublicKeyCertificate": "28...8F",
  "IssuerPublicKeyRemainder": "26...AB"
}
```

### ICC context format
Files: `emv-icc-context.json`.
```json
{
  "ApplicationPan": "9250 4220 2311 0001",
  "IccPrivateKey": {
    "Modulus": "94...AB",
    "PrivateExponent": "8B...21",
    "PublicExponent": "01 00 01"
  },
  "IccPublicKeyCertificate": "16...AE",
  "IccPublicKeyRemainder": "C8...AB"
}
```

### Card model format
Files: `emv-card-model.json`.

### Card data format
Files: `emv-card-data.json`.

### Card DGI format
Files: `emv-cardpersonalisation-dgi.json`.
