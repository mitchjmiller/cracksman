using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour {
  [SerializeField] private Enemy enemy;
  [SerializeField][Range(0, 1)] private float footstepVolume = 0.6f;
  [SerializeField] private AudioClip[] footsteps;
  [SerializeField][Range(0, 1)] private float alertVolume = 1f;
  [SerializeField] private AudioClip alertSound;
  private AudioSource source;

  private void Awake() {
    source = GetComponent<AudioSource>();
  }

  private void Start() {
    enemy.OnStateChange.AddListener(OnStateChange);
    enemy.OnFootstep.AddListener(OnFootstep);
  }

  private void OnDestroy() {
    enemy.OnStateChange.RemoveListener(OnStateChange);
    enemy.OnFootstep.RemoveListener(OnFootstep);
  }

  public void OnFootstep() {
    int index = Random.Range(0, footsteps.Length);
    source.PlayOneShot(footsteps[index], footstepVolume);
  }

  private void OnStateChange() {
    if (enemy.GetState() == "EnemyAlertState") OnAlert();
  }

  private void OnAlert() {
    source.PlayOneShot(alertSound, alertVolume);
  }
}
