using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action<int> OnGoldChanged;

    [Header("Currency")]
    [SerializeField] private int goldNuggets = 0;

    private void Awake()
    {
        if (Instance != null &&
            Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void AddGold(int amount)
    {
        goldNuggets += amount;

        OnGoldChanged?.Invoke(goldNuggets);

        Debug.Log(
            "Gold Nuggets: " +
            goldNuggets
        );
    }

    public bool SpendGold(int amount)
    {
        if (goldNuggets < amount)
            return false;

        goldNuggets -= amount;

        OnGoldChanged?.Invoke(goldNuggets);

        Debug.Log(
            "Gold Nuggets: " +
            goldNuggets
        );

        return true;
    }

    public int GetGold()
    {
        return goldNuggets;
    }
}