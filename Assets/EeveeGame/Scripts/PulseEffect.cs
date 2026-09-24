using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [Header("Pulse Size")]
    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.1f;

    [Header("Pulse Visibility")]
    [SerializeField] private float minAlpha = 0.5f;
    [SerializeField] private float maxAlpha = 0.9f;

    [Header("Speed")]
    [SerializeField] private float pulseSpeed = 2f;

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        // Pulse size
        float scale = Mathf.Lerp(minScale, maxScale, pulse);

        transform.localScale = new Vector3(
            originalScale.x * scale,
            originalScale.y * scale,
            originalScale.z
        );

        // Pulse transparency
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;

            color.a = Mathf.Lerp(
                minAlpha,
                maxAlpha,
                pulse
            );

            spriteRenderer.color = color;
        }
    }
}