using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
  [SerializeField] private Transform target;
  [SerializeField] private Vector3 offset;

  private void OnDrawGizmos() {
    Update();
  }

  private void Update() {
    transform.position = target.position + offset;
  }
}
