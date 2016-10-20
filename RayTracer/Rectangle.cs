using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    /// <summary>
    /// Rectangle class
    /// </summary>
    class Rectangle
    {
        public readonly Point3D Point1;
        public readonly Point3D Point2;
        public readonly Point3D Point3;
        public readonly Point3D Point4;

        /// <summary>
        /// Default constructor. Check if the given points make a rectangle before creating it.
        /// </summary>
        /// <param name="point1">First point</param>
        /// <param name="point2">Second point</param>
        /// <param name="point3">Third point</param>
        /// <param name="point4">Fourth point</param>
        public Rectangle(Point3D point1, Point3D point2, Point3D point3, Point3D point4)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            Point4 = point4;
        }

        /// <summary>
        /// Check if this rectangle exists
        /// </summary>
        /// <param name="point1">First point</param>
        /// <param name="point2">Second point</param>
        /// <param name="point3">Third point</param>
        /// <param name="point4">Fourth point</param>
        /// <returns>True if these points define a rectangle</returns>
        public static bool Exists(Point3D point1, Point3D point2, Point3D point3, Point3D point4)
        {
            bool exists = false;

            Vector3D vector1 = point2 - point1;
            Vector3D vector2 = point3 - point2;
            Vector3D vector3 = point4 - point3;
            Vector3D vector4 = point1 - point4;

            if (vector1 == -vector3 && vector2 == -vector4) //It's a parralelogram
            {
                var firstAngle = Vector3D.AngleBetween(vector1, vector2);

                if (firstAngle.AboutEquals(90)) //One right angle. It's a rectangle
                {
                    exists = true;
                }
            }

            return exists;
        }
    }
}
