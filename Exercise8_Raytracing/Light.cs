using System;
using System.Drawing;
using System.Numerics;

namespace Exercise8_Raytracing
{
    public class Light
    {
        public Vector3 Position;
        public float Brightness;
        public Sphere Sphere;

        public Light(Vector3 position, float brightness)
        {
            Position = position;
            Brightness = brightness;
            Sphere = new Sphere(0.5f, position, Color.Orange);
            Sphere.IsLight = true;
        }
    }
}
