using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    /// <summary>
    /// Box class. A box is made of Rectangles.
    /// </summary>
    class Box : Geometry
    {
        private List<Rectangle> faces = new List<Rectangle>();
        public List<Rectangle> Faces
        {
            get
            {
                return faces;
            }
        }

        //Box faces
        private Rectangle faceUp;
        private Rectangle faceDown;
        private Rectangle faceRight;
        private Rectangle faceLeft;
        private Rectangle faceFront;
        private Rectangle faceBottom;

        //Box points
        private Point3D point1 = new Point3D();
        private Point3D point2 = new Point3D();
        private Point3D point3 = new Point3D();
        private Point3D point4 = new Point3D();
        private Point3D point5 = new Point3D();
        private Point3D point6 = new Point3D();
        private Point3D point7 = new Point3D();

        private ConcurrentDictionary<Vector3D, Vector3D> normalsDictionary = new ConcurrentDictionary<Vector3D, Vector3D>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="origin">Origin of the box</param>
        /// <param name="width">Width</param>
        /// <param name="length">Length</param>
        /// <param name="height">Height</param>
        /// <param name="xRotation">Box rotation compared to the x axis</param>
        /// <param name="zRotation">Box rotation compared to the z axis</param>
        [Newtonsoft.Json.JsonConstructor]
        public Box(Vector3D origin, double width, double length, double height, double xRotation, double zRotation)
        {
            faces = new List<Rectangle>();

            //These directions will be used to calculate the first points of the box
            Vector3D direction1 = new Vector3D(Math.Cos(zRotation),
                                               Math.Cos(xRotation) * Math.Sin(zRotation),
                                               Math.Sin(xRotation));
            Vector3D direction2 = new Vector3D(-Math.Sin(zRotation),
                                               Math.Cos(xRotation) * Math.Cos(zRotation),
                                               Math.Sin(xRotation));
            Vector3D direction3 = Vector3D.CrossProduct(direction1, direction2);

            direction1.Normalize();
            direction2.Normalize();
            direction3.Normalize();

            //Calculate the points of the box

            //point1, point2, point3 are the necessary points to create faceDown
            point1 = new Point3D(0, 0, 0) + origin;
            point2 = point1 + width * direction1;
            point3 = point1 + length * direction2;

            faceDown = new Rectangle(point1, point2, point3);

            //point4 is the last point of the faceDown
            point4 = faceDown.Point4;

            point5 = point1 + height * direction3;

            faceFront = new Rectangle(point1, point3, point5);
            faceRight = new Rectangle(point1, point2, point5);

            point6 = faceRight.Point4;
            point7 = faceFront.Point4;

            //calculate the last faces
            faceUp = new Rectangle(point5, point6, point7);
            faceBottom = new Rectangle(point2, point4, point6);
            faceLeft = new Rectangle(point3, point4, point7);

            //Add the faces to the list of faces of the box
            faces.Add(faceUp);
            faces.Add(faceDown);
            faces.Add(faceFront);
            faces.Add(faceBottom);
            faces.Add(faceRight);
            faces.Add(faceLeft);
        }

        override public Vector3D GetNormalAtPoint(Vector3D point)
        {
            Vector3D normal = new Vector3D();
            normalsDictionary.TryGetValue(point, out normal);
            return normal;
        }

        /// <summary>
        /// Calculate the intesection point between the ray and the plane of the rectangle.
        /// </summary>
        /// <param name="ray">The ray to check</param>
        /// <param name="intersectionPoint">The result intersection point</param>
        /// <returns>True if the intersection is in the rectangle</returns>
        override public bool Intersects(Ray ray, ref Vector3D intersectionPoint)
        {
            Vector3D tempIntersectionPoint = new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue);
            double distance = double.MaxValue;
            bool doesIntersect = false;

            foreach (Rectangle rectangle in faces)
            {
                if (rectangle.Intersects(ray, ref tempIntersectionPoint))
                {
                    doesIntersect = true;

                    double distanceToFace = Vector3D.Subtract(ray.Source, tempIntersectionPoint).Length;

                    if (distanceToFace < distance)
                    {
                        distance = distanceToFace;
                        normalsDictionary.TryAdd(intersectionPoint, rectangle.Normal);
                    }
                }
            }

            return doesIntersect;
        }
    }
}
