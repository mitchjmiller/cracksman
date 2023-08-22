using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
  public static Player Instance;
  public UnityEvent OnStateChange = new UnityEvent();
  public UnityEvent OnFootstep = new UnityEvent();

  [Header("Parameters")]
  [SerializeField] public float movementForce = 15f;
  [SerializeField] public float jumpForce = 10f;
  [SerializeField] public float fallingGravityScale = 1.8f;
  [SerializeField] public float climbingVelocity = 2f;
  [SerializeField] public Vector2 groundedCastSize = new Vector2(0.6f, 0.2f);
  [SerializeField] public LayerMask groundedLayerMask;

  [Header("Info")]
  [SerializeField] public Vector2 movementVector;
  [SerializeField] public bool grounded;
  [SerializeField] public bool climbing;
  [SerializeField] public bool hidden = false;


  public Rigidbody2D rb { get; private set; }
  public Collider2D coll { get; private set; }
  public PlayerInventory inventory { get; private set; }
  public Climbable climbable { get; private set; }
  public Interactable interactable { get; private set; }

  private PlayerState _currentState;
  private PlayerState currentState {
    get { return _currentState; }
    set {
      _currentState = value;
      Debug.Log($"<color=green>Player State -> {value.GetType().Name}</color>");
      OnStateChange.Invoke();
    }
  }

  public PlayerGroundedState playerGroundedState;
  public PlayerJumpState playerJumpState;
  public PlayerFallingState playerFallingState;
  public PlayerHiddenState playerHiddenState;
  public PlayerClimbingState playerClimbingState;

  public int lights = 0;

  void OnDrawGizmos() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position, groundedCastSize);
  }

  #region Lifecycle Events
  private void Awake() {
    if (Instance != null) {
      Destroy(this.gameObject);
      return;
    }

    Instance = this;
    rb = GetComponent<Rigidbody2D>();
    coll = GetComponent<Collider2D>();
    inventory = GetComponent<PlayerInventory>();

    playerGroundedState = new PlayerGroundedState(this);
    playerJumpState = new PlayerJumpState(this);
    playerFallingState = new PlayerFallingState(this);
    playerHiddenState = new PlayerHiddenState(this);
    playerClimbingState = new PlayerClimbingState(this);
  }

  private void Start() {
    InputManager.Instance.inputActions.Player.Interact.performed += OnInteractPerformed;
    ChangeState(playerGroundedState);
  }

  private void OnDestroy() {
    ChangeState(null);
    InputManager.Instance.inputActions.Player.Interact.performed -= OnInteractPerformed;
  }

  private void Update() {
    currentState.Update();
    HandleDetectability();
  }

  private void FixedUpdate() {
    movementVector = InputManager.Instance.inputActions.Player.Move.ReadValue<Vector2>();
    currentState.FixedUpdate();
  }
  #endregion

  public string GetState() => currentState.GetType().Name;

  public void ChangeState(PlayerState newState) {
    this.currentState?.ExitState();
    if (newState == null) return;

    this.currentState = newState;
    this.currentState.EnterState();
  }

  public bool CheckGrounded() {
    RaycastHit2D hit = Physics2D.BoxCast(transform.position, groundedCastSize, 0f, Vector2.down, 0f, groundedLayerMask);
    grounded = hit.collider != null ? true : false;
    return grounded;
  }

  public void HandleMovement() {
    Vector2 dir = new Vector2(movementVector.x, 0f);
    rb.AddForce(dir * movementForce * rb.mass, ForceMode2D.Force);
  }

  public void HandleClimbing() {
    if (climbable != null && movementVector.y > 0.5f)
      ChangeState(playerClimbingState);
  }

  public void HandleRotation() {
    if (movementVector.x > 0)
      transform.rotation = Quaternion.Euler(0, 0, 0);
    else if (movementVector.x < 0)
      transform.rotation = Quaternion.Euler(0, 180, 0);
  }

  private void HandleDetectability() {
    if (GetVisibility() > 0) {
      rb.bodyType = RigidbodyType2D.Dynamic;
      coll.enabled = true;
    }
    else {
      rb.bodyType = RigidbodyType2D.Static;
      coll.enabled = false;
    }
  }

  private void OnInteractPerformed(InputAction.CallbackContext context) {
    interactable?.Interact(this);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.TryGetComponent(typeof(Collectable), out Component collectableComponent)) {
      Collectable collectable = collectableComponent as Collectable;
      collectable.Collect(this);
    }
    if (other.TryGetComponent<Interactable>(out Interactable interactable))
      this.interactable = interactable;
    if (other.TryGetComponent<Climbable>(out Climbable climbable))
      this.climbable = climbable;

    currentState.OnTriggerEnter2D(other);
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.TryGetComponent<Interactable>(out Interactable interactable))
      this.interactable = null;
    if (other.TryGetComponent<Climbable>(out Climbable climbable))
      this.climbable = null;

    currentState.OnTriggerExit2D(other);
  }

  // Handle Lighting
  public void AddLight() => lights++;
  public void RemoveLight() => lights--;

  public float GetVisibility() => Mathf.Max(0, 1 - (hidden ? 0.3f : 0) - (lights <= 0 ? 0.7f : 0));

  public void Footstep() => OnFootstep.Invoke();
}
