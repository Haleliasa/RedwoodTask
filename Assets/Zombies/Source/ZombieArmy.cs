#nullable enable

using System.Collections;
using UnityEngine;

namespace Zombies {
    public class ZombieArmy : MonoBehaviour, IInjectComponent {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private ZombieType[] types = null!;

        [SerializeField]
        private Transform[] positions = null!;

        [SerializeField]
        private Transform? target;

        [Header(EditorHeaders.Properties)]
        [Tooltip("sec")]
        [Min(0f)]
        [SerializeField]
        private float minSpawnInterval = 1f;

        [Tooltip("sec")]
        [Min(0f)]
        [SerializeField]
        private float maxSpawnInterval = 10f;

        private IObjectPool<Zombie> zombiePool = null!;
        private Injector injector = null!;
        private Coroutine? spawnCoroutine;

        [Inject]
        public void Inject(
            IObjectPool<Zombie> zombiePool,
            Injector injector) {
            this.zombiePool = zombiePool;
            this.injector = injector;
        }

        public void Init(Transform target) {
            this.target = target;
            StartIfNotStarted();
        }

        private void Start() {
            StartIfNotStarted();
        }

        private void StartIfNotStarted() {
            if (this.spawnCoroutine != null
                || this.target == null) {
                return;
            }
            this.spawnCoroutine = StartCoroutine(SpawnZombies());
        }

        private IEnumerator SpawnZombies() {
            while (true) {
                yield return new WaitForSeconds(
                    Random.Range(this.minSpawnInterval, this.maxSpawnInterval));
                if (this.types.Length == 0
                    || this.positions.Length == 0
                    || this.target == null) {
                    continue;
                }
                ZombieType type = this.types[Random.Range(0, this.types.Length)];
                Vector2 pos = this.positions[Random.Range(0, this.positions.Length)].position;
                Zombie zombie = this.zombiePool.Get();
                this.injector.Inject(zombie.gameObject);
                zombie.Init(type, pos, this.target);
                zombie.Died += OnZombieDied;
            }
        }

        private void OnZombieDied(Zombie zombie) {
            zombie.Died -= OnZombieDied;
            this.zombiePool.Return(zombie);
        }
    }
}
