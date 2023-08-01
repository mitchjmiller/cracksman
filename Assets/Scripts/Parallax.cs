using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
  [SerializeField] private Transform cam;
  [SerializeField] private SpriteRenderer reference;
  [SerializeField] private float parallaxEffect = 0.5f;
  [SerializeField] private bool repeat = true;
  private float startPos, offsetX;

  private void Start() {
    startPos = transform.position.x;
  }

  private void Update() {
    float distance = cam.position.x * parallaxEffect;
    transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

    if (repeat) {
      float tempDistance = cam.transform.position.x * (1 - parallaxEffect);
      float length = reference.bounds.size.x;
      if (tempDistance > startPos + length) startPos += length;
      else if (tempDistance < startPos - length) startPos -= length;
    }
  }
}
