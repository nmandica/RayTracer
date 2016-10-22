using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    public interface IMaterial
    {
        void GetColor(Vector3D point, ref int r, ref int g, ref int b);
    }

    public class SolidColor : IMaterial
    {
        public int r, g, b;
        public double Phong = 0;

        public static SolidColor Default = new SolidColor(255, 255, 255);
        public static SolidColor Black = new SolidColor(0, 0, 0);
        public static SolidColor Red = new SolidColor(255, 0, 0);
        public static SolidColor Green = new SolidColor(0, 255, 0);
        public static SolidColor Blue = new SolidColor(0, 0, 255);

        public SolidColor(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public void GetColor(Vector3D point, ref int r, ref int g, ref int b)
        {
            r = this.r;
            g = this.g;
            b = this.b;
        }
    }
}
