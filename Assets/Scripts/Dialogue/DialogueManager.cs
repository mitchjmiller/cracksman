using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
  public static DialogueManager Instance;

  [SerializeField] private Dialogue dialoguePrefab;

  private Dictionary<string, string> dictionary = new Dictionary<string, string>();
  private List<KeyValuePair<Transform, Dialogue>> activeDialogues = new List<KeyValuePair<Transform, Dialogue>>();

  private void Awake() {
    if (Instance != null) {
      Destroy(gameObject);
      return;
    }
    Instance = this;

    dictionary.Add("huh", "Huh!?");
    dictionary.Add("someone_there", "Is there someone there?");
    dictionary.Add("saw_something", "Thought I saw something...");
    dictionary.Add("hey", "Hey!");
    dictionary.Add("oi", "Oi!");
    dictionary.Add("damn_it", "Damn it!");
    /**************************************************************/
    dictionary.Add("weather_miserable", "Weather's miserable...");
    dictionary.Add("up_ahead", "The warehouse should be up ahead...");
    dictionary.Add("there_it_is", "There it is!\nNow I just have to find a way in...");
    dictionary.Add("no_trespassing", "Hey! You there!\nThis is private property, no trespassing!");
    /**************************************************************/
    dictionary.Add("enter", "Right... I'm in!\nTime to look around");
    dictionary.Add("see_key", "Looks like I've found a key");
    dictionary.Add("see_exit", "Hmm...     \nThat door looks promising");
    dictionary.Add("door_locked", "Damn, it's locked\nNeed to find the key");
    dictionary.Add("door_unlocked", "And we're in!");
    dictionary.Add("key_collected", "Got it!\nNow to get to the door");

  }

  public void CreateDialogue(Transform target, string dialogueId) {
    if (TryGetActiveDialogue(target, out Dialogue dialogue)) {
      RemoveActiveDialogue(target, dialogue);
      Destroy(dialogue.gameObject);
    }

    if (dictionary.TryGetValue(dialogueId, out string text)) {
      dialogue = Instantiate(dialoguePrefab);
      dialogue.SetFollowTarget(target);
      dialogue.SetText(text);
      activeDialogues.Add(new KeyValuePair<Transform, Dialogue>(target, dialogue));
    }
    else {
      Debug.LogWarning($"No dialogue entry with id: {dialogueId}");
    }
  }

  private bool TryGetActiveDialogue(Transform target, out Dialogue dialogue) {
    KeyValuePair<Transform, Dialogue> kvp = activeDialogues.Find((kvp) => kvp.Key == target);
    dialogue = kvp.Value;
    return dialogue != null;
  }

  public void RemoveActiveDialogue(Transform target, Dialogue dialogue) {
    KeyValuePair<Transform, Dialogue> kvp = activeDialogues.Find((kvp) => kvp.Key == target);
    activeDialogues.Remove(kvp);
  }
}
