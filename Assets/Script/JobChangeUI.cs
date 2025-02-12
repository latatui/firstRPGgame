using UnityEngine;
using UnityEngine.UI;

public class JobChangeUI : MonoBehaviour
{
    public Text jobChangeText;
    public Button yesButton;
    public Button noButton;

    public void ShowJobChangeUI(string jobDescription)
    {
        jobChangeText.text = jobDescription;
        gameObject.SetActive(true);
    }

    public void HideJobChangeUI()
    {
        gameObject.SetActive(false);
    }

    public void SetUpJobChangeButtons(System.Action yesAction, System.Action noAction)
    {
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => yesAction());

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() => noAction());
    }
}
