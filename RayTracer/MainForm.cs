using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    public partial class MainForm : Form
    {
        private Raytracer rayTracer;
        private Scene scene;
        private DateTime startTime;

        public MainForm()
        {
            InitializeComponent();
            rayTracer = new Raytracer();
            rayTracer.OnProgress += new Raytracer.ProgressHandler(rt_OnProgress);
            RayDepthNumericUpDown.Value = rayTracer.RayDepth = 1;
            RayDepthNumericUpDown.ValueChanged += new EventHandler(RayDepthNumericUpDown_ValueChanged);
        }

        void RayDepthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            rayTracer.RayDepth = (int)RayDepthNumericUpDown.Value;
        }

        void rt_OnProgress(double percent)
        {
            Invoke(new Action(() =>
            {
                toolStripProgressBar1.Value = (int)(Math.Ceiling(100 * percent));
                var elapsed = DateTime.Now - startTime;
                toolStripStatusLabel4.Text = string.Format("{0}m {1}s {2}ms", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds);
            }));
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now;
            RayTrace();
        }

        private void RayTrace()
        {
            ControlPanel.Enabled = false;

            PictureBox.Image = new Bitmap(PictureBox.Size.Width, PictureBox.Size.Height);
            PictureBox.Update();

            rayTracer.Size = PictureBox.Image.Size;

            rayTracer.Scene = scene;
            rayTracer.BackColor = Color.Black;

            rayTracer.Raytrace(PictureBox.Image, () =>
            {
                Invoke(new Action(() =>
                {
                    PictureBox.Refresh();
                }));
            }, () =>
            {
                Invoke(new Action(() =>
                {
                    ControlPanel.Enabled = true;
                }));
            });
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON Scene|*.json";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    scene = Scene.Load(ofd.FileName);
                    GoButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error loading scene", MessageBoxButtons.OK);
                    return;
                }

                tsslblScene.Text = Path.GetFileName(ofd.FileName);
                ControlPanel.Enabled = true;

            }

        }
    }
}
