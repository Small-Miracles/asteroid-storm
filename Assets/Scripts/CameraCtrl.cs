using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {
  public float fixedZ;
  public Transform follow;

  Vector2 currShakeVec = Vector2.zero;
  Vector2 shakeAmountNormal = new Vector2(.5f, 0f);
  Vector2 shakeAmountLittle = new Vector2(.1f, 0f);
  Vector2 shakeAmount;

  public Game game;

  void Update() {
    if (game.IsGameOver()) return;

    Lerp();

    transform.position = new Vector3(
      follow.position.x + currShakeVec.x, //only shake on x axis
      follow.position.y, 
      fixedZ
    );
  }

  enum ShakeDirection {
    CenterToRight,
    RightToCenter,
    CenterToLeft,
    LeftToCenter
  }

  public void Shake(bool enemyHit) {
    if (shaking) { return; }

    shakeAmount = enemyHit ? shakeAmountNormal : shakeAmountLittle;
    Shake(ShakeDirection.CenterToRight);
  }
  void Shake(ShakeDirection direction) {
    // Vector.zero lerp -> Vector(shakeAmount, 0f) lerp -> Vector(-shakeAmount, 0f) lerp -> Vector.zero
    // zero -> shakeAmount
    // shakeAmount -> zero
    // zero -> -shakeAmount
    // -shakeAmount -> zero
    shaking = true;
    timeElapsed = 0f;
    shakeDirection = direction;
  }

  bool shaking = false;
  ShakeDirection shakeDirection;
  float timeElapsed;
  float shakeDuration = .05f;

  void Lerp() {
    if (timeElapsed < shakeDuration) {
      var p = timeElapsed / shakeDuration;

      switch (shakeDirection) {
        case ShakeDirection.CenterToRight:
          currShakeVec = Vector2.Lerp(Vector2.zero, shakeAmount, p);
          break;
        case ShakeDirection.RightToCenter:
          currShakeVec = Vector2.Lerp(shakeAmount, Vector2.zero, p);
          break;
        case ShakeDirection.CenterToLeft:
          currShakeVec = Vector2.Lerp(Vector2.zero, -shakeAmount, p);
          break;
        case ShakeDirection.LeftToCenter:
          currShakeVec = Vector2.Lerp(-shakeAmount, Vector2.zero, p);
          break;
      }
      //Vector2.Lerp(from, to, timeElapsed / lerpDuration)

      timeElapsed += Time.deltaTime;
    } else {
      // set side to next, reset time
      // or stop if done
      switch (shakeDirection) {
        case ShakeDirection.CenterToRight:
          timeElapsed = 0f;
          shakeDirection = ShakeDirection.RightToCenter;
          break;
        case ShakeDirection.RightToCenter:
          timeElapsed = 0f;
          shakeDirection = ShakeDirection.CenterToLeft;
          break;
        case ShakeDirection.CenterToLeft:
          timeElapsed = 0f;
          shakeDirection = ShakeDirection.LeftToCenter;
          break;
        case ShakeDirection.LeftToCenter:
          shaking = false;
          break;
      }


    }
  }

}
