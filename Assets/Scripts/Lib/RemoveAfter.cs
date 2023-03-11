using System.Collections;
using UnityEngine;

public class RemoveAfter : MonoBehaviour {

  public float ttl = 1f;

  void Start() {
    Invoke("Die", ttl);
  }

  public void Go() {
    //Invoke("Die", ttl);
  }
  void Die() {
    Destroy(gameObject);
  }

}