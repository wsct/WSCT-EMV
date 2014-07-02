using NUnit.Framework;
using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    [TestFixture]
    public class CryptographyUnitTest
    {
        [Test]
        public void ComputeHash()
        {
            var hash = Cryptography.ComputeHashSha1("sha1 this string".FromString());

            Assert.AreEqual("CF23DF2207D99A74FBE169E3EBA035E633B65D94", hash.ToHexa('\0'));
        }
    }
}
