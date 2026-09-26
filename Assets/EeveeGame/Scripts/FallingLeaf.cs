using UnityEngine;

public class FallingLeaf : MonoBehaviour
{
    [Header("Falling")]
    [SerializeField] private float minFallSpeed = 0.8f;
    [SerializeField] private float maxFallSpeed = 1.5f;

    [Header("Horizontal Drift")]
    [SerializeField] private float minDriftSpeed = -0.25f;
    [SerializeField] private float maxDriftSpeed = 0.25f;

    [Header("Sway")]
    [SerializeField] private float minSwayAmount = 0.1f;
    [SerializeField] private float maxSwayAmount = 0.35f;

    [SerializeField] private float minSwaySpeed = 1f;
    [SerializeField] private float maxSwaySpeed = 2.5f;

    [Header("Rotation")]
    [SerializeField] private float minRotationSpeed = -40f;
    [SerializeField] private float maxRotationSpeed = 40f;

    private float fallSpeed;
    private float driftSpeed;
    private float swayAmount;
    private float swaySpeed;
    private float rotationSpeed;

    private float lifeTime;

    private void Start()
    {
        fallSpeed = Random.Range(
            minFallSpeed,
            maxFallSpeed
        );

        driftSpeed = Random.Range(
            minDriftSpeed,
            maxDriftSpeed
        );

        swayAmount = Random.Range(
            minSwayAmount,
            maxSwayAmount
        );

        swaySpeed = Random.Range(
            minSwaySpeed,
            maxSwaySpeed
        );

        rotationSpeed = Random.Range(
            minRotationSpeed,
            maxRotationSpeed
        );
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        lifeTime += deltaTime;

        // Calculate sway.
        float sway =
            Mathf.Sin(lifeTime * swaySpeed) *
            swayAmount;

        // Combine falling, drifting and sway
        // into a single movement.
        Vector3 position = transform.position;

        position.x +=
            (driftSpeed + sway) * deltaTime;

        position.y -=
            fallSpeed * deltaTime;

        transform.position = position;

        // Rotate leaf.
        transform.Rotate(
            0f,
            0f,
            rotationSpeed * deltaTime
        );
    }
}