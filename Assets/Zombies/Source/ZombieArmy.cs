#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zombies {
    public class ZombieArmy : MonoBehaviour, IInjectComponent, ISerializedEventListener {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private ZombieType[] types = null!;

        [SerializeField]
        private Transform[] positions = null!;

        [SerializeField]
        private Transform target = null!;

        [Header(EditorHeaders.EventSubscriptions)]
        [SerializeField]
        private SerializedEvent[] startOnEvents = null!;

        [SerializeField]
        private SerializedEvent[] stopOnEvents = null!;

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
        private IObjectPool<AudioSourceGroup> zombieDeathSoundPool = null!;
        private Injector injector = null!;
        private bool started = false;
        private Coroutine? spawnCoroutine;
        private readonly List<Zombie> zombies = new();

        [Inject]
        public void Inject(
            IObjectPool<Zombie> zombiePool,
            // key in case of multiple pools of type AudioSourceGroup
            [ZombieDeathSoundPool] IObjectPool<AudioSourceGroup> zombieDeathSoundPool,
            Injector injector) {
            this.zombiePool = zombiePool;
            this.zombieDeathSoundPool = zombieDeathSoundPool;
            this.injector = injector;
        }

        public void Init(Transform target) {
            this.target = target;
            StartArmy(clearZombies: false);
        }

        void ISerializedEventListener.OnSerializedEvent(SerializedEvent ev, Object source) {
            if (this.startOnEvents.Contains(ev)) {
                StartArmy(clearZombies: true);
                this.started = true;
            } else if (this.stopOnEvents.Contains(ev)) {
                StopArmy();
                this.started = false;
            }
        }

        private void OnEnable() {
            if (this.started) {
                StartArmy(clearZombies: false);
            }
            this.startOnEvents.ForEach(e => e.Subscribe(this));
            this.stopOnEvents.ForEach(e => e.Subscribe(this));
        }

        private void OnDisable() {
            StopArmy();
            this.startOnEvents.ForEach(e => e.Unsubscribe(this));
            this.stopOnEvents.ForEach(e => e.Unsubscribe(this));
        }

        private void StartArmy(bool clearZombies) {
            if (!enabled) {
                return;
            }

            if (clearZombies) {
                this.zombies.ForEach(z => DestroyZombie(z, removeFromList: false));
                this.zombies.Clear();
            }

            if (this.spawnCoroutine != null) {
                return;
            }

            this.spawnCoroutine = StartCoroutine(SpawnZombies());
        }

        private void StopArmy() {
            if (this.spawnCoroutine != null) {
                StopCoroutine(this.spawnCoroutine);
                this.spawnCoroutine = null;
            }
            this.zombies.NotNull().ForEach(z => z.enabled = false);
        }

        private IEnumerator SpawnZombies() {
            while (true) {
                yield return new WaitForSeconds(
                    Random.Range(this.minSpawnInterval, this.maxSpawnInterval));
                if (this.types.Length == 0
                    || this.positions.Length == 0) {
                    continue;
                }
                ZombieType type = this.types[Random.Range(0, this.types.Length)];
                Vector2 pos = this.positions[Random.Range(0, this.positions.Length)].position;
                Zombie zombie = this.zombiePool.Get();
                this.injector.Inject(zombie.gameObject);
                zombie.Init(type, pos, this.target);
                zombie.Died += OnZombieDied;
                this.zombies.Add(zombie);
            }
        }

        private void OnZombieDied(Zombie zombie) {
            DestroyZombie(zombie, removeFromList: true);
            StartCoroutine(PlayDeathSound());
        }

        private void DestroyZombie(Zombie zombie, bool removeFromList) {
            if (removeFromList) {
                this.zombies.Remove(zombie);
            }
            zombie.Died -= OnZombieDied;
            this.zombiePool.Return(zombie);
        }

        private IEnumerator PlayDeathSound() {
            AudioSourceGroup deathSound = this.zombieDeathSoundPool.Get();
            yield return deathSound.PlayOneRandomlyRoutine();
            this.zombieDeathSoundPool.Return(deathSound);
        }
    }
}
