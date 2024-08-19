using UnityEngine;

namespace Zombies {
    [CreateAssetMenu(
        fileName = nameof(ZombieType),
        menuName = "Zombies/" + nameof(ZombieType))]
    public class ZombieType : ScriptableObject {
        [SerializeField]
        private ZombieViewType viewType = ZombieViewType.Regular;

        [Min(1f)]
        [SerializeField]
        private float health = 100f;

        [Tooltip("units/sec")]
        [Min(1f)]
        [SerializeField]
        private float speed = 10f;

        [Min(0)]
        [SerializeField]
        private int minResources = 1;

        [Min(0)]
        [SerializeField]
        private int maxResources = 10;

        public ZombieViewType ViewType => this.viewType;

        public float Health => this.health;

        public float Speed => this.speed;

        public int MinResources => this.minResources;

        public int MaxResources => this.maxResources;
    }
}
