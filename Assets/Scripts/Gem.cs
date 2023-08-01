using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Gem : Collectable {
  [SerializeField] private string gemId;
  [SerializeField] private SpriteRenderer sprite;
  [SerializeField] private Light2D light2d;

  private AudioSource audioSource;
  private float stayAlive = 0;

  private void Awake() {
    audioSource = GetComponent<AudioSource>();
    stayAlive = audioSource.clip.length;
  }

  public override void Collect(Player player) {
    base.Collect(player);
    player.inventory.AddItem(gemId);
    sprite.enabled = false;
    light2d.enabled = false;
    audioSource.Play();
    Destroy(this.gameObject, stayAlive);
  }
}
