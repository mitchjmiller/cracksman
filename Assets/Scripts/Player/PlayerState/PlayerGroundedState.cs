using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerState {
  public PlayerGroundedState(Player player) : base(player) { }

  #region Lifecycle Events
  public override void EnterState() {
    InputManager.Instance.inputActions.Player.Jump.performed += OnJumpPerformed;
    InputManager.Instance.inputActions.Player.Hide.performed += OnHidePerformed;
    player.rb.gravityScale = 1;

    player.OnFootstep.Invoke();
  }

  public override void ExitState() {
    InputManager.Instance.inputActions.Player.Jump.performed -= OnJumpPerformed;
    InputManager.Instance.inputActions.Player.Hide.performed -= OnHidePerformed;
  }

  public override void Update() {
    if (!player.CheckGrounded()) {
      player.ChangeState(player.playerFallingState);
      return;
    }
    if (InputManager.Instance.inputActions.Player.Hide.IsPressed()) {
      player.ChangeState(player.playerHiddenState);
      return;
    }
  }

  public override void FixedUpdate() {
    player.HandleRotation();
    player.HandleMovement();
    player.HandleClimbing();
  }
  #endregion


  private void OnJumpPerformed(InputAction.CallbackContext context) {
    player.ChangeState(player.playerJumpState);
  }

  private void OnHidePerformed(InputAction.CallbackContext context) {
    player.ChangeState(player.playerHiddenState);
  }

  private void HandleFalling() {
    if (player.rb.velocity.y < 0)
      player.ChangeState(player.playerFallingState);
  }
}
