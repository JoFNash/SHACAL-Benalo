using System;
using System.Numerics;
using System.Xml;

namespace SHACAL.classes
{
    public class KeyExtension : interfaces.IKeyExtension
    {
        public byte[][] GetExtensedKey(BigInteger key) /* 512 bit */ //byte[] bytesArray
        {
            var roundKeys = new byte[80][];
            var first16RoundKeys = new byte[16][];
            // var value = BitConverter.T

            for (int i = 0; i < 16; i++)
            {
                var tmpK = (uint)(key >> (i * 32)) & ((1 << 32) - 1);
                roundKeys[i] = BitConverter.GetBytes(tmpK);
            }

            for (int i = 16; i < 80; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var tmp = roundKeys[i - 3][j] ^ roundKeys[i - 8][j] ^ roundKeys[i - 14][j] ^ roundKeys[i - 16][j];
                    roundKeys[i][j] = (byte)tmp;
                }
            }
            return (roundKeys);
        }
    }
}