using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

  public GameObject playerDeathEffect;

  public GameObject playUI;
  public GameObject mainScreen;
  public Gun gun;
  bool started = false;
  bool gameOver = false;
  public bool Started() { return started; }
  public bool IsGameOver() { return gameOver; }

  void Start() {
    SoundMngr.instance.StartMusic();  
  }
  void Awake() {
    Time.timeScale = 1f;
  }
  void Update() {
  }

  public void StartGame() {
    mainScreen.SetActive(false);
    playUI.SetActive(true);
    started = true;
    gun.StartLazer();
  }

  public void WinGame() {
    gameOver = true;
    var enemies = GameObject.FindGameObjectsWithTag("Enemy");
    foreach (var e in enemies) { e.GetComponent<Enemy>().TakeDamage(); }
    GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Gun>().StopLazer();

    Invoke("StopGame", 3f);
  }
  public void GameOver() {
    gameOver = true;
    GameObject.FindGameObjectWithTag("EnemyDeathEffect").GetComponent<DestroyEffects>().SpawnPlayer(gun.transform.position);
    GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Gun>().enabled = false;
    SoundMngr.instance.EnemyDeath(gun.transform); SoundMngr.instance.EnemyDeath(gun.transform);

    Destroy(GameObject.FindGameObjectWithTag("PlayerGun"));
    Destroy(GameObject.FindGameObjectWithTag("Player"));

    Invoke("StopGame", 3f);
  }
  
  void StopGame() {
    Time.timeScale = 0;
    
    StartCoroutine(RestartGame());
  }

  IEnumerator RestartGame() {
    float ttr = Time.realtimeSinceStartup + 5f;

    while (Time.realtimeSinceStartup < ttr) {
      yield return null;
    }
    Restart();
  }
  void Restart() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

}