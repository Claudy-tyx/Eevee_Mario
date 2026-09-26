using UnityEngine;

public class UpgradeCrystal : MonoBehaviour
{
    private CrystalManager crystalManager;
    private bool collected = false;

    public void Setup(CrystalManager manager)
    {
        crystalManager = manager;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        collected = true;

        if (crystalManager != null)
        {
            crystalManager.CollectCrystal();
        }

        Destroy(gameObject);
    }
}