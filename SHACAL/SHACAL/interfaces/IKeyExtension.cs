using System;
using System.Numerics;

namespace SHACAL.interfaces
{
    public interface IKeyExtension
    {
        public byte[][] GetExtensedKey(BigInteger key); /* key (512 bit) */
    }
}