using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void Start()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.OnGoldChanged += UpdateGold;

        UpdateGold(
            GameManager.Instance.GetGold()
        );
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGoldChanged -= UpdateGold;
        }
    }

    private void UpdateGold(int amount)
    {
        if (goldText != null)
        {
            goldText.text =
                ": " + amount;
        }
    }
}