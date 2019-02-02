using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    private const string GAME_MANAGER_ID = "GameManager";

    private GameManager _gameManager;
    private Vector3 _stageDimensions;

    private Rigidbody2D _rigidBody;

    private void Start()
    {
        _gameManager = GameObject.Find(GAME_MANAGER_ID).GetComponent<GameManager>();

        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(_gameManager.CurrentGameState == GameManager.GameState.Game)
        {
            _rigidBody.velocity = new Vector2(-( _gameManager.BackgroundScrollSpeed * _gameManager.CurrentGameSpeedPercentage ), 0);
        }
        else if(_rigidBody.velocity.x != 0)
        {
            _rigidBody.velocity = new Vector2(0, 0);
        }
    }

}
