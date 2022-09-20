using System;
using System.Collections.Generic;
using System.Drawing;

namespace Exercise1
{
    public class RegularPolygon : Polygon
    {
        public RegularPolygon(Canvas canvas, Pen pen, Point center, double radius, int edges) 
            : base(canvas, pen, CreateRegularPolygon(center, radius, edges))
        {
        }

        private static Point[] CreateRegularPolygon(Point center, double radius, int edges)
        {
            var points = new List<Point>();
            for (var i = 0; i < edges; i++)
            {
                var point = new Point(
                    center.X + (int)(radius * Math.Cos(2 * Math.PI * i / edges)),
                    center.Y + (int)(radius * Math.Sin(2 * Math.PI * i / edges)));
                points.Add(point);
            }
            return points.ToArray();
        }
    }
}
