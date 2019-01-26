using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SpawnManager spawnManager;

    [SerializeField]
    private GameObject player;

    private GameObject currentPlayer;

    public enum GameState { Start, Game, GameOver };

    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    void Start()
    {
        currentGameState = GameState.Game;
        spawnManager.StartEnemySpawning();

        currentPlayer = Instantiate(player);
    }

    void Update()
    {
        
    }

    public GameObject GetPlayer()
    {
        return currentPlayer;
    }
}
