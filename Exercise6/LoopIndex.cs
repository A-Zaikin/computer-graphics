using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise6
{
    public class LoopIndex
    {
        public readonly Array Array;
        public readonly bool HasLooped;
        public readonly int Value;

        public LoopIndex(Array array, int value = 0)
        {
            Array = array;

            if (value >= Array.Length)
            {
                HasLooped = true;
            }
            value %= Array.Length;
            if (value < 0)
            {
                HasLooped = true;
                value += Array.Length;
            }
            Value = value;
        }

        public static LoopIndex operator+(LoopIndex index, int change)
        {
            return new LoopIndex(index.Array, index.Value + change);
        }

        public static LoopIndex operator -(LoopIndex index, int change)
        {
            return new LoopIndex(index.Array, index.Value - change);
        }

        public static implicit operator int(LoopIndex loopIndex) => loopIndex.Value;
    }
}
