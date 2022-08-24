using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private GameObject _gameOverContainer;
    [SerializeField] private GameObject _winContainer;

    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _nextLevelButton;

    [SerializeField] private Player _player;

    private void OnEnable()
    {
        GameManager.onStartGame += GameStart;
        GameManager.onScoreUpdated += UpdateScore;
        GameManager.onCountdownUpdated += UpdateCountdown;

        _player.onCollidedWithObstacle += ShowGameOverScreen;

        LevelEnd.onLevelComplete += ShowWinScreen;

        _restartButton.onClick.AddListener(Restart);
        _nextLevelButton.onClick.AddListener(NextLevel);
    }

    private void OnDisable()
    {
        GameManager.onStartGame -= GameStart;
        GameManager.onScoreUpdated -= UpdateScore;
        GameManager.onCountdownUpdated -= UpdateCountdown;

        _player.onCollidedWithObstacle -= ShowGameOverScreen;

        LevelEnd.onLevelComplete -= ShowWinScreen;

        _restartButton.onClick.RemoveListener(Restart);
        _nextLevelButton.onClick.RemoveListener(NextLevel);
    }

    private void Start()
    {
        UpdateLevel(GameManager.instance.level);
        UpdateScore(GameManager.instance.score);
    }

    private void UpdateCountdown(int _countdown)
    {
        _countdownText.gameObject.SetActive(true);

        if (_countdown == 0)
        {
            _countdownText.SetText("Go!");
        }
        else
        {
            _countdownText.SetText(_countdown.ToString());
        }
    }

    private void UpdateLevel(int level)
    {
        _levelText.SetText($"Level: {level}");
    }

    private void UpdateScore(int score)
    {
        _scoreText.SetText($"Score: {score}");
    }

    private void GameStart()
    {
        _countdownText.gameObject.SetActive(false);
    }

    private void ShowGameOverScreen()
    {
        _gameOverContainer.SetActive(true);
    }

    private void ShowWinScreen()
    {
        _winContainer.SetActive(true);
    }

    private void Restart()
    {
        _restartButton.interactable = false;

        GameManager.instance.RestartGame();
    }

    private void NextLevel()
    {
        _nextLevelButton.interactable = false;

        GameManager.instance.LoadNextLevel();
    }
}
