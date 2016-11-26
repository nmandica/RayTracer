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
        private double radius = 200;
        /// <summary>
        /// Number of sub-lights on the light sphere
        /// </summary>
        private int numberOfSubLights = 4;

        /// <summary>
        /// Light color
        /// </summary>
        public Color Color
        {
            get
            {
                return Color.FromArgb(color.A,
                    color.R / numberOfSubLights,
                    color.G / numberOfSubLights,
                    color.B / numberOfSubLights);
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
        /// Random sub-light location on the light sphere
        /// </summary>
        private Vector3D RandomSubLightLocation()
        {
                var x = StaticRandom.NextGaussian();
                var y = StaticRandom.NextGaussian();
                var z = StaticRandom.NextGaussian();

                var randomLocation = new Vector3D(x, y, z);
                randomLocation.Normalize();
                randomLocation = radius * randomLocation + location;
                //Console.WriteLine(randomLocation.ToString());
                return randomLocation;
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
                randomLight.Color = color;
                randomLight.Location = RandomSubLightLocation();

                randomList.Add(randomLight);
            }

            return randomList;
        }
    }
}
