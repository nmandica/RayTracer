using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    internal class Raytracer
    {
        private int pixelSubdivision = 2;

        private object RandomLock = new object();
        private object callbackLock = new object();
        private Graphics graphic;
        private int processorCount;
        private bool stop = false;
        private Action updateCallback;
        private volatile int current = 0;

        public delegate void ProgressHandler(double percent);
        public event ProgressHandler OnProgress;

        public Raytracer()
        {
            //processorCount = Math.Max(Environment.ProcessorCount - 1, 1);
            processorCount = 1; //Bug if multi thread
        }

        private void DoProgress(double percent)
        {
            OnProgress?.Invoke(percent);
        }

        private void ReportProgress()
        {
            if (current == 0)
            {
                DoProgress(0);
            }
            else
            {
                DoProgress((double)current / (double)(Size.Width * Size.Height));
            }
        }

        public Color BackColor { get; set; }

        public int RayDepth { get; set; } = 3;

        public Scene Scene { get; set; }

        public Size Size { get; set; }

        public bool Stop { get; set; }

        /// <summary>
        /// Raytrace the scene onto the given image.
        /// </summary>
        /// <param name="image">Image that will be rendered to.</param>
        public void Raytrace(Image image)
        {
            Raytrace(image, null, null);
        }

        /// <summary>
        /// Raytrace the scene onto the given image.
        /// </summary>
        /// <param name="image">Image that will be rendered to.</param>
        /// <param name="onUpdate">Called after each row is rendered.</param>
        /// <param name="onFinished">Called when rendering is complete.</param>
        public void Raytrace(Image image, Action onUpdate, Action onFinished)
        {
            Size = new Size(image.Width, image.Height);
            Scene.Camera.Width = image.Width * pixelSubdivision;
            Scene.Camera.Height = image.Height * pixelSubdivision;
            Scene.Camera.FocalLength *= pixelSubdivision;
            Scene.Camera.Setup();

            updateCallback = onUpdate;
            graphic = Graphics.FromImage(image);

            new Thread(() =>
            {
                Raytrace(onFinished);
            }).Start();
        }

        private ColorAccumulator CalculateLighting(HitInfo info, int count)
        {
            ColorAccumulator colorAccumulator = new ColorAccumulator();

            foreach (Light light in Scene.Lights)
            {
                GetColor(info, light, colorAccumulator, count);

                //Bidirectional ray tracing
                var randomDirection = StaticRandom.RandomVectorInUnitSphere();
                var randomRay = new Ray(light.Location + randomDirection, randomDirection);
                var hitInfo = FindHitObject(randomRay);
                var secondaryLight = new Light();
                secondaryLight.Location = hitInfo.hitPoint - randomRay.Direction;
                int squaredDistance = (int)(secondaryLight.Location - randomRay.Source).LengthSquared + 1;
                secondaryLight.Color = light.Color;
                secondaryLight.Intensity = light.Intensity / squaredDistance;

                GetColor(info, secondaryLight, colorAccumulator, count);
            }

            return colorAccumulator;
        }

        private void CastCameraRay(int col, int row)
        {
            if (this.stop)
            {
                return;
            }

            int r = 0, g = 0, b = 0;
            //Calculate each pixel with the mean of all the subpixels
            for (int i = col * pixelSubdivision; i < (col + 1) * pixelSubdivision; i++)
            {
                for (int j = row * pixelSubdivision; j < (row + 1) * pixelSubdivision; j++)
                {
                    var ray = Scene.Camera.GetCameraRay(i, j);
                    var colorAccu = CastRay(ray, 1);
                    colorAccu.Clamp();

                    r += colorAccu.R;
                    g += colorAccu.G;
                    b += colorAccu.B;
                }
            }

            r /= pixelSubdivision * pixelSubdivision;
            g /= pixelSubdivision * pixelSubdivision;
            b /= pixelSubdivision * pixelSubdivision;

            SolidBrush brush = new SolidBrush(Color.FromArgb(r, g, b));

            lock (graphic)
            {
                graphic.FillRectangle(brush, col, row, 1, 1);
            }
        }

        private ColorAccumulator CastRay(Ray ray, int count)
        {
            if (count > RayDepth)
            {
                return null;
            }

            ColorAccumulator colorAccumulator = null;
            HitInfo info = FindHitObject(ray);

            if (info.hitObject != null)
            {
                colorAccumulator = CalculateLighting(info, count);
                //colorAccumulator.Clamp();
            }
            else
            {
                colorAccumulator = new ColorAccumulator(BackColor.R, BackColor.G, BackColor.B);
            }

            return colorAccumulator;
        }

        private HitInfo FindHitObject(Ray ray) => FindHitObject(ray, null, HitMode.Closest);

        private HitInfo FindHitObject(Ray ray, Geometry originator, HitMode mode)
        {
            Vector3D intersectionPoint = new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue);
            HitInfo info = new HitInfo(null, intersectionPoint, ray);
            double distance = double.MaxValue;

            foreach (Geometry geometry in Scene.Geometries)
            {
                if (geometry != originator && geometry.Intersects(ray, ref intersectionPoint))
                {
                    double distanceToObject = Vector3D.Subtract(ray.Source, intersectionPoint).Length;
                    if (distanceToObject < distance)
                    {
                        info.hitPoint = intersectionPoint;
                        distance = distanceToObject;
                        info.hitObject = geometry;

                        if (mode == HitMode.Any)
                        {
                            break;
                        }
                    }
                }
            }

            return info;
        }

        private void GetColor(HitInfo info, Light light, ColorAccumulator colorAccu, int count)
        {
            Vector3D lightLocation = light.Location;
            Vector3D lightNormal = info.hitPoint - lightLocation;
            lightNormal.Normalize();

            double lambert = Vector3D.DotProduct(lightNormal, info.normal);

            if (lambert <= 0)
            {
                int r, g, b;
                r = g = b = 0;

                int r2 = 0;
                int g2 = 0;
                int b2 = 0;

                info.hitObject.GetColor(info.hitPoint, ref r, ref g, ref b);

                if (info.hitObject.Material != null)
                {
                    var objectMaterial = info.hitObject.Material;

                    //Phong
                    if (objectMaterial is SolidColor)
                    {
                        if (InShadow(info, lightLocation, lightNormal))
                        {
                            return;
                        }

                        double phongTerm = Math.Pow(lambert, 20) * (objectMaterial as SolidColor).Phong * 2 * light.Intensity;
                        r2 = (int)(light.Color.R * phongTerm);
                        g2 = (int)(light.Color.G * phongTerm);
                        b2 = (int)(light.Color.B * phongTerm);

                        var intensityFactor = light.Intensity * -lambert / 255;

                        colorAccu.R += (int)(light.Color.R * r * intensityFactor) + r2;
                        colorAccu.G += (int)(light.Color.G * g * intensityFactor) + g2;
                        colorAccu.B += (int)(light.Color.B * b * intensityFactor) + b2;
                    }    
                    //Reflection
                    else if (objectMaterial is Metal)
                    {
                        var metal = objectMaterial as Metal;

                        //double phongTerm = Math.Pow(lambert, 20) * 0.6 * 2 * light.Intensity;
                        //r2 = (int)(light.Color.R * phongTerm);
                        //g2 = (int)(light.Color.G * phongTerm);
                        //b2 = (int)(light.Color.B * phongTerm);

                        double reflet = 2.0f * (Vector3D.DotProduct(info.normal, info.ray.Direction));
                        Vector3D direction = info.ray.Direction - info.normal * reflet;
                        direction.Normalize();
                        direction += metal.Fuzz * StaticRandom.RandomVectorInUnitSphere(); //Random in cone
                        //direction.Normalize();
                        Ray reflect = new Ray(info.hitPoint + direction / 100000, direction);
                        ColorAccumulator reflectedColorAccu = CastRay(reflect, ++count);

                        if (reflectedColorAccu != null)
                        {
                            var attenuation = metal.Reflection * light.Intensity;
                            colorAccu.R += (int)(reflectedColorAccu.R * attenuation) + r2;
                            colorAccu.G += (int)(reflectedColorAccu.G * attenuation) + g2;
                            colorAccu.B += (int)(reflectedColorAccu.B * attenuation) + b2;
                        }
                    }
                }
            }
        }

        private bool InShadow(HitInfo info, Vector3D lightLocation, Vector3D lightNormal)
        {
            Ray shadowRay = new Ray(lightLocation, lightNormal);
            HitInfo shadingInfo = FindHitObject(shadowRay, info.hitObject, HitMode.Closest);

            if (shadingInfo.hitObject != null && 
                Vector3D.Subtract(lightLocation, info.hitPoint).Length > 
                Vector3D.Subtract(lightLocation, shadingInfo.hitPoint).Length)
            {
                return true;
            }

            return false;
        }

        private void Raytrace(Action onFinished)
        {
            current = 0;
            double segmentSize = Math.Ceiling(Size.Height / (double)processorCount);
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < processorCount; i++)
            {
                int start = 0 + (int)(i * segmentSize);
                int stop = Math.Min((int)segmentSize + (int)(i * segmentSize), Size.Height);

                Thread t = new Thread(() =>
                {
                    RenderRows(start, stop);
                });
                t.Start();
                threads.Add(t);
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }

            if (onFinished != null && !this.stop)
            {
                onFinished.Invoke();
            }
        }

        private void RenderRow(int row)
        {
            if (this.stop)
            {
                return;
            }

            for (int i = 0; i < Size.Width; i++)
            {
                CastCameraRay(i, row);
                current++;
            }

            if (updateCallback != null)
            {
                try
                {
                    lock (callbackLock)
                    {
                        if (!this.stop)
                        {
                            updateCallback.Invoke();
                        }
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void RenderRows(int start, int stop)
        {
            for (int j = start; j < stop; j++)
            {
                RenderRow(j);
                ReportProgress();
            }
        }
    }

    internal enum HitMode
    {
        Any,
        Closest
    }

    internal class ColorAccumulator
    {
        public int R = 0;
        public int G = 0;
        public int B = 0;

        public ColorAccumulator()
        {
        }

        public ColorAccumulator(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static ColorAccumulator operator +(ColorAccumulator left, ColorAccumulator right)
        {
            ColorAccumulator sum = new ColorAccumulator();
            sum.R = left.R + right.R;
            sum.G = left.G + right.G;
            sum.B = left.B + right.B;
            return sum;
        }

        public void Clamp()
        {
            double ratio = 1;
            ratio = Math.Max(R / 255.0, ratio);
            ratio = Math.Max(G / 255.0, ratio);
            ratio = Math.Max(B / 255.0, ratio);

            R = (int)(R / ratio);
            G = (int)(G / ratio);
            B = (int)(B / ratio);
        }
    }

    internal class HitInfo
    {
        public Geometry hitObject;
        public Vector3D hitPoint;
        public Ray ray;

        public HitInfo(Geometry hitObj, Vector3D hitPoint, Ray ray)
        {
            this.hitObject = hitObj;
            this.hitPoint = hitPoint;
            this.ray = ray;
        }

        public Vector3D normal
        {
            get
            {
                if (hitObject != null)
                {
                    return hitObject.GetNormalAtPoint(hitPoint);
                }
                else
                {
                    throw new Exception("hitObject is null");
                }
            }
        }
    }
}
