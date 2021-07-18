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
      if (currentHeroSprite != heroRenderer.sprite) {
        currentHeroSprite = heroRenderer.sprite;
      }

      SetSprite();
      SetPosition();
  }

// TODO: account for mirroring when available
  private void SetSprite() {
    int currentSpriteIndex = int.Parse(currentHeroSprite.name.Replace("hero-body_", ""));
    objectRenderer.sprite = spriteGroup[currentSpriteIndex];
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
