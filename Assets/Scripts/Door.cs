using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour {
  [SerializeField] private GameObject doorOpen;
  [SerializeField] private GameObject doorClosed;
  [SerializeField] private AudioClip[] doorOpenSounds;
  [SerializeField] private AudioClip[] doorCloseSounds;
  [SerializeField] private string keyId;

  private AudioSource audioSource;

  public UnityEvent OnOpen;
  public UnityEvent OnOpenFailed;
  public UnityEvent OnClose;

  private enum State {
    Open,
    Closed
  }
  private State currentState;

  private void Awake() {
    audioSource = GetComponent<AudioSource>();
    currentState = State.Closed;
    doorClosed.SetActive(true);
    doorOpen.SetActive(false);
  }

  public void Interact(Player player) {
    switch (currentState) {
      case State.Closed:
        TryOpenDoor(player);
        break;
      case State.Open:
        CloseDoor();
        break;
    }
  }

  private void TryOpenDoor(Player player) {
    if (player.inventory.HasItem(keyId)) {
      OpenDoor();
    }
    else {
      Debug.Log("Door Open Failed");
      OnOpenFailed.Invoke();
    }
  }

  private void OpenDoor() {
    Debug.Log("Door Opened");
    audioSource.PlayOneShot(doorOpenSounds[Random.Range(0, doorOpenSounds.Length)]);
    doorClosed.SetActive(false);
    doorOpen.SetActive(true);
    currentState = State.Open;
    OnOpen.Invoke();
  }

  private void CloseDoor() {
    Debug.Log("Door Closed");
    audioSource.PlayOneShot(doorCloseSounds[Random.Range(0, doorCloseSounds.Length)]);
    doorClosed.SetActive(true);
    doorOpen.SetActive(false);
    currentState = State.Closed;
    OnClose.Invoke();
  }
}
