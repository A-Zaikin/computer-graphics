using System.Numerics;

namespace Exercise8_Raytracing
{
    public static class Vectors3
    {
        public static readonly Vector3 Up = new(0, 1, 0);
        public static readonly Vector3 Down = new(0, -1, 0);
        public static readonly Vector3 Right = new(1, 0, 0);
        public static readonly Vector3 Left = new(-1, 0, 0);
        public static readonly Vector3 Forward = new(0, 0, 1);
        public static readonly Vector3 Back = new(0, 0, -1);
    }
}
