using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Catalyst.Editor")]
[assembly: InternalsVisibleTo("Test")]
namespace Catalyst.Engine.Serialization
{
    internal static class SceneSerializer
    {

        internal static string WriteToJson(Scene scene)
        {
            return JsonConvert.SerializeObject(scene.Manager.Entities.Values.ToList(), Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        internal static List<Entity> ReadFromJson(string json)
        {
            return JsonConvert.DeserializeObject<List<Entity>>(json, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

    }
}
