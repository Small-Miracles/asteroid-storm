using System.Collections;
using UnityEngine;

public class EnemySpawnerParent : MonoBehaviour {

  public Enemy prefab_small;
  public Enemy prefab_large;

  //public GameObject SpawnPointContainer;
  [SerializeField]
  GameObject[] spawnPoints;

  public GameObject container;

  void Awake() {
    spawnPoints = new GameObject[transform.childCount];
    for (int i = 0; i < transform.childCount; i++) {
      spawnPoints[i] = transform.GetChild(i).gameObject;
    }
  }

  public void SpawnWave(GameObject player, Game game) {
    for (int i = 0; i < spawnPoints.Length; i++) {
      var chance = player.GetComponent<PlayerCtrl>().IsEarly() ? .5f : .85f;
      if (Random.value > chance) continue;

      var spawnPoint = spawnPoints[i];

      var isLarge = Random.value >= .75f;
      var spawnPos = spawnPoint.transform.position + new Vector3(Random.Range(0f, 2f), Random.Range(0f, 2f), 0f);
      var enemy = Instantiate(isLarge ? prefab_large : prefab_small, spawnPos, Quaternion.identity);
      enemy.game = game;
      enemy.target = player;
      enemy.isLarge = isLarge;
      enemy.transform.SetParent(container.transform);
      enemy.StartDistanceCheck();
    }
  }
}