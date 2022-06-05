using System;
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
            
            for (int round = 0; round < 80; round++)
            {
                numberA = BitConverter.ToUInt32(_extensedKey[round]) + (numberA << 5) + _roundTranformation.RoundFunction(numberB, numberC, numberD, round) +
                                    numberE;
                numberB = numberA;
                numberC = numberB << 30;
                numberD = numberC;
                numberE = numberD;
            }

            res = ((BigInteger)numberA << 32 * 4 | (BigInteger)numberB << 32 * 3 |  (BigInteger)(numberC) << 32 * 2 | (BigInteger)numberD << 32 | numberE).ToByteArray();
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
            
            for (int round = 79; round >= 0; round--)
            {
                numberA = numberB;
                numberB = numberC << 2;
                numberC = numberD;
                numberD = numberE;
                numberE = numberA + ~(numberB << 5) +
                          ~_roundTranformation.RoundFunction((numberC << 2), numberD, numberE, round) + ~BitConverter.ToUInt32(_extensedKey[round]) + 4;
            }
            res = ((BigInteger)numberA << 32 * 4 | (BigInteger)numberB << 32 * 3 |  (BigInteger)(numberC) << 32 * 2 | (BigInteger)numberD << 32 | numberE).ToByteArray();
            return (res);
        }

        public void GetExtensedKey(BigInteger dKey)
        {
            throw new NotImplementedException();
        }

        public uint GetRoundKey(int round)
        {
            if (round < 0 || round > 79)
                throw new ArgumentException("Incorrect round value in getRoundKey");
            
            if (round >= 0 && round < 16)
            {
                return (BitConverter.ToUInt32(_extensedKey[round]));
            }

            var keyTmp1 = BitConverter.ToUInt32(_extensedKey[round - 3]);
            var keyTmp2 = BitConverter.ToUInt32(_extensedKey[round - 8]);
            var keyTmp3 = BitConverter.ToUInt32(_extensedKey[round - 14]);
            var keyTmp4 = BitConverter.ToUInt32(_extensedKey[round - 16]);

            var keyResult = keyTmp1 ^ keyTmp2 ^ keyTmp3 ^ keyTmp4;
            return (keyResult << 1);
        }

        public void GetExtensedKey(byte[] dKey)
        {
            _extensedKey = _keyGenerator.GetExtensedKeys(dKey);
        }

        public uint GetConstantEncrypt(int round)
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
        
        public uint GetConstantDecrypt(int round)
        {
            uint res = 0;

            if (round >= 0 && round <= 19)
                res = 0xCA62C1D6;
            if (round >= 20 && round <= 39)
                res = 0x8F1BBCDC;
            if (round >= 40 && round <= 59)
                res = 0x6ED9EBA1;
            if (round >= 60 && round <= 79)
                res = 0x5A827999;
            
            return (res);
        }
    }
}