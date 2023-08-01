using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Collectable : MonoBehaviour {
  public UnityEvent OnCollect;

  public virtual void Collect(Player player) {
    OnCollect.Invoke();
  }
}
