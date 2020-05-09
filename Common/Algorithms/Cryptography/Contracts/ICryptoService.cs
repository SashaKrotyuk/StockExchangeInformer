namespace Common.Algorithms.Cryptography.Contracts
{
    using System.Xml.Linq;

    public interface ICryptoService
    {
        string RandomString(int length);

        string GenerateSaltedHash(string text, string salt);

		string GenerateHash(byte[] data);

		byte[] EncryptAES(string inputString);

		string EncryptAESToBase64String(string inputString);

		string DecryptAES(byte[] cipherBytes);

		string DecryptAESFromBase64String(string cipher);

		string EncryptStringRSA(string inputString, string containerName);

        string EncryptStringRSA(string inputString, XElement xml);

        string DecryptStringRSA(string inputString, XElement xml);

        string DecryptStringRSA(string inputString, string containerName);

        string GetOrCreateKeyInContainerRSA(string containerName);

        void DeleteKeyFromContainerRSA(string containerName);
    }
}