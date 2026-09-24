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

    private int displayedHealth = 3;


    public void SetHealth(int health)
    {
        int oldHealth = displayedHealth;
        displayedHealth = health;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].text = i < health ? "♥" : "♡";
        }

        // Animate the heart that changed.
        if (health < oldHealth)
        {
            int lostHeart = oldHealth - 1;

            if (lostHeart >= 0 && lostHeart < hearts.Length)
                StartCoroutine(AnimateHeart(hearts[lostHeart]));
        }
        else if (health > oldHealth)
        {
            int gainedHeart = health - 1;

            if (gainedHeart >= 0 && gainedHeart < hearts.Length)
                StartCoroutine(AnimateHeart(hearts[gainedHeart]));
        }
    }


    private IEnumerator AnimateHeart(TMP_Text heart)
    {
        heart.transform.localScale =
            Vector3.one * effectScale;

        yield return new WaitForSeconds(animationTime);

        heart.transform.localScale =
            Vector3.one * normalScale;
    }
}