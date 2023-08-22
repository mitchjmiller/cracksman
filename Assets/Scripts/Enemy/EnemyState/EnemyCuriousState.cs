using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCuriousState : EnemyState {
  public EnemyCuriousState(Enemy enemy) : base(enemy) { }

  private float cooldownTime = 5f;
  private float sightLineDistance;
  private float sightlineIncreaseScale = 1f;
  private float timer;

  public override void OnDrawGizmos() {
    foreach (Vector2 sightLine in enemy.sightLineArray) {
      Vector2 dir = new Vector2(sightLine.x * enemy.transform.right.x, sightLine.y);
      Gizmos.color = Color.red;
      Gizmos.DrawRay(enemy.viewPoint.position, dir * sightLineDistance * enemy.visibilityMultiplier);
    }
  }

  public override void EnterState() {
    sightLineDistance = enemy.nearSightDistance;
    timer = cooldownTime;
    int index = Random.Range(0, enemy.curiousDialogueIds.Length);
    AudioManager.Instance.SetMusicState(AudioManager.MusicState.Suspense);
    EffectsManager.Instance.ScreenShake(0.2f);
    DialogueManager.Instance.CreateDialogue(enemy.transform, enemy.curiousDialogueIds[index]);
  }

  public override void Update() {
    enemy.UpdateVisibility();
    IncreaseSightLineDistance();
    SightLineCheck();
    Cooldown();
  }



  public override void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.TryGetComponent<Player>(out Player player))
      enemy.ChangeState(enemy.enemyAlertState);
  }

  private void IncreaseSightLineDistance() {
    sightLineDistance += Time.deltaTime * sightlineIncreaseScale;
  }

  private void SightLineCheck() {
    foreach (Vector2 sightLine in enemy.sightLineArray) {
      Vector2 dir = new Vector2(sightLine.x * enemy.transform.right.x, sightLine.y);
      RaycastHit2D hit = Physics2D.Raycast(enemy.viewPoint.position, dir, sightLineDistance * enemy.visibilityMultiplier, enemy.sightMask);
      if (hit.collider != null && hit.collider.gameObject.TryGetComponent<Player>(out Player player)) {
        enemy.ChangeState(enemy.enemyAlertState);
        return;
      }
    }
  }

  private void Cooldown() {
    timer -= Time.deltaTime;
    if (timer <= 0)
      enemy.ChangeState(enemy.enemyPatrollingState);
  }
}