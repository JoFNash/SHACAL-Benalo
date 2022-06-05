using System;
using System.Collections.Generic;
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

            byte[] res = new byte[20];

            for (int i = 0; i < 4; i++)
            {
                blockA[i] = (byte)block[i];
                blockB[i] = (byte)block[i + 4];
                blockC[i] = (byte)block[i + 8];
                blockD[i] = (byte)block[i + 12];
                blockE[i] = (byte)block[i + 16];
            }
            
            uint numberA = BitConverter.ToUInt32(blockA, 0);
            uint numberB = BitConverter.ToUInt32(blockB, 0);
            uint numberC = BitConverter.ToUInt32(blockC, 0);
            uint numberD = BitConverter.ToUInt32(blockD, 0);
            uint numberE = BitConverter.ToUInt32(blockE, 0);
            
            List<uint> oldBlocks = new List<uint> {numberA, numberB, numberC, numberD, numberE};
            List<uint> newBlocks = new List<uint> {numberA, numberB, numberC, numberD, numberE};

            for (int round = 0; round < 80; round++)
            {
                newBlocks[0] = BitConverter.ToUInt32(_extensedKey[round]) + (oldBlocks[0] << 5) +
                               _roundTranformation.RoundFunction(oldBlocks[1], oldBlocks[2], oldBlocks[3], round) +
                               oldBlocks[4] + GetConstant(round);
                newBlocks[1] = oldBlocks[0];
                newBlocks[2] = oldBlocks[1] << 30;
                newBlocks[3] = oldBlocks[2];
                newBlocks[4] = oldBlocks[3];

                for (int i = 0; i < 5; i++)
                    oldBlocks[i] = newBlocks[i];
            }

            res = (newBlocks[0] | (BigInteger)newBlocks[1] << 32 |  (BigInteger)(newBlocks[2]) << (32 * 2) | (BigInteger)newBlocks[3] << (32 * 3) | (BigInteger)newBlocks[4] << (32 * 4)).ToByteArray();
            return (res);
        }

        public byte[] Decryption(byte[] block)
        {
            byte[] blockA = new byte[4];
            byte[] blockB = new byte[4];
            byte[] blockC = new byte[4];
            byte[] blockD = new byte[4];
            byte[] blockE = new byte[4];

            byte[] res = new byte[20];

            for (int i = 0; i < 4; i++)
            {
                blockA[i] = (byte)block[i];
                blockB[i] = (byte)block[i + 4];
                blockC[i] = (byte)block[i + 8];
                blockD[i] = (byte)block[i + 12];
                blockE[i] = (byte)block[i + 16];
            }
            
            uint numberA = BitConverter.ToUInt32(blockA, 0);
            uint numberB = BitConverter.ToUInt32(blockB, 0);
            uint numberC = BitConverter.ToUInt32(blockC, 0);
            uint numberD = BitConverter.ToUInt32(blockD, 0);
            uint numberE = BitConverter.ToUInt32(blockE, 0);
            
            List<uint> oldBlocks = new List<uint> {numberA, numberB, numberC, numberD, numberE};
            List<uint> newBlocks = new List<uint> {numberA, numberB, numberC, numberD, numberE};

            for (int round = 79; round <= 0; round--)
            {
                newBlocks[0] = oldBlocks[1];
                newBlocks[1] = oldBlocks[2] << 2;
                newBlocks[2] = oldBlocks[3];
                newBlocks[3] = oldBlocks[4];
                newBlocks[4] = oldBlocks[0] + ~(oldBlocks[1] << 5) +
                               ~_roundTranformation.RoundFunction((oldBlocks[2] << 2), oldBlocks[3], oldBlocks[4], round) + ~BitConverter.ToUInt32(_extensedKey[round]) + 4 + ~GetConstant(round);
                
                for (int i = 0; i < 5; i++)
                    oldBlocks[i] = newBlocks[i];
            }
            res = (newBlocks[0] | (BigInteger)newBlocks[1] << 32 |  (BigInteger)(newBlocks[2]) << (32 * 2) | (BigInteger)newBlocks[3] << (32 * 3) | (BigInteger)newBlocks[4] << (32 * 4)).ToByteArray();
            return (res);
        }
        
        public void GetExtensedKey(byte[] dKey)
        {
            _extensedKey = _keyGenerator.GetExtensedKeys(dKey);
        }

        public uint GetConstant(int round)
        {
            uint res = 0;

            if (round >= 0 && round <= 19)
                res = 0x5A827999;
            if (round >= 20 && round <= 39)
                res = 0x6ED9EBA1;
            if (round >= 40 && round <= 59)
                res = 0x8F1BBCDC;
            if (round >= 60 && round <= 79)
                res = 0xCA62C1D6;
            
            return (res);
        }
    }
}