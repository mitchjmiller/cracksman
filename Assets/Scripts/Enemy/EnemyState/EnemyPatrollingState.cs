using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyState {
  public EnemyPatrollingState(Enemy enemy) : base(enemy) { }

  public override void OnDrawGizmos() {
    foreach (Vector2 sightLine in enemy.sightLineArray) {
      Vector2 dir = new Vector2(sightLine.x * enemy.transform.right.x, sightLine.y);
      Gizmos.color = Color.yellow;
      Gizmos.DrawRay(enemy.viewPoint.position, dir * enemy.farSightDistance * enemy.visibilityMultiplier);
    }
  }

  public override void Update() {
    enemy.UpdateVisibility();
    SightLineCheck();
    Patrol();
  }

  public override void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.TryGetComponent<Player>(out Player player))
      enemy.ChangeState(enemy.enemyAlertState);
  }

  public override void OnTriggerEnter2D(Collider2D other) {
    if (enemy.patrolPoints.Length == 0) return;
    if (other.name == enemy.patrolPoints[enemy.patrolPointIndex].name) {
      enemy.patrolPointIndex++;
      enemy.patrolPointIndex = enemy.patrolPointIndex >= enemy.patrolPoints.Length ? 0 : enemy.patrolPointIndex;
    }
  }

  private void Patrol() {
    if (enemy.patrolPoints.Length == 0) return;
    enemy.MoveTo(enemy.patrolPoints[enemy.patrolPointIndex]);
  }

  private void SightLineCheck() {
    foreach (Vector2 sightLine in enemy.sightLineArray) {
      Vector2 dir = new Vector2(sightLine.x * enemy.transform.right.x, sightLine.y);
      RaycastHit2D hit = Physics2D.Raycast(enemy.viewPoint.position, dir, enemy.farSightDistance * enemy.visibilityMultiplier, enemy.sightMask);
      if (hit.collider != null && hit.collider.gameObject.TryGetComponent<Player>(out Player player)) {
        enemy.ChangeState(enemy.enemyCuriousState);
        return;
      }
    }
  }
}