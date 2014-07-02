using System.Collections.Generic;
using NUnit.Framework;
using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    [TestFixture]
    public class Sha1HashAlgorithmProviderUnitTest
    {
        private readonly byte[] hashBytes = "CF23DF2207D99A74FBE169E3EBA035E633B65D94".FromHexa();

        [Test]
        public void ComputeHash1()
        {
            var sha1 = new Sha1HashAlgorithmProvider();

            var data = new List<byte[]> { "sha1 this string".FromString() };
            var hash = sha1.ComputeHash(data);

            Assert.AreEqual(hashBytes, hash);
        }

        [Test]
        public void ComputeHash2()
        {
            var sha1 = new Sha1HashAlgorithmProvider();

            var data = new List<byte[]> { "sha1".FromString(), " this string".FromString() };
            var hash = sha1.ComputeHash(data);

            Assert.AreEqual(hashBytes, hash);
        }

        [Test]
        public void ComputeHash3()
        {
            var sha1 = new Sha1HashAlgorithmProvider();

            var data = new List<byte[]> { "sha1".FromString(), " this".FromString(), " string".FromString() };
            var hash = sha1.ComputeHash(data);

            Assert.AreEqual(hashBytes, hash);
        }
    }
}