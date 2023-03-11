using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAway : MonoBehaviour {

  public float timeToLive = .05f;
  //public float pushForce 

  void Awake() {
    StartCoroutine(Remove());
  }

  void OnTriggerEnter2D(Collider2D collision) {
    //Debug.Log("pushing away " + collision.gameObject);
    if (collision.CompareTag("Enemy")) {
      var direction = collision.gameObject.transform.position - transform.position;
      direction = direction.normalized;
      collision.gameObject.GetComponent<Enemy>().Push(direction);
    }
  }

  IEnumerator Remove() {
    float endTime = Time.time + timeToLive;
    while (Time.time < endTime) { yield return null; }
    Destroy(gameObject);
  }

}
