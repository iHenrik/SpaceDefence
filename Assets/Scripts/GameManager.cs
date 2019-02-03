using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const float MAX_SPEED_CHANGE = 0.85f;
    private const float GAME_OVER_VIEW_TIME = 1.5f;
    private const string ENEMY_TAG = "Enemy";

    [SerializeField]
    private float _normalGameSpeed = 4f;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private int _playerTotalPlanes = 3;

    [SerializeField]
    private GameObject _hud;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private TextMeshProUGUI _planeCountText;

    [SerializeField]
    private int HitScore = 20;

    [SerializeField]
    private AudioClip gameMusic;

    [SerializeField]
    private AudioClip menuMusic;

    private AudioSource audioSource;

    public float BackgroundScrollSpeed = 2f;
    public Image StartView;
    public Image GameOverView;

    [HideInInspector]
    public float CurrentGameSpeedPercentage = 1f;

    [HideInInspector]
    public GameObject CurrentPlayer;

    private Vector3 _stageDimensions;
    private int _playerRemainingPlanes = 0;
    private int _score;
    private float _gameOverViewTimeStamp = 0;

    public enum GameState { Menu, Game, GameOver };

    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    private void Start()
    {
        Cursor.visible = false;

        audioSource = GetComponent<AudioSource>();
        _stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        InitializeMenu();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            if(currentGameState == GameState.Menu)
            {
                _hud.SetActive(true);
                StartView.enabled = false;
                InitializeGame();

                return;
            }
            if(currentGameState == GameState.GameOver)
            {
                if(Time.time > ( _gameOverViewTimeStamp + GAME_OVER_VIEW_TIME ))
                {
                    InitializeMenu();
                    return;
                }
            }
        }

        if(currentGameState == GameState.Game)
        {
            CalculateSpeed();
        }
    }

    public void PlayerDestroyed()
    {
        DecreasePlaneCount();

        if(_playerRemainingPlanes < 0)
        {
            InitializeGameOver();
            return;
        }

        InstantiatePlayer();
    }

    public void AddScore()
    {
        _score += HitScore;
        _scoreText.text = _score.ToString();
    }

    private void DecreasePlaneCount()
    {
        _playerRemainingPlanes--;

        if(_playerRemainingPlanes >= 0)
        {
            _planeCountText.text = _playerRemainingPlanes.ToString();
        }
    }

    private void InitializeMenu()
    {
        currentGameState = GameState.Menu;

        _hud.SetActive(false);
        StartView.enabled = true;
        GameOverView.enabled = false;

        //Play menu music
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    private void InitializeGame()
    {
        currentGameState = GameState.Game;

        _score = 0;
        _scoreText.text = "0";

        _playerRemainingPlanes = _playerTotalPlanes;
        DecreasePlaneCount();

        //Play menu music
        audioSource.clip = gameMusic;
        audioSource.Play();

        _spawnManager.StartEnemySpawning(_stageDimensions);
        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        CurrentPlayer = Instantiate(_player, new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1), Quaternion.identity);
    }

    private void InitializeGameOver()
    {
        currentGameState = GameState.GameOver;
        GameOverView.enabled = true;

        CleanObjects();

        _gameOverViewTimeStamp = Time.time;
    }

    private void CalculateSpeed()
    {
        var maximumSpeedChange = _normalGameSpeed * MAX_SPEED_CHANGE;
        var relativeSpeedChange = Mathf.Abs(CurrentPlayer.transform.position.x) / _stageDimensions.x * MAX_SPEED_CHANGE;

        if(CurrentPlayer.transform.position.x < 0) // Reduce speed
        {
            CurrentGameSpeedPercentage = 1f - relativeSpeedChange;
            return;
        }

        if(CurrentPlayer.transform.position.x > 0) // Increase speed
        {
            CurrentGameSpeedPercentage = 1f + relativeSpeedChange;
            return;
        }

        CurrentGameSpeedPercentage = 1f;
    }

    private void CleanObjects()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(ENEMY_TAG);
        foreach(var gItem in gameObjects)
        {
            GameObject.Destroy(gItem);
        }
    }

    public GameObject GetPlayer()
    {
        return CurrentPlayer;
    }
}
