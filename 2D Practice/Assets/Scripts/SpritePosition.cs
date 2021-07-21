using UnityEngine;

public class SpritePosition : MonoBehaviour {
  [SerializeField] private string objectName;
  [SerializeField] private int objectIndex;
  [SerializeField] private int objectR;
  [SerializeField] private int objectG;
  [SerializeField] private int objectB;
  private Rigidbody2D body;
  private SpriteRenderer objectRenderer;
  private GameObject hero;
  private Rigidbody2D heroRigidBody;
  private SpriteRenderer heroRenderer;
  private Sprite currentHeroSprite;
  private HeroResources heroResourcesScript;
  private HeroMovement heroMovementScript;
  private Sprite[] spriteGroup;

  private void Start() {
    body = GetComponent<Rigidbody2D>();
    objectRenderer = GetComponent<SpriteRenderer>();

    hero = GameObject.Find("Hero");
    heroRigidBody = hero.GetComponent<Rigidbody2D>();
    heroRenderer = hero.GetComponent<SpriteRenderer>();
    currentHeroSprite = heroRenderer.sprite;

    heroResourcesScript = hero.GetComponent<HeroResources>();
    Debug.Log(objectName + "(" + objectR + ", " + objectG + ", " + objectB + ")");
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
    return index >= 0 && index <= 34 ||
            index >= 69 && index <= 71 ||
            index >= 81 && index <= 83 ||
            index >= 97 && index <= 99 ||
            index >= 117 && index <= 118 ||
            index >= 133 && index <= 175;
  }
  private void SetSprite() {
    int currentSpriteIndex = int.Parse(currentHeroSprite.name.Replace("hero-body_", ""));
    objectRenderer.sprite = spriteGroup[currentSpriteIndex];

    objectRenderer.color = new Color32((byte)objectR, (byte)objectG, (byte)objectB, 255);

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
}
