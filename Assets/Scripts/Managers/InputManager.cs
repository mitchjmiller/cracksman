using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
  public static InputManager Instance;
  public InputActions inputActions { get; private set; }

  private void Awake() {
    if (Instance != null) {
      Destroy(this.gameObject);
      return;
    }
    Instance = this;
    inputActions = new InputActions();
  }
}
