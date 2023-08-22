using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour {
  [SerializeField] private SpriteRenderer[] caseSprites;
  [SerializeField] private ParticleSystem shatterParticleEffect;
  [SerializeField] private GameObject spawnItem;
  private BoxCollider2D boxCollider;
  private CircleCollider2D circleCollider;
  private Rigidbody2D rb;
  private AudioSource audioSource;

  private void Awake() {
    boxCollider = GetComponent<BoxCollider2D>();
    circleCollider = GetComponent<CircleCollider2D>();
    rb = GetComponent<Rigidbody2D>();
    audioSource = GetComponent<AudioSource>();
  }

  public void Shatter() {
    Debug.Log("Shatter");
    boxCollider.enabled = false;
    circleCollider.enabled = false;
    rb.bodyType = RigidbodyType2D.Static;

    foreach (SpriteRenderer sprite in caseSprites)
      sprite.gameObject.SetActive(false);

    shatterParticleEffect.Play();
    audioSource.Play();
    Instantiate(spawnItem, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    Destroy(gameObject, 3f);
  }
}
