﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    /// <summary>
    /// Sphere class
    /// </summary>
    public class Sphere : Geometry
    {

        public Vector3D Center;
        public double Radius;

        public Sphere(Vector3D location, double radius)
        {
            Radius = radius;
            Center = location;
        }

        public override Vector3D GetNormalAtPoint(Vector3D point)
        {
            Vector3D normal = point - Center;
            normal.Normalize();
            return normal;
        }

        public override bool Intersects(Ray ray, ref Vector3D intersectionPoint)
        {
            double distance = double.NaN;

            Vector3D originOffset = ray.Source - Center;

            // a = 1 since  ray.D.Dot() = 1
            double b = 2.0 * (Vector3D.DotProduct(ray.Direction, originOffset));
            double c = Vector3D.DotProduct(originOffset, originOffset) - (Radius * Radius);

            double discriminant = b * b - 4.0 * c;
            if (discriminant < 0)
            {
                return false;
            }

            // compute q as described above
            double distSqrt = Math.Sqrt(discriminant);
            double q;
            if (b > 0)
            {
                q = (-b - distSqrt) / 2.0;
            }
            else
            {
                q = (-b + distSqrt) / 2.0;
            }

            // compute t0 and t1
            double t0 = q;
            double t1 = c / q;

            // make sure t0 is smaller than t1
            if (t0 > t1)
            {

                // if t0 is bigger than t1 swap them around
                double temp = t0;
                t0 = t1;
                t1 = temp;
            }

            // if t1 is less than zero, the object is in the ray's negative direction
            // and consequently the ray misses the sphere
            if (t1 < 0)
            {
                return false;
            }

            // if t0 is less than zero, the intersection point is at t1
            if (t0 < 0)
            {
                distance = t1;
            }
            else
            {

                // else the intersection point is at t0
                distance = t0;
            }

            intersectionPoint = ray.Source + ray.Direction * distance;

            return true;
        }
    }
}
