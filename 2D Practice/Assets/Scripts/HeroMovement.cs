using UnityEngine;

public class HeroMovement : MonoBehaviour {
  [SerializeField] public float speed;
  [SerializeField] private float jumpHeight;
  private Rigidbody2D body;
  private Animator anim;
  private SpriteRenderer heroRenderer;
  private string currentSpriteName;
  private bool isGrounded;
  private bool isFalling;
  private bool isJumping;
  public bool isFacingLeft;

  public int spriteIndex;

  private bool horizontalCollision;

  public int collisionCounter = 0;

  public float horizontalInput = 0;

  // called when script is loaded
  private void Awake() {
    body = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    heroRenderer = GetComponent<SpriteRenderer>();
    currentSpriteName = heroRenderer.sprite.name;
    spriteIndex = 0;
  }

  // called on every frame of the game
  private void Update() {
    horizontalInput = Input.GetAxis("Horizontal");
    float verticalSpeed = body.velocity.y;

    if (currentSpriteName != heroRenderer.sprite.name) {
      currentSpriteName = heroRenderer.sprite.name;
      spriteIndex = getSpriteIndex();
    }

    Debug.Log(horizontalInput);

    // x axis movement
    if (!horizontalCollision) {
      body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

      // flip player when moving left
      if (horizontalInput > 0.01f && isGrounded) {
        transform.localScale = Vector3.one;
        isFacingLeft = false;
      }
      // flip player when moving right
      else if (horizontalInput < -0.01f && isGrounded) {
        transform.localScale = new Vector3(-1, 1, 1);
        isFacingLeft = true;
      }
    }

    // jumping
    if (Input.GetKey(KeyCode.Space) && isGrounded) {
      Jump();
    }

    // set animator parameters
    anim.SetBool("isRunning", horizontalInput != 0 && !isJumping && !isFalling);
    anim.SetBool("isGrounded", isGrounded);
    anim.SetBool("isFalling", isFalling);
    anim.SetBool("isJumping", isJumping);
    anim.SetBool("horizontalCollision", horizontalCollision);

    if (!isGrounded && verticalSpeed < -1) {
      Fall();
    }
  }

  private int getSpriteIndex() {
    return int.Parse(currentSpriteName.Substring(currentSpriteName.LastIndexOf("_") + 1));
  }

  private void Fall() {
    isFalling = true;
  }

  private void Jump() {
    body.velocity = new Vector2(body.velocity.x, jumpHeight);
    isJumping = true;
    isGrounded = false;
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    Collider2D collider = collision.collider;
    Collider2D otherCollider = collision.otherCollider;

    if (collision.gameObject.tag == "Ground") {
      if (otherCollider.tag == "Hero") {
        if (!isHorizontalCollision(otherCollider, collider)) {
          isGrounded = true;
          isFalling = false;
          isJumping = false;
          horizontalCollision = false;
        } else {          
          horizontalCollision = true;

          if (isBottomCollision(otherCollider, collider)) {
            horizontalCollision = false;
          }
        }
      }      
    }

    collisionCounter++;
  }

  private bool isBottomCollision(Collider2D collider1, Collider2D collider2) {
    int c1BottomEdge = (int) collider1.bounds.max.y;
    int c2TopEdge = (int) collider2.bounds.min.y;

    return c1BottomEdge == c2TopEdge;
  }

  private bool isHorizontalCollision(Collider2D collider1, Collider2D collider2) {
    int c1RightEdge = (int) collider1.bounds.max.x;
    int c1LeftEdge = (int) collider1.bounds.min.x;

    int c2RightEdge = (int) collider2.bounds.max.x;
    int c2LeftEdge = (int) collider2.bounds.min.x;

    return (c1RightEdge == c2LeftEdge) || (c1LeftEdge == c2RightEdge);
  }

  private void OnCollisionExit2D(Collision2D collision) {
    collisionCounter--;

    if (collisionCounter == 0) {
      isGrounded = false;
    }
  }
}
