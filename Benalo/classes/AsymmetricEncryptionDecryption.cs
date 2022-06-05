using System.Numerics;
using Benalo.interfaces;

namespace Benalo.classes
{
    public sealed class AsymmetricEncryptionDecryption : interfaces.IAsymmetricEncryptionDecription
    {
        private Key _keys;
        private classes.KeyGenerator _keyGenerator;

        public AsymmetricEncryptionDecryption(Key keys, KeyGenerator generator)
        {
            _keys = keys;
            _keyGenerator = generator;
        }

        public BigInteger Encryption(BigInteger block)
        {
            BigInteger u;
            while (true)
            {
                u = extra.ExtraFunctional.GetRandomInteger(2, _keys.n - 1);
                if (BigInteger.GreatestCommonDivisor(u, _keys.n) == 1)
                    break;
            }

            var a = BigInteger.ModPow(_keys.y, block, _keys.n);
            var b = BigInteger.ModPow(u, _keys.r, _keys.n);
            return (BigInteger.Multiply(a, b) % _keys.n);
        }

        public BigInteger Decryption(BigInteger block)
        {
            BigInteger mRes = 0;
            var a = BigInteger.ModPow(block, _keys.fi / _keys.n, _keys.n);
            for (BigInteger m = 0; m < _keys.r; m++)
            {
                var value = BigInteger.ModPow(_keys.x, m, _keys.n);
                if (value == a)
                    mRes = m;
            }
            return (mRes);
        }

        public void GetPublicOpenKeys(BigInteger block)
        {
            _keys = 
        }
    }
}