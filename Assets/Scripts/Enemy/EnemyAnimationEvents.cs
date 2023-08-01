using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour {
  [SerializeField] private Enemy enemy;

  private void OnFootstep() => enemy.Footstep();
}
