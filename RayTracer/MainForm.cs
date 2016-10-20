using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var rectangle = new Rectangle(new Point3D(-1, 1, 10), new Point3D(3, 4, 10), new Point3D(4.2048, 2.3936, 10), new Point3D(0.2048, -0.6064, 10));
            var ray = new Ray(new Vector3D(0, 2, 0), new Vector3D(0, -0.1, 1));
            var intersection = new Vector3D(0, 0, 0);

            MessageBox.Show(rectangle.Intersects(ray, ref intersection).ToString());
            MessageBox.Show(intersection.ToString());
        }
    }
}
