using System.Numerics;

namespace Benalo.interfaces
{
    public interface IAsymmetricEncryptionDecription
    {
        public BigInteger Encryption(BigInteger messsage);
        
        public BigInteger Decryption(BigInteger message);

        public void GetPublicOpenKeys(BigInteger message);

    }
}