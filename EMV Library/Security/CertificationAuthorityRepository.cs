using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents a collection of public keys of Certificate Authorities.
    /// </summary>
    [XmlRoot("certificationAuthorities")]
    public class CertificationAuthorityRepository : IXmlSerializable
    {
        #region >> Fields

        Dictionary<String, PublicKey> _keys;

        List<CertificationAuthority> _certificationAuthorities;

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="CertificationAuthorityRepository"/> instance.
        /// </summary>
        public CertificationAuthorityRepository()
        {
            _keys = new Dictionary<string, PublicKey>();
            _certificationAuthorities = new List<CertificationAuthority>();
        }

        #endregion

        #region >> Internal Methods

        private String GetIdentifier(String rid, String index)
        {
            return (rid + index).Replace(" ", "");
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Adds a new Authority Certificate public key.
        /// </summary>
        /// <param name="rid">RID of the EMV application.</param>
        /// <param name="index">Index of the Certificate Authority.</param>
        /// <param name="publicKey">The Public Key of the Certificate Authority.</param>
        public void Add(String rid, String index, PublicKey publicKey)
        {
            _keys.Add(GetIdentifier(rid, index), publicKey);
        }

        /// <summary>
        /// Get the EMV Certificate Authority public key associated with <paramref name="rid"/> and <paramref name="index"/>.
        /// </summary>
        /// <param name="rid">RID of the EMV application.</param>
        /// <param name="index">Index of the Certificate Authority.</param>
        /// <returns>The Public Key of the Certificate Authority.</returns>
        /// <exception cref="Exception">If no public key found.</exception>
        public PublicKey Get(String rid, String index)
        {
            PublicKey publicKey;
            if (_keys.TryGetValue(GetIdentifier(rid, index), out publicKey))
            {
                return publicKey;
            }

            throw new EMVCertificationAuthorityNotFoundException(String.Format("CertificateAuthorityRepository: no Certificate Authority found for RID+index [{0}+{1}]", rid, index));
        }

        #endregion

        #region >> IXmlSerializable Members

        /// <inheritdoc />
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc />
        public void ReadXml(XmlReader reader)
        {
            var serializer = new XmlSerializer(typeof(CertificationAuthority));
            reader.ReadStartElement();

            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        var ca = (CertificationAuthority)serializer.Deserialize(reader);
                        _certificationAuthorities.Add(ca);
                        _keys.Add(GetIdentifier(ca.Rid, ca.Index), ca.PublicKey);
                        break;
                    case XmlNodeType.Comment:
                        reader.Read();
                        break;
                }
            }
            if (reader.NodeType == XmlNodeType.EndElement)
            {
                reader.ReadEndElement();
            }
        }

        /// <inheritdoc />
        public void WriteXml(XmlWriter writer)
        {
            var serializer = new XmlSerializer(typeof(CertificationAuthority));
            foreach (var ca in _certificationAuthorities)
            {
                serializer.Serialize(writer, ca);
            }
        }

        #endregion
    }
}
