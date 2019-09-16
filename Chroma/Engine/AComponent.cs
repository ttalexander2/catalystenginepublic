using Microsoft.Xna.Framework;

namespace Chroma.Engine
{
    public abstract class AComponent
    {

        public static string Name => "Abstract Component";
        int UID { get; }
        bool Active { get; set; }

        internal AComponent(int UID)
        {
            this.UID = UID;
            this.Active = true;
        }





    }
}