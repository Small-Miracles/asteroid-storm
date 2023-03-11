using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

  public float moveSpeed = 5f;

  public Rigidbody2D rb;
  Vector2 movement;

  void Update() {
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");
    movement = movement.normalized;
  }
  void FixedUpdate() {
    //if (Game.gameOver) return; <- use Time.timeScale = 0 on game over
    rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    //rb.MoveRotation(3f * Time.deltaTime);
    //rb.rotation = 3f * Time.deltaTime;
    //rb.SetRotation
    rb.rotation += 1.0f;
  }
}
