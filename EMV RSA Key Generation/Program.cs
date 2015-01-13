using System;
using System.IO;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using WSCT.EMV.Personalization;
using WSCT.EMV.Security;
using WSCT.Helpers;
using WSCT.Helpers.Desktop;
using WSCT.Helpers.Json;

namespace WSCT.EMV.IccCertificateGenerationConsole
{
    class Program
    {
        private static ConsoleColor defaultColor;

        private const string OutputJsonFileName = @"emv-rsa.json";

        private static void Main( /*string[] args*/)
        {
            RegisterPcl.Register();

            ShowHeader();

            Console.WriteLine("Computing a new RSA Key");

            PrivateKey privateKey;

            try
            {
                var r = new RsaKeyPairGenerator();
                r.Init(new RsaKeyGenerationParameters(new BigInteger("10001", 16), SecureRandom.GetInstance("SHA1PRNG"), 1024, 80));
                var keys = r.GenerateKeyPair();

                var rsaPrivate = (RsaPrivateCrtKeyParameters)keys.Private;
                var rsaPublic = (RsaKeyParameters)keys.Public;

                privateKey = new PrivateKey
                {
                    Modulus = rsaPrivate.Modulus.ToByteArray().ToHexa('\0'),
                    PrivateExponent = rsaPrivate.Exponent.ToByteArray().ToHexa('\0'),
                    PublicExponent = rsaPublic.Exponent.ToByteArray().ToHexa('\0')
                };
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error computing a new RSA Key:");
                Console.ForegroundColor = defaultColor;
                Console.WriteLine(e);
                return;
            }

            Console.WriteLine("Saving RSA Key in {0} file", OutputJsonFileName);
            privateKey.WriteToJsonFile(OutputJsonFileName, true);
        }

        private static T LoadFile<T>(string fileName)
        {
            try
            {
                return fileName.CreateFromJsonFile<T>();
            }
            catch (FileNotFoundException)
            {
                ShowFileNotFound(fileName);
                return default(T);
            }
            catch (Exception e)
            {
                ShowReadError(fileName, e);
                return default(T);
            }
        }

        private static void ShowReadError(string fileName, Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error reading file '{0}':", fileName);
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(e);
        }

        private static void ShowFileNotFound(string fileName)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File not found: '{0}'", fileName);
            Console.ForegroundColor = defaultColor;
        }

        private static void ShowHeader()
        {
            defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("wsct-emvrsa :: EMV RSA Key generation");
            Console.WriteLine("  input files: ");
            Console.WriteLine("  output file: {0}", OutputJsonFileName);
            Console.WriteLine();

            Console.ForegroundColor = defaultColor;
        }
    }
}
