﻿namespace Shard.Web.ImplementationAPI.Units;

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
}