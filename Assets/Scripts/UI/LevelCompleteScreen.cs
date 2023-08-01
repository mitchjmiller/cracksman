using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteScreen : MonoBehaviour {
  [SerializeField] private Image background;
  [SerializeField][Range(0f, 1f)] private float maxOpacity = 0.75f;
  [SerializeField] private float fadeInTime = 3;

  private Color bgColor;
  private float interpol;

  private void OnEnable() {
    bgColor = background.color;
    bgColor.a = 0f;
    interpol = 0f;
  }

  private void Update() {
    if (interpol <= 1f) {
      interpol += Time.deltaTime / fadeInTime;
      bgColor.a = Mathf.Lerp(0, maxOpacity, interpol);
      background.color = bgColor;
    }
  }
}
