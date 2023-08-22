using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertState : EnemyState {
  public EnemyAlertState(Enemy enemy) : base(enemy) { }

  public override void EnterState() {
    int index = Random.Range(0, enemy.alertDialogueIds.Length);
    EffectsManager.Instance.ScreenShake(1);
    DialogueManager.Instance.CreateDialogue(enemy.transform, enemy.alertDialogueIds[index]);
    Enemy.OnDetected.Invoke();
  }

  public override void Update() {
    enemy.LookAt(Player.Instance.transform);
  }
}