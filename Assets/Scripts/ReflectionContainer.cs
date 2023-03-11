using System.Collections;
using UnityEngine;

public class ReflectionContainer : MonoBehaviour {

  public Game game;
  public Transform follow;
  public float spinSpeed = 5f;

  void Update() {
    if (game.IsGameOver()) return; 

    transform.position = follow.position;
    transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
  }
}