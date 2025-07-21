using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform playerPosition;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;

    [Header("SFX")]
    [SerializeField] AudioClip jumpSound;

    [Header("UI")]
    [SerializeField] private GameObject congratulationsPanel;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (playerPosition.position.x >= 55)
        {
            congratulationsPanel.SetActive(true);
            gameObject.SetActive(false);
        }
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Jump();
            SoundManager.instance.PlaySound(jumpSound);
        }

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", IsGrounded());
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        anim.SetTrigger("jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded();
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
