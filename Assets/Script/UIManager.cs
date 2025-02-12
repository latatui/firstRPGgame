using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text levelText;
    public Text healthText;
    public Text statPointsText;

    private PlayerStats playerStats;
    private HealthManager healthManager;

    void Start()
    {
        // 자동으로 PlayerStats와 HealthManager 찾기
        playerStats = FindObjectOfType<PlayerStats>();
        healthManager = FindObjectOfType<HealthManager>();

        if (playerStats == null)
            Debug.LogError("PlayerStats를 찾을 수 없습니다. Player 오브젝트가 씬에 있는지 확인하세요.");

        if (healthManager == null)
            Debug.LogError("HealthManager를 찾을 수 없습니다. HealthManager가 Player에 있는지 확인하세요.");
    }

    void Update()
    {
        if (playerStats != null && healthManager != null)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        levelText.text = "레벨: " + playerStats.level;
        healthText.text = "체력: " + healthManager.currentHealth + " / " + healthManager.maxHealth;
        statPointsText.text = "스탯 포인트: " + playerStats.statPoints;
    }
}
