using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCtrl : MonoBehaviour {

  public Game game;

  float roundTime = 600;
  float playTime;

  int maxHealth = 200;
  int damageLarge = 10;
  int damageSmall = 5;
  int currHealth;

  void Awake() {
    currHealth = maxHealth;
    playTime = 0;
  }

  public Camera cam;
  public PushAway pushPrefab;

  public HealthBar healthBar;
  public TMP_Text timerTxt;

  void Update() {
    if (!game.Started()) return;

    UpdateTimer();
  }

  void UpdateTimer() {
    //timeLeft = Mathf.Max(0f, timeLeft - Time.deltaTime);
    playTime += Time.deltaTime;
    var timeLeft = Mathf.Max(0f, roundTime - playTime);
    var minutesLeft = (int)timeLeft / 60;
    var secondsLeft = timeLeft % 60;
    //timerTxt.text = (roundTime-playTime).ToString("0.00").Replace('.', ':').Replace(',', ':');
    var pad = secondsLeft < 10 ? "0" : "";
    timerTxt.text = minutesLeft + ":" + pad + secondsLeft.ToString("0");

    if (playTime >= roundTime) {
      game.WinGame();
    }

  }

  public bool IsEarly() {
    return playTime <= 60f;
  }

  void OnCollisionEnter2D(Collision2D collision) {
    //Debug.Log("player collision " + collision.gameObject);
    HandleCollision(collision.gameObject);
  }
  void OnTriggerEnter2D(Collider2D collision) {
    //Debug.Log("player trigger " + collision.gameObject);
  }
  void HandleCollision(GameObject other) {
    
  }

  public void GetHit(bool largeAsteroid) {
    currHealth -= largeAsteroid ? damageLarge : damageSmall;

    healthBar.SetHealthPercentage((float)currHealth / (float)maxHealth);
    Instantiate(pushPrefab, transform.position, Quaternion.identity);//.GetComponent<PushAway>();

    //Debug.Log("player health " + health);
    if (currHealth <= 0) {
      GameObject.FindGameObjectWithTag("Game").GetComponent<Game>().GameOver();
    }
  }
}
