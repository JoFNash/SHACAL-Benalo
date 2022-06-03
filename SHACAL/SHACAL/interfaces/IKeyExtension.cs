using System;
using System.Numerics;

namespace SHACAL.interfaces
{
    public interface IKeyExtension
    {
        public byte[][] GetExtensedKeys(BigInteger key); /* key (512 bit) */
    }
}