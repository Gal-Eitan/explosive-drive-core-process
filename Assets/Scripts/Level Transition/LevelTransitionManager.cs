
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
    public static LevelTransitionManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "ExplosiveDrive";
    [SerializeField] private string gameOverSceneName = "GameOverScreen";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGameOver()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
