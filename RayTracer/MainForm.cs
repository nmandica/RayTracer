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

            MessageBox.Show(Rectangle.Exists(new Point3D(0, 0, 0), new Point3D(10, 0, 0), new Point3D(10, 5, 0), new Point3D(0, 5, 0)).ToString());
        }
    }
}
