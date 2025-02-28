using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public int health = 100;
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public int damage = 10;
    public float attackCooldown = 1.5f;
    public int expReward = 10;

    private Transform player;
    private bool isAttacking = false;
    private Animator animator;
    private PlayerStats playerStats;

    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 물리적 충돌 방지
    }


    void Update()
    {
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else if (!isAttacking)
        {
            StartCoroutine(Attack());
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

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            playerStats.TakeDamage(damage);
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
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
