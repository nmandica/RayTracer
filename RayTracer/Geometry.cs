using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    /// <summary>
    /// Base class for all shapes
    /// </summary>
    public abstract class Geometry
    {
        private IMaterial material = SolidColor.Default;

        /// <summary>
        /// The material that determines the color of the object.
        /// </summary>
        public IMaterial Material
        {
            get
            {
                return material;
            }
            set
            {
                material = value;
            }
        }

        /// <summary>
        /// Returns the color at the given point on the Geometry.
        /// The point should be one returned by Intersects(Ray, Vector3D);
        /// </summary>
        /// <param name="point">A point of the surface of the Geometry.</param>
        /// <param name="r">The red component of the color.</param>
        /// <param name="g">The green component of the color.</param>
        /// <param name="b">The blue component of the color.</param>
        virtual public void GetColor(Vector3D point, ref int r, ref int g, ref int b)
        {
            Material.GetColor(point, ref r, ref g, ref b);
        }

        /// <summary>
        /// Gets the normal at a given point on the object.
        /// The point should be one returned by Intersects(Ray, Vector3D);
        /// </summary>
        /// <param name="point">A point of the surface of the Geometry.</param>
        /// <returns>The normal for the given point.</returns>
        abstract public Vector3D GetNormalAtPoint(Vector3D point);

        /// <summary>
        /// Find the point of intersection with the given ray, if any.
        /// </summary>
        /// <param name="ray">The ray to test against this Geometry</param>
        /// <param name="intersectionPoint">The point of intersection if found.</param>
        /// <returns>True if intersects otherwise false.</returns>
        abstract public bool Intersects(Ray ray, ref Vector3D intersectionPoint);
    }
}
