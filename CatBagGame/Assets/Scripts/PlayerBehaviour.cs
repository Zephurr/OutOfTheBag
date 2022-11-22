using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isMoving = false;

    [SerializeField] private Vector3 jumpDir;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpTimeElapsed = 0;
    [SerializeField] private float raycastYOffset;
    [SerializeField] private float raycastRightXOffset;
    [SerializeField] private float raycastLeftXOffset;
    [SerializeField] private float raycastInteractXOffset;
    [SerializeField] private bool testCanJump;
    [SerializeField] private bool jumping = false;
    [SerializeField] private LayerMask jumpLayerMask;

    [SerializeField] private LayerMask interactLayerMask;
    RaycastHit2D objHitInfo;

    Rigidbody2D rb2d;
    Animator animator;

    private bool facingRight = true;
    Vector2 interactDir = Vector2.right;

    [SerializeField] float escapeTime;
    [SerializeField] float escapeTimeRate;

    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public bool FacingRight { get => facingRight; set => facingRight = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
    public float Speed { get => speed; set => speed = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpForce = 18;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(raycastLeftXOffset, raycastYOffset), -jumpDir, Color.red);
        Debug.DrawRay(transform.position + new Vector3(raycastRightXOffset, raycastYOffset), -jumpDir, Color.red);
        Debug.DrawRay(transform.position + new Vector3(raycastInteractXOffset, 0), interactDir, Color.green);

        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (canMove)
        {
            if (CanJump() && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (jumping)
            {
                HighJump();
                animator.SetBool("walk", false);
            }
        }

        if (CanInteract() && Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetAxis("Horizontal") > 0 && !facingRight)
            {
                gameObject.transform.Rotate(0, 180, 0, Space.Self);
                raycastLeftXOffset *= -1;
                raycastRightXOffset = Mathf.Abs(raycastRightXOffset);
                raycastInteractXOffset = Mathf.Abs(raycastInteractXOffset);
                facingRight = true;
                interactDir = Vector2.right;
            }
            else if (Input.GetAxis("Horizontal") < 0 && facingRight)
            {
                gameObject.transform.Rotate(0, -180, 0, Space.Self);
                raycastLeftXOffset = Mathf.Abs(raycastLeftXOffset);
                raycastRightXOffset *= -1;
                raycastInteractXOffset *= -1;
                facingRight = false;
                interactDir = Vector2.left;
            }
        }
    }

    void Move()
    {
        // run
        Vector3 newPos = transform.position;
        newPos.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.position = newPos;

        if (Input.GetAxis("Horizontal") != 0)
        {
            isMoving = true;

            if (!jumping && isMoving)
            {
                animator.SetBool("walk", true);
            }
            else
            {
                animator.SetBool("walk", false);
            }
        }
        else if (Input.GetAxis("Horizontal") == 0)
        {
            animator.SetBool("walk", false);
            isMoving = false;
        }
    }

    void Jump()
    {
        jumpTimeElapsed = 0;
        jumping = true;
        animator.SetBool("jump", true);
    }

    void HighJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (jumping && jumpTimeElapsed < jumpTime)
            {
                jumpTimeElapsed += Time.deltaTime;
                rb2d.AddForce(jumpDir * jumpForce, ForceMode2D.Force);
            }
        }
        else
        {
            jumping = false;
            animator.SetBool("jump", false);
        }
    }

    bool CanJump()
    {
        RaycastHit2D vertHitInfo1;
        RaycastHit2D vertHitInfo2;

        vertHitInfo1 = Physics2D.Raycast(transform.position + new Vector3(raycastRightXOffset, raycastYOffset), -jumpDir, 0.5f, jumpLayerMask);
        vertHitInfo2 = Physics2D.Raycast(transform.position + new Vector3(raycastLeftXOffset, raycastYOffset), -jumpDir, 0.5f, jumpLayerMask);

        if (vertHitInfo1.collider != null || vertHitInfo2.collider != null)
        {
            if (animator.GetBool("jump"))
            {
                animator.SetBool("jump", false);
                animator.Play("jumpStartEnd");
            }
            return true;
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("jump", true);
            return false;
        }
    }

    bool CanInteract()
    {
        objHitInfo = Physics2D.Raycast(transform.position + new Vector3(raycastInteractXOffset, 0), interactDir, 1f, interactLayerMask);

        if (objHitInfo.collider != null)
        {
            //Debug.Log("can interact");
            return true;
        }
        else
        {
            return false;
        }
    }

    void Interact()
    {
        Debug.Log("interact");
        GameObject interactObj = objHitInfo.collider.gameObject;

        if (interactObj.TryGetComponent(typeof(InteractableObject), out Component com))
        {
            InteractableObject iObj;
            iObj = (InteractableObject)com;
            if (!iObj.hasBeenInteracted)
            {
                Debug.Log("Interacted with " + iObj.gameObject);
                iObj.Interact();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            animator.Play("jumpStartEnd");
        }
    }

    public void EscapePets()
    {
        float time = 0;
        if (Input.GetAxis("Horizontal") > 0)
        {
            time += escapeTimeRate;
        }

        if (time >= escapeTime)
        {
            Debug.Log("escaped");
            canMove = true;
        }
    }
}
