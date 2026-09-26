using System.Collections;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] hearts;

    [Header("Animation")]
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float effectScale = 1.4f;

    [Header("Sound")]
    [SerializeField] private AudioClip healSound;
    [SerializeField] private float healSoundVolume = 1f;

    private int displayedHealth = 3;
    private int displayedMaxHealth = 3;


    public void SetHealth(int health)
    {
        SetHealth(health, displayedMaxHealth);
    }


    public void SetHealth(int health, int maxHealth)
    {
        int oldHealth = displayedHealth;
        int oldMaxHealth = displayedMaxHealth;

        displayedHealth = health;
        displayedMaxHealth = maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            // Hearts beyond Eevee's current max HP
            // should not be visible at all.
            bool unlocked = i < maxHealth;

            hearts[i].gameObject.SetActive(unlocked);

            if (unlocked)
            {
                hearts[i].text =
                    i < health ? "♥" : "♡";
            }
        }

        // Animate a newly unlocked max heart.
        if (maxHealth > oldMaxHealth)
        {
            int newHeart = maxHealth - 1;

            if (newHeart >= 0 &&
                newHeart < hearts.Length)
            {
                StartCoroutine(
                    AnimateHeart(hearts[newHeart])
                );
            }

            PlayHealSound();
            return;
        }

        // Normal damage.
        if (health < oldHealth)
        {
            int lostHeart = oldHealth - 1;

            if (lostHeart >= 0 &&
                lostHeart < hearts.Length)
            {
                StartCoroutine(
                    AnimateHeart(hearts[lostHeart])
                );
            }
        }

        // Normal healing.
        else if (health > oldHealth)
        {
            int gainedHeart = health - 1;

            if (gainedHeart >= 0 &&
                gainedHeart < hearts.Length)
            {
                StartCoroutine(
                    AnimateHeart(hearts[gainedHeart])
                );
            }

            PlayHealSound();
        }
    }


    private void PlayHealSound()
    {
        if (healSound != null &&
            Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(
                healSound,
                Camera.main.transform.position,
                healSoundVolume
            );
        }
    }


    private IEnumerator AnimateHeart(TMP_Text heart)
    {
        heart.transform.localScale =
            Vector3.one * effectScale;

        yield return new WaitForSeconds(
            animationTime
        );

        heart.transform.localScale =
            Vector3.one * normalScale;
    }
}