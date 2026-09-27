using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusic : MonoBehaviour
{
    private static PersistentMusic instance;

    [Header("Music")]
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip map3Music;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    )
    {
        AudioClip wantedMusic;

        if (scene.name == "Level03")
        {
            wantedMusic = map3Music;
        }
        else
        {
            wantedMusic = mainMusic;
        }

        // Don't restart if the correct song
        // is already playing.
        if (audioSource.clip == wantedMusic)
            return;

        audioSource.clip = wantedMusic;
        audioSource.Play();
    }
}