using System;
using System.Numerics;
using SHACAL.classes;

namespace SHACAL
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytesArray = new byte[20] {255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0};
            //new Random().NextBytes(bytesArray);
            for (int i = 0; i < bytesArray.Length; i++)
                Console.Write(bytesArray[i] + " ");
            Console.WriteLine();

            SymmetricEncryptionDecryption sed = new SymmetricEncryptionDecryption(new KeyExtension(), new RoundTransformation());
            byte[] key = new byte[] { 8, 1, 3, 4, 5, 5};
            sed.GetExtensedKey(key);
            byte[] result = sed.Encryption(bytesArray);
            for (int i = 0; i < result.Length; i++)
                Console.Write(result[i] + " ");

            Console.WriteLine();
            byte[] myBlock = sed.Decryption(bytesArray);
            for (int i = 0; i < myBlock.Length; i++)
                Console.Write(myBlock[i] + " ");
            
            // KeyExtension keyObject = new KeyExtension();
            // byte[][] result = keyObject.GetExtensedKeys(122345);
        }
        
    }
}