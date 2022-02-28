using UnityEngine;

public class EnemyMovement : MonoBehaviour {
  [SerializeField] public float speed;

  private Rigidbody2D body;
  private Animator anim;
  private SpriteRenderer enemyRenderer;
  private float enemyHeight = 0f;
  private float enemyWidth = 0f;
  
  public bool isFacingLeft = false;
  public bool isWalking;
  // public float leftPlatformEdge = 0f;
  // public float rightPlatformEdge = 0f;
  
  void Awake() {
    body = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    enemyRenderer = GetComponent<SpriteRenderer>();
    enemyHeight = enemyRenderer.bounds.size.y;
    enemyWidth = enemyRenderer.bounds.size.x;

    isWalking = true;
  }

  void Update() {
    if (isWalking) {
      body.velocity = new Vector2((isFacingLeft ? -1 : 1) * speed, body.velocity.y);

      int direction = isFacingLeft ? -1 : 1;

      RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + ((enemyWidth / 2) * direction), transform.position.y - enemyHeight / 2), transform.TransformDirection(new Vector2(1 * (direction), -1)), 1f);
      Debug.DrawRay(new Vector2(transform.position.x + ((enemyWidth / 2) * direction), transform.position.y - enemyHeight / 2), new Vector2(1 * (direction), -1), Color.green);
    

      if (!hit) {
        isFacingLeft = !isFacingLeft;
      }
    }

    if (isFacingLeft) {
      transform.localScale = new Vector3(-1, 1, 1);
    } else {
      transform.localScale = Vector3.one;
    }

    anim.SetBool("isWalking", isWalking);
  }
}
