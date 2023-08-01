using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyExclamation : MonoBehaviour {
  [SerializeField] private Enemy enemy;
  [SerializeField] private Color curiousColor = Color.yellow;
  [SerializeField] private Color alertColor = Color.red;

  private TextMeshPro exclamation;

  #region Lifycycle Events
  private void Awake() {
    exclamation = GetComponent<TextMeshPro>();
  }

  private void Start() {
    enemy.OnStateChange.AddListener(OnStateChange);
  }

  private void OnDestroy() {
    enemy.OnStateChange.RemoveListener(OnStateChange);
  }

  private void LateUpdate() {
    this.transform.forward = Camera.main.transform.forward;
  }
  #endregion

  private void OnStateChange() {
    switch (enemy.GetState()) {
      case "EnemyCuriousState":
        exclamation.text = "?";
        exclamation.color = curiousColor;
        break;
      case "EnemyAlertState":
        exclamation.text = "!";
        exclamation.color = alertColor;
        break;
      default:
        exclamation.text = "";
        break;
    }
  }
}
