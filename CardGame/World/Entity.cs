namespace CardGame
{
    public class Entity
    {
        public static int s_ID = 0;

        public int Tag { get; private set; }
        public int ID { get; private set; }
        public string Name { get; set; }
        public Transform2D Transform { get; set; }
        public bool Active;
        public bool IsDestroyed { get; private set; }
        private string m_Tag;

        public Entity(string name, bool active = true)
        {
            Name = name;
            ID = s_ID++;
            Transform = new Transform2D(this);
            Active = active;
            IsDestroyed = false;
        }

        ~Entity()
        {
            IsDestroyed = true;
            Active = false;
        }

        // Set tag from string, tag stored as int for speed
        public void SetTag(string tag)
        {
            Tag = tag.GetHashCode();
            m_Tag = tag;
        }

        // Get the tag as a string
        public string TagToString()
        {
            return m_Tag;
        }

        public virtual void Update(float deltaTime)
        {
            // do update
        }

        public void Destroy()
        {
            IsDestroyed = true;
        }
    }
}