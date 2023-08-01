using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerState {
  public PlayerFallingState(Player player) : base(player) { }

  private float movementForceMultiplier = 0.5f;

  #region Lifecycle Events
  public override void EnterState() {
    player.rb.gravityScale = player.fallingGravityScale;
    player.movementForce *= movementForceMultiplier;
  }

  public override void ExitState() {
    player.movementForce /= movementForceMultiplier;
  }

  public override void Update() {
    if (player.CheckGrounded())
      player.ChangeState(player.playerGroundedState);
  }

  public override void FixedUpdate() {
    player.HandleRotation();
    player.HandleMovement();
    player.HandleClimbing();
  }
  #endregion
}
