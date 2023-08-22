using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CollectablesDrawer : MonoBehaviour {
  [SerializeField] private Image key;
  [SerializeField] private Image bigGem;
  [SerializeField] private Image redGem;
  [SerializeField] private Image greenGem;
  [SerializeField] private Image blueGem;
  [SerializeField] private Image yellowGem;
  private Image[] gemsArray;
  [SerializeField][Range(0, 1)] private float uncollectedAlpha = 0.2f;

  private Animator animator;
  private const string OPEN = "Open";
  private const string DRAWER_OPEN = "DrawerOpen";
  private const string COLLECTED = "Collected";

  private void Awake() {
    animator = GetComponent<Animator>();
    key.color = new Color(1, 1, 1, 0);
    bigGem.color = new Color(1, 1, 1, 0);
    gemsArray = new Image[] { redGem, greenGem, blueGem, yellowGem };
    foreach (Image gem in gemsArray) gem.color = new Color(1, 1, 1, uncollectedAlpha);
  }

  private void Start() {
    Player.Instance.inventory.OnInventoryAdded.AddListener(OnInventoryUpdated);
  }

  private void OnDestroy() {
    Player.Instance.inventory.OnInventoryAdded.RemoveListener(OnInventoryUpdated);
  }

  private void OnInventoryUpdated(string itemId) {
    StartCoroutine(UpdateUI(itemId));
  }

  private IEnumerator UpdateUI(string itemId) {
    Image item = null;
    switch (itemId) {
      case "exit_key": item = key; break;
      case "big_gem": item = bigGem; break;
      case "red_gem": item = redGem; break;
      case "green_gem": item = greenGem; break;
      case "blue_gem": item = blueGem; break;
      case "yellow_gem": item = yellowGem; break;
      default: yield break;
    }

    OpenCollectablesDrawer();
    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(DRAWER_OPEN));
    item.color = Color.white;
    item.GetComponent<Animator>().SetTrigger(COLLECTED);
    yield return new WaitForSeconds(3);
    CloseCollectablesDrawer();
  }

  public void OpenCollectablesDrawer() => animator.SetBool(OPEN, true);
  public void CloseCollectablesDrawer() => animator.SetBool(OPEN, false);
}
