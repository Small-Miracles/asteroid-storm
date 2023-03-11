using System.Collections;
using UnityEngine;

public class ReflectionWindow : MonoBehaviour {

  public Transform center;
  public float offset = 0f;

  public bool rotateToCenter = true;

  SpriteRenderer sr;
  Color origClr;
  //Color activeClr = //Color.red;
  // 
  static Color activeClr = new Color(108f/255f, 185f/255f, 201f/255f);
  void Awake() {
    sr = GetComponent<SpriteRenderer>();
    origClr = sr.color;
  }

  void Update() {
    if (rotateToCenter) {
      transform.eulerAngles = new Vector3(0, 0, Gun.LookAtAngle(center.position, transform.position, offset));
    }
    
    //transform.eulerAngles
  }

  public void SetActiveTarget(bool active) {
    sr.color = active ? activeClr : origClr;
  }



}