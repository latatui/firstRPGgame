using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public int statPoints = 0;
    
    public int strength = 0;
    public int agility = 0;
    public int defense = 0;
    public int currentHealth;
    public int maxHealth = 100;
    public float criticalChance = 0f;
    public float criticalDamage = 1.5f;
    public float attackMultiplier = 1f; // 기본 공격력 배율

    public string currentJob = "초보자";

    public GameObject statPanel;
    public GameObject jobChangePanel; // 전직 UI
    public Text jobChangeText;
    public Button jobYesButton;
    public Button jobNoButton;

    public Text strengthText, agilityText, defenseText, healthText, critChanceText, critDamageText, statPointText, jobText;

    private UIManager uiManager; // UIManager 참조
    private bool isStatPanelOpen = false;

    void Start()
    {
        UpdateStatUI();
        currentHealth = maxHealth;
        jobChangePanel.SetActive(false);
        uiManager = FindObjectOfType<UIManager>(); // UIManager 찾기

        if (uiManager == null)
            Debug.LogError("UIManager를 찾을 수 없습니다!");

        jobYesButton.onClick.AddListener(() => ChangeJob("전사"));
        jobNoButton.onClick.AddListener(() => jobChangePanel.SetActive(false));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleStatPanel();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isStatPanelOpen)
        {
            ToggleStatPanel();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            LevelUp();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(25);
        }
    }

    void ToggleStatPanel()
    {
        isStatPanelOpen = !isStatPanelOpen;
        statPanel.SetActive(isStatPanelOpen);
    }

    void LevelUp()
    {
        level++;
        statPoints += 3;
        UpdateStatUI();
    }

    public void IncreaseStat(string statName)
    {
        if (statPoints <= 0) return;

        switch (statName)
        {
            case "Strength":
                strength++;
                break;
            case "Agility":
                agility++;
                break;
            case "Defense":
                defense++;
                break;
            case "Health":
                maxHealth += 10;
                currentHealth += 10;
                break;
            case "CritChance":
                criticalChance += 1f;
                break;
            case "CritDamage":
                criticalDamage += 0.1f;
                break;
        }

        statPoints--;
        UpdateStatUI();
        CheckForJobChange();
    }

    void CheckForJobChange()
    {
        if (strength >= 3 && currentJob == "초보자")
        {
            ToggleStatPanel();
            jobChangePanel.SetActive(true);
            jobChangeText.text = "전사로 전직하시겠습니까?";
        }
    }

    void ChangeJob(string newJob)
    {
        currentJob = newJob;
        attackMultiplier += 0.05f; // 5% 추가 공격력
        jobChangePanel.SetActive(false);
        UpdateStatUI();
    }

    void UpdateStatUI()
    {
        strengthText.text = "힘: " + strength;
        agilityText.text = "민첩: " + agility;
        defenseText.text = "방어: " + defense;
        healthText.text = "체력: " + maxHealth;
        critChanceText.text = "치명타 확률: " + criticalChance + "%";
        critDamageText.text = "치명타 데미지: " + criticalDamage + "x";
        statPointText.text = "남은 스탯 포인트: " + statPoints;
        jobText.text = "직업: " + currentJob;
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
        uiManager.UpdateUI();
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
