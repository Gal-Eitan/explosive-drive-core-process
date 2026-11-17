
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "ExplosiveDrive";
    [SerializeField] private string gameOverSceneName = "GameOverScreen";

    public void ShowGameOver()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}
