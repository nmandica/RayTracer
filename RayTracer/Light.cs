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
        private Color m_Color = Color.White;

        /// <summary>
        /// Light color
        /// </summary>
        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;
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
