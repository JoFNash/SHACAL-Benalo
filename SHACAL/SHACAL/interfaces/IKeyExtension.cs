using System;
using System.Numerics;

namespace SHACAL.interfaces
{
    public interface IKeyExtension
    {
        public byte[][] GetExtensedKeys(byte[] key); /* key (512 bit) */
    }
}