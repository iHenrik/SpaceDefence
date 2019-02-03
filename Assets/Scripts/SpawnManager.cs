using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private const float SPAWN_MARGIN_X = 1f;
    private const float SPAWN_TOP_MARGIN_Y = 0.25f; //percentage of the screen height
    private const float SPAWN_BOTTOM_MARGIN_Y = 0.95f; //percentage of the screen height

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _enemy;

    [SerializeField]
    private float _spawnRate = 5f;

    private float _currentSpawnRate = 0f;
    private float _spawnRateDecrease = 0.2f;
    private Vector3 _stageDimensions;
    private float _spawnYMinPosition;
    private float _spawnYMaxPosition;

    public void StartEnemySpawning(Vector3 stageDimensions)
    {
        _stageDimensions = stageDimensions;

        _currentSpawnRate = _spawnRate;
        CalculateSpawnArea(_stageDimensions);
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while(_gameManager.CurrentGameState == GameManager.GameState.Game)
        {
            Vector3 spawnPosition = GetEnemySpawnPosition();

            var enemyObject = GameObject.Instantiate(_enemy, spawnPosition, Quaternion.identity);
            var enemyScript = enemyObject.GetComponent<Enemy>();

            enemyScript.SetStageDimensions(_stageDimensions);
            if(Random.Range(0, 2) == 1)
            {
                enemyScript.IsHoming = true;
            }

            yield return new WaitForSeconds(_currentSpawnRate);

            _currentSpawnRate -= _spawnRateDecrease;

            if(_currentSpawnRate < 1)
            {
                _currentSpawnRate = 1f;
            }
        }

        StopCoroutine(SpawnEnemy());
    }

    private void CalculateSpawnArea(Vector3 stageDimensions)
    {
        _spawnYMinPosition = -( stageDimensions.y - ( stageDimensions.y * SPAWN_BOTTOM_MARGIN_Y ) ); //-Mathf.Abs(stageDimensions.y - (stageDimensions.y * (SPAWN_BOTTOM_MARGIN_Y / 2)));
        _spawnYMaxPosition = stageDimensions.y - ( stageDimensions.y * SPAWN_TOP_MARGIN_Y );
    }

    private Vector3 GetEnemySpawnPosition()
    {
        if(_stageDimensions.x == 0 && _stageDimensions.y == 0)
        {
            Debug.LogWarning("Stage dimensions are zero.");
        }

        Vector3 spawnPosition = new Vector3();
        spawnPosition.x = _stageDimensions.x + SPAWN_MARGIN_X;
        spawnPosition.y = Random.Range(_spawnYMinPosition, _spawnYMaxPosition);
        spawnPosition.z = -1f;

        return spawnPosition;
    }
}
