using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
  public UnityEvent OnInteractable;
  public UnityEvent OnInteract;


  [SerializeField] private Dialogue dialoguePrefab;
  [SerializeField] private string interactTerm = "Interact";
  [SerializeField] private Vector3 dialogueOffset = new Vector3(0, 1, 0);
  private bool interactable;
  private Dialogue dialogue;

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.TryGetComponent<Player>(out Player player)) {
      ShowDialogue();
      interactable = true;
      OnInteractable.Invoke();
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.TryGetComponent<Player>(out Player player)) {
      interactable = false;
      HideDialogue();
    }
  }

  public string GetInteractText() => "Interact";
  public void SetInteractText(string term) => interactTerm = term;
  public bool IsInteractable() => interactable;

  public void Interact(Player player) {
    OnInteract.Invoke();
  }

  public void Redraw() {
    if (dialogue == null) return;

    HideDialogue();
    ShowDialogue();
  }

  private void ShowDialogue() {
    dialogue = Instantiate(dialoguePrefab, transform.position, Quaternion.identity);
    dialogue.SetFollowTarget(transform);
    dialogue.SetFollowOffset(dialogueOffset);
    dialogue.SetText(interactTerm);
    dialogue.SetTimeout(0);
  }

  private void HideDialogue() {
    if (dialogue)
      Destroy(dialogue.gameObject);
  }
}
