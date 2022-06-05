using System;
using System.Numerics;
using Benalo.interfaces;

namespace Benalo
{
    class Program
    {
        static void Main(string[] args)
        {
            var block = new BigInteger(10209);
            var asymmetric = new classes.AsymmetricEncryptionDecryption(block, Tests.SoloveyStrassen,0.7, (ulong)block.GetByteCount() + 1);

            var encrypted = asymmetric.Encryption(block);
            var decrypted = asymmetric.Decryption(encrypted);
            
            Console.WriteLine(encrypted);
            Console.WriteLine(decrypted);
        }
    }
}