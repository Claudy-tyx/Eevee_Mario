using UnityEngine;

public class DarkMapLighting : MonoBehaviour
{
    public static bool IsActive { get; private set; }

    private void OnEnable()
    {
        IsActive = true;
    }

    private void OnDisable()
    {
        IsActive = false;
    }
}