using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float NORMAL_SPEED_AREA_PERCENTAGE = 0.2f;
    private const float MAX_SPEED_REDUCTION = 0.5f;

    [SerializeField]
    private float _normalGameSpeed= 4f;

    [SerializeField]
    private SpawnManager spawnManager;

    [SerializeField]
    private GameObject player;

    private GameObject currentPlayer;
    private Vector3 _stageDimensions;
    private float _normalSpeedAreaLimit;

    public enum GameState { Start, Game, GameOver };

    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    public float CurrentGameSpeed = 1f;

    void Start()
    {
        currentGameState = GameState.Game;

        _stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        CalculateNormalSpeedAreaLimit();

        spawnManager.StartEnemySpawning(_stageDimensions);

        currentPlayer = Instantiate(player);
    }

    void Update()
    {
        CalculateSpeed();
    }

    private void CalculateNormalSpeedAreaLimit()
    {
        _normalSpeedAreaLimit = _stageDimensions.x * NORMAL_SPEED_AREA_PERCENTAGE;
    }

    private void CalculateSpeed()
    {
        if (Mathf.Abs(currentPlayer.transform.position.x) <= _normalSpeedAreaLimit)
        {
            CurrentGameSpeed = _normalGameSpeed;
            return;
        }

        CurrentGameSpeed = _normalGameSpeed * (Mathf.Abs(currentPlayer.transform.position.x) - _normalSpeedAreaLimit) / (_stageDimensions.x - _normalSpeedAreaLimit) * MAX_SPEED_REDUCTION;
    }

    public GameObject GetPlayer()
    {
        return currentPlayer;
    }
}
