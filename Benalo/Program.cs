using System;
using System.Numerics;
using Benalo.interfaces;

namespace Benalo
{
    class Program
    {
        static void Main(string[] args)
        {
            var block = new BigInteger(1000);
            var asymmetric = new classes.AsymmetricEncryptionDecryption(block, Tests.Ferma,0.7, (ulong)block.GetByteCount() + 1);

            //var lalka = new BigInteger(228);
            //var encrypted = asymmetric.Encryption(block);
            //var decrypted = asymmetric.Encryption(encrypted);
            
            //Console.WriteLine("Default = {0}", lalka);
            //Console.WriteLine("Encrypted = {0}", encrypted);
            //Console.WriteLine("Decrypted = {0}", decrypted);
        }
    }
}