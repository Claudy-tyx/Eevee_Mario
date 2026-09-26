using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPortal : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private string destinationScene;
    [SerializeField] private string destinationSpawnID;

    [Header("Unlock")]
    [SerializeField] private bool requiresMap2Unlock = false;
    [SerializeField] private bool requiresMap3Unlock = false;

    [Header("Visuals")]
    [SerializeField] private GameObject lockedVisual;
    [SerializeField] private GameObject unlockedVisual;

    [Header("Run")]
    [SerializeField] private bool startsRun = false;

    

    private bool isLoading = false;


    private void Start()
    {
        UpdateVisual();
    }


    private void OnEnable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.OnMap2Unlocked += HandleMap2Unlocked;
        GameManager.Instance.OnMap3Unlocked += HandleMap3Unlocked;
    }


    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.OnMap2Unlocked -= HandleMap2Unlocked;
        GameManager.Instance.OnMap3Unlocked -= HandleMap3Unlocked;
    }


    private void HandleMap2Unlocked()
    {
        UpdateVisual();
    }


    private void HandleMap3Unlocked()
    {
        UpdateVisual();
    }


    private void UpdateVisual()
    {
        bool unlocked = CanUsePortal();

        if (lockedVisual != null)
        {
            lockedVisual.SetActive(!unlocked);
        }

        if (unlockedVisual != null)
        {
            unlockedVisual.SetActive(unlocked);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLoading)
            return;

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        if (!CanUsePortal())
        {
            Debug.Log("Portal is still locked.");
            return;
        }

        LoadDestination();
    }


    private bool CanUsePortal()
    {
        if (GameManager.Instance == null)
            return false;

        if (requiresMap2Unlock &&
            !GameManager.Instance.IsMap2Unlocked())
        {
            return false;
        }

        if (requiresMap3Unlock &&
            !GameManager.Instance.IsMap3Unlocked())
        {
            return false;
        }

        return true;
    }


    private void LoadDestination()
    {
        if (string.IsNullOrEmpty(destinationScene))
        {
            Debug.LogWarning(
                gameObject.name +
                ": No destination scene assigned."
            );

            return;
        }

        isLoading = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetDestinationSpawn(
                destinationSpawnID
            );

            if (startsRun)
            {
                GameManager.Instance.StartRun();
            }
        }

        Time.timeScale = 1f;

        SceneManager.LoadScene(destinationScene);
    }
}