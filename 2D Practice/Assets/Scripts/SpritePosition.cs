using UnityEngine;

public class SpritePosition : MonoBehaviour {
  [SerializeField] public int objectIndex;
  [SerializeField] private int objectR;
  [SerializeField] private int objectG;
  [SerializeField] private int objectB;
  
  private SpriteRenderer objectRenderer;

  private void Start() {
    objectRenderer = GetComponent<SpriteRenderer>();

    SetSprite();
  }

  private void Update() {
    SetPosition();
  }

  private void SetSprite() {
   objectRenderer.color = new Color32((byte)objectR, (byte)objectG, (byte)objectB, 255);
  }

  private void SetPosition() {
    transform.localPosition = Vector2.zero;    
  }
}
