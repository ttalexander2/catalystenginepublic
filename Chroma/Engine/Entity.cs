namespace Chroma.Engine
{
    public class Entity
    {
        public bool active = true;
        public bool collidable = false;

        public bool visible = true;

        public Entity()
        {
            Uid = World.EntityIdNum;
            World.EntityIdNum++;
        }

        public int Uid { get; }
    }
}