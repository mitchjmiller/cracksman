using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;

  public enum State {
    Playing,
    Paused,
    GameOver,
    LevelComplete
  }
  private State _currentState;
  private State currentState {
    get { return _currentState; }
    set {
      _currentState = value;
      Debug.Log($"<color=\"lightblue\">Game State -> {_currentState}</color>");
      OnStateChange.Invoke();
      if (value == State.GameOver)
        OnGameOver.Invoke();
    }
  }

  public UnityEvent OnStateChange;
  public UnityEvent OnGameOver;

  private void Awake() {
    if (Instance != null) {
      Destroy(this.gameObject);
      return;
    }

    Instance = this;
    currentState = State.Playing;
  }

  private void Start() {
    Enemy.OnDetected.AddListener(OnEnemyDetected);
  }

  private void OnEnemyDetected() {
    currentState = State.GameOver;
  }

  private void TogglePause() {
    if (currentState == State.Playing) {
      currentState = State.Paused;
      Time.timeScale = 0;
    }
    else if (currentState == State.Paused) {
      currentState = State.Playing;
      Time.timeScale = 1;
    }
  }

  public State GetState() => currentState;
  public void LevelComplete() => currentState = State.LevelComplete;
  public void GameOver() => currentState = State.GameOver;
  public void ReloadScene() {
    Debug.Log("Reloading Scene...");
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}
