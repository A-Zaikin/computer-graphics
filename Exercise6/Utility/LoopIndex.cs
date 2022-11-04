using System.Collections;

namespace Exercise6
{
    public class LoopIndex
    {
        public readonly bool HasLooped;
        public readonly int Value;

        private readonly int length;

        public LoopIndex(int length, int value = 0)
        {
            this.length = length;

            if (value >= length)
            {
                HasLooped = true;
            }
            value %= length;
            if (value < 0)
            {
                HasLooped = true;
                value += length;
            }
            Value = value;
        }

        public LoopIndex(IList array, int value = 0) : this(array.Count, value) { }

        public static LoopIndex operator+(LoopIndex index, int change)
        {
            return new LoopIndex(index.length, index.Value + change);
        }

        public static LoopIndex operator -(LoopIndex index, int change)
        {
            return new LoopIndex(index.length, index.Value - change);
        }

        public static implicit operator int(LoopIndex loopIndex) => loopIndex.Value;
    }
}
