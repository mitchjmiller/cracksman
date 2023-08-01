using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpState : PlayerState {
  public PlayerJumpState(Player player) : base(player) { }

  private float movementForceMultiplier = 0.5f;

  #region Lifecycle Events
  public override void EnterState() {
    player.inputActions.Player.Jump.canceled += OnJumpCanceled;
    player.movementForce *= movementForceMultiplier;
    player.rb.AddForce(Vector2.up * player.jumpForce * player.rb.mass, ForceMode2D.Impulse);
    player.Footstep();
    player.grounded = false;
  }

  public override void ExitState() {
    player.inputActions.Player.Jump.canceled -= OnJumpCanceled;
    player.movementForce /= movementForceMultiplier;
  }

  public override void FixedUpdate() {
    player.HandleRotation();
    player.HandleMovement();
    player.HandleClimbing();
    HandleFalling();
  }
  #endregion

  private void OnJumpCanceled(InputAction.CallbackContext context) {
    player.rb.gravityScale = player.fallingGravityScale;
  }

  private void HandleFalling() {
    if (player.rb.velocity.y <= 0)
      player.ChangeState(player.playerFallingState);
  }
}
