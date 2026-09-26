using UnityEngine;

public class WaterShimmer : MonoBehaviour
{
    [Header("Shimmer Pieces")]
    [SerializeField] private Transform shimmerA;
    [SerializeField] private Transform shimmerB;

    [Header("Movement")]
    [SerializeField] private float horizontalSpeed = 0.15f;

    [Header("Vertical Bob")]
    [SerializeField] private float verticalAmount = 0.03f;
    [SerializeField] private float verticalSpeed = 1f;

    private float spriteWidth;

    private float startYA;
    private float startYB;


    private void Start()
    {
        if (shimmerA == null || shimmerB == null)
            return;

        SpriteRenderer renderer =
            shimmerA.GetComponent<SpriteRenderer>();

        if (renderer == null)
            return;

        // Get the actual displayed width.
        spriteWidth = renderer.bounds.size.x;

        startYA = shimmerA.localPosition.y;
        startYB = shimmerA.localPosition.y;

        // A stays where you placed it.
        // B starts directly beside A.
        shimmerB.position = new Vector3(
            shimmerA.position.x - spriteWidth,
            shimmerA.position.y,
            shimmerA.position.z
        );
    }


    private void Update()
    {
        if (shimmerA == null || shimmerB == null)
            return;

        float movement =
            horizontalSpeed * Time.deltaTime;

        // Move both pieces horizontally.
        shimmerA.position += Vector3.right * movement;
        shimmerB.position += Vector3.right * movement;


        // Small vertical bob.
        float bob =
            Mathf.Sin(Time.time * verticalSpeed)
            * verticalAmount;

        Vector3 posA = shimmerA.position;
        Vector3 posB = shimmerB.position;

        posA.y =
            transform.TransformPoint(
                new Vector3(0f, startYA, 0f)
            ).y + bob;

        posB.y = posA.y;

        shimmerA.position = posA;
        shimmerB.position = posB;


        // ------------------------
        // LOOP
        // ------------------------

        if (horizontalSpeed > 0f)
        {
            // If A is one full width ahead of B,
            // move A behind B.
            if (shimmerA.position.x - shimmerB.position.x
                >= spriteWidth * 1.99f)
            {
                shimmerA.position = new Vector3(
                    shimmerB.position.x - spriteWidth,
                    shimmerA.position.y,
                    shimmerA.position.z
                );
            }

            // Same for B.
            if (shimmerB.position.x - shimmerA.position.x
                >= spriteWidth * 1.99f)
            {
                shimmerB.position = new Vector3(
                    shimmerA.position.x - spriteWidth,
                    shimmerB.position.y,
                    shimmerB.position.z
                );
            }
        }
        else
        {
            if (shimmerB.position.x - shimmerA.position.x
                >= spriteWidth * 1.99f)
            {
                shimmerA.position = new Vector3(
                    shimmerB.position.x + spriteWidth,
                    shimmerA.position.y,
                    shimmerA.position.z
                );
            }

            if (shimmerA.position.x - shimmerB.position.x
                >= spriteWidth * 1.99f)
            {
                shimmerB.position = new Vector3(
                    shimmerA.position.x + spriteWidth,
                    shimmerB.position.y,
                    shimmerB.position.z
                );
            }
        }
    }
}