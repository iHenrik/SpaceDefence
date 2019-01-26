using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float MAX_SPEED_CHANGE = 0.5f;

    [SerializeField]
    private float _normalGameSpeed = 4f;

    [SerializeField]
    private SpawnManager spawnManager;

    [SerializeField]
    private GameObject player;

    [HideInInspector]
    public float CurrentGameSpeed = 1f;

    private GameObject currentPlayer;
    private Vector3 _stageDimensions;

    public enum GameState { Start, Game, GameOver };

    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    void Start()
    {
        currentGameState = GameState.Game;

        _stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        spawnManager.StartEnemySpawning(_stageDimensions);

        currentPlayer = Instantiate(player);
    }

    void Update()
    {
        CalculateSpeed();
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
