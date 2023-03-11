using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class MyCoroutines {
  // for scaleBack this expects that localScale.x == localScale.y
  public static IEnumerator ScaleTo(
                              float scaleTo,
                              float animationTime,
                              Transform transform,
                              bool scaleBack,
                              MonoBehaviour t,
                              Action onEnd = null,
                              bool debug = false) {
    var originalScale = transform.localScale.x;
    float startTime = Time.time;
    var startVec = transform.localScale;
    var finalVec = new Vector2(scaleTo, scaleTo);
    bool done = false;
    while (!done) {
      float timeCovered = Time.time - startTime;
      float fracTime = timeCovered / animationTime;
      Vector2 result = Vector2.Lerp(startVec, finalVec, fracTime);
      transform.localScale = new Vector3(result.x, result.y, 1);
      if (debug) Debug.Log("cr " + result + " - " + fracTime);
      done = fracTime >= 1f;

      if (done && onEnd != null) { onEnd(); }

      yield return null;
    }
    if (debug) Debug.Log("coroutine is done");

    if (scaleBack) {
      t.StartCoroutine(ScaleTo(originalScale, animationTime, transform, false, t));
    }
  }

  public static IEnumerator FlashToColor(
    SpriteRenderer sr,
    float animationTime,
    Color toColor,
    MonoBehaviour t,
    bool changeBack
    ) {
    var origColor = sr.color;
    float startTime = Time.time;
    bool done = false;
    while (!done) {
      float timeCovered = Time.time - startTime;
      float fracTime = timeCovered / animationTime;
      Color c = Color.Lerp(origColor, toColor, fracTime);
      sr.color = c;
      done = fracTime >= 1f;

      yield return null;
    }

    if (changeBack) {
      t.StartCoroutine(FlashToColor(sr, animationTime, origColor, t, false));
    }
    
  }
}
