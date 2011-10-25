using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Helpers;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV Dynamic Data Authentication certificate (DDA)
    /// </summary>
    public class DynamicDataAuthentication : AbstractDataAuthenticationFormat05
    {
        #region >> Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DynamicDataAuthentication()
            : base()
        {
        }

        #endregion
    }
}
