using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellmade.Sound;

public class SoundMngr : MonoBehaviour {

  public static SoundMngr instance;

  [SerializeField]
  AudioClip hitByPlayer;
  [SerializeField]
  float hitByPlayerVolume = .75f;

  [SerializeField]
  AudioClip enemyDeath;
  [SerializeField]
  float enemyDeathVolume = .75f;

  [SerializeField]
  AudioClip shootGun;
  [SerializeField]
  float shootGunVolume = .33f;

  [SerializeField]
  AudioClip music;
  [SerializeField]
  float musicVolume = .33f;


  void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    } else if (instance != this) {
      Destroy(gameObject);
    }
  }

  public void StartMusic() {
    EazySoundManager.PlayMusic(music, musicVolume, true, true);
  }
  public void HitByPlayer(Transform position) {
    EazySoundManager.PlaySound(hitByPlayer, hitByPlayerVolume);
  }
  public void EnemyDeath(Transform position) {
    EazySoundManager.PlaySound(enemyDeath, enemyDeathVolume);
  }

  public void ShootLaser(Transform position) {
    EazySoundManager.PlaySound(shootGun, shootGunVolume);
  }
}
