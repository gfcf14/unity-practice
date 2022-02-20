using UnityEngine;

public class WieldablePosition : MonoBehaviour {
  GameObject hero;
  HeroMovement heroMovementScript;
  protected Animator anim;

  private bool isAirAttackSingle;

  [SerializeField]
  private PolygonCollider2D[] colliders;
  private int currentColliderIndex = 0;

  void SetColliderForSprite(int spriteNum) {
    colliders[currentColliderIndex].enabled = false;
    currentColliderIndex = spriteNum;
    colliders[currentColliderIndex].enabled = true;
  }
  void Start() {
    hero = GameObject.Find("Hero");
    heroMovementScript = hero.GetComponent<HeroMovement>();
    anim = GetComponent<Animator>();
    isAirAttackSingle = heroMovementScript.isAirAttackSingle;
  }

  void Update() {
    SetPosition();

    isAirAttackSingle = heroMovementScript.isAirAttackSingle;
    anim.SetBool("isAirAttackSingle", isAirAttackSingle);
  }

  private void SetPosition() {
    transform.position = hero.transform.position; 
  }

  void OnCollisionEnter2D(Collision2D collision) {}
}
