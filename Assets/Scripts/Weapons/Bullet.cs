using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

  public int uses = 1;

  public float timeToLive = 3f;
  float timeToDespawn;

  Rigidbody2D rb;
  Vector2 shootDirection;
  public float shootSpeed = 10f;

  void Awake() {
    timeToDespawn = Time.time + timeToLive;
    rb = GetComponent<Rigidbody2D>();
  }
  void FixedUpdate() {
    rb.MovePosition(rb.position + shootDirection * shootSpeed * Time.fixedDeltaTime);
  }
  void Update() {
    if (Time.time > timeToDespawn) {
      // todo maybe set used=true here?
      Die();
      return;
    }

    if (noReflect && Time.time >= currentNoReflectCooldown) {
      noReflect = false;
    }

  }
  void OnCollisionEnter2D(Collision2D collision) {
    Debug.Log("OnCollisionEnter2D Bullet " + collision.gameObject);
    //HandleCollision(collision.gameObject);

    
    if (!noReflect) {
      ReflectOnCollision(collision.contacts[0]);
    }
  }
  void ReflectOnCollision(ContactPoint2D contact /*from OnCollisionEnter2D*/) {
    shootDirection = Vector2.Reflect(shootDirection, contact.normal);
  }
  void OnTriggerEnter2D(Collider2D collision) {
    Debug.Log("OnTriggerEnter2D Bullet " + collision.gameObject);
    HandleCollision(collision.gameObject);
  }
  void HandleCollision(GameObject other) {
    if (uses > 0 && other.CompareTag("Enemy")) {
      other.GetComponent<Enemy>().TakeDamage();
      uses--;
      
      if (uses == 0) { Die(); }

      return;
    } else if (other.CompareTag("Player") || other.CompareTag("Enemy")) {
      // do nothing but handle collision, todo remove?
      return;
    } else if (other.CompareTag("Bullet")) {
      Debug.LogError("bullet collided with bullet !?");
      return; 
    }

    //Debug.LogError("bullet collided with " + other);

  }

  float currentNoReflectCooldown = 0f;
  float reflectCooldown = 0.1f; 
  // if bullet is spawned inside reflection-object it will instantly collide with it so we set cooldown here 
  bool noReflect = false;

  public void Fire(Vector3 direction, float force, int uses) {
    this.uses = uses;
    shootDirection = direction; // direction.normalized;
    currentNoReflectCooldown = Time.time + reflectCooldown;
    noReflect = true;
  }

  public void Die() {
    Destroy(gameObject);
  }

}