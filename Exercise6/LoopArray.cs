using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise6
{
    public class LoopArray<T>
    {
        private T[] array;

        public LoopArray(T[] array)
        {
            this.array = array;
        }

        public LoopArray(int length)
        {
            array = new T[length];
        }

        public T this[int index]
        {
            get
            {
                index %= array.Length;
                if (index < 0)
                {
                    index += array.Length;
                }
                return array[index];
            }
        }
    }
}
