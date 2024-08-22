using Collectables;
using Microsoft.Extensions.DependencyInjection;
using Projectiles;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zombies;

public class SceneContext : MonoBehaviour {
    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private ObjectPool<Zombie> zombiePool;

    [SerializeField]
    private ObjectPool<Collectable> zombieResourcePool;

    [SerializeField]
    private ObjectPool<AudioSourceGroup> zombieDeathSoundPool;

    [SerializeField]
    private ObjectPool<Projectile> playerProjectilePool;

    [SerializeField]
    private ZombieArmy zombieArmy;

    private Injector injector;

    private void Awake() {
        IServiceCollection collection = new ServiceCollection()
            .AddSingleton(this.camera)
            .AddSingleton<IConcreteObjectPool<Zombie>>(this.zombiePool)
            .AddKeyedSingleton<IObjectPool<Collectable>>(
                InjectKeys.ZombieResourcePool,
                this.zombieResourcePool)
            .AddKeyedSingleton<IObjectPool<AudioSourceGroup>>(
                InjectKeys.ZombieDeathSoundPool,
                this.zombieDeathSoundPool)
            .AddKeyedSingleton<IObjectPool<Projectile>>(
                InjectKeys.PlayerProjectilePool,
                this.playerProjectilePool)
            .AddSingleton(this.zombieArmy);
        this.injector = new Injector(collection.BuildServiceProvider());
        this.injector.Inject(SceneManager.GetActiveScene());
    }
}
