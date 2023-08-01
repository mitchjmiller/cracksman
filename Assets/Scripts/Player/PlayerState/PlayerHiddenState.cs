using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHiddenState : PlayerState {
  public PlayerHiddenState(Player player) : base(player) { }

  #region Lifecycle Events
  public override void EnterState() {
    player.inputActions.Player.Hide.canceled += OnHideCanceled;
    player.hidden = true;
  }

  public override void ExitState() {
    player.inputActions.Player.Hide.canceled -= OnHideCanceled;
    player.hidden = false;
  }
  #endregion

  private void OnHideCanceled(InputAction.CallbackContext context) {
    player.ChangeState(player.playerGroundedState);
  }
}
