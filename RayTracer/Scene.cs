using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    /// <summary>
    /// The scene class for the raytracer.
    /// A scene consists of a collection of geometries and lights.
    /// </summary>
    public class Scene
    {
        private Camera camera;
        private List<Geometry> geometries = new List<Geometry>();
        private List<Light> lights = new List<Light>();

        /// <summary>
        /// The camera of the scene
        /// </summary>
        public Camera Camera
        {
            get
            {
                return camera;
            }
            set
            {
                camera = value;
            }
        }

        /// <summary>
        /// Loads a scene file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The scene to be rendered</returns>
        public static Scene Load(string filePath)
        {
            using (JsonReader reader = new JsonTextReader(new StreamReader(filePath)))
            {
                JsonSerializer ser = new JsonSerializer();
                ser.NullValueHandling = NullValueHandling.Ignore;
                ser.TypeNameHandling = TypeNameHandling.Auto;
                ser.ObjectCreationHandling = ObjectCreationHandling.Replace;
                return ser.Deserialize<Scene>(reader);
            }
        }

        /// <summary>
        /// Serializes this scene to a file named scene.json
        /// TODO: allow naming of file
        /// </summary>
        public void Serialize()
        {
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter("scene.json")))
            {
                JsonSerializer ser = new JsonSerializer();
                ser.NullValueHandling = NullValueHandling.Ignore;
                ser.TypeNameHandling = TypeNameHandling.Auto;
                ser.Serialize(writer, this);
            }
        }

        /// <summary>
        /// The list of objects in the scene
        /// </summary>
        public List<Geometry> Geometries
        {
            get
            {
                return geometries;
            }
            set
            {
                geometries = value;
            }
        }

        /// <summary>
        /// The list of lights in the scene
        /// </summary>
        public List<Light> Lights
        {
            get
            {
                return lights;
            }
            set
            {
                lights = value;
            }
        }
    }
}
