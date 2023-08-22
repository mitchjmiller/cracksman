using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
  [SerializeField] private GameObject pausedScreen;
  [SerializeField] private GameObject gameOverScreen;
  [SerializeField] private GameObject toBeContinuedScreen;
  [SerializeField] private CollectablesDrawer collectablesDrawer;

  private GameObject activeScreen;

  private void Start() {
    GameManager.Instance.OnStateChange.AddListener(OnGameStateChange);
  }

  private void OnGameStateChange() {
    collectablesDrawer.CloseCollectablesDrawer();
    pausedScreen.SetActive(false);

    switch (GameManager.Instance.GetState()) {
      case GameManager.State.GameOver:
        gameOverScreen.SetActive(true); break;
      case GameManager.State.Paused:
        pausedScreen.SetActive(true);
        collectablesDrawer.OpenCollectablesDrawer();
        break;
      case GameManager.State.End:
        toBeContinuedScreen.SetActive(true);
        break;
    }
  }
}
