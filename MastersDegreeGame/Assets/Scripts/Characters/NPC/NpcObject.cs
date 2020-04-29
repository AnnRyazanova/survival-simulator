namespace Characters.NPC
{
    public class NpcObject : Object
    {
        public HealthProperty Health { get; private set; }
        public DamageProperty Damage { get; private set; }

        protected override void Start()
        {
            base.Start();
            Health = GetComponent<HealthProperty>();
            Damage = GetComponent<DamageProperty>();

            type = ObjectType.Mob;
        }
    }
}
