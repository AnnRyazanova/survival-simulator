namespace Characters.NPC
{
    public class NpcObject : Object
    {
        public HealthProperty Health { get; private set; }
    
        protected override void Start()
        {
            base.Start();
            Health = GetComponent<HealthProperty>();
            type = ObjectType.Mob;
        }
    }
}
