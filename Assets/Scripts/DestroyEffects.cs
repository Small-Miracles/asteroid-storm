using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffects : MonoBehaviour {
  public GameObject prefab;
  public GameObject prefabPlayer;

  public void Spawn(Vector3 pos) {
    var e = Instantiate(prefab, pos, Quaternion.identity);
    e.transform.SetParent(gameObject.transform);
    var ra = e.GetComponent<RemoveAfter>();
    ra.Go(); // ???
  }

  public void SpawnPlayer(Vector3 pos) {
    var e = Instantiate(prefabPlayer, pos, Quaternion.identity);
    e.transform.SetParent(gameObject.transform);
    var ra = e.GetComponent<RemoveAfter>();
    ra.Go(); // ???
  }
}
