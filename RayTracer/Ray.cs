using System.Windows.Media.Media3D;

namespace RayTracer
{
    /// <summary>
    /// Ray class
    /// </summary>
    public class Ray
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="source">Ray source</param>
        /// <param name="direction">Ray direction</param>
        public Ray(Vector3D source, Vector3D direction)
        {
            Source = source;
            direction.Normalize();
            Direction = direction;
        }

        /// <summary>
        /// Ray direction
        /// </summary>
        public Vector3D Direction
        {
            get; set;
        }

        /// <summary>
        /// Ray source
        /// </summary>
        public Vector3D Source
        {
            get; set;
        }
    }
}
