using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePosition : MonoBehaviour {
  [SerializeField] private string objectName;
  [SerializeField] private int objectIndex;

  private Rigidbody2D body;
  private SpriteRenderer objectRenderer;
  private GameObject hero;
  private Rigidbody2D heroRigidBody;
  private SpriteRenderer heroRenderer;
  private Sprite currentHeroSprite;
  private HeroResources heroResourcesScript;
  private HeroMovement heroMovementScript;
  private Sprite[] spriteGroup;

  private void Awake() {
    body = GetComponent<Rigidbody2D>();
    objectRenderer = GetComponent<SpriteRenderer>();

    hero = GameObject.Find("Hero");
    heroRigidBody = hero.GetComponent<Rigidbody2D>();
    heroRenderer = hero.GetComponent<SpriteRenderer>();
    currentHeroSprite = heroRenderer.sprite;

    heroResourcesScript = hero.GetComponent<HeroResources>();
    spriteGroup = heroResourcesScript.spriteGroup[objectName];
  }

  private void Update() {
    heroMovementScript = hero.GetComponent<HeroMovement>();

    if (currentHeroSprite != heroRenderer.sprite) {
      currentHeroSprite = heroRenderer.sprite;
    }

    SetSprite();
    SetPosition();
  }

  private bool shouldMirrorSprite(int index) {
    // 0-26, 52-54, 61-63, 74-76, 89-90, 99-131 
    return index >= 0 && index <= 26 ||
            index >= 52 && index <= 54 ||
            index >= 61 && index <= 63 ||
            index >= 74 && index <= 76 ||
            index >= 89 && index <= 90 ||
            index >= 99 && index <= 131;
  }
  private void SetSprite() {
    int currentSpriteIndex = int.Parse(currentHeroSprite.name.Replace("hero-body_", ""));
    objectRenderer.sprite = spriteGroup[currentSpriteIndex];

    Debug.Log(heroMovementScript.isFacingLeft);

    if (heroMovementScript.isFacingLeft && shouldMirrorSprite(currentSpriteIndex)) {
      transform.localScale = new Vector3(-1, 1, 1);
    } else {
      transform.localScale = Vector3.one;
    }
  }

  // for this to work, the game object must have a
  // RigidBody2D component with Freeze Position active
  // for X and Y axis
  private void SetPosition() {
    Vector2 currentHeroPosition = heroRigidBody.position;
    transform.position = currentHeroPosition;
  }
  // test webhook
}
