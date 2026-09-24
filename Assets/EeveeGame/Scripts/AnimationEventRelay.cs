using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    public void SpawnSwift()
    {
        if (playerController != null)
        {
            playerController.SpawnSwift();
        }
    }
}