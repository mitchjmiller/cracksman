using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class ToggleButton : MonoBehaviour {
  public UnityEvent OnEnable;
  public UnityEvent OnDisable;

  [SerializeField] private bool toggleEnabled = false;
  [SerializeField] private SpriteRenderer button;
  [SerializeField] private Light2D light2D;
  [SerializeField] private string enabledText = "Turn Off";
  [SerializeField] private string disabledText = "Turn On";
  private Interactable interactable;

  private void Awake() {
    interactable = GetComponent<Interactable>();
  }

  private void Start() {
    if (toggleEnabled)
      Enable();
    else
      Disable();
  }

  public void Toggle() {
    toggleEnabled = !toggleEnabled;

    if (toggleEnabled)
      Enable();
    else
      Disable();
  }

  private void Enable() {
    button.color = Color.green;
    light2D.color = Color.green;
    interactable.SetInteractText(enabledText);
    interactable.Redraw();
    OnEnable.Invoke();
  }

  private void Disable() {
    button.color = Color.red;
    light2D.color = Color.red;
    interactable.SetInteractText(disabledText);
    interactable.Redraw();
    OnDisable.Invoke();
  }
}
