using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;

  public enum State {
    Playing,
    Paused,
    GameOver,
    End
  }
  private State _currentState;
  private State currentState {
    get { return _currentState; }
    set {
      _currentState = value;
      Debug.Log($"<color=\"lightblue\">Game State -> {_currentState}</color>");
      OnStateChange.Invoke();
      switch (value) {
        case State.Playing:
          AudioManager.Instance.ResetMasterVolume();
          InputManager.Instance.inputActions.UI.Disable();
          InputManager.Instance.inputActions.Player.Enable();
          break;
        case State.Paused:
          AudioManager.Instance.MuteMasterVolume();
          InputManager.Instance.inputActions.Player.Disable();
          InputManager.Instance.inputActions.UI.Enable();
          break;
        case State.GameOver:
          AudioManager.Instance.FadeOutMasterVolume(10);
          InputManager.Instance.inputActions.Player.Disable();
          InputManager.Instance.inputActions.UI.Enable();
          OnGameOver.Invoke();
          break;
        case State.End:
          AudioManager.Instance.FadeOutMasterVolume(10);
          InputManager.Instance.inputActions.Player.Disable();
          InputManager.Instance.inputActions.UI.Enable();
          break;
      }
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
  }

  private void Start() {
    currentState = State.Playing;
    AudioManager.Instance.SetSnapshot(AudioManager.Snapshot.Inside);
    AudioManager.Instance.SetMusicState(AudioManager.MusicState.Regular);
    InputManager.Instance.inputActions.Player.Pause.performed += OnPause;
    InputManager.Instance.inputActions.UI.Unpause.performed += OnPause;
    Enemy.OnDetected.AddListener(OnEnemyDetected);
  }

  private void OnEnemyDetected() {
    currentState = State.GameOver;
  }

  private void OnPause(InputAction.CallbackContext context) {
    TogglePause();
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
  public void GameOver() => currentState = State.GameOver;
  public void End() => currentState = State.End;
  public void LoadScene(int index) => SceneManager.LoadScene(index);
  public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

}
