using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {

  public SpriteRenderer sr;
  Color origColor;
  public Game game;
  public GameObject target;

  public float moveSpeed = 1f;
  public float pushForce = 1f;
  public Rigidbody2D rb;

  public Action OnDestroy; // used by EnemySpawner

  Vector2 movement;
  Vector2 pushDirection;

  public int hitPoints = 3;
  public bool isLarge;

  void Awake() {
    origColor = sr.color;
  }
  void Update() {
    SetMoveDirectionToTarget();
    if (isPushed && Time.time >= pushEndTime) { isPushed = false; }
    if (isStunned && Time.time >= stunEndTime) { isStunned = false; }
  }
  void SetMoveDirectionToTarget() {
    if (game.IsGameOver()) return; 
    movement = new Vector2(
      target.transform.position.x, target.transform.position.y
    ) - rb.position;
    movement = movement.normalized;
  }
  void FixedUpdate() {
    var dir = isPushed ? pushDirection : movement;
    var speed = isPushed ? pushForce : moveSpeed;
    // if !stunned -> move or push
    // if stunned -> only push
    if (!isStunned) {
      rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    } else if (isPushed) {
      rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    }
    rb.rotation += .5f;
  }
  /* handle being pushed */
  public float pushedTime = .33f; // how long they are pushed for
  bool isPushed = false;
  float pushEndTime = 0f;
  public void Push(Vector3 direction) {
    pushDirection = new Vector2(direction.x, direction.y);
    isPushed = true;
    pushEndTime = Time.time + pushedTime;
  }
  /* handle being pushed */
  /* handle being stunned */
  public float stunOnHitTime = .1f;
  bool isStunned = false;
  float stunEndTime = 0f;
  void GetStunned() {
    isStunned = true;
    stunEndTime = Time.time + stunOnHitTime;
  }
  /* handle being stunned */

  void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Enemy") || collision.CompareTag("Bullet")) {
      return;
    }

    if (collision.CompareTag("Player")) {
      collision.gameObject.GetComponent<PlayerCtrl>().GetHit(isLarge);
      return;
    }

  }
  void OnCollisionEnter2D(Collision2D collision) {
    HandleCollision(collision);
  }
  void HandleCollision(Collision2D collision) {
    Debug.LogError("enemy collision with " + collision.gameObject);
  }

  bool IsDead() { return hitPoints == 0; }

  Coroutine flashFromDamage;
  public void TakeDamage() {
    if (IsDead()) return;

    hitPoints--;
    GetStunned();

    if (flashFromDamage != null) {
      StopCoroutine(flashFromDamage);
      sr.color = origColor;
    }
    flashFromDamage = StartCoroutine(MyCoroutines.FlashToColor(sr, .1f, Color.white, this, true));

    if (IsDead()) { Die(); }
  }

  void Die() {
    if (OnDestroy != null) { OnDestroy(); }
    GameObject.FindGameObjectWithTag("EnemyDeathEffect").GetComponent<DestroyEffects>().Spawn(transform.position);
    Destroy(gameObject);
  }

  public void StartDistanceCheck() {
    StartCoroutine(CheckDistanceToPlayer());
  }
  float maxDistToPlayer = 40f;
  IEnumerator CheckDistanceToPlayer() {
    while (!game.IsGameOver()) {
      var dist = Vector2.Distance(transform.position, target.transform.position);
      if (dist > maxDistToPlayer) {
        Destroy(gameObject);
      }
      yield return new WaitForSeconds(.33f);
    }
    
  }

}