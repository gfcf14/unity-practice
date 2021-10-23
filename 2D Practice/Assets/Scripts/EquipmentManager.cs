using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {
  private Animator anim;
  private int tunicIndex;

  void Start() {
    anim = GetComponent<Animator>();
    // tunicIndex = GameObject.Find("tunic").GetComponent<SpritePosition>().objectIndex;

    // SetEquipmentIndex("tunic", tunicIndex);
    SetEquipmentIndex("tunic", 0);
  }

  void SetEquipmentIndex(string equipmentObject, int objectIndex) {
    Debug.Log("Setting tunic index to " + objectIndex);
    switch(equipmentObject) {
      case "tunic":
        anim.SetInteger("tunicIndex", objectIndex);
        break;
      default:
        Debug.Log("No valid condition provided");
        break;
    }
  }

  void Update() {
    // testing to change tunic index
    if (Input.GetKey(KeyCode.Keypad1)) {
      SetEquipmentIndex("tunic", 1);
    }

    if (Input.GetKey(KeyCode.Keypad0)) {
      SetEquipmentIndex("tunic", 0);
    }
  }
}
