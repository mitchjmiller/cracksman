using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Alarm : MonoBehaviour {
  [SerializeField] private float rotationSpeed = 10;
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private Transform[] childTransforms;
  [SerializeField] private Enemy enemyPrefab;

  [SerializeField] private float enemy1SpawnTimer = 2;
  [SerializeField] private Transform enemy1SpawnLocation;
  [SerializeField] private float enemy1MovementSpeed = 1;
  [SerializeField] private Transform[] enemy1PatrolPoints;

  [SerializeField] private float enemy2SpawnTimer = 5;
  [SerializeField] private Transform enemy2SpawnLocation;
  [SerializeField] private float enemy2MovementSpeed = 1;
  [SerializeField] private Transform[] enemy2PatrolPoints;
  private bool isActive = false;
  private bool enemy1Spawned = false;
  private bool enemy2Spawned = false;

  public UnityEvent OnAlarmTriggered;

  private void Update() {
    if (isActive) {
      foreach (Transform transform in childTransforms)
        transform.transform.Rotate(0, 0, rotationSpeed);

      enemy1SpawnTimer = Mathf.Max(0, enemy1SpawnTimer - Time.deltaTime);
      if (enemy1SpawnTimer <= 0 && !enemy1Spawned) {
        enemy1Spawned = true;
        Enemy enemy = Instantiate<Enemy>(enemyPrefab, enemy1SpawnLocation.position, Quaternion.identity);
        enemy.patrolPoints = enemy1PatrolPoints;
        enemy.movementSpeed = enemy1MovementSpeed;
      }

      enemy2SpawnTimer = Mathf.Max(0, enemy2SpawnTimer - Time.deltaTime);
      if (enemy2SpawnTimer <= 0 && !enemy2Spawned) {
        enemy2Spawned = true;
        Enemy enemy = Instantiate<Enemy>(enemyPrefab, enemy2SpawnLocation.position, Quaternion.identity);
        enemy.patrolPoints = enemy2PatrolPoints;
        enemy.movementSpeed = enemy2MovementSpeed;
      }
    }
  }

  public void Trigger() {
    isActive = true;
    foreach (Transform transform in childTransforms) transform.gameObject.SetActive(true);
    audioSource.Play();
    EffectsManager.Instance.ScreenShake(1);
    AudioManager.Instance.SetMusicState(AudioManager.MusicState.Alert);
    OnAlarmTriggered.Invoke();
  }
}
