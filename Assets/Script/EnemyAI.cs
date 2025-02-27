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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
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
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(player);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && player.GetComponent<PlayerController>().isAttacking)
        {
            TakeDamage(playerStats.attackPower);
        }
    }
}
