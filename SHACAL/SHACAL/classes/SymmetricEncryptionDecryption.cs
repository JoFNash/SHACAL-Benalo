﻿using System;
using System.Numerics;
using SHACAL.interfaces;

namespace SHACAL.classes
{
    public class SymmetricEncryptionDecryption : interfaces.ISymmetricEncryptionDecryption
    {
        private readonly interfaces.IKeyExtension _keyGenerator;
        private readonly interfaces.IRoundTranformation _roundTranformation;
        private byte[][] _extensedKey;

        public SymmetricEncryptionDecryption(IKeyExtension keyEx, IRoundTranformation roundTranform)
        {
            _keyGenerator = keyEx;
            _roundTranformation = roundTranform;
        }

        public byte[] Encryption(byte[] block) /* block (160 Bit) = 20 bytes */
        {
            byte[] blockA = new byte[4];
            byte[] blockB = new byte[4];
            byte[] blockC = new byte[4];
            byte[] blockD = new byte[4];
            byte[] blockE = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                blockA[i] = (byte)block[i];
                blockB[i] = (byte)block[i + 4];
                blockC[i] = (byte)block[i + 8];
                blockD[i] = (byte)block[i + 12];
                blockE[i] = (byte)block[i + 16];
            }
            
            uint numberBlockA = BitConverter.ToUInt32(blockA, 0);
            uint numberBlockB = BitConverter.ToUInt32(blockB, 0);
            uint numberBlockC = BitConverter.ToUInt32(blockC, 0);
            uint numberBlockD = BitConverter.ToUInt32(blockD, 0);
            uint numberBlockE = BitConverter.ToUInt32(blockE, 0);
            
            for (int round = 0; round < 80; round++)
            {
                numberBlockA = BitConverter.ToUInt32(_extensedKey[round]) + (numberBlockA << 5) + _roundTranformation.RoundFunction(numberBlockB, numberBlockC, numberBlockD, round) +
                                    numberBlockE + GetConstant(round);
                numberBlockB = numberBlockA;
                numberBlockC = numberBlockB << 30;
                numberBlockD = numberBlockC;
                numberBlockE = numberBlockD;
            }
            return (new byte[5]);
        }

        public byte[] Decryption(byte[] block)
        {
            throw new System.NotImplementedException();
        }

        public void getExtensedKey(BigInteger dKey)
        {
            _extensedKey = _keyGenerator.GetExtensedKey(dKey);
        }

        public uint GetConstant(int round)
        {
            var const1 = 0x5A827999;
            var const2 = 0x6ED9EBA1;
            var const3 = 0x8F1BBCDC;
            var const4 = 0xCA62C1D6;
            uint res = 0;

            if (round >= 0 && round <= 19)
                res = (uint)const1;
            if (round >= 20 && round <= 39)
                res = (uint)const1;
            if (round >= 40 && round <= 59)
                res = (uint)const1;
            if (round >= 60 && round <= 79)
                res = (uint)const1;
            
            return (res);
        }
    }
}