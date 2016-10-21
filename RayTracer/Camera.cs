using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    public class Camera
    {
        /// <summary>
        /// Camera location
        /// </summary>
        public Vector3D Location
        {
            get; set;
        }

        /// <summary>
        /// The location the camera is looking at
        /// </summary>
        public Vector3D LookAt
        {
            get; set;
        }

        /// <summary>
        /// The direction in which the camera is looking
        /// </summary>
        private Vector3D zaxis;

        /// <summary>
        /// Default constructor
        /// </summary>
        private void Init()
        {
            zaxis = LookAt - Location;
        }

        /// <summary>
        /// Get the ray for the given positions in the matrix
        /// </summary>
        /// <param name="x">x position in the image matrix</param>
        /// <param name="y">y position in the image matrix</param>
        /// <returns>The launched ray</returns>
        public Ray GetCameraRay(int x, int y)
        {
            Vector3D lookAt = new Vector3D(x - Location.X, -(y - Location.Y), 1 - Location.Z);
            return new Ray(Location, lookAt);
        }
    }
}
