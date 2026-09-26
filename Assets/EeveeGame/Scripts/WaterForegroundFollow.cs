using UnityEngine;

public class WaterForegroundFollow : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private float fixedY;

    private void Start()
    {
        fixedY = transform.position.y;

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        transform.position = new Vector3(
            cameraTransform.position.x,
            fixedY,
            transform.position.z
        );
    }
}