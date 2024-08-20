using System.Collections;

namespace Player {
    public interface IShootTiming {
        IEnumerator BeforeShoot(float interval);
    }
}
