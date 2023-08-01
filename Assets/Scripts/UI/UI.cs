using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
  [SerializeField] private GameObject pausedScreen;
  [SerializeField] private GameObject gameOverScreen;
  [SerializeField] private GameObject levelCompleteScreen;

  private GameObject activeScreen;

  private void Start() {
    GameManager.Instance.OnStateChange.AddListener(OnGameStateChange);
  }

  private void OnGameStateChange() {
    switch (GameManager.Instance.GetState()) {
      case GameManager.State.GameOver:
        gameOverScreen.SetActive(true); break;
      case GameManager.State.LevelComplete:
        levelCompleteScreen.SetActive(true); break;
      case GameManager.State.Paused:
        pausedScreen.SetActive(true); break;
    }
  }
}
