namespace WSCT.EMV
{
    /// <summary>
    /// Enumeration of Cryptogram Types.
    /// </summary>
    public enum CryptogramType
    {
        /// <summary>
        /// Application Authentication Cryptogram - Transaction declined.
        /// </summary>
        AAC = 0x00,
        /// <summary>
        /// Authorisation Request Cryptogram - Online authorisation requested.
        /// </summary>
        TC = 0x40,
        /// <summary>
        /// Transaction Certificate - Transaction approuved.
        /// </summary>
        ARQC = 0x80,
        /// <summary>
        /// Undefined Cryptogram Type.
        /// </summary>
        Undefined = 0xC0
    }
}
