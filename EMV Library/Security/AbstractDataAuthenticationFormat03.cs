namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV Data Authentication (Data Format: 03)
    /// </summary>
    /// <remarks>Format used for SDA</remarks>
    public abstract class AbstractDataAuthenticationFormat03 : AbstractSignatureContainer
    {
        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AbstractDataAuthenticationFormat03()
            : base(2)
        {
        }

        #endregion
    }
}
