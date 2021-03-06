﻿using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Cryptography methods used by EMV Specification.
    /// </summary>
    /// <remarks>
    /// From EMV Book 2:
    /// <para> Symmetric Algorithms: The double-length key triple DES encipherment algorithm (see ISO/IEC 18033-3) is the approved cryptographic algorithm to be used in the encipherment and 
    /// MAC mechanisms specified in Annex A1. The algorithm is based on the (single) DES algorithm standardised in ISO 16609. </para>
    /// <para>Asymmetric Algorithms: This reversible algorithm (see reference [2] in Annex C) is the approved algorithm for encipherment and digital signature generation as described in 
    /// Annex A2. The only values allowed for the public key exponent are 3 and 2 16  + 1.</para>
    /// <para>Hashing Algorithms: This algorithm is standardised in ISO/IEC 10118-3.  SHA-1 takes as input messages of arbitrary length and produces a 20-byte hash value.</para>
    /// </remarks>
    public static class Cryptography
    {
        // The digital signature scheme and offline PIN encryption use the RSA transform (see RSA digital signature scheme) as defined in ISO/IEC CD 18033-2 [6]. 
        //            RSAParameters rsaParameters = new RSAParameters();
        //            rsaParameters.Modulus = new byte[] { };
        //            rsaParameters.Exponent = 3;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static byte[] GenerateSignature(this byte[] data, AsymmetricKeyParameter privateKey)
        {
            var n = ((RsaKeyParameters)privateKey).Modulus.BitLength / 8;

            // L = Length(MSG)
            var l = data.Length;

            // Compute H = Hash(MSG)
            var h = ComputeHashSha1(data);

            // MSG = MSG1 || MSG2 where MSG1 = N-22 leftmost bytes of MSG & MSG2 = remaining L-N+22 bytes of MSG
            var msg1 = new byte[n - 22];
            Array.Copy(data, 0, msg1, 0, n - 22);
            var msg2 = new byte[l - n + 22];
            Array.Copy(data, 0, msg2, 0, l - n + 22);

            // B = '6A'
            const byte b = 0x6A;

            // E = 'BC'
            const byte e = 0xBC;

            // X = ( B || MSG1 || H || E )
            var x = new byte[n];
            x[0] = b;
            msg1.CopyTo(x, 1);
            h.CopyTo(x, n - 21);
            x[n - 1] = e;

            // S = Sign(Sk)[X]
            var engine = new RsaEngine();

            engine.Init(true, privateKey);
            var s = engine.ProcessBlock(x, 0, x.Length);

            return s;
        }

        /// <summary>
        /// Executes the EMV Recovery Function to retrieve data.
        /// </summary>
        /// <param name="signature">EMV certificate to recover data from.</param>
        /// <param name="publicKey">Public Key to use.</param>
        /// <returns>Data recovered from the certificate.</returns>
        public static byte[] RecoverMessage(this byte[] signature, AsymmetricKeyParameter publicKey)
        {
            IAsymmetricBlockCipher engine = new RsaEngine();
            engine.Init(false, publicKey);
            var x = engine.ProcessBlock(signature, 0, signature.Length);

            return x;
            /*
             *  This section describes the special case of the digital signature scheme giving message recovery using a hash function according to ISO/IEC 9796-2 [3], which is used in the EMV specification for both static and dynamic data authentication.
             *  The digital signature scheme uses the following two types of algorithms.
             *  •  	A reversible asymmetric algorithm consisting of a signing function Sign($$S_{{K}})$$[ ] depending on a Private Key $$S_{{K}}$$ and a recovery function Recover($$P_{{K}})$$[ ] depending on a Public Key $$P_{{K}}$$. Both functions map N-byte numbers onto N-byte numbers and have the property that
             *      $${\rm Recover}({P}_{{K}})[{\rm Sign}({S}_{{K}})[{X}]] = {X},$$
             *      for any N-byte number X.
             *  •  	A hash algorithm Hash[ ] that maps a message of arbitrary length onto an 20-byte hash code.
             */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="message"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static Boolean VerifySignature(byte[] signature, byte[] message, AsymmetricKeyParameter publicKey)
        {
            var n = ((RsaKeyParameters)publicKey).Modulus.BitLength / 8;

            // Check Length(S) = N
            if (signature.Length != n)
            {
                return false;
            }

            // X = Recover(Pk)[S]
            var x = RecoverMessage(signature, publicKey);

            // X = ( B || MSG1 || H || E )
            var b = x[0];
            var msg1 = new byte[n - 22];
            Array.Copy(x, 1, msg1, 0, n - 22);
            var h = new byte[20];
            Array.Copy(x, 1 + n - 22, h, 0, 20);
            var e = x[x.Length - 1];

            // Check B = '6A'
            if (b != 0x6A)
            {
                return false;
            }

            // Check E = 'BC'
            if (e != 0xBC)
            {
                return false;
            }

            // TODO
            // Check H = Hash(MSG)
            // TODO
            return true;
        }

        /// <summary>
        /// Compute the Hash of <paramref name="data"/> using the SHA-1 algorithm.
        /// </summary>
        /// <remarks>The approved algorithm for hashing is SHA-1 as specified in ISO/IEC 10118-3 [5].</remarks>
        /// <param name="data">Data to hash.</param>
        /// <returns>Hash value.</returns>
        public static byte[] ComputeHashSha1(this byte[] data)
        {
            IDigest sha1 = new Sha1Digest();
            var h = new byte[sha1.GetDigestSize()];

            sha1.BlockUpdate(data, 0, data.Length);
            sha1.DoFinal(h, 0);

            return h;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputePayPassMac(this byte[] data, KeyParameter key)
        {
            IBlockCipher cipher = new DesEdeEngine();
            IMac mac = new CbcBlockCipherMac(cipher, 64, new ISO7816d4Padding());

            mac.Init(key);
            mac.BlockUpdate(data, 0, data.Length);

            var m = new byte[mac.GetMacSize()];
            mac.DoFinal(m, 0);

            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeMacForPersoCryptogram(this byte[] data, KeyParameter key)
        {
            IBlockCipher cipher = new DesEdeEngine();
            IMac mac = new ISO9797Alg3Mac(cipher, 64, new ISO7816d4Padding());

            mac.Init(key);
            mac.BlockUpdate(data, 0, data.Length);

            var m = new byte[mac.GetMacSize()];
            mac.DoFinal(m, 0);

            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeMacForIntegrity(this byte[] data, KeyParameter key)
        {
            /*
             *  The computation of an s-byte MAC (4 ≤ s ≤ 8; see MAC algorithms) is according to ISO/IEC 9797-1 [2] using a 64-bit block cipher ALG in CBC mode as specified in ISO/IEC 10116. More precisely, the computation of a MAC S over a message MSG consisting of an arbitrary number of bytes with a MAC Session Key K$$_{{\rm S}}$$ takes place in the following steps:
             *  1.  Padding and Blocking
             *      Pad the message M according to ISO/IEC 7816-4 (which is equivalent to method 2 of ISO/IEC 9797-1), hence add a mandatory ‘80’ byte to the right of MSG, and then add the smallest number of ‘00’ bytes to the right such that the length of resulting message MSG := (MSG ‖ ‖ ‘80’ ‖ ‖ ‘00’ ‖ ‖ ‘00’ ‖ ‖ … ‖ ‖ ‘00’) is a multiple of 8 bytes.
             *      MSG is then divided into 8-byte blocks $$X_{1}$$, $$X_{2}$$, $$\ldots$$, $$X_{{\rm k}}$$.
             *  2.  MAC Session Key
             *      The MAC Session Key $$K_{{\rm S}}$$ either consists of only a leftmost key block $$K_{{\rm S}} = K_{{\rm SL}}$$ or the concatenation of a leftmost and a rightmost key block $$K_{{\rm S}} = (K_{{\rm SL} }\vert \vert K_{\rm SR})$$.
             *  3.  Cryptogram Computation
             *      Process the 8-byte blocks $$X_{1}$$, $$X_{2}$$, …, $$X_{{k}}$$ with the block cipher in CBC mode (see modes ofoperation) using the leftmost MAC Session Key block $$K_{{\rm SL}}$$:
             *      $${H}_{{i}} := {\rm ALG}({K}_{{\rm SL}})[{X}_{{i}} \oplus {H}_{{i} - 1}], {\rm for } i = 1, 2, \ldots, {k}$$
             *      with initial value $$H_{0}$$ := (‘00’ ‖ ‖ ‘00’ ‖ ‖ ‘00’ ‖ ‖ ‘00’ ‖ ‖ ‘00’ ‖ ‖ ‘00’ ‖ ‖ ‘00’ ‖ ‖ ‘00’).
             *      Compute the 8-byte block $$H_{{k} + 1}$$ in one of the following two ways:
             *      •  	According to ISO/IEC 9797-1 Algorithm 1:
             *          $${H}_{{k}+1} := {H}_{{k}}.$$
             *      •  	According to Optional Process 1 of ISO/IEC 9797-1 Algorithm 3:
             *          $${H}_{{k}+1} := {\rm    ALG}({K}_{{\rm SL}})[{\rm ALG}^{-1}({K}_{{\rm SR}})[{H}_{{k}}]].$$
             *  The MAC S is then equal to the s most significant bytes of $$H_{{k} + 1}$$. 
             */
            IBlockCipher cipher = new DesEngine();
            IMac mac = new ISO9797Alg3Mac(cipher, 64, new ISO7816d4Padding());

            mac.Init(key);
            mac.BlockUpdate(data, 0, data.Length);

            var m = new byte[mac.GetMacSize()];
            mac.DoFinal(m, 0);

            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeCbc3Des(this byte[] data, KeyParameter key)
        {
            IBlockCipher engine = new DesEdeEngine();
            var cipher = new BufferedBlockCipher(new CbcBlockCipher(engine));

            cipher.Init(true, key);

            var cbc = new byte[cipher.GetOutputSize(data.Length)];

            var length = cipher.ProcessBytes(data, 0, data.Length, cbc, 0);
            cipher.DoFinal(cbc, length);

            return cbc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeEcb3Des(this byte[] data, KeyParameter key)
        {
            IBlockCipher engine = new DesEdeEngine();
            BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(engine);
            var ecb = new byte[16];

            cipher.Init(true, key);
            var length = cipher.ProcessBytes(data, 0, 16, ecb, 0);
            cipher.DoFinal(ecb, length);

            return ecb;
        }
    }
}