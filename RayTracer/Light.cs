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
        private double radius = 100;
        /// <summary>
        /// Number of sub-lights on the light sphere
        /// </summary>
        private int numberOfSubLights = 5;

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
                color = Color.FromArgb(value.A,
                                       (int) Math.Ceiling(((double) value.R) / numberOfSubLights),
                                       (int) Math.Ceiling(((double) value.G) / numberOfSubLights),
                                       (int) Math.Ceiling(((double) value.B) / numberOfSubLights));
            }
        }

        /// <summary>
        /// Light source location
        /// </summary>
        public Vector3D Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        /// <summary>
        /// Light source sphere radius
        /// </summary>
        public double Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        /// <summary>
        /// Get a list of random sub-lights.
        /// </summary>
        /// <returns>A list of random lights on the sphere</returns>
        public List<Light> RandomSubLights()
        {
            var randomList = new List<Light>();

            for (int i = 0; i < numberOfSubLights; i++)
            {
                var randomLight = new Light();
                randomLight.color = color;
                randomLight.Location = StaticRandom.RandomVectorInSphere(location, radius);

                randomList.Add(randomLight);
            }

            return randomList;
        }
    }
}
