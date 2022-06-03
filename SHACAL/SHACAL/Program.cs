using System;
using SHACAL.classes;

namespace SHACAL
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytesArray = new byte[20] {1, 9, 12, 9, 67, 3, 2, 1, 3, 90, 12, 255, 1, 4, 5, 6, 7, 3, 2, 2};
            //new Random().NextBytes(bytesArray);

            SymmetricEncryptionDecryption sed = new SymmetricEncryptionDecryption(new KeyExtension(), new RoundTransformation());
            byte[] result = sed.Encryption(bytesArray);
            for (int i = 0; i < 4; i++)
                Console.Write(result[i]);
        }
        
    }
}