using System;
using System.Collections.Generic;
using SHACAL.interfaces;

namespace SHACAL.classes
{
    public enum EncryptedModes
    {
        ECB,
        CBC, 
        CFB
    }

    public class ModeFeatures
    {
        public ISymmetricEncryptionDecryption Encrypter { get; set; }
        private byte[] _initializationVector;
        private readonly EncryptedModes _mode;
        private string _str;
        public const int BlockSize = 16;

        public ModeFeatures(EncryptedModes mode, byte[] vector = null, string str = null)
        {
            _mode = mode;
            _initializationVector = vector;
            _str = str;
        }

        private byte[] CleverXor(byte[] firstBlock, byte[] secondBlock)
        {
            var res = new byte[firstBlock.Length];
            var minLength = Math.Min(firstBlock.Length, secondBlock.Length);
            for (var i = 0; i < minLength; i++)
            {
                res[i] = (byte)(firstBlock[i] ^ secondBlock[i]);
            }
            return (res);
        }
        
        

        private static byte[] PaddingPkcs7(byte[] block)
        {
            
            byte mod = (byte)(BlockSize - block.Length % BlockSize);
            byte[] paddedData = new byte[block.Length + mod];
            Array.Copy(block, paddedData, block.Length);
            Array.Fill(paddedData, mod, block.Length, mod);
            return paddedData;
        }
        
        private static byte[] ListToByteArray(List<byte[]> blocksList)
        {
            var resultBlock = new byte[BlockSize * blocksList.Count];
            for (var i = 0; i < blocksList.Count; ++i)
            {
                Array.Copy(blocksList[i], 0, resultBlock,
                    i * BlockSize, BlockSize);
            }
            return resultBlock;
        }
        
        public byte[] Encrypt(byte[] block)
        {
            var paddedBlock = PaddingPkcs7(block); 
            var encryptedBlocksList = new List<byte[]>();
            switch (_mode)
            {
                case EncryptedModes.ECB:
                {
                    var currBlock = new byte[BlockSize];
                    for (var i = 0; i < paddedBlock.Length / BlockSize; ++i)
                    {
                        Array.Copy(paddedBlock, i * BlockSize, currBlock,
                            0, BlockSize);
                        encryptedBlocksList.Add(Encrypter.Encryption(currBlock));
                    }
                    break;
                }
                
                case EncryptedModes.CBC:
                {
                    var prevBlock = new byte[BlockSize];
                    var currBlock = new byte[BlockSize];
                    Array.Copy(_initializationVector, prevBlock, prevBlock.Length);
                    for (var i = 0; i < paddedBlock.Length / BlockSize; ++i)
                    {
                        Array.Copy(paddedBlock, i * BlockSize, currBlock, 
                            0, BlockSize);
                        encryptedBlocksList.Add(Encrypter.Encryption(CleverXor(currBlock, prevBlock)));
                        Array.Copy(encryptedBlocksList[i], prevBlock, BlockSize);
                    }
                    break;
                }
                
                case EncryptedModes.CFB:
                {
                    var prevBlock = new byte[BlockSize];
                    var currBlock = new byte[BlockSize];
                    Array.Copy(_initializationVector, prevBlock, prevBlock.Length);
                    for (var i = 0; i < paddedBlock.Length / BlockSize; ++i)
                    {
                        Array.Copy(paddedBlock, i * BlockSize, currBlock,
                            0, BlockSize);
                        encryptedBlocksList.Add(CleverXor(Encrypter.Encryption(prevBlock), currBlock));
                        Array.Copy(encryptedBlocksList[i], prevBlock, BlockSize);
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(_mode));
            }
            return ListToByteArray(encryptedBlocksList);
        }
        
        public byte[] Decrypt(byte[] block)
        {
            var decryptedBlocksList = new List<byte[]>();
            switch (_mode)
            {
                case EncryptedModes.ECB:
                {
                    var currBlock = new byte[BlockSize];
                    for (var i = 0; i < block.Length / BlockSize; ++i)
                    {
                        Array.Copy(block, i * BlockSize, currBlock,
                            0, BlockSize);
                        decryptedBlocksList.Add(Encrypter.Decryption(currBlock));
                    }
                    break;
                }
                
                case EncryptedModes.CBC:
                {
                    var prevBlock = new byte[BlockSize];
                    var currBlock = new byte[BlockSize];
                    Array.Copy(_initializationVector, prevBlock, prevBlock.Length);
                    for (var i = 0; i < block.Length / BlockSize; ++i)
                    {
                        Array.Copy(block, i * BlockSize, currBlock, 
                            0, BlockSize);
                        decryptedBlocksList.Add(CleverXor(prevBlock, Encrypter.Decryption(currBlock)));
                        Array.Copy(currBlock, prevBlock, BlockSize);
                    }
                    break;
                }
                
                case EncryptedModes.CFB:
                {
                    var prevBlock = new byte[BlockSize];
                    var currBlock = new byte[BlockSize];
                    Array.Copy(_initializationVector, prevBlock, prevBlock.Length);
                    for (var i = 0; i < block.Length / BlockSize; ++i)
                    {
                        Array.Copy(block, i * BlockSize, currBlock, 
                            0, BlockSize);
                        decryptedBlocksList.Add(CleverXor(Encrypter.Encryption(prevBlock), currBlock));
                        Array.Copy(currBlock, prevBlock, BlockSize);
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(_mode), "");
            }
            var connectedBlocks = ListToByteArray(decryptedBlocksList); 
            var resultBlock = new byte[connectedBlocks.Length - connectedBlocks[^1]];
            Array.Copy(connectedBlocks, resultBlock, resultBlock.Length);
            return resultBlock;
        }
    }
}