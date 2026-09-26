using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private string destinationSpawnID = "";


    // =========================
    // EVENTS
    // =========================

    public event Action<int> OnGoldChanged;
    public event Action<int> OnTotalKillsChanged;
    public event Action<int> OnMaxHealthChanged;

    public event Action OnMap2Unlocked;
    public event Action OnMap3Unlocked;



    // =========================
    // CURRENCY
    // =========================

    [Header("Currency")]
    [SerializeField] private int goldNuggets = 0;


    // =========================
    // PLAYER
    // =========================

    [Header("Player")]
    [SerializeField] private int maxHealth = 3;


    // =========================
    // KILLS
    // =========================

    [Header("Kills")]
    [SerializeField] private int totalKills = 0;

    [SerializeField] private int map1Kills = 0;
    [SerializeField] private int map2Kills = 0;
    [SerializeField] private int map3Kills = 0;


    // =========================
    // MAP PROGRESSION
    // =========================

    [Header("Map Progression")]
    [SerializeField] private int map2UnlockKills = 20;
    [SerializeField] private int map3UnlockKills = 40;

    [SerializeField] private bool map2Unlocked = false;
    [SerializeField] private bool map3Unlocked = false;

    [SerializeField] private bool map3IntroSeen = false;


    // =========================
    // CRYSTALS
    // =========================

    [Header("Crystal Progress")]
    [SerializeField] private int crystalsCollected = 0;

    [SerializeField] private int nextCrystalKills = 5;


    // =========================
    // PLAYER UPGRADES
    // =========================

    [Header("Player Upgrades")]
    private HashSet<UpgradeType> ownedUpgrades =
        new HashSet<UpgradeType>();


    // =========================
    // RUN
    // =========================

    [Header("Run")]
    [SerializeField] private float runTime = 0f;

    [SerializeField] private int deathCount = 0;

    [SerializeField] private bool runStarted = false;
    [SerializeField] private bool runCompleted = false;

    private bool timerRunning = false;


    // =========================
    // SINGLETON
    // =========================

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


    private void Update()
    {
        if (timerRunning &&
            runStarted &&
            !runCompleted)
        {
            runTime += Time.unscaledDeltaTime;
        }
    }


    // =========================
    // GOLD
    // =========================

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


    // =========================
    // MAX HEALTH
    // =========================

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;

        OnMaxHealthChanged?.Invoke(maxHealth);

        Debug.Log(
            "Max Health: " +
            maxHealth
        );
    }


    public int GetMaxHealth()
    {
        return maxHealth;
    }


    // =========================
    // KILLS
    // =========================

    public void RegisterKill(int mapNumber)
    {
        totalKills++;

        switch (mapNumber)
        {
            case 1:
                map1Kills++;
                break;

            case 2:
                map2Kills++;
                break;

            case 3:
                map3Kills++;
                break;
        }

        OnTotalKillsChanged?.Invoke(totalKills);

        CheckMapUnlocks();

        Debug.Log(
            "Total Kills: " +
            totalKills
        );
    }


    private void CheckMapUnlocks()
    {
        if (!map2Unlocked &&
            totalKills >= map2UnlockKills)
        {
            map2Unlocked = true;

            Debug.Log("Map 2 Unlocked!");

            OnMap2Unlocked?.Invoke();
        }


        if (!map3Unlocked &&
            totalKills >= map3UnlockKills)
        {
            map3Unlocked = true;

            Debug.Log("Map 3 Unlocked!");

            OnMap3Unlocked?.Invoke();
        }
    }


    public int GetTotalKills()
    {
        return totalKills;
    }


    public int GetMapKills(int mapNumber)
    {
        switch (mapNumber)
        {
            case 1:
                return map1Kills;

            case 2:
                return map2Kills;

            case 3:
                return map3Kills;

            default:
                return 0;
        }
    }


    // =========================
    // MAP UNLOCKS
    // =========================

    public bool IsMap2Unlocked()
    {
        return map2Unlocked;
    }


    public bool IsMap3Unlocked()
    {
        return map3Unlocked;
    }


    // =========================
    // MAP 3 INTRO
    // =========================

    public bool HasSeenMap3Intro()
    {
        return map3IntroSeen;
    }


    public void MarkMap3IntroSeen()
    {
        map3IntroSeen = true;
    }


    // =========================
    // CRYSTALS
    // =========================

    public int GetCrystalsCollected()
    {
        return crystalsCollected;
    }


    public void RegisterCrystalCollected()
    {
        crystalsCollected++;
    }


    public int GetNextCrystalKills()
    {
        return nextCrystalKills;
    }


    public void SetNextCrystalKills(int amount)
    {
        nextCrystalKills = amount;
    }


    // =========================
    // PLAYER UPGRADES
    // =========================

    public void RegisterUpgrade(UpgradeType upgrade)
    {
        ownedUpgrades.Add(upgrade);
    }


    public bool HasUpgrade(UpgradeType upgrade)
    {
        return ownedUpgrades.Contains(upgrade);
    }


    public List<UpgradeType> GetOwnedUpgrades()
    {
        return new List<UpgradeType>(ownedUpgrades);
    }


    // =========================
    // RUN TIMER
    // =========================

    public void StartRun()
    {
        // The run begins when the player
        // enters Map 1 after the tutorial.
        if (runStarted)
            return;

        runStarted = true;
        runCompleted = false;
        timerRunning = true;

        Debug.Log("Run started!");
    }


    public void StartTimer()
    {
        if (runStarted)
            return;

        runStarted = true;
        timerRunning = true;
    }


    public void StopTimer()
    {
        timerRunning = false;
    }


    public float GetRunTime()
    {
        return runTime;
    }


    public bool HasRunStarted()
    {
        return runStarted;
    }


    // =========================
    // DEATHS
    // =========================

    public void RegisterDeath()
    {
        deathCount++;

        Debug.Log(
            "Deaths: " +
            deathCount
        );
    }


    public int GetDeathCount()
    {
        return deathCount;
    }


    // =========================
    // RUN COMPLETION
    // =========================

    public void CompleteRun()
    {
        if (runCompleted)
            return;

        runCompleted = true;
        timerRunning = false;

        Debug.Log(
            "Run completed!" +
            " | Time: " + runTime +
            " | Kills: " + totalKills +
            " | Crystals: " + crystalsCollected +
            " | Deaths: " + deathCount
        );
    }


    public bool IsRunCompleted()
    {
        return runCompleted;
    }


    // =========================
    // DESTINATION SPAWN
    // =========================

    public void SetDestinationSpawn(string spawnID)
    {
        destinationSpawnID = spawnID;
    }


    public string GetDestinationSpawn()
    {
        return destinationSpawnID;
    }


    public void ClearDestinationSpawn()
    {
        destinationSpawnID = "";
    }


    // =========================
    // NEW GAME / RESET RUN
    // =========================

    public void ResetRun()
    {
        // Currency
        goldNuggets = 0;

        // Player
        maxHealth = 3;

        // Kills
        totalKills = 0;
        map1Kills = 0;
        map2Kills = 0;
        map3Kills = 0;

        // Map progression
        map2Unlocked = false;
        map3Unlocked = false;
        map3IntroSeen = false;

        // Crystals
        crystalsCollected = 0;
        nextCrystalKills = 5;

        // Upgrades
        ownedUpgrades.Clear();

        // Run
        runTime = 0f;
        deathCount = 0;
        runStarted = false;
        runCompleted = false;
        timerRunning = false;

        // Spawn destination
        destinationSpawnID = "";

        // Update anything currently listening
        // to these values.
        OnGoldChanged?.Invoke(goldNuggets);
        OnTotalKillsChanged?.Invoke(totalKills);
        OnMaxHealthChanged?.Invoke(maxHealth);

        Debug.Log("Run reset.");
    }
}