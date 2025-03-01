using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public int health = 100;
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public int damage = 10;
    public float attackCooldown = 1.5f;
    public int expReward = 10;
    public Slider healthBar; // 체력바 UI

    private Transform player;
    private bool isAttacking = false;
    private Animator animator;
    private PlayerStats playerStats;
    private Rigidbody rb;
    private float maxHealth = 50;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 물리적 충돌 방지
        UpdateHealthBar();
    }

    void Update()
    {
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange && !isAttacking)
        {
            MoveTowardsPlayer();
        }
        else if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        else if (distance <= attackRange && isAttacking)
        {
            animator.SetBool("isMoving", false);
        }
    }

    void MoveTowardsPlayer()
    {
        animator.SetBool("isMoving", true);
        Vector3 direction = (player.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerStats.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(1f); // 공격 후 1초 멈춤
        yield return new WaitForSeconds(attackCooldown - 1f);
        isAttacking = false;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        UpdateHealthBar();
        if (health <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = health / maxHealth;
        }
    }

    void Die()
    {
        playerStats.GainExp(expReward);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null && playerController.isAttacking)
            {
                TakeDamage(playerStats.attackPower);
            }
        }
    }
}
