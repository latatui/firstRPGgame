using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text levelText;
    public Text healthText;
    public Slider hp_slider;
    public Text statPointsText;

    private PlayerStats playerStats;

    void Start()
    {
        // 자동으로 PlayerStats와 HealthManager 찾기
        playerStats = FindObjectOfType<PlayerStats>();
        hp_slider.maxValue = playerStats.maxHealth;

        if (playerStats == null)
            Debug.LogError("PlayerStats를 찾을 수 없습니다. Player 오브젝트가 씬에 있는지 확인하세요.");
    }

    void Update()
    {
        if (playerStats != null)
        {
            UpdateUI();
            CheckHP();
        }
    }

    public void UpdateUI()
    {
        levelText.text = "레벨: " + playerStats.level;
        healthText.text = "체력: " + playerStats.currentHealth + " / " + playerStats.maxHealth;
        statPointsText.text = "스탯 포인트: " + playerStats.statPoints;
    }
    private void CheckHP()//* HP 게이지 변경 
    { 
        hp_slider.maxValue = playerStats.maxHealth;//슬라이더의 최대값을 스텟의 최대체력으로 지정
        hp_slider.value = playerStats.currentHealth;//슬라이더의 값을 스텟의 체력으로 지정

    }
}
