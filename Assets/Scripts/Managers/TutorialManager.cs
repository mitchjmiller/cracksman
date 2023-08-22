using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
  public void Start() {
    AudioManager.Instance.ResetMasterVolume();
    AudioManager.Instance.SetSnapshot(AudioManager.Snapshot.Outside);
    InputManager.Instance.inputActions.Player.Enable();
  }

  public void LoadNextScene() {
    SceneManager.LoadScene(1);
  }
}
