using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
  [SerializeField] private Transform target;
  [SerializeField] private string dialogueId = "";
  [SerializeField] private bool triggerOnce = true;

  public void Trigger() {
    DialogueManager.Instance.CreateDialogue(target.transform, dialogueId);
    if (triggerOnce)
      Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.TryGetComponent<Player>(out Player player)) {
      Trigger();
    }
  }
}
