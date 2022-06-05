using System;
using System.Numerics;
using System.Xml;

namespace SHACAL.classes
{
    public class KeyExtension : interfaces.IKeyExtension
    {
        public byte[][] GetExtensedKeys(byte[] key) /* 512 bit */
        {
            int rows = 80;
            int columns = 4; 
            var roundKeys = new byte[rows][];

            for (int i = 0; i < rows; i++)
                roundKeys[i] = new byte[columns]; /* 32 bit */
            
            int count = 0;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (count == key.Length) break;
                    roundKeys[i][j] = key[count];
                    count++;
                }
            }
                // var tmpK = (key >> (i * 32)) & (((ulong)1 << 32) - 1);
                // roundKeys[i] = BitConverter.GetBytes((uint)tmpK);

            for (int i = 16; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var tmp = roundKeys[i - 3][j] ^ roundKeys[i - 8][j] ^ roundKeys[i - 14][j] ^ roundKeys[i - 16][j];
                    roundKeys[i][j] = (byte)(tmp << 1);
                }
            }
            return (roundKeys);
        }
    }
}