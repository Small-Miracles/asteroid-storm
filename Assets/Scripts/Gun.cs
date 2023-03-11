using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour {

  public Transform follow;
  public Camera cam;
  public Transform firePoint;

  public LaserSegment LazerSegmentPrefab;
  public LaserRenderer lazerRenderer;
  ReflectionWindowLaser laser;

  public Vector3 positionOffset;

  public CameraCtrl cameraCtrl;
  Coroutine fireCR;

  void Awake() {
    CreateReflectionWindowLazer();
  }
  void Update() {
    Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

    transform.position = follow.position + positionOffset;
    transform.localEulerAngles = new Vector3(0, 0, LookAtAngle(mousePos, transform.position));
  }
  public void StartLazer() {
    StopLazer();
    fireCR = StartCoroutine(Fire());
    firePreviewCR = StartCoroutine(FirePreview());
  }
  public void StopLazer() {
    if (fireCR != null) { StopCoroutine(fireCR); }
    if (firePreviewCR != null) { StopCoroutine(firePreviewCR); }
  }

  IEnumerator Fire() {
    while (true) {
      laser.Fire(firePoint);
      yield return new WaitForSeconds(.2f);
    }
  }
  Coroutine firePreviewCR;
  IEnumerator FirePreview() {
    while (true) {
      laser.DrawAim(firePoint.position, firePoint.up);
      yield return new WaitForSeconds(.005f);
    }
  }

  void CreateReflectionWindowLazer() {
    var laser = new GameObject("laser");
    laser.transform.SetParent(transform);
    laser.AddComponent<ReflectionWindowLaser>();
    laser.GetComponent<ReflectionWindowLaser>().lazerSegmentPrefab = LazerSegmentPrefab;
    laser.GetComponent<ReflectionWindowLaser>().laserRenderer = lazerRenderer;
    this.laser = laser.GetComponent<ReflectionWindowLaser>();
    this.laser.cameraCtrl = cameraCtrl;
  }

  public static float LookAtAngle(Vector2 lookAt, Vector2 lookFrom, float offset = 90f) {
    Vector2 lookDir = lookAt - lookFrom;

    float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - offset;

    return angle;
  }
}