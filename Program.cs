using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.IO.Pem;
using OpenSSL = Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;

internal class Program
{
    static void Main(string[] args)
    {
        // Test 1: Create key pair
        (ECKeyParameters privateEcKey, ECKeyParameters publicEcKey) = CreateEcKeyPair(new DerObjectIdentifier("1.3.132.0.10"));

        // Test 2: Export PEM encoded private key in PKCS#8 and SEC#1 format, export PEM encoded public key in SPKI format
        string privatePkcs8Pem = ExportPrivateAsPkcs8Pem(privateEcKey);
        string privateSec1Pem = ExportPrivateAsSec1Pem(privateEcKey);
        string publicSpkiPem = ExportPublicAsSpkiPem(publicEcKey);
        Console.WriteLine(privatePkcs8Pem);
        Console.WriteLine(privateSec1Pem);
        Console.WriteLine(publicSpkiPem);

        // Test 3: Import PEM encoded private key from PKCS#8 and SEC1#1 format, import PEM encoded public key from SPKI format
        ECKeyParameters privateEcKeyReloaded1 = (ECKeyParameters)ImportPrivateFromPkcs8Pem(privatePkcs8Pem);
        ECKeyParameters privateEcKeyReloaded2 = ImportPrivateFromSec1Pem(privateSec1Pem);
        ECKeyParameters publicEcKeyReloaded1 = (ECKeyParameters)ImportPublicFromSpkiPem(publicSpkiPem);

        // Test 4: ECDH
        (ECKeyParameters privateEcKeyOtherSide, ECKeyParameters publicEcKeyOtherSide) = CreateEcKeyPair(new DerObjectIdentifier("1.3.132.0.10"));
        byte[] sharedSecret = CreateEcdhSharedSecret(privateEcKeyReloaded1, publicEcKeyOtherSide);
        byte[] sharedSecretOtherSide = CreateEcdhSharedSecret(privateEcKeyOtherSide, publicEcKeyReloaded1);
        Console.WriteLine(Hex.ToHexString(sharedSecret));
        Console.WriteLine(Hex.ToHexString(sharedSecretOtherSide));
        Console.WriteLine();

        // Test 5a: ECDSA: IEEE P1363 format
        byte[] message = Encoding.UTF8.GetBytes("The quick brown fox jumps ove rthe lazy dog");
        
        byte[] signature1 = EcSign("SHA-256withPLAIN-ECDSA", message, privateEcKeyReloaded1);
        bool verified1 = EcVerify("SHA-256withPLAIN-ECDSA", message, signature1, publicEcKeyReloaded1);
        Console.WriteLine(Hex.ToHexString(signature1));
        Console.WriteLine(verified1);
        Console.WriteLine();
        
        // Test 5b: ECDSA: ASN.1/DER format
        byte[] signature2 = EcSign("SHA-256withECDSA", message, privateEcKeyReloaded2);
        bool verified2 = EcVerify("SHA-256withECDSA", message, signature2, publicEcKeyReloaded1);
        Console.WriteLine(Hex.ToHexString(signature2));
        Console.WriteLine(verified2);
    }

    public static (ECKeyParameters privateEc, ECKeyParameters publicEc) CreateEcKeyPair(DerObjectIdentifier oid) // X9ObjectIdentifiers.Prime256v1: P-256 aka secp256r1; 1.3.132.0.10: secp256k1
    {
        ECKeyPairGenerator ecKeyPairGenerator = new ECKeyPairGenerator();
        ecKeyPairGenerator.Init(new ECKeyGenerationParameters(oid, new SecureRandom()));
        AsymmetricCipherKeyPair keyPair = ecKeyPairGenerator.GenerateKeyPair();
        return (keyPair.Private as ECKeyParameters, keyPair.Public as ECKeyParameters);
    }

    public static string ExportPrivateAsPkcs8Pem(AsymmetricKeyParameter privateKey)
    {
        OpenSSL.Pkcs8Generator pkcs8Generator = new OpenSSL.Pkcs8Generator(privateKey);
        PemObject pemObjectPkcs8 = pkcs8Generator.Generate();
        OpenSSL.PemWriter pemWriter = new OpenSSL.PemWriter(new StringWriter());
        pemWriter.WriteObject(pemObjectPkcs8);
        return pemWriter.Writer.ToString();
    }

    public static string ExportPrivateAsSec1Pem(ECKeyParameters privateKey)
    {
        OpenSSL.PemWriter pemWriter = new OpenSSL.PemWriter(new StringWriter());
        pemWriter.WriteObject(privateKey);
        return pemWriter.Writer.ToString();
    }

    public static string ExportPublicAsSpkiPem(AsymmetricKeyParameter publicKey)
    {
        OpenSSL.PemWriter pemWriter = new OpenSSL.PemWriter(new StringWriter());
        pemWriter.WriteObject(publicKey);
        return pemWriter.Writer.ToString();
    }

    public static AsymmetricKeyParameter ImportPrivateFromPkcs8Pem(string privatePkcs8Pem)
    {
        OpenSSL.PemReader pemReader = new OpenSSL.PemReader(new StringReader(privatePkcs8Pem));
        return (AsymmetricKeyParameter)pemReader.ReadObject();
    }

    public static ECKeyParameters ImportPrivateFromSec1Pem(string privatePkcs1Pem)
    {
        OpenSSL.PemReader pemReader = new OpenSSL.PemReader(new StringReader(privatePkcs1Pem));
        return (ECKeyParameters)((AsymmetricCipherKeyPair)pemReader.ReadObject()).Private;
    }

    public static AsymmetricKeyParameter ImportPublicFromSpkiPem(string publicSpkiPem)
    {
        OpenSSL.PemReader pemReader = new OpenSSL.PemReader(new StringReader(publicSpkiPem));
        return (AsymmetricKeyParameter)pemReader.ReadObject();
    }

    public static byte[] CreateEcdhSharedSecret(ECKeyParameters privateEcKey, ECKeyParameters publicEcKeyOtherSide)
    {
        IBasicAgreement agreementOtherSide = AgreementUtilities.GetBasicAgreement("ECDH");
        agreementOtherSide.Init(privateEcKey);
        BigInteger sharedSecretOtherSideBN = agreementOtherSide.CalculateAgreement(publicEcKeyOtherSide);
        return sharedSecretOtherSideBN.ToByteArrayUnsigned();
    }

    private static ISigner EcSignVerify(string algo, bool isSigner, byte[] msg, ECKeyParameters key)
    {
        ISigner signerVerifier = SignerUtilities.GetSigner(algo);
        signerVerifier.Init(isSigner, key);
        signerVerifier.BlockUpdate(msg, 0, msg.Length);
        return signerVerifier;
    }

    public static byte[] EcSign(string algo, byte[] msg, ECKeyParameters key)
    {
        return EcSignVerify(algo, true, msg, key).GenerateSignature();
    }

    public static bool EcVerify(string algo, byte[] msg, byte[] signature, ECKeyParameters key)
    {
        return EcSignVerify(algo, false, msg, key).VerifySignature(signature);
    }
}

