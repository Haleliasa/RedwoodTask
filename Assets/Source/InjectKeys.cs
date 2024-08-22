public static class InjectKeys {
    public const string ZombieResourcePool = "ZombieResourcePool";

    public const string PlayerProjectilePool = "PlayerProjectilePool";

    public const string ZombieDeathSoundPool = "ZombieDeathSoundPool";
}

public class ZombieResourcePoolAttribute : InjectKeyAttribute {
    public ZombieResourcePoolAttribute() : base(InjectKeys.ZombieResourcePool) { }
}

public class PlayerProjectilePoolAttribute : InjectKeyAttribute {
    public PlayerProjectilePoolAttribute() : base(InjectKeys.PlayerProjectilePool) { }
}

public class ZombieDeathSoundPoolAttribute : InjectKeyAttribute {
    public ZombieDeathSoundPoolAttribute() : base(InjectKeys.ZombieDeathSoundPool) { }
}
