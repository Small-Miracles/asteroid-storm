using System.Collections;
using UnityEngine;

public class StopCollide : MonoBehaviour {

  Rigidbody2D rb;
  Vector2 pos;
  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    pos = rb.position;
  }
  private void Update() { 
    //rb.MovePosition(pos); // ?????????????????????????
  }
}