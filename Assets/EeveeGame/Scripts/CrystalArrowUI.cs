using UnityEngine;

public class CrystalArrowUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform arrow;
    [SerializeField] private Canvas canvas;

    [Header("Screen Edge")]
    [SerializeField] private float edgePadding = 60f;

    private Transform target;
    private Camera mainCamera;


    private void Awake()
    {
        mainCamera = Camera.main;

        Hide();
    }


    private void Update()
    {
        if (target == null ||
            mainCamera == null)
        {
            Hide();
            return;
        }

        UpdateArrow();
    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (arrow != null)
            arrow.gameObject.SetActive(true);
    }


    public void ClearTarget()
    {
        target = null;
        Hide();
    }


    private void Hide()
    {
        if (arrow != null)
            arrow.gameObject.SetActive(false);
    }


    private void UpdateArrow()
    {
        Vector3 viewportPosition =
            mainCamera.WorldToViewportPoint(
                target.position
            );

        // Check whether the crystal is currently
        // visible inside the camera view.
        bool isOnScreen =
            viewportPosition.z > 0f &&
            viewportPosition.x >= 0f &&
            viewportPosition.x <= 1f &&
            viewportPosition.y >= 0f &&
            viewportPosition.y <= 1f;

        // Crystal is visible, so we don't need the arrow.
        if (isOnScreen)
        {
            arrow.gameObject.SetActive(false);
            return;
        }

        // Crystal is off-screen, show the arrow.
        if (!arrow.gameObject.activeSelf)
        {
            arrow.gameObject.SetActive(true);
        }


        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(
                target.position
            );

        // If target is behind camera,
        // reverse the screen position.
        if (screenPosition.z < 0f)
        {
            screenPosition *= -1f;
        }

        Vector2 screenCenter =
            new Vector2(
                Screen.width * 0.5f,
                Screen.height * 0.5f
            );

        Vector2 direction =
            (Vector2)screenPosition -
            screenCenter;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        direction.Normalize();


        // Keep arrow inside screen edges.
        float x =
            Mathf.Clamp(
                screenPosition.x,
                edgePadding,
                Screen.width - edgePadding
            );

        float y =
            Mathf.Clamp(
                screenPosition.y,
                edgePadding,
                Screen.height - edgePadding
            );

        arrow.position =
            new Vector2(x, y);


        // Point arrow toward the crystal.
        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

        arrow.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angle
            );
    }
}