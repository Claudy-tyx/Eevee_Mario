using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeedMultiplier = 1f;

    [Header("Healing")]
    [SerializeField] private int flowerHealAmount = 1;

    [Header("Jump")]
    [SerializeField] private int extraJumps = 0;

    [Header("Swift")]
    [SerializeField] private int swiftDamage = 1;
    [SerializeField] private float swiftSpeedMultiplier = 1f;
    [SerializeField] private float swiftLifetimeMultiplier = 1f;

    [Header("Swift Abilities")]
    [SerializeField] private bool directionalSwift = false;
    [SerializeField] private bool homingSwift = false;

    [Header("Dash")]
    [SerializeField] private float dashCooldownMultiplier = 1f;

    private HashSet<UpgradeType> ownedUpgrades =
        new HashSet<UpgradeType>();


    private void Awake()
    {
        LoadPersistentUpgrades();
    }


    private void LoadPersistentUpgrades()
    {
        if (GameManager.Instance == null)
            return;

        List<UpgradeType> savedUpgrades =
            GameManager.Instance.GetOwnedUpgrades();

        foreach (UpgradeType upgrade in savedUpgrades)
        {
            ApplyUpgrade(upgrade, false);
        }
    }


    // ---------- GETTERS ----------

    public float GetMoveSpeedMultiplier()
    {
        return moveSpeedMultiplier;
    }

    public int GetFlowerHealAmount()
    {
        return flowerHealAmount;
    }

    public int GetExtraJumps()
    {
        return extraJumps;
    }

    public int GetSwiftDamage()
    {
        return swiftDamage;
    }

    public float GetSwiftSpeedMultiplier()
    {
        return swiftSpeedMultiplier;
    }

    public float GetSwiftLifetimeMultiplier()
    {
        return swiftLifetimeMultiplier;
    }

    public bool HasDirectionalSwift()
    {
        return directionalSwift;
    }

    public bool HasHomingSwift()
    {
        return homingSwift;
    }

    public float GetDashCooldownMultiplier()
    {
        return dashCooldownMultiplier;
    }


    public bool HasUpgrade(UpgradeType upgrade)
    {
        return ownedUpgrades.Contains(upgrade);
    }

    public void ApplyUpgrade(
        UpgradeType upgrade,
        bool saveToGameManager = true
    )
    {
        // Never apply the exact same upgrade twice.
        if (ownedUpgrades.Contains(upgrade))
            return;

        ownedUpgrades.Add(upgrade);

        if (saveToGameManager &&
            GameManager.Instance != null)
        {
            GameManager.Instance.RegisterUpgrade(upgrade);
        }

        switch (upgrade)
        {
            case UpgradeType.QuickFeet1:
            case UpgradeType.QuickFeet2:
            case UpgradeType.QuickFeet3:
                UpgradeMovementSpeed();
                break;


            case UpgradeType.BetterHealing1:
            case UpgradeType.BetterHealing2:
                UpgradeHealing();
                break;


            case UpgradeType.ExtraJump1:
            case UpgradeType.ExtraJump2:
                UpgradeExtraJump();
                break;


            case UpgradeType.SwiftPower1:
            case UpgradeType.SwiftPower2:
            case UpgradeType.SwiftPower3:
            case UpgradeType.SwiftPower4:
                UpgradeSwiftDamage();
                break;


            case UpgradeType.SwiftAim:
                UnlockDirectionalSwift();
                break;


            case UpgradeType.HomingSwift:
                UnlockHomingSwift();
                break;


            case UpgradeType.SwiftSpeed1:
            case UpgradeType.SwiftSpeed2:
            case UpgradeType.SwiftSpeed3:
                UpgradeSwiftSpeed();
                break;


            case UpgradeType.SwiftDuration1:
            case UpgradeType.SwiftDuration2:
            case UpgradeType.SwiftDuration3:
                UpgradeSwiftLifetime();
                break;


            case UpgradeType.QuickDash1:
            case UpgradeType.QuickDash2:
                UpgradeDashCooldown();
                break;
        }

        Debug.Log(
            "Upgrade obtained: " + upgrade
        );
    }


    // ---------- UPGRADES ----------

    public void UpgradeMovementSpeed()
    {
        moveSpeedMultiplier += 0.1f;
    }

    public void UpgradeHealing()
    {
        flowerHealAmount += 1;
    }

    public void UpgradeExtraJump()
    {
        extraJumps += 1;
    }

    public void UpgradeSwiftDamage()
    {
        swiftDamage += 1;
    }

    public void UnlockDirectionalSwift()
    {
        directionalSwift = true;
    }

    public void UnlockHomingSwift()
    {
        homingSwift = true;
    }

    public void UpgradeSwiftSpeed()
    {
        swiftSpeedMultiplier += 0.2f;
    }

    public void UpgradeSwiftLifetime()
    {
        swiftLifetimeMultiplier += 0.25f;
    }

    public void UpgradeDashCooldown()
    {
        dashCooldownMultiplier -= 0.2f;

        dashCooldownMultiplier =
            Mathf.Max(dashCooldownMultiplier, 0.5f);
    }
}