namespace Chroma.Engine
{
    public class Entity
    {
        public bool active = true;

        public Entity()
        {
            Uid = World.EntityIdNum;
            World.EntityIdNum++;
        }

        public int Uid { get; }
    }
}