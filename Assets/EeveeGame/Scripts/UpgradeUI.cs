using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject upgradePanel;

    [Header("Choices")]
    [SerializeField] private Button choice1Button;
    [SerializeField] private Button choice2Button;

    [SerializeField] private TMP_Text choice1Name;
    [SerializeField] private TMP_Text choice1Description;

    [SerializeField] private TMP_Text choice2Name;
    [SerializeField] private TMP_Text choice2Description;

    [Header("Confirm")]
    [SerializeField] private Button confirmButton;

    [Header("Player")]
    [SerializeField] private PlayerStats playerStats;

    private UpgradeOption choice1;
    private UpgradeOption choice2;

    private int selectedChoice = -1;

    private void Start()
    {

        PlayerStats currentPlayerStats =
            FindFirstObjectByType<PlayerStats>();

        if (currentPlayerStats != null)
        {
            playerStats = currentPlayerStats;
        }
        else
        {
            Debug.LogError(
                "UpgradeUI: Could not find PlayerStats!"
            );
        }
    
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        if (choice1Button != null)
        {
            choice1Button.onClick.AddListener(
                () => SelectChoice(0)
            );
        }

        if (choice2Button != null)
        {
            choice2Button.onClick.AddListener(
                () => SelectChoice(1)
            );
        }

        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(
                ConfirmSelection
            );
        }
    }

    private bool IsUpgradeAvailable(
        UpgradeType upgrade
    )
    {
        if (playerStats.HasUpgrade(upgrade))
            return false;

        switch (upgrade)
        {
            // Tier II requires Tier I.

            case UpgradeType.QuickFeet2:
                return playerStats.HasUpgrade(
                    UpgradeType.QuickFeet1
                );

            case UpgradeType.BetterHealing2:
                return playerStats.HasUpgrade(
                    UpgradeType.BetterHealing1
                );

            case UpgradeType.ExtraJump2:
                return playerStats.HasUpgrade(
                    UpgradeType.ExtraJump1
                );

            case UpgradeType.SwiftPower2:
                return playerStats.HasUpgrade(
                    UpgradeType.SwiftPower1
                );

            case UpgradeType.SwiftSpeed2:
                return playerStats.HasUpgrade(
                    UpgradeType.SwiftSpeed1
                );

            case UpgradeType.SwiftDuration2:
                return playerStats.HasUpgrade(
                    UpgradeType.SwiftDuration1
                );

            case UpgradeType.QuickDash2:
                return playerStats.HasUpgrade(
                    UpgradeType.QuickDash1
                );


            // Tier III requires Tier II.

            case UpgradeType.QuickFeet3:
                return playerStats.HasUpgrade(
                    UpgradeType.QuickFeet2
                );

            case UpgradeType.SwiftPower3:
                return playerStats.HasUpgrade(
                    UpgradeType.SwiftPower2
                );

            case UpgradeType.SwiftSpeed3:
                return playerStats.HasUpgrade(
                    UpgradeType.SwiftSpeed2
                );

            case UpgradeType.SwiftDuration3:
                return playerStats.HasUpgrade(
                    UpgradeType.SwiftDuration2
                );


            // Tier IV requires Tier III.

            case UpgradeType.SwiftPower4:
                return playerStats.HasUpgrade(
                    UpgradeType.SwiftPower3
                );


            // Homing requires Swift Aim.

            case UpgradeType.HomingSwift:
                return playerStats.HasUpgrade(
                    UpgradeType.SwiftAim
                );
        }

        // Tier I / standalone upgrade.
        return true;
    }

    private List<UpgradeOption> GetAllUpgrades()
    {
        return new List<UpgradeOption>()
        {
            new UpgradeOption(
                UpgradeType.QuickFeet1,
                "Quick Feet I",
                "+10% movement speed"
            ),

            new UpgradeOption(
                UpgradeType.QuickFeet2,
                "Quick Feet II",
                "+10% movement speed"
            ),

            new UpgradeOption(
                UpgradeType.QuickFeet3,
                "Quick Feet III",
                "+10% movement speed"
            ),


            new UpgradeOption(
                UpgradeType.BetterHealing1,
                "Better Healing I",
                "Flowers heal +1 heart"
            ),

            new UpgradeOption(
                UpgradeType.BetterHealing2,
                "Better Healing II",
                "Flowers heal +1 heart"
            ),


            new UpgradeOption(
                UpgradeType.ExtraJump1,
                "Extra Jump I",
                "+1 mid-air jump"
            ),

            new UpgradeOption(
                UpgradeType.ExtraJump2,
                "Extra Jump II",
                "+1 mid-air jump"
            ),


            new UpgradeOption(
                UpgradeType.SwiftPower1,
                "Swift Power I",
                "+1 Swift damage"
            ),

            new UpgradeOption(
                UpgradeType.SwiftPower2,
                "Swift Power II",
                "+1 Swift damage"
            ),

            new UpgradeOption(
                UpgradeType.SwiftPower3,
                "Swift Power III",
                "+1 Swift damage"
            ),

            new UpgradeOption(
                UpgradeType.SwiftPower4,
                "Swift Power IV",
                "+1 Swift damage"
            ),


            new UpgradeOption(
                UpgradeType.SwiftAim,
                "Swift Aim",
                "Aim Swift toward the mouse"
            ),

            new UpgradeOption(
                UpgradeType.HomingSwift,
                "Homing Swift",
                "Swift curves toward nearby enemies"
            ),


            new UpgradeOption(
                UpgradeType.SwiftSpeed1,
                "Swift Speed I",
                "+20% projectile speed"
            ),

            new UpgradeOption(
                UpgradeType.SwiftSpeed2,
                "Swift Speed II",
                "+20% projectile speed"
            ),

            new UpgradeOption(
                UpgradeType.SwiftSpeed3,
                "Swift Speed III",
                "+20% projectile speed"
            ),


            new UpgradeOption(
                UpgradeType.SwiftDuration1,
                "Swift Duration I",
                "+25% projectile duration"
            ),

            new UpgradeOption(
                UpgradeType.SwiftDuration2,
                "Swift Duration II",
                "+25% projectile duration"
            ),

            new UpgradeOption(
                UpgradeType.SwiftDuration3,
                "Swift Duration III",
                "+25% projectile duration"
            ),


            new UpgradeOption(
                UpgradeType.QuickDash1,
                "Quick Dash I",
                "-20% dash cooldown"
            ),

            new UpgradeOption(
                UpgradeType.QuickDash2,
                "Quick Dash II",
                "-20% dash cooldown"
            )
        };
    }

    private void GenerateChoices()
    {
        List<UpgradeOption> available =
            new List<UpgradeOption>();

        foreach (
            UpgradeOption upgrade
            in GetAllUpgrades()
        )
        {
            if (IsUpgradeAvailable(upgrade.type))
            {
                available.Add(upgrade);
            }
        }

        if (available.Count < 2)
        {
            Debug.LogWarning(
                "Not enough upgrades remaining!"
            );

            return;
        }

        int firstIndex =
            Random.Range(0, available.Count);

        choice1 =
            available[firstIndex];

        available.RemoveAt(firstIndex);

        int secondIndex =
            Random.Range(0, available.Count);

        choice2 =
            available[secondIndex];
    }

    public void Open()
    {
        selectedChoice = -1;

        GenerateChoices();

        choice1Name.text =
            choice1.displayName;

        choice1Description.text =
            choice1.description;

        choice2Name.text =
            choice2.displayName;

        choice2Description.text =
            choice2.description;

        upgradePanel.SetActive(true);

        confirmButton.interactable = false;

        Time.timeScale = 0f;
    }

    private void SelectChoice(int choice)
    {
        selectedChoice = choice;

        confirmButton.interactable = true;

        Debug.Log(
            "Selected upgrade choice: " +
            (choice + 1)
        );
    }

    private void ConfirmSelection()
    {
        if (selectedChoice == -1)
            return;

        UpgradeOption selectedUpgrade =
            selectedChoice == 0
            ? choice1
            : choice2;
        
        if (playerStats == null)
        {
            playerStats =
                FindFirstObjectByType<PlayerStats>();

            if (playerStats == null)
            {
                Debug.LogError(
                    "UpgradeUI: PlayerStats not found."
                );

                return;
            }
        }

        playerStats.ApplyUpgrade(
            selectedUpgrade.type
        );

        Debug.Log(
            "Confirmed upgrade: " +
            selectedUpgrade.displayName
        );

        upgradePanel.SetActive(false);

        Time.timeScale = 1f;
    }
}