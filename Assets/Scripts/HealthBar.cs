using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

  public Slider healthSlider;

  void Awake() {
    healthSlider.value = 1f;
  }

  public void SetHealthPercentage(float p) {
    healthSlider.value = p;
  }

}