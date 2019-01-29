using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float MAX_SPEED_CHANGE = 0.65f;

    [SerializeField]
    private float _normalGameSpeed = 4f;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private int _playerTotalLives = 3;

    [HideInInspector]
    public float CurrentGameSpeed = 1f;

    private GameObject currentPlayer;
    private Vector3 _stageDimensions;
    private int _playerRemainingLives = 0;

    public enum GameState { Start, Game, GameOver };

    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    public void PlayerDestroyed()
    {
        _playerRemainingLives--;

        if (_playerRemainingLives == 0)
        {
            currentGameState = GameState.GameOver;
            return;
        }

        currentPlayer = Instantiate(_player);
    }

    private void Start()
    {
        currentGameState = GameState.Start;
        _stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    private void Update()
    {

        if (currentGameState == GameState.Start)
        {
            //TODO: Show start game / welcome screen

            if (Input.GetKeyUp(KeyCode.Space))
            {
                InitializeGame();
            }

            return;
        }
        if (currentGameState == GameState.Game)
        {
            CalculateSpeed();
            return;
        }
        if (currentGameState == GameState.GameOver)
        {
            //TODO: Show game over screen

            if (Input.GetKeyUp(KeyCode.Space))
            {
                currentGameState = GameState.Start;
            }
            return;
        }
    }

    private void InitializeGame()
    {
        currentGameState = GameState.Game;

        _playerRemainingLives = _playerTotalLives;
        _spawnManager.StartEnemySpawning(_stageDimensions);
        currentPlayer = Instantiate(_player, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0),Quaternion.identity);
    }

    private void CalculateSpeed()
    {
        var maximumSpeedChange = _normalGameSpeed * MAX_SPEED_CHANGE;
        var relativeSpeedChange = Mathf.Abs(currentPlayer.transform.position.x) / _stageDimensions.x;

        if (currentPlayer.transform.position.x < 0) // Reduce speed
        {
            CurrentGameSpeed = _normalGameSpeed - (relativeSpeedChange * maximumSpeedChange);
            return;
        }

        if (currentPlayer.transform.position.x > 0) // Increase speed
        {
            CurrentGameSpeed = _normalGameSpeed + (relativeSpeedChange * maximumSpeedChange);
            return;
        }

        CurrentGameSpeed = _normalGameSpeed;
    }

    public GameObject GetPlayer()
    {
        return currentPlayer;
    }
}
