using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public static event Action<int> onScoreUpdated;
    public static event Action<int> onCountdownUpdated;
    public static event Action onStartGame;

    public int level { get { return _level; } }
    public int score { get { return _score; } }

    [SerializeField] private int _scoreIncrementPerCollect = 1;
    [SerializeField] private int _countdownStart = 3;

    private int _countdown = 3;
    private int _score = 0;
    private int _level = 1;

    public void RestartGame()
    {
        _level = 1;
        _score = 0;
        _countdown = _countdownStart;

        StartCoroutine(LoadLevel());
    }

    public void LoadNextLevel()
    {
        _level++;
        _countdown = _countdownStart;

        StartCoroutine(LoadLevel());
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _countdown = _countdownStart;
        StartCoroutine(Countdown());
    }

    private void OnEnable()
    {
        Collectible.onCollected += CollectibleCollected;
    }

    private void OnDisable()
    {
        Collectible.onCollected -= CollectibleCollected;
    }

    private void CollectibleCollected()
    {
        _score += _scoreIncrementPerCollect;

        onScoreUpdated?.Invoke(_score);
    }

    private IEnumerator Countdown()
    {
        while (_countdown > 0)
        {
            _countdown--;

            onCountdownUpdated?.Invoke(_countdown);

            yield return new WaitForSeconds(1f);
        }

        onStartGame?.Invoke();
    }

    private IEnumerator LoadLevel()
    {
        var asyncLoadScene = SceneManager.LoadSceneAsync(
            SceneManager.GetActiveScene().name
        );

        while (!asyncLoadScene.isDone)
        {
            yield return null;
        }

        StartCoroutine(Countdown());
    }
}
