﻿using Collectables;
using Microsoft.Extensions.DependencyInjection;
using Projectiles;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zombies;

public class SceneContext : MonoBehaviour {
    [SerializeField]
    private ObjectPool<Zombie> zombiePool;

    [SerializeField]
    private ObjectPool<Collectable> zombieResourcePool;

    [SerializeField]
    private ObjectPool<Projectile> playerProjectilePool;

    [SerializeField]
    private ZombieArmy zombieArmy;

    private Injector injector;

    private void Awake() {
        IServiceCollection collection = new ServiceCollection()
            .AddSingleton<IObjectPool<Zombie>>(this.zombiePool)
            .AddKeyedSingleton<IObjectPool<Collectable>>(
                InjectKeys.ZombieResourcePool,
                this.zombieResourcePool)
            .AddKeyedSingleton<IObjectPool<Projectile>>(
                InjectKeys.PlayerProjectilePool,
                this.playerProjectilePool)
            .AddSingleton(this.zombieArmy);
        this.injector = new Injector(collection.BuildServiceProvider());
        this.injector.Inject(SceneManager.GetActiveScene());
    }
}
