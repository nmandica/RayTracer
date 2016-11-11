using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    /// <summary>
    /// Light class
    /// </summary>
    public class Light
    {
        private Color color = Color.White;
        private Vector3D location;
        private double radius = 3;

        /// <summary>
        /// Light color
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Light source location
        /// </summary>
        public Vector3D Location
        {
            get
            {
                Vector3D randomLocation;
                var r = new Random();
                var x = r.NextGaussian();
                var y = r.NextGaussian();
                var z = r.NextGaussian();

                randomLocation = new Vector3D(x, y, z);
                randomLocation.Normalize();
                randomLocation = radius * randomLocation + location;
                //Console.WriteLine(randomLocation.ToString());
                return randomLocation;
            }
            set
            {
                location = value;
            }
        }
    }
}
