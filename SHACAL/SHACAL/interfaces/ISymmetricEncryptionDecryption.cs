using System.Numerics;

namespace SHACAL.interfaces
{
    public interface ISymmetricEncryptionDecryption
    {
        public byte[] Encryption(byte[] block);
        
        public byte[] Decryption(byte[] block);
        
        public void getExtensedKeys(BigInteger dKey); /* это тут нужно? */
    }
}