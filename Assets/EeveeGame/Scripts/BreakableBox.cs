using System.Collections;
using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int hitsToBreak = 3;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hitSprite;

    [Header("Hit Effect")]
    [SerializeField] private float hitDelay = 0.5f;
    [SerializeField] private float hitSpriteDuration = 0.15f;

    [Header("Drop")]
    [SerializeField] private GameObject goldNuggetPrefab;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip sparkleSound;
    [SerializeField] private float sparkleDelay = 0.15f;

    private int currentHits = 0;
    private bool isBroken = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D boxCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider2D>();

        if (normalSprite != null)
            spriteRenderer.sprite = normalSprite;
    }

    public void Hit()
    {
        if (isBroken)
            return;

        currentHits++;

        Debug.Log(
            "Box hit: " +
            currentHits +
            "/" +
            hitsToBreak
        );

        StartCoroutine(DelayedHit());
    }

    private IEnumerator DelayedHit()
    {
        // Wait until Eevee's Tackle reaches the impact point.
        yield return new WaitForSeconds(hitDelay);

        if (currentHits >= hitsToBreak)
        {
            Break();
        }
        else
        {
            spriteRenderer.sprite = hitSprite;

            yield return new WaitForSeconds(hitSpriteDuration);

            spriteRenderer.sprite = normalSprite;
        }
    }

    private IEnumerator ShowHitSprite()
    {
        spriteRenderer.sprite = hitSprite;

        yield return new WaitForSeconds(hitSpriteDuration);

        spriteRenderer.sprite = normalSprite;
    }

    private void Break()
    {
        if (isBroken)
            return;

        isBroken = true;

        // Play box breaking sound.
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(
                breakSound,
                transform.position
            );
        }

        // Spawn Gold Nugget.
        if (goldNuggetPrefab != null)
        {
            Instantiate(
                goldNuggetPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // Play sparkle shortly after the box breaks.
        if (sparkleSound != null)
        {
            StartCoroutine(PlaySparkleAndDestroy());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator PlaySparkleAndDestroy()
    {
        yield return new WaitForSeconds(sparkleDelay);

        AudioSource.PlayClipAtPoint(
            sparkleSound,
            transform.position
        );

        Destroy(gameObject);
    }
}