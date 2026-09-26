using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    [Header("Crystal")]
    [SerializeField] private GameObject crystalPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Crystal Progress")]
    [SerializeField] private int firstCrystalKills = 5;
    [SerializeField] private int killsPerCrystal = 3;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip crystalSpawnSound;

    [Header("UI")]
    [SerializeField] private CrystalArrowUI crystalArrowUI;
    [SerializeField] private UpgradeUI upgradeUI;

    private int currentKills = 0;
    private int nextCrystalKills;
    private bool crystalSpawned = false;
    private GameObject activeCrystal;


    private void Start()
    {
        nextCrystalKills = firstCrystalKills;
    }

    public void RegisterKill()
    {
        currentKills++;

        Debug.Log(
            "Crystal progress: " +
            currentKills +
            "/" +
            nextCrystalKills
        );

        CheckCrystalSpawn();
    }

    private void CheckCrystalSpawn()
    {
        // Don't spawn another crystal while one
        // is already waiting to be collected.
        if (crystalSpawned)
            return;

        if (currentKills >= nextCrystalKills)
        {
            SpawnCrystal();
        }
    }


    private void SpawnCrystal()
    {
        if (crystalPrefab == null)
        {
            Debug.LogWarning(
                "CrystalManager: Crystal prefab is missing."
            );

            return;
        }

        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogWarning(
                "CrystalManager: No crystal spawn points assigned."
            );

            return;
        }

        int randomIndex =
            Random.Range(0, spawnPoints.Length);

        Transform chosenPoint =
            spawnPoints[randomIndex];

        activeCrystal = Instantiate(
            crystalPrefab,
            chosenPoint.position,
            Quaternion.identity
        );

        UpgradeCrystal crystal =
            activeCrystal.GetComponent<UpgradeCrystal>();

        if (crystal != null)
        {
            crystal.Setup(this);
        }

        if (crystalArrowUI != null)
        {
            crystalArrowUI.SetTarget(
                activeCrystal.transform
            );
        }

        crystalSpawned = true;

        if (audioSource != null &&
            crystalSpawnSound != null)
        {
            audioSource.PlayOneShot(
                crystalSpawnSound
            );
        }

        Debug.Log(
            "Crystal spawned at: " +
            chosenPoint.name
        );
    }

    public void CollectCrystal()
    {
        if (activeCrystal == null)
            return;

        Debug.Log("Upgrade crystal collected!");

        if (crystalArrowUI != null)
        {
            crystalArrowUI.ClearTarget();
        }

        activeCrystal = null;
        crystalSpawned = false;

        // Increase the requirement for the next crystal.
        nextCrystalKills += killsPerCrystal;

        Debug.Log(
            "Next crystal unlocks at " +
            nextCrystalKills +
            " total kills."
        );

        if (upgradeUI != null)
        {
            upgradeUI.Open();
        }

        // The player may already have enough kills
        // for the next crystal.
        CheckCrystalSpawn();

        // Upgrade menu will open here later.
    }


    public GameObject GetActiveCrystal()
    {
        return activeCrystal;
    }
}