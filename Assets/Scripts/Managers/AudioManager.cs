using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
  public static AudioManager Instance { get; private set; }
  [SerializeField] private AudioMixer mixer;
  [SerializeField] private AudioSource regularMusic;
  [SerializeField] private AudioSource suspenseMusic;
  [SerializeField] private float crossfadeTime = 1f;

  public enum Snapshot {
    Outside,
    Inside
  }

  public enum MusicState {
    Regular,
    Suspense,
    Alert
  }
  private MusicState currentMusicState;

  const string MASTER_VOLUME = "MasterVolume";
  const string OUTSIDE = "Outside";
  const string INSIDE = "Inside";

  private void Awake() {
    if (Instance != null) {
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  private IEnumerator FadeIn(AudioSource src) {
    while (src.volume < 1) {
      src.volume = Mathf.Min(1, src.volume + Time.deltaTime / crossfadeTime);
      yield return null;
    }
  }

  private IEnumerator FadeOut(AudioSource src) {
    while (src.volume > 0) {
      src.volume = Mathf.Max(0, src.volume - Time.deltaTime / crossfadeTime);
      yield return null;
    }
  }

  private IEnumerator _FadeOutMasterVolume(float duration) {
    while (mixer.GetFloat(MASTER_VOLUME, out float currentVolume) && currentVolume > -80f) {
      mixer.SetFloat(MASTER_VOLUME, currentVolume - Time.deltaTime / duration * 80);
      yield return null;
    }
  }

  public void MuteMasterVolume() {
    mixer.SetFloat(MASTER_VOLUME, -80f);
  }

  public void ResetMasterVolume() {
    mixer.SetFloat(MASTER_VOLUME, 0f);
  }

  public void FadeOutMasterVolume(float duration = 1) {
    StartCoroutine(_FadeOutMasterVolume(duration));
  }



  public void SetSnapshot(Snapshot snapshot) {
    switch (snapshot) {
      case Snapshot.Outside:
        mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { mixer.FindSnapshot(OUTSIDE) }, new float[] { 1 }, 0);
        break;
      case Snapshot.Inside:
        mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { mixer.FindSnapshot(INSIDE) }, new float[] { 1 }, 0);
        break;
    }
  }

  public void SetMusicState(MusicState state) {
    switch (state) {
      case MusicState.Regular:
        StartCoroutine(FadeIn(regularMusic));
        StartCoroutine(FadeOut(suspenseMusic));
        break;
      case MusicState.Suspense:
        StartCoroutine(FadeIn(suspenseMusic));
        StartCoroutine(FadeOut(regularMusic));
        break;
      case MusicState.Alert:
        suspenseMusic.pitch = 1;
        StartCoroutine(FadeIn(suspenseMusic));
        StartCoroutine(FadeOut(regularMusic));
        break;
    }
  }
}
