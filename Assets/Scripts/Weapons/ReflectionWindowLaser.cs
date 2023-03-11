using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionWindowLaser : MonoBehaviour {

  public LaserSegment lazerSegmentPrefab;
  public LaserRenderer laserRenderer;
  public CameraCtrl cameraCtrl;

  public float shootCooldown = .33f;
  float currentShootCooldown = 0f;

  void Update() {
     currentShootCooldown = Mathf.Max(0f, currentShootCooldown - Time.deltaTime);
  }

  void Awake() {}

  public void FirePreview(Transform firePoint) {
    ShootLazer(firePoint.up, firePoint.position);
  }
  public void Fire(Transform firePoint) {
    if (currentShootCooldown > 0f) return;
    currentShootCooldown = shootCooldown;

    ShootLazer(firePoint.up, firePoint.position);
  }

  void ShootLazer(Vector3 shootDir, Vector3 origin) {
    ShootLaser(shootDir, origin, new List<GameObject>(), new List<GameObject>(), new List<Vector2>());
  }

  ReflectionWindow currentActiveTarget;

  void ShootLaser(
    Vector3 shootDirection, 
    Vector3 shootOrigin, 
    List<GameObject> hitReflectionWindows, 
    List<GameObject> hitEnemies,
    List<Vector2> hitPoints) {

    // shoot the laser using a raycast to first determine if it hit another reflection platform and if so, determine in which diretion the laser will be reflected
    RaycastHit2D[] hits = Physics2D.RaycastAll(shootOrigin, shootDirection);
    bool hasHitReflectionWindow = false;
    Vector2 shootDirectionReflected = Vector2.zero;
    Vector3 shootOriginReflected = Vector3.zero;
    // go through all hits, check if we hit a reflection surface:
    for (int i = 0; i < hits.Length; i++) {
      var hit = hits[i];
      if (hit.collider == null || !hit.collider.gameObject.CompareTag("ReflectionWindow")) { continue; }

      // check if the laser has hit this reflection window already to prevent "looping" 
      var hitAlready = hitReflectionWindows.Exists(other => other == hit.collider.gameObject);
      if (!hitAlready) {
        shootDirectionReflected = Vector2.Reflect(shootDirection, hit.normal); // determine the "bounce" direction to where the laser continues firing
        shootOriginReflected = hit.point;
        hitPoints.Add(hit.point); // store the point at which the hit occured for rendering it later
        hasHitReflectionWindow = true;
        hitReflectionWindows.Add(hit.collider.gameObject);
        break;
      }
    }

    // next: store all enemies that this reflection "segment" did hit using Physics2D.BoxCast
    if (hitPoints.Count > 1 && hasHitReflectionWindow) { // only hit enemies if we have bounced of a reflection already && hit another reflection window; the part between the players space station and the first reflection window is only used for targeting and does not hit asteroids itself

      float lazerWidth = .2f;
      Vector2 boxSize = new Vector2(lazerWidth, lazerWidth);

      // VisualizeBoxCast(new BoxCastDebug(origin, boxSize, angle, shootDir));

      var boxCastHits = Physics2D.BoxCastAll(shootOrigin, boxSize, 0f, shootDirection);
      for (int i = 0; i < boxCastHits.Length; i++) {
        var hit = boxCastHits[i];

        // Don't hit enemies behind the reflection window 
        if (hit.collider.gameObject.CompareTag("ReflectionWindow") && hit.distance > .2f ) {
          break;
        }

        if (hit.collider == null || !hit.collider.gameObject.CompareTag("Enemy")) { continue; }

        var hitAlready = hitEnemies.Exists(other => other == hit.collider.gameObject);
        if (!hitAlready) {
          hitEnemies.Add(hit.collider.gameObject);
        }
      }

    }

    if (hasHitReflectionWindow) {
      // reflect the laster (another reflection window was hit)
      ShootLaser(shootDirectionReflected, shootOriginReflected, hitReflectionWindows, hitEnemies, hitPoints);
    } else {
      // deal damage to hit asteroids
      if (currentActiveTarget != null) { currentActiveTarget.SetActiveTarget(false); }
      if (hitPoints.Count > 0) {
        currentActiveTarget = hitReflectionWindows[0].GetComponentInParent<ReflectionWindow>();
        currentActiveTarget.SetActiveTarget(true);
      }

      if (hitPoints.Count > 1) {
        cameraCtrl.Shake(hitEnemies.Count > 0);
        DrawLaserSegments(hitPoints);
        SoundMngr.instance.ShootLaser(transform);
        HitEnemies(hitEnemies);
      }
    }
  }

  private void VisualizeBoxCast(BoxCastDebug b) {
    BoxCastDrawer.Draw(b.origin, b.size, b.angle, b.direction);
  }

  void DrawLaserSegments(List<Vector2> hitPoints) {
    laserRenderer.laserSegmentPrefab = lazerSegmentPrefab;
    laserRenderer.DrawLazerSegments(hitPoints);
  }

  LaserSegment previewSegment;
  public void DrawAim(Vector3 from, Vector3 shootDir) {
    if (previewSegment == null) {
      previewSegment = Instantiate(lazerSegmentPrefab, Vector3.zero, Quaternion.identity);
      previewSegment.staticLine = true;
    }
    RaycastHit2D[] hits = Physics2D.RaycastAll(from, shootDir);
    bool drawn = false;
    for (int i = 0; i < hits.Length; i++) {
      var hit = hits[i];
      if (hit.collider == null || !hit.collider.gameObject.CompareTag("ReflectionWindow")) { continue; }

      previewSegment.Draw(
        from,
        hit.point,
        Color.white,
        .02f,
        null,
        false
      );

      drawn = true;
      break;
    }

    if (!drawn) {
      previewSegment.Remove();
    }

  }

  void HitEnemies(List<GameObject> enemies) {
    //cameraCtrl.Shake(true);
    if (enemies.Count > 0) { 
      SoundMngr.instance.EnemyDeath(transform);
      
    }
    
    enemies.ForEach(e => e.GetComponent<Enemy>().TakeDamage());
  }
}

public struct BoxCastDebug {
  public Vector3 origin;
  public Vector2 size;
  public float angle;
  public Vector2 direction;

  public BoxCastDebug(Vector3 origin, Vector2 size, float angle, Vector2 direction) {
    this.origin = origin;
    this.size = size;
    this.angle = angle;
    this.direction = direction;
  }
}