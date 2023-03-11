using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

  public Game game;
  public GameObject player;
  public float spawnRate = 5f;
  float currentTimer = 0f;
  int lastSpawnPoint = -1;

  public GameObject SpawnPointContainer;
  [SerializeField]
  EnemySpawnerParent[] spawnPoints;

  void Start() {
    spawnPoints = new EnemySpawnerParent[SpawnPointContainer.transform.childCount];
    for (int i = 0; i < SpawnPointContainer.transform.childCount; i++) {
      var x = SpawnPointContainer.transform.GetChild(i).gameObject;
      if (x.activeInHierarchy) {
        spawnPoints[i] = x.GetComponent<EnemySpawnerParent>();
      }
    }
  }

  void Awake() {
    // immediately spawn initial wave
    currentTimer = spawnRate;
  }

  void Update() {
    if (!game.Started()) return;

    currentTimer += Time.deltaTime;
    if (currentTimer >= spawnRate) {
      currentTimer = 0f;
      SpawnWave();
    }
  }

  void SpawnWave() {
    for(int i = 0; i < spawnPoints.Length; i++) {
      spawnPoints[i].SpawnWave(player, game);
    }
    return;

    int spawnAt = Random.Range(0, spawnPoints.Length);
    if (spawnAt == lastSpawnPoint) { SpawnWave(); return; }
    lastSpawnPoint = spawnAt;
    spawnPoints[spawnAt].SpawnWave(player, game);

  }
}