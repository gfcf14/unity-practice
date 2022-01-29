using UnityEngine;

public class HeroMovement : MonoBehaviour {
  [SerializeField] public float speed;
  [SerializeField] private float jumpHeight;
  [SerializeField] private float jetpackHeight;
  private Rigidbody2D body;
  private Animator anim;
  private SpriteRenderer heroRenderer;

  private bool isRunning;
  private bool isGrounded;
  private bool isFalling;
  private bool isJumping;

  private bool isJetpackUp;

  private string jetpackHorizontal = "";
  private float maxJetpackTime = 1500;
  private float jetpackTime = 0;

  private float currentYPosition = 0;
  private float currentXPosition = 0;

  private float throwbackHeight = 0;

  private int isHurt = 0;
  private int isDead = 0;

  private bool isGliding;
  public bool isFacingLeft;

  public bool isAttackingSingle;
  public bool isAttackingHeavy;

  public bool isAirAttackSingle;
  public bool isAirAttackHeavy;

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

  public string[] weapons = new string[] {"fists", "single", "heavy", "throwables", "projectile-single", "projectile-heavy", "projectile-auto", "projectile-pull"};

  // public string jetpackUp = "🡣🡡⌴";
  public string jetpackUp = "du$";
  // public string jetpackLeft = "🡣🡠⌴";
  public string jetpackLeft = "dl$";
  // public string jetpackRight = "🡣🡢⌴";
  public string jetpackRight = "dr$";
  public float timeoutDuration = 0.25f;

  private string userInput = "";
  private float timeoutTime = 0.0f;

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
    if (!horizontalCollision && isHurt < 1) {
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

    if (isHurt == 1) {
      body.velocity = new Vector2(0, body.velocity.y);
    }

    if (isHurt == 2 && isGrounded) {
      transform.position = new Vector2(transform.position.x + ((isFacingLeft ? 1 : -1) * 0.01f), currentYPosition);
    }

    if (isHurt == 3) {
      if (throwbackHeight != 0 && transform.position.y > (currentYPosition - throwbackHeight)) {
        transform.position = new Vector2(transform.position.x + ((isFacingLeft ? 1 : -1) * 0.025f), transform.position.y + 0.025f);
      } else {
        throwbackHeight = 0;
        transform.position = new Vector2(transform.position.x + ((isFacingLeft ? 1 : -1) * 0.025f), transform.position.y - 0.025f);
      }
    }

    foreach (KeyCode currentKey in System.Enum.GetValues(typeof(KeyCode))) {
      if(Input.GetKeyUp(currentKey)) {
        if (userInput.Length == 0) {
          timeoutTime = Time.time + timeoutDuration;
        }

        // if (currentKey.ToString() == "Space") {
        //   // userInput += "⌴";
        //   userInput += "$";
        // } else 
        if (currentKey.ToString() == "UpArrow") {
          // userInput += "🡡";
          userInput += "u";
        } else if (currentKey.ToString() == "DownArrow") {
          // userInput += "🡣";
          userInput += "d";
        } else if (currentKey.ToString() == "LeftArrow") {
          // userInput += "🡠";
          userInput += "l";
        } else if (currentKey.ToString() == "RightArrow") {
          // userInput += "🡢";
          userInput += "r";
        } else {
          if (currentKey.ToString() != "Space") {
            userInput += currentKey.ToString();
          }
        }

        // if (userInput.Contains(jetpackUp)) {
        //   JetpackUp();
        //   userInput = "";
        // } else if (userInput.Contains(jetpackLeft)) {
        //   Debug.Log("jetpack left");
        //   userInput = "";
        // } else if (userInput.Contains(jetpackRight)) {
        //   Debug.Log("jetpack right");
        //   userInput = "";
        // } else if (userInput == "s") { // jumping?
        //   Jump();
        // }
      } else if (Time.time > timeoutTime && userInput.Length > 0) { // input is cleared
        userInput = "";
      }
    }

    // jumping
    if (Input.GetKey(KeyCode.Space)) {
      userInput += "$";
      if (isGrounded) {
        if (userInput.Contains(jetpackUp)) {
          JetpackUp();
          userInput = "";
        } else {
          Jump();
          userInput = "";
        }
      } else {
        if (userInput.Contains(jetpackLeft)) {
          JetpackHorizontal("left");
          userInput = "";
        } else if (userInput.Contains(jetpackRight)) {
          JetpackHorizontal("right");
          userInput = "";
        }
      }      
    }

    // gliding
    if (Input.GetKey(KeyCode.UpArrow)) {
      if (!isGrounded) {
        if (Input.GetKey(KeyCode.Space)) {
          Glide();
        } else {
          isGliding = false;
        }
      }
    }

    isRunning = horizontalInput != 0 && !isJumping && !isFalling && !isAttackingSingle && !isJetpackUp;

    if (!isGrounded && verticalSpeed < -1 && jetpackHorizontal == "") {
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
        } else if (currentWeapon == "heavy") {
          isAttackingHeavy = true;
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
        } else if (currentWeapon == "heavy") {
          isAirAttackHeavy = true;
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

    if (Input.GetKeyDown(KeyCode.Keypad7)) {
      SimulateHurt(1);
    }

    if (Input.GetKeyDown(KeyCode.Keypad8) && isGrounded) {
      SimulateHurt(2);
    }

    if (Input.GetKeyDown(KeyCode.Keypad9)) {
      SimulateHurt(3);
    }

    if (Input.GetKeyDown(KeyCode.Backspace)) {
      SimulateDeath(isGrounded);
    }

    if (isDropKicking) {
      body.velocity = new Vector2(body.velocity.x + (jumpHeight * (isFacingLeft ? -1 : 1)), -(float)(jumpHeight * 0.75));
    }

    if (isGliding) {
      body.velocity = new Vector2(body.velocity.x + (jumpHeight * (isFacingLeft ? -1 : 1)), -(float)(jumpHeight * 0.25));
    }

    if (jetpackHorizontal != "") {
      body.velocity = new Vector2(body.velocity.x + (jetpackHeight * (jetpackHorizontal == "left" ? -1 : 1)), body.velocity.y);
      transform.position = new Vector2(transform.position.x, currentYPosition);
      if ((Time.time * 1000) > jetpackTime + maxJetpackTime) {
        jetpackHorizontal = "";
        jetpackTime = 0;
        body.velocity = new Vector2(0, 0);
      }
    }

    if (isDead == 2) {
      if (!isGrounded) {
        body.velocity = new Vector2(-body.velocity.x + (jumpHeight * (isFacingLeft ? 2 : -2)), -(float)jumpHeight);
      } else {
        body.velocity = new Vector2(0, 0);
      }
    }

    // set animator parameters
    anim.SetBool("isRunning", isRunning);
    anim.SetBool("isGrounded", isGrounded);
    anim.SetBool("isFalling", isFalling);
    anim.SetBool("isJumping", isJumping);
    anim.SetBool("isJetpackUp", isJetpackUp);
    anim.SetBool("horizontalCollision", horizontalCollision);
    anim.SetBool("isAttackingSingle", isAttackingSingle);
    anim.SetBool("isAirAttackSingle", isAirAttackSingle);
    anim.SetBool("isAirAttackHeavy", isAirAttackHeavy);
    anim.SetBool("isKicking", isKicking);
    anim.SetBool("isDropKicking", isDropKicking);
    anim.SetBool("isPunching", isPunching);
    anim.SetBool("isAirPunching", isAirPunching);
    anim.SetBool("isThrowing", isThrowing);
    anim.SetBool("isShootingSingle", isShootingSingle);
    anim.SetBool("isShootingAuto", isShootingAuto);
    anim.SetBool("isShootingPull", isShootingPull);
    anim.SetBool("isAirShooting", isAirShooting);
    anim.SetBool("isAttackingHeavy", isAttackingHeavy);
    anim.SetBool("isJetpackHorizontal", jetpackHorizontal != "");
    anim.SetBool("isGliding", isGliding);
    anim.SetInteger("isHurt", isHurt);
    anim.SetInteger("isDead", isDead);
  }

  void SimulateDeath(bool isGrounded) {
    isDead = isGrounded ? 1 : 2;
  }

  void SimulateHurt(int hurtLevel) {
    body.velocity = new Vector2(0, 0);
    isHurt = hurtLevel;

    if (hurtLevel > 1) {
      currentXPosition = transform.position.x;
      currentYPosition = transform.position.y;
    }

    if (hurtLevel == 3) {
      throwbackHeight = 5f;
    }
  }

  void Recover() {
    isHurt = 0;
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

  void ClearAirAttackHeavy() {
    isAirAttackHeavy = false;
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

  void ClearAttackHeavy() {
    isAttackingHeavy = false;
  }

  public void OnGUI() {
    string guiLabel = "Running: " + isRunning + "\n" +
                      "Grounded: " + isGrounded + "\n" +
                      "Falling: " + isFalling + "\n" +
                      "Jumping: " + isJumping + "\n" +
                      "Gliding: " + isGliding + "\n" +
                      "JetpackUp: " + isJetpackUp + "\n" +
                      "JetpackHorizontal: " + (jetpackHorizontal != "" ? jetpackHorizontal : "none") + "\n" +
                      "horizontalCollision: " + horizontalCollision + "\n" +
                      "Equipment: " + currentWeapon + "\n" +
                      "Attack_Single: " + isAttackingSingle + "\n" +
                      "Attack_Heavy: " + isAttackingHeavy + "\n" +
                      "Air_Attack_Single: " + isAirAttackSingle + "\n" +
                      "Air_Attack_Heavy: " + isAirAttackHeavy + "\n" +
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

  private void Glide() {
    isGliding = true;
  }

  private void JetpackUp() {
    body.velocity = new Vector2(body.velocity.x, jetpackHeight);
    isJetpackUp = true;
    isJumping = false;
    isGrounded = false;
  }

  private void JetpackHorizontal(string direction) {
    jetpackHorizontal = direction;
    jetpackTime = Time.time * 1000;
    currentYPosition = transform.position.y;
    isJumping = false;
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
          isJetpackUp = false;
          horizontalCollision = false;
          isDropKicking = false;

          if (isHurt == 3) {
            Recover();
          }

          // disable air attack animations if these haven't finished when player hits ground
          isAirPunching = false;
          isAirShooting = false;
          isAirAttackSingle = false;
          isAirAttackHeavy = false;
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
