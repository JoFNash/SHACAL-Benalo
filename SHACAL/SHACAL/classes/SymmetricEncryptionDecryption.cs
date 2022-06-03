using System.Numerics;

namespace SHACAL.classes
{
    public class SymmetricEncryptionDecryption : interfaces.ISymmetricEncryptionDecryption
    {
        private readonly interfaces.IKeyExtension _keyGenerator;
        private byte[][] _extensedKey;

        public SymmetricEncryptionDecryption(interfaces.IKeyExtension keyEx)
        {
            _keyGenerator = keyEx;
        }

        public byte[] Encryption(byte[] block) /* block (160 Bit) */
        {
            uint[] blockA;
            uint[] blockB;
            uint[] blockC;
            uint[] blockD;
            uint[] blockE;
            int rounds = 80;

            for (int i = 0; i < 5; i++)
            {
                
            }

        }

        public byte[] Decryption(byte[] block)
        {
            throw new System.NotImplementedException();
        }

        public void getExtensedKeys(BigInteger dKey)
        {
            _extensedKey = _keyGenerator.GetExtensedKey(dKey);
        }
    }
}