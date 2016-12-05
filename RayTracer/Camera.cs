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
        public Vector3D Location;

        /// <summary>
        /// Focal length of the camera
        /// </summary>
        public double FocalLength;

        /// <summary>
        /// The location the camera is looking at
        /// </summary>
        public Vector3D LookAt;

        /// <summary>
        /// Image width (x resolution of the image)
        /// </summary>
        public int Width;

        /// <summary>
        /// Image height (y resolution of the image)
        /// </summary>
        public int Height;

        /// <summary>
        /// The direction in which the camera is looking
        /// </summary>
        private Vector3D viewDirection;

        /// <summary>
        /// U vector of the camera
        /// </summary>
        private Vector3D U;

        /// <summary>
        /// V vector of the camera
        /// </summary>
        private Vector3D V;

        /// <summary>
        /// Top left point of the view plane of the camera. Used to determined the rays
        /// </summary>
        private Vector3D viewPlaneTopLeftPoint;

        /// <summary>
        /// x increment vector
        /// </summary>
        private Vector3D xIncVector;

        /// <summary>
        /// y increment vector
        /// </summary>
        private Vector3D yIncVector;

        /// <summary>
        /// Default constructor
        /// </summary>
        private void Init()
        {
            
        }

        /// <summary>
        /// Setup the camera before using it
        /// </summary>
        public void Setup()
        {
            viewDirection = LookAt - Location;
            viewDirection.Normalize();

            U = Vector3D.CrossProduct(viewDirection, new Vector3D(0, 1, 0));
            V = Vector3D.CrossProduct(U, viewDirection);

            U.Normalize();
            V.Normalize();

            double viewPlaneWidth = Width / FocalLength;
            double viewPlaneHeight = Height / FocalLength;

            viewPlaneTopLeftPoint = LookAt - V * (viewPlaneHeight / 2) - U * (viewPlaneWidth / 2);
            xIncVector = (U * viewPlaneWidth) / Width;
            yIncVector = (V * viewPlaneHeight) / Height;

            //Console.WriteLine("xIncVec : " + xIncVector.ToString());
            //Console.WriteLine("yIncVec : " + yIncVector.ToString());
            //Console.WriteLine("Width : " + Width.ToString());
            //Console.WriteLine("Height : " + Height.ToString());
        }

        /// <summary>
        /// Get the ray for the given positions in the matrix
        /// </summary>
        /// <param name="x">x position in the image matrix</param>
        /// <param name="y">y position in the image matrix</param>
        /// <returns>The launched ray</returns>
        public Ray GetCameraRay(int x, int y)
        {
            var viewPlanePoint = viewPlaneTopLeftPoint + x * xIncVector + y * yIncVector;
            var castRay = viewPlanePoint - Location;
            //Console.WriteLine("(" + x.ToString() + ", " + y.ToString() + ") : " + viewPlanePoint.ToString());
            return new Ray(Location, castRay);
        }
    }
}
