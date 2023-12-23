namespace Shard.Web.ImplementationAPI.Units.Fighting;

public static class UnitFightingDetails
{
    public static class Bomber
    {
        public const int InitialHealth = 50;
        public const int AttackDamage = 400;
        public const int AttackPeriod = 60;
    }

    public static class Fighter
    {
        public const int InitialHealth = 80;
        public const int AttackDamage = 10;
        public const int AttackPeriod = 6;
    }

    public static class Cruiser
    {
        public const int InitialHealth = 400;
        public const int AttackDamage = 40;
        public const int AttackPeriod = 6;
    }
    
    public static class Cargo
    {
        public const int InitialHealth = 100;
        public const int AttackDamage = 0;
        public const int AttackPeriod = 1000;
    }
}