using System;
using System.Collections;
using UnityEngine;

public class LaserSegment : MonoBehaviour {

  LineRenderer lr;

  Vector2 from;
  Vector2 to;
  bool started = false;
  bool finished = false;
  LaserSegment previous;
  bool animated = false;

  public bool staticLine = false;

  private void Awake() {
    lr = GetComponent<LineRenderer>();
  }

  public void Draw(
    Vector2 from, 
    Vector2 to, 
    Color clr, 
    float lazerWidth, 
    LaserSegment previous,
    bool animate) {

    this.animated = animate;
    this.from = from;
    this.to = to;
    this.previous = previous;

    lr.positionCount = 2;
    lr.SetPosition(0, from);
    lr.SetPosition(1, animate ? from : to);
    lr.startColor = clr;
    lr.endColor = clr;
    lr.startWidth = lazerWidth;
    lr.endWidth = lazerWidth;

    timeElapsed = 0f;
    started = true; // previous == null;
    lerpDuration = animate ? lerpDuration_normal : lerpDuration_short;
  }
  public void Remove() {
    lr.positionCount = 0;
  }

  void Update() {
    if (staticLine) {
      return;
    }
    if (started) {
      Lerp();
    } else {
      started = previous.finished;
    }
    
  }

  float timeElapsed;
  float lerpDuration_normal = .15f;
  float lerpDuration_short = .01f;
  float lerpDuration;
  void Lerp() {
    if (timeElapsed < lerpDuration) {
      if (animated) {
        lr.SetPosition(1, Vector2.Lerp(from, to, timeElapsed / lerpDuration));
      }
      
      timeElapsed += Time.deltaTime;
    } else {
      lr.SetPosition(1, to);
      finished = true;
      Destroy(gameObject);
    }
  }

}