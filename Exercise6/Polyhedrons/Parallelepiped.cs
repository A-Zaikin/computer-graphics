using OpenTK.Mathematics;

namespace Exercise6
{
    public static class Parallelepiped
    {
        public static Polyhedron Create(float widthX, float widthZ, float height)
        {
            var plane1 = new Vector2[]
            {
                new Vector2(-widthX / 2, -widthZ / 2),
                new Vector2(-widthX / 2, widthZ / 2),
                new Vector2(widthX / 2, widthZ / 2),
                new Vector2(widthX / 2, -widthZ / 2),
            };
            var plane2 = plane1.Clone() as Vector2[];
            return Prismatoid.Create(plane1, plane2, height);
        }
    }
}
