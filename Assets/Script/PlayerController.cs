using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;
    public Transform playerCamera;
    public GameObject statUI;

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
        if (rb == null)
        {
            Debug.LogError("Rigidbody가 없습니다. Player 오브젝트에 Rigidbody를 추가하세요!");
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
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
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void Attack()
    {
        float attackDamage = 10f * playerStats.attackMultiplier;
        Debug.Log("공격! 데미지: " + attackDamage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

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
