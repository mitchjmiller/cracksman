using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour {
  [SerializeField] private Enemy enemy;
  [SerializeField] private AudioClip[] footsteps;
  private AudioSource source;

  private void Awake() {
    source = GetComponent<AudioSource>();
  }

  private void Start() {
    enemy.OnFootstep.AddListener(OnFootstep);
  }

  private void OnDestroy() {
    enemy.OnFootstep.RemoveListener(OnFootstep);
  }

  public void OnFootstep() {
    int index = Random.Range(0, footsteps.Length);
    source.PlayOneShot(footsteps[index]);
  }
}
