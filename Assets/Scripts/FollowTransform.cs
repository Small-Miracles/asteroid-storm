using System.Collections;
using UnityEngine;

public class FollowTransform : MonoBehaviour {

  public Transform follow;
  public Game game;

  void Update() {
    if (game.IsGameOver()) return;
    transform.position = follow.position;  
  }
}