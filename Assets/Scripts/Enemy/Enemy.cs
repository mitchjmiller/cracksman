using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {
  public static UnityEvent OnDetected = new UnityEvent();
  public UnityEvent OnStateChange = new UnityEvent();
  public UnityEvent OnFootstep = new UnityEvent();

  [SerializeField] public Transform viewPoint;
  [SerializeField] public float movementSpeed = 2f;
  [SerializeField] public string[] curiousDialogueIds = { };
  [SerializeField] public string[] alertDialogueIds = { };
  [Space]
  [SerializeField] public float nearSightDistance = 5f;
  [SerializeField] public float farSightDistance = 10f;
  [Space]
  [SerializeField] public LayerMask sightMask;
  [Header("Patrol")]
  [SerializeField] public Transform[] patrolPoints;
  [SerializeField] public int patrolPointIndex = 0;

  public float visibilityMultiplier = 1;

  public Vector2[] sightLineArray = {
    new Vector2(0.8f, 0.2f),
    new Vector2(0.9f, 0.1f),
    new Vector2(1f, 0f),
    new Vector2(0.9f, -0.1f),
    new Vector2(0.8f, -0.2f)
  };

  public Rigidbody2D rb { get; private set; }

  public EnemyPatrollingState enemyPatrollingState;
  public EnemyCuriousState enemyCuriousState;
  public EnemyAlertState enemyAlertState;

  private EnemyState _currentState;
  private EnemyState currentState {
    get {
      return _currentState;
    }
    set {
      _currentState = value;
      Debug.Log($"<color=orange>{name} -> {value.GetType().Name}</color>");
      OnStateChange.Invoke();
    }
  }

  private void OnDrawGizmos() => currentState?.OnDrawGizmos();

  #region Lifecycle Events
  private void Awake() {
    rb = GetComponent<Rigidbody2D>();

    enemyPatrollingState = new EnemyPatrollingState(this);
    enemyCuriousState = new EnemyCuriousState(this);
    enemyAlertState = new EnemyAlertState(this);

    for (int i = 0; i < sightLineArray.Length; i++) sightLineArray[i].Normalize();
  }

  private void Start() {
    StartCoroutine(DelayedStart());
  }

  private IEnumerator DelayedStart() {
    yield return new WaitForSeconds(Random.Range(0, 2));
    ChangeState(enemyPatrollingState);
  }

  private void Update() => currentState?.Update();
  private void FixedUpdate() => currentState?.FixedUpdate();
  private void OnCollisionEnter2D(Collision2D other) => currentState?.OnCollisionEnter2D(other);
  private void OnCollisionExit2D(Collision2D other) => currentState?.OnCollisionExit2D(other);
  private void OnTriggerEnter2D(Collider2D other) => currentState?.OnTriggerEnter2D(other);
  private void OnTriggerExit2D(Collider2D other) => currentState?.OnTriggerExit2D(other);
  #endregion


  public void ChangeState(EnemyState newState) {
    this.currentState?.ExitState();
    this.currentState = newState;
    this.currentState.EnterState();
  }

  public void UpdateVisibility() {
    visibilityMultiplier = Player.Instance.GetVisibility();
  }

  public void MoveTo(Transform point) {
    LookAt(point);
    Vector2 direction = transform.position.x < point.position.x ? Vector2.right : Vector2.left;
    rb.velocity = direction * movementSpeed;
  }

  public void LookAt(Transform point) {
    if (transform.position.x < point.position.x)
      transform.rotation = Quaternion.Euler(0, 0, 0);
    else if (transform.position.x > point.position.x)
      transform.rotation = Quaternion.Euler(0, 180, 0);
  }

  public string GetState() => currentState.GetType().Name;
  public void Footstep() => OnFootstep.Invoke();
}
