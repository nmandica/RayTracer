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
            get; set;
        }
    }
}
