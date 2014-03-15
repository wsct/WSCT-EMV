using System;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Exception called when a Certification Authority was not found in a <see cref="CertificationAuthorityRepository"/>
    /// </summary>
    public class EMVCertificationAuthorityNotFoundException : Exception
    {
        /// <inheritdoc />
        public EMVCertificationAuthorityNotFoundException()
            : base("Certification Authority not found")
        {
        }

        /// <inheritdoc />
        public EMVCertificationAuthorityNotFoundException(String message)
            : base(message)
        {
        }
    }
}
