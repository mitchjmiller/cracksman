using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClimbingState : PlayerState {
  public PlayerClimbingState(Player player) : base(player) { }

  private const int SOLID_LAYER = 6;
  private bool leftGround = false;

  #region Lifecycle Events
  public override void EnterState() {
    InputManager.Instance.inputActions.Player.Interact.performed += OnInteractPerformed;
    InputManager.Instance.inputActions.Player.Jump.performed += OnJumpPerformed;
    player.climbing = true;
    player.rb.gravityScale = 0f;
    leftGround = false;

    Vector3 alignedPosition = player.transform.position;
    alignedPosition.x = player.climbable.transform.position.x;
    player.transform.position = alignedPosition;
  }

  public override void ExitState() {
    InputManager.Instance.inputActions.Player.Interact.performed -= OnInteractPerformed;
    InputManager.Instance.inputActions.Player.Jump.performed -= OnJumpPerformed;
    player.climbing = false;
    player.rb.gravityScale = 1f;
  }

  public override void Update() {
    if (player.climbable == null)
      player.ChangeState(player.playerFallingState);

    if (player.CheckGrounded()) {
      if (leftGround)
        player.ChangeState(player.playerGroundedState);
    }
    else
      leftGround = true;
  }

  public override void FixedUpdate() {
    player.HandleRotation();
    HandleClimbing();
  }
  #endregion

  private void HandleClimbing() {
    player.rb.velocity = player.movementVector * Vector2.up * player.climbingVelocity;
  }

  private void OnInteractPerformed(InputAction.CallbackContext context) {
    player.ChangeState(player.playerFallingState);
  }

  private void OnJumpPerformed(InputAction.CallbackContext context) {
    player.ChangeState(player.playerJumpState);
  }
}
