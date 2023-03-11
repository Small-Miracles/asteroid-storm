using System.Collections;
using UnityEngine;

public class RotateRBToMouse : MonoBehaviour {
  public Camera cam;
  public Rigidbody2D rb;

  Vector2 mousePos;

  void Update() {
    mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
  }

  void FixedUpdate() {
    Vector2 lookDir = mousePos - rb.position;
    // you have to do -90f because for Unity the std 0 degree is to the right. and you later addForce to the up vector. if you use rb.AddForce(firePoint.right) you dont have to do -90.But beware you have to rotate the sprite in the prefab also to point to the right.
    float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    rb.rotation = angle;
  }
}