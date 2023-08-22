using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
  [SerializeField] private float raisedPositionOffset;
  [SerializeField] private float speed = 1;
  [SerializeField] private bool raised = false;
  private Rigidbody2D rb;
  private Vector3 loweredPosition;
  private Vector3 raisedPosition;
  private AudioSource audioSource;
  private bool moving = false;

  private void OnDrawGizmosSelected() {
    CalculatePositions();
    Debug.DrawLine(loweredPosition, raisedPosition, Color.cyan);
  }

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    audioSource = GetComponent<AudioSource>();
    CalculatePositions();
  }

  private void Update() {
    if (!moving) return;

    if (raised) {
      if (transform.position.y < raisedPosition.y)
        rb.velocity = Vector3.up * speed;
      else {
        rb.velocity = Vector3.zero;
        transform.position = raisedPosition;
        moving = false;
        audioSource.Stop();
      }
    }
    else if (!raised)
      if (transform.position.y > loweredPosition.y)
        rb.velocity = Vector3.down * speed;
      else {
        rb.velocity = Vector3.zero;
        transform.position = loweredPosition;
        moving = false;
        audioSource.Stop();
      }
  }

  private void CalculatePositions() {
    loweredPosition = raised ? transform.position - Vector3.up * raisedPositionOffset : transform.position;
    raisedPosition = raised ? transform.position : transform.position + Vector3.up * raisedPositionOffset;
  }

  public void SetRaised(bool raised) {
    this.raised = raised;
    this.moving = true;
    audioSource.Play();
  }
}
