#nullable enable

using System.Collections;
using UnityEngine;

namespace Zombies {
    public class ZombieArmy : MonoBehaviour {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private ZombieType[] types = null!;

        [SerializeField]
        private Transform[] positions = null!;

        [SerializeField]
        private Transform target = null!;

        [SerializeField]
        private Zombie zombiePrefab = null!;

        [Header(EditorHeaders.Properties)]
        [Tooltip("sec")]
        [Min(0f)]
        [SerializeField]
        private float minSpawnInterval = 1f;

        [Tooltip("sec")]
        [Min(0f)]
        [SerializeField]
        private float maxSpawnInterval = 10f;

        private Coroutine? spawnCoroutine;

        private void Start() {
            this.spawnCoroutine = StartCoroutine(SpawnZombies());
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
                Zombie zombie = Instantiate(this.zombiePrefab, transform);
                zombie.Init(type, pos, this.target);
                zombie.Died += OnZombieDied;
                print($"{type.name} spawned");
            }
        }

        private void OnZombieDied(Zombie zombie) {
            zombie.Died -= OnZombieDied;
            Destroy(zombie.gameObject);
        }
    }
}
