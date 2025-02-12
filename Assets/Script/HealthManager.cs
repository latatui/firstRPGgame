using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;
    public PlayerStats playerStats;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // 게임 오버 처리 또는 플레이어 사망 처리
            Debug.Log("플레이어 사망");
        }
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // 체력 UI 업데이트
        Debug.Log("현재 체력: " + currentHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }
}
