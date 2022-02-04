using UnityEngine;

public class WieldablePosition : MonoBehaviour {
  void Start() {}

  void Update() {
    SetPosition();
  }

  private void SetPosition() {
    transform.localPosition = Vector2.zero; 
  }
}
