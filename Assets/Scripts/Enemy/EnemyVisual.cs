using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour {
  private const string X_VELOCITY = "XVelocity";
  private const string Y_VELOCITY = "YVelocity";

  [SerializeField] private Enemy enemy;
  [SerializeField] private Animator animator;

  private void Update() {
    animator.SetFloat(X_VELOCITY, Mathf.Abs(enemy.rb.velocity.x));
    animator.SetFloat(Y_VELOCITY, Mathf.Abs(enemy.rb.velocity.y));
  }
}
