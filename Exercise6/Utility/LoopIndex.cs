using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise6
{
    public class LoopIndex
    {
        public readonly IList Array;
        public readonly bool HasLooped;
        public readonly int Value;

        public LoopIndex(IList array, int value = 0)
        {
            Array = array;

            if (value >= Array.Count)
            {
                HasLooped = true;
            }
            value %= Array.Count;
            if (value < 0)
            {
                HasLooped = true;
                value += Array.Count;
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
