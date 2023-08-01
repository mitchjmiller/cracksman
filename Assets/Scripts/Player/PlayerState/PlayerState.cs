using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState {
  protected Player player;

  public PlayerState(Player player) {
    this.player = player;
  }

  public virtual void EnterState() { }
  public virtual void ExitState() { }

  public virtual void Update() { }
  public virtual void FixedUpdate() { }

  public virtual void OnCollisionEnter2D(Collision2D other) { }
  public virtual void OnCollisionExit2D(Collision2D other) { }

  public virtual void OnTriggerEnter2D(Collider2D other) { }
  public virtual void OnTriggerExit2D(Collider2D other) { }
}