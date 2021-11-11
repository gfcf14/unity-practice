using UnityEngine;

public class HeroMovement : MonoBehaviour {
  [SerializeField] public float speed;
  [SerializeField] private float jumpHeight;
  private Rigidbody2D body;
  private Animator anim;
  private SpriteRenderer heroRenderer;

  private bool isRunning;
  private bool isGrounded;
  private bool isFalling;
  private bool isJumping;
  public bool isFacingLeft;

  public bool isAttackingSingle;

  public bool isAirAttackSingle;

  public bool isKicking;
  public bool isDropKicking;

  public bool isPunching;
  public bool isAirPunching;
  public bool isAirShooting;

  public bool isThrowing;

  public bool isShootingSingle;

  public bool isShootingAuto;

  public bool isShootingPull;

  private bool horizontalCollision;

  public int collisionCounter = 0;

  public float horizontalInput = 0;

  public string[] weapons = new string[] {"fists", "single", "throwables", "projectile-single", "projectile-heavy", "projectile-auto", "projectile-pull"};
  public int weaponIndex = 0;

  public string currentWeapon;

  // called when script is loaded
  private void Awake() {
    body = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    heroRenderer = GetComponent<SpriteRenderer>();

    currentWeapon = weapons[weaponIndex % weapons.Length];
  }

  // called on every frame of the game
  private void Update() {
    horizontalInput = Input.GetAxis("Horizontal");
    float verticalSpeed = body.velocity.y;

    // x axis movement
    if (!horizontalCollision) {
      body.velocity = new Vector2(!isDropKicking ? horizontalInput * speed : 0, body.velocity.y);

      // flip player when moving left
      if (horizontalInput > 0.01f && isGrounded && !isAttackingSingle) {
        transform.localScale = Vector3.one;

        if (!isDropKicking) {
          isFacingLeft = false;
        }        
      }
      // flip player when moving right
      else if (horizontalInput < -0.01f && isGrounded && !isAttackingSingle) {
        transform.localScale = new Vector3(-1, 1, 1);
        
        if (!isDropKicking) {
          isFacingLeft = true;
        }
      }
    }

    // jumping
    if (Input.GetKey(KeyCode.Space) && isGrounded) {
      Jump();
    }

    isRunning = horizontalInput != 0 && !isJumping && !isFalling && !isAttackingSingle;

    if (!isGrounded && verticalSpeed < -1) {
      Fall();
    }

    if (Input.GetKey(KeyCode.Keypad4) && currentWeapon == "projectile-auto") {
      isShootingAuto = true;
    }

    if (Input.GetKeyUp(KeyCode.Keypad4) && isShootingAuto) {
      isShootingAuto = false;
    }

    if (Input.GetKeyDown(KeyCode.Keypad4)) {
      if (isGrounded) {
        if (currentWeapon == "fists" || currentWeapon == "projectile-single") {
          isPunching = true;
        } else if (currentWeapon == "single") {
          isAttackingSingle = true;
        } else if (currentWeapon == "throwables") {
          isThrowing = true;
        } else if (currentWeapon == "projectile-heavy") {
          isShootingSingle = true;
        } else if (currentWeapon == "projectile-pull") {
          isShootingPull = true;
        }
      } else if (isJumping || isFalling) {
        if (currentWeapon == "fists") {
          isAirPunching = true;
        } else if (currentWeapon == "single") {
          isAirAttackSingle = true;
        } else if (currentWeapon == "projectile-single") {
          isAirShooting = true;
        }
      }      
    }

    if (Input.GetKeyDown(KeyCode.Keypad5)) {
      if (isGrounded && !isRunning) {
        isKicking = true;
      } else if (isJumping) {
        DropKick();
      }
    }

    if (Input.GetKeyDown(KeyCode.RightControl)) {
      weaponIndex++;
      currentWeapon = weapons[weaponIndex % weapons.Length];
    }

    if (isDropKicking) {
      body.velocity = new Vector2(body.velocity.x + (jumpHeight * (isFacingLeft ? -1 : 1)), -(float)(jumpHeight * 0.75));
    }

    // set animator parameters
    anim.SetBool("isRunning", isRunning);
    anim.SetBool("isGrounded", isGrounded);
    anim.SetBool("isFalling", isFalling);
    anim.SetBool("isJumping", isJumping);
    anim.SetBool("horizontalCollision", horizontalCollision);
    anim.SetBool("isAttackingSingle", isAttackingSingle);
    anim.SetBool("isAirAttackSingle", isAirAttackSingle);
    anim.SetBool("isKicking", isKicking);
    anim.SetBool("isDropKicking", isDropKicking);
    anim.SetBool("isPunching", isPunching);
    anim.SetBool("isAirPunching", isAirPunching);
    anim.SetBool("isThrowing", isThrowing);
    anim.SetBool("isShootingSingle", isShootingSingle);
    anim.SetBool("isShootingAuto", isShootingAuto);
    anim.SetBool("isShootingPull", isShootingPull);
    anim.SetBool("isAirShooting", isAirShooting);
  }

  void ClearPunch() {
    isPunching = false;
  }

  void ClearAirPunch() {
    isAirPunching = false;
  }

  void ClearAttackSingle() {
    isAttackingSingle = false;
  }

  void ClearAirAttackSingle() {
    isAirAttackSingle = false;
  }

  void ClearAirShooting() {
    isAirShooting = false;
  }

  void ClearKick() {
    isKicking = false;
  }

  void ClearThrow() {
    isThrowing = false;
  }

  void ClearShootingSingle() {
    isShootingSingle = false;
  }

  void ClearShootingPull() {
    isShootingPull = false;
  }

  public void OnGUI() {
    string guiLabel = "Running: " + isRunning + "\n" +
                      "Grounded: " + isGrounded + "\n" +
                      "Falling: " + isFalling + "\n" +
                      "Jumping: " + isJumping + "\n" +
                      "horizontalCollision: " + horizontalCollision + "\n" +
                      "Equipment: " + currentWeapon + "\n" +
                      "Attack_Single: " + isAttackingSingle + "\n" +
                      "Air_Attack_Single: " + isAirAttackSingle + "\n" +
                      "Air_Shooting: " + isAirShooting + "\n" +
                      "Kick: " + isKicking + "\n" +
                      "Drop_Kick: " + isDropKicking + "\n" +
                      "Punching: " + isPunching + "\n" +
                      "Air_Punch: " + isAirPunching + "\n" +
                      "Throwing: " + isThrowing + "\n" +
                      "Shooting: " + (isShootingSingle || isShootingAuto || isShootingPull || isAirShooting) + "\n";
    GUI.Label(new Rect(0, 0, 200, 400), guiLabel);
  }

  private void Fall() {
    isFalling = true;
  }

  private void Jump() {
    body.velocity = new Vector2(body.velocity.x, jumpHeight);
    isJumping = true;
    isGrounded = false;
  }

  private void DropKick() {
    isDropKicking = true;
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
          isDropKicking = false;
        } else {          
          horizontalCollision = true;

          if (isBottomCollision(otherCollider, collider)) {
            horizontalCollision = false;
            ClearAirAttackSingle();
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
