using UnityEngine;

public class LevelStart : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartTimer();
        }
    }
}