using System;

namespace SHACAL.classes
{
    public class RoundTransformation : interfaces.IRoundTranformation
    {
        public uint RoundFunction(uint blockB, uint blockC, uint blockD, int round)
        {
            uint res = 0;

            if (round < 0 || round > 79)
                throw new ArgumentException("Incorrect round value");
            
            if (round >= 0 && round <= 19)
            {
                res = (blockB & blockC) | (~blockB & blockD);
            }
            else if (round >= 20 && round <= 39 || round >= 60 && round <= 79)
            {
                res = blockB ^ blockD ^ blockD;
            }
            else if (round >= 60 && round <= 79)
            {
                res = (blockB ^ blockC) | (blockB ^ blockD) | (blockC ^ blockD);
            }
            
            return (res);
        }
    }
}