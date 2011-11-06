using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents a collection of public keys of Certificate Authorities
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
        /// Default constructor
        /// </summary>
        public CertificationAuthorityRepository()
        {
            _keys = new Dictionary<string, PublicKey>();
            _certificationAuthorities = new List<CertificationAuthority>();
        }

        #endregion

        #region >> Internal Methods

        private String getIdentifier(String rid, String index)
        {
            return (rid + index).Replace(" ", "");
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Add a new Certificate Authority public key
        /// </summary>
        /// <param name="rid">RID of the EMV application</param>
        /// <param name="index">Index of the Certificate Authority</param>
        /// <param name="publicKey">The Public Key of the Certificate Authority</param>
        public void add(String rid, String index, PublicKey publicKey)
        {
            _keys.Add(getIdentifier(rid, index), publicKey);
        }

        /// <summary>
        /// Get the EMV Certificate Authority public key associated with <paramref name="rid"/> and <paramref name="index"/>
        /// </summary>
        /// <param name="rid">RID of the EMV application</param>
        /// <param name="index">Index of the Certificate Authority</param>
        /// <returns>The Public Key of the Certificate Authority</returns>
        /// <exception cref="Exception">If no public key found</exception>
        public PublicKey get(String rid, String index)
        {
            PublicKey publicKey;
            if (_keys.TryGetValue(getIdentifier(rid, index), out publicKey))
                return publicKey;
            else
                throw new EMVCertificationAuthorityNotFoundException(String.Format("CertificateAuthorityRepository: no Certificate Authority found for RID+index [{0}+{1}]", rid, index));
        }

        #endregion

        #region >> IXmlSerializable Members

        /// <inheritdoc />
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc />
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CertificationAuthority));
            reader.ReadStartElement();

            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        CertificationAuthority ca = (CertificationAuthority)serializer.Deserialize(reader);
                        _certificationAuthorities.Add(ca);
                        _keys.Add(getIdentifier(ca.rid, ca.index), ca.publicKey);
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
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CertificationAuthority));
            foreach (CertificationAuthority ca in _certificationAuthorities)
            {
                serializer.Serialize(writer, ca);
            }
        }

        #endregion
    }
}
