using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MapTestSwitcher : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current == null)
            return;

        // Press 1 for Map 1
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level01");
        }

        // Press 2 for Map 2
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level02");
        }
    }
}