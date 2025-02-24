using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;  // 플레이어 스탯
    public Transform playerCamera;   // 카메라
    public GameObject statUI;        // 스탯 UI
    public Animator animator; 

    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 7f;
    private float rotationX = 0f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isStatUIOpen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();  // ✅ 애니메이터 연결

        if (rb == null)
        {
            Debug.LogError("Rigidbody가 없습니다. Player 오브젝트에 Rigidbody를 추가하세요!");
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator가 없습니다! 2Handed Warrior 게임 오브젝트에 Animator를 추가하세요.");
        }

        LockCursor(true); // 게임 시작 시 마우스 숨기기
    }

    void Update()
    {
        if (!isStatUIOpen)
        {
            HandleMovement();
            HandleMouseLook();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
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

    /// <summary>
    /// 플레이어 이동 처리 및 애니메이션 실행
    /// </summary>
   void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

        // ⭐ 이동 여부 체크 ⭐
        bool isMoving = (horizontal != 0 || vertical != 0);

        // ⭐ Animator가 있을 경우 애니메이션 변경 ⭐
        if (animator != null)
        {
            if (isMoving)
            {
                animator.SetBool("isMoving", true); // 이동 중
            }
            else
            {
                animator.SetBool("isMoving", false); // 가만히 있을 때 Idle 애니메이션 실행
            }
        }
    }



    /// <summary>
    /// 마우스 움직임 처리
    /// </summary>
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    /// <summary>
    /// 점프 처리 및 애니메이션 실행
    /// </summary>
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;

        // ✅ 점프 애니메이션 실행
        animator.SetTrigger("Jump");
    }

    /// <summary>
    /// 공격 처리 및 애니메이션 실행
    /// </summary>
    void Attack()
    {
        float attackDamage = 10f * playerStats.attackMultiplier;
        Debug.Log("공격! 데미지: " + attackDamage);

        // ✅ 공격 애니메이션 실행
        animator.SetTrigger("AttackTrigger");
    }

    /// <summary>
    /// 바닥 감지 처리
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    /// <summary>
    /// 스탯 UI 토글
    /// </summary>
    void ToggleStatUI()
    {
        isStatUIOpen = !isStatUIOpen;
        statUI.SetActive(isStatUIOpen);

        if (isStatUIOpen)
        {
            LockCursor(false); // 마우스 보이게
        }
        else
        {
            LockCursor(true); // 마우스 숨기기
        }
    }

    /// <summary>
    /// 마우스 커서 잠금 및 해제
    /// </summary>
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
}
