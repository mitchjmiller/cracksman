using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerVisual : MonoBehaviour {
  private const string X_VELOCITY = "XVelocity";
  private const string Y_VELOCITY = "YVelocity";
  private const string STATE = "State";
  private enum State {
    Grounded,
    Jumping,
    Falling,
    Hiding,
    Climbing
  }

  [SerializeField] private Player player;
  [SerializeField] private Animator animator;
  [SerializeField] private ParticleSystem footDust;
  [SerializeField] private Color hiddenColour = new Color(0.25f, 0.25f, 0.25f, 0.5f);

  SpriteRenderer[] spriteArray;

  private Color defaultColor;

  #region Lifecycle Events
  private void Awake() {
    spriteArray = GetComponentsInChildren<SpriteRenderer>();
  }

  private void Start() {
    player.OnStateChange.AddListener(OnPlayerStateChange);
  }

  private void OnDestroy() {
    player.OnStateChange.RemoveListener(OnPlayerStateChange);
  }

  private void Update() {
    animator.SetFloat(X_VELOCITY, Mathf.Abs(player.rb.velocity.x));
    animator.SetFloat(Y_VELOCITY, Mathf.Abs(player.rb.velocity.y));
  }
  #endregion

  private void OnPlayerStateChange() {
    DisplayHidden(false);

    switch (player.GetState()) {
      case "PlayerGroundedState":
        animator.SetInteger(STATE, (int)State.Grounded); break;
      case "PlayerJumpState":
        animator.SetInteger(STATE, (int)State.Jumping); break;
      case "PlayerFallingState":
        animator.SetInteger(STATE, (int)State.Falling); break;
      case "PlayerHiddenState":
        animator.SetInteger(STATE, (int)State.Hiding);
        DisplayHidden(true);
        break;
      case "PlayerClimbingState":
        animator.SetInteger(STATE, (int)State.Climbing); break;
    }
  }

  private void DisplayHidden(bool isHidden) {
    foreach (SpriteRenderer sprite in spriteArray) {
      sprite.color = isHidden ? hiddenColour : Color.white;
    }
  }

  public void FootStep() {
    footDust.Play();
  }
}
