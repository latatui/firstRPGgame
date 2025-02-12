using UnityEngine;
using UnityEngine.UI;

public class StatButton : MonoBehaviour
{
    public string statName;
    public PlayerStats playerStats;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        playerStats.IncreaseStat(statName);
    }
}
