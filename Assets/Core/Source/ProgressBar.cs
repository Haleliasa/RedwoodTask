using UnityEngine;

public class ProgressBar : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer progress = null!;

    private float? progressSize;
    private float? progressPos;

    private float StartSize => this.progressSize ??= this.progress.size.x;

    private float StartPos => this.progressPos ??= this.progress.transform.localPosition.x;

    public void SetProgress(float percent) {
        float size = StartSize * percent;
        this.progress.size = this.progress.size.Set(x: size);
        this.progress.transform.localPosition =
            this.progress.transform.localPosition.Set(x: StartPos - ((StartSize - size) / 2f));
    }
}
