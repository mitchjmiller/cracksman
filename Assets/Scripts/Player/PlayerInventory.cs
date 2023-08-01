using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour {
  public UnityEvent<string> OnInventoryAdded = new UnityEvent<string>();
  public UnityEvent<string> OnInventoryRemoved = new UnityEvent<string>();

  private List<string> items = new List<string>();

  public void AddItem(string itemId) {
    items.Add(itemId);
    OnInventoryAdded.Invoke(itemId);
  }

  public void RemoveItem(string itemId) {
    items.Remove(itemId);
    OnInventoryRemoved.Invoke(itemId);
  }

  public bool HasItem(string id) => items.Exists((string i) => i == id);
}
