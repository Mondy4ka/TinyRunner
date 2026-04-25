using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("MenuUI Settings")]
    [SerializeField] private Canvas _menuCanvas;
    [SerializeField] private TMP_Text _bestScoreText;

    [Header("GameUI Settings")]
    [SerializeField] private Canvas _gameCanvas;
    [SerializeField] private GameObject _playingUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _gameoverUI;
    [SerializeField] private TMP_Text _scoreText;

    //[SerializeField] private TMP_Text _fpsText;

    private GameManager _gameManager;
    private ScoreManager _scoreManager;

    //private void Update()
    //{
    //    _fpsText.text = Mathf.Round(1.0f / Time.deltaTime).ToString();
    //}

    public void Initialize(GameManager gameManager, ScoreManager scoreManager)
    {
        _gameManager = gameManager;
        _scoreManager = scoreManager;

        _gameManager.OnGameStateChanged += OnGameStateChanged;
        _scoreManager.OnScoreChanged += SetScoreText;
        _scoreManager.OnBestScoreChanged += SetBestScoreText;
    }

    public void Deinitialize()
    {
        _gameManager.OnGameStateChanged -= OnGameStateChanged;
        _scoreManager.OnScoreChanged -= SetScoreText;
        _scoreManager.OnBestScoreChanged -= SetBestScoreText;
    }

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                ActivateMenuUI();
                break;
            case GameState.Paused:
                ActivatePauseUI();
                break;
            case GameState.GameOver:
                ActivateGameoverUI();
                break;
            case GameState.StartPlaying:
            case GameState.Playing:
                ActivateGameUI();
                break;
        }
    }

    private void ActivateGameUI()
    {
        SetMenuUI(false);
        SetGameUI(true);
        SetPlayingUI(true);
        SetPauseUI(false);
        SetGameoverUI(false);
    }

    private void ActivateMenuUI()
    {
        SetMenuUI(true);
        SetGameUI(false);
    }

    private void ActivatePauseUI()
    {
        SetPlayingUI(false);
        SetPauseUI(true);
        SetGameoverUI(false);
    }

    private void ActivateGameoverUI()
    {
        SetPlayingUI(false);
        SetPauseUI(false);
        SetGameoverUI(true);
    }

    private void SetScoreText(int score) => _scoreText.text = $"Score: {score}";

    private void SetBestScoreText(int bestScore) => _bestScoreText.text = $"Best score: {bestScore}";

    private void SetMenuUI(bool isActive) => _menuCanvas.enabled = isActive;

    private void SetGameUI(bool isActive) => _gameCanvas.enabled = isActive;

    private void SetPlayingUI(bool isActive) => _playingUI.SetActive(isActive);

    private void SetPauseUI(bool isActive) => _pauseUI.SetActive(isActive);

    private void SetGameoverUI(bool isActive) => _gameoverUI.SetActive(isActive);
}