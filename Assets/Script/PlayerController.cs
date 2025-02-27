using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;
    public Transform playerCamera;
    public GameObject statUI;
    public Animator animator;

    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 7f;
    public float gravity = 9.81f;  // 자체 중력 값 (수동 설정)
    private float rotationX = 0f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isStatUIOpen = false;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody가 없습니다. Player 오브젝트에 Rigidbody를 추가하세요!");
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.useGravity = false;  // 중력 비활성화
        }
        if (animator == null)
        {
            Debug.LogError("Animator가 없습니다! 2Handed Warrior 게임 오브젝트에 Animator를 추가하세요.");
        }

        LockCursor(true);
    }

    void Update()
    {
        Debug.Log("isGrounded: " + isGrounded);

        if (!isStatUIOpen && !isAttacking)
        {
            HandleMovement();
            HandleMouseLook();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAttacking)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleStatUI();
        }

        if (isStatUIOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleStatUI();
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float currentMoveSpeed = (vertical < 0) ? moveSpeed - 1f : moveSpeed;

        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        rb.MovePosition(rb.position + movement * currentMoveSpeed * Time.deltaTime);

        bool isMoving = (horizontal != 0 || vertical != 0);
        bool isMovingBackward = vertical < 0;

        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
            animator.SetBool("isMovingBackward", isMovingBackward);
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("AttackTrigger");

        float attackMoveSpeed = 2.5f;
        float attackDuration = 1.0f;

        float elapsedTime = 0f;
        while (elapsedTime < attackDuration)
        {
            rb.MovePosition(rb.position + transform.forward * attackMoveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isAttacking = false;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);  // y축 속도 초기화
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        animator.SetTrigger("isJumping");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded!");
            if (isGrounded == false) animator.SetBool("isJumping",false);
            isGrounded = true;
        }
    }

    void ToggleStatUI()
    {
        isStatUIOpen = !isStatUIOpen;
        statUI.SetActive(isStatUIOpen);

        if (isStatUIOpen)
        {
            LockCursor(false);
        }
        else
        {
            LockCursor(true);
        }
    }

    void LockCursor(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // 중력 적용
    void FixedUpdate()
    {
        if (!isGrounded)
        {
            Vector3 gravityForce = Vector3.down * gravity;  // 아래 방향 중력
            rb.AddForce(gravityForce, ForceMode.Acceleration);  // 중력 지속 적용
        }
    }
}