using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BigGem : Collectable {
  [SerializeField] private string gemId;
  [SerializeField] private SpriteRenderer sprite;
  [SerializeField] private Light2D light2d;

  private BoxCollider2D coll;
  private CircleCollider2D triggerColl;
  private AudioSource audioSource;
  private float stayAlive = 0;

  private void Awake() {
    coll = GetComponent<BoxCollider2D>();
    triggerColl = GetComponent<CircleCollider2D>();
    audioSource = GetComponent<AudioSource>();
    stayAlive = audioSource.clip.length;
  }

  public override void Collect(Player player) {
    base.Collect(player);
    player.inventory.AddItem(gemId);
    coll.enabled = false;
    triggerColl.enabled = false;
    sprite.enabled = false;
    light2d.enabled = false;
    audioSource.Play();
    Destroy(this.gameObject, stayAlive);
  }
}
