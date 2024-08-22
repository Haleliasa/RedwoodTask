#nullable enable

using System.Collections;
using UnityEngine;

public class AudioSourceGroup : MonoBehaviour {
    [SerializeField]
    private AudioSource[] sources = null!;

    public AudioSource? Playing { get; private set; }

    public void PlayOneRandomly() {
        StartCoroutine(PlayOneRandomlyRoutine());
    }

    public IEnumerator PlayOneRandomlyRoutine() {
        return Play(this.sources[Random.Range(0, this.sources.Length)]);
    }

    private IEnumerator Play(AudioSource source) {
        source.Play();
        Playing = source;
        yield return new WaitWhile(() => source.isPlaying);
        Playing = null;
    }
}
