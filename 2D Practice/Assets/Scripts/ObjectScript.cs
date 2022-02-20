using UnityEngine;

public class ObjectScript : MonoBehaviour {
  private SpriteRenderer objectRenderer;

  void Start() {
    objectRenderer = GetComponent<SpriteRenderer>();
  }

  void Update() {
      
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.tag == "Weapon") {
      objectRenderer.color = new Color32((byte)255, (byte)0, (byte)0, 255);
    }
  }
}
