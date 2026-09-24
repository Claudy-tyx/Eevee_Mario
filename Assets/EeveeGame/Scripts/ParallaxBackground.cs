using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [Range(0f, 1f)]
    [SerializeField] private float parallaxStrength = 0.5f;

    private Vector3 startPosition;
    private Vector3 cameraStartPosition;

    private void Start()
    {
        startPosition = transform.position;

        if (cameraTransform != null)
            cameraStartPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        float cameraMovement =
            cameraTransform.position.x - cameraStartPosition.x;

        transform.position = new Vector3(
            startPosition.x + cameraMovement * parallaxStrength,
            startPosition.y,
            startPosition.z
        );
    }
}