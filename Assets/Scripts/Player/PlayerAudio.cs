using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
  [SerializeField] private AudioClip[] footsteps;
  private AudioSource source;

  private void Awake() {
    source = GetComponent<AudioSource>();
  }

  public void Footstep() {
    int index = Random.Range(0, footsteps.Length);
    source.PlayOneShot(footsteps[index]);
  }
}
