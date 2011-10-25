using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Helpers;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace WSCT.EMV.Security
{
    // TODO: implement CDA
    /// <summary>
    /// Represents an EMV Combined Data Authentication certificate (CDA)
    /// </summary>
    public class CombinedDataAuthentication : AbstractDataAuthenticationFormat05
    {
        #region >> Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CombinedDataAuthentication()
            : base()
        {
        }

        #endregion
    }
}
