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

        private double intensity = 1;

        /// <summary>
        /// Light intensity
        /// </summary>
        public double Intensity
        {
            get
            {
                return intensity;
            }
            set
            {
                intensity = value;
            }
        }

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
            var dividedIntensity = intensity / numberOfSubLights;

            for (int i = 0; i < numberOfSubLights; i++)
            {
                var randomLight = new Light();
                randomLight.color = color;
                randomLight.intensity = dividedIntensity;
                randomLight.Location = StaticRandom.RandomVectorInSphere(location, radius);

                randomList.Add(randomLight);
            }

            return randomList;
        }
    }
}
