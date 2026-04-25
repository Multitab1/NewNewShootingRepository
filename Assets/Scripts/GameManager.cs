using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Color[] roundColors = new Color[6];

    [Header("--- UI Settings ---")]
    [SerializeField] private Image[] uiImages;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    private void Awake()
    {
        if (instance == null) instance = this;
        Time.timeScale = 1f;
    }

    public void SetRound(Color[] colors)
    {
        roundColors = colors;
        for (int i = 0; i < uiImages.Length; i++)
        {
            if (uiImages[i] != null)
                uiImages[i].color = roundColors[i];
        }
    }

    public void CheckCensor(int lane, Color color, bool isShot)
    {
        bool isCorrect = (color == roundColors[lane]);

        if (isShot)
        {
            if (isCorrect) GameOver("WRONG TARGET!");
        }
        else
        {
            if (!isCorrect) GameOver("SPY ESCAPED!");
        }
    }

    public void GameOver(string message)
    {
        if (Time.timeScale == 0) return;

        gameOverPanel.SetActive(true);
        gameOverText.text = message;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}