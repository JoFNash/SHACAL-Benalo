using System;
using System.Numerics;
using SHACAL.classes;

namespace SHACAL
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytesArray = new byte[20] {1, 9, 12, 9, 67, 3, 2, 1, 3, 90, 12, 255, 1, 40, 5, 6, 7, 3, 2, 2};
            //new Random().NextBytes(bytesArray);
            for (int i = 0; i < 20; i++)
                Console.Write(bytesArray[i] + " ");
            Console.WriteLine();

            SymmetricEncryptionDecryption sed = new SymmetricEncryptionDecryption(new KeyExtension(), new RoundTransformation());
            byte[] key = new byte[] { 8, 1, 3, 4, 5, 5};
            sed.GetExtensedKey(key);
            byte[] result = sed.Encryption(bytesArray);
            for (int i = 0; i < 20; i++)
                Console.Write(result[i] + " ");

            Console.WriteLine();
            byte[] myBlock = sed.Decryption(bytesArray);
            for (int i = 0; i < 20; i++)
                Console.Write(myBlock[i] + " ");
            
            // KeyExtension keyObject = new KeyExtension();
            // byte[][] result = keyObject.GetExtensedKeys(122345);
        }
        
    }
}