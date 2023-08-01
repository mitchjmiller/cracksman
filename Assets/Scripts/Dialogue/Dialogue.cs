using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour {
  [SerializeField] private Transform target;
  [SerializeField] private Vector2 offset = new Vector2();
  [SerializeField][TextArea] private string text;
  [SerializeField] private float timeout = 3f;

  private SpriteRenderer background;
  private TMP_Text tmpText;
  private string displayText = "";
  private float margin = 0.3f;

  #region LIfecycle Events
  private void Awake() {
    background = GetComponentInChildren<SpriteRenderer>();
    tmpText = GetComponentInChildren<TMP_Text>();
  }

  private void Start() {
    tmpText.SetText(displayText);
    StartCoroutine(TypeDisplayText(text));
  }

  private void Update() {
    UpdateDisplay();
  }

  private void OnDestroy() {
    DialogueManager.Instance.RemoveActiveDialogue(target, this);
  }
  #endregion

  public void SetFollowTarget(Transform target) => this.target = target;
  public void SetFollowOffset(Vector2 offset) => this.offset = offset;
  public void SetText(string text) => this.text = text;
  public void SetTimeout(int timeout) => this.timeout = timeout;

  private IEnumerator TypeDisplayText(string text) {
    for (int i = 0; i < text.Length; i++) {
      displayText += text[i];
      yield return new WaitForSeconds(1f / 100);
    }
    if (timeout > 0) Destroy(gameObject, timeout);
  }

  private void UpdateDisplay() {
    tmpText.SetText(displayText);
    tmpText.ForceMeshUpdate();
    background.size = new Vector2(tmpText.renderedWidth + margin, tmpText.renderedHeight + margin);
    background.transform.position = new Vector2(
      tmpText.transform.position.x + (tmpText.renderedWidth / 2),
      tmpText.transform.position.y - (tmpText.renderedHeight / 2)
    );

    transform.position = Camera.main.WorldToViewportPoint(target.position).x < 0.8f
      ? (Vector2)target.position + offset
      : (Vector2)target.position + (background.size * new Vector2(-1, 0)) + (offset * new Vector2(-1, 1));
  }
}
