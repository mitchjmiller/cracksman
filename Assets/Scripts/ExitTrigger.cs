using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour {

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.TryGetComponent<Player>(out Player player)) {
      if (player.inventory.HasItem("big_gem")) {
        GameManager.Instance.End();
      }
      else {
        DialogueManager.Instance.CreateDialogue(player.transform, "need_the_gem");
      }
    }
  }
}
