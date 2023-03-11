using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRenderer : MonoBehaviour {

  public LaserSegment laserSegmentPrefab;

  float previewWidth = 0.01f;
  float lazerWidth = .2f;
  static Color lazerClr = new Color(110f / 255f, 81f / 255f, 129f / 255f, 1f);
  static Color previewClr = new Color(1f, 0f, 0f, .15f);

  void Awake() {
  }

  public void DrawLazerSegments(List<Vector2> hitPoints) {

    // hitPoints.Count-1 because lazer doesnt shoot out of last reflection currently
    LaserSegment previous = null;
    for (int i = 0; i < hitPoints.Count - 1; i++) {
      var segment = Instantiate(laserSegmentPrefab, Vector3.zero, Quaternion.identity);
      segment.GetComponent<LaserSegment>().Draw(
        hitPoints[i],
        hitPoints[i + 1],
        lazerClr,
        lazerWidth, 
        previous,
        true
      );
      previous = segment.GetComponent<LaserSegment>();
    }
  }
}
// git push -u origin_gh master