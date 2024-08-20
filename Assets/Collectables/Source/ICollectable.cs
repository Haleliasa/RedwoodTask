namespace Collectables {
    public interface ICollectable {
        string Name { get; }

        string Type { get; }

        int Value { get; }

        void Collect();
    }
}
