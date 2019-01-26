using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private const float SPAWN_MARGIN_X = 1f;
    private const float SPAWN_MARGIN_Y = 0.2f; //percentage of the screen height

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _enemy;

    [SerializeField]
    private float _spawnRate = 5f;

    private Vector3 _stageDimensions;
    private float _spawnYMinPosition;
    private float _spawnYMaxPosition;

    void Start()
    {
        _spawnYMinPosition = -Mathf.Abs(_stageDimensions.y - (_stageDimensions.y * (SPAWN_MARGIN_Y / 2)));
        _spawnYMaxPosition = _stageDimensions.y - (_stageDimensions.y * (SPAWN_MARGIN_Y / 2));
    }
    
    public void StartEnemySpawning(Vector3 stageDimensions)
    {
        _stageDimensions = stageDimensions;

        CalculateSpawnArea(_stageDimensions);
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (_gameManager.CurrentGameState == GameManager.GameState.Game)
        {
            Vector3 spawnPosition = GetEnemySpawnPosition();

            var enemyObject = GameObject.Instantiate(_enemy, spawnPosition, Quaternion.identity);
            var enemyScript = enemyObject.GetComponent<Enemy>();

            if (Random.Range(0, 2) == 1)
            {
                enemyScript.IsHoming = true;
            }

            yield return new WaitForSeconds(_spawnRate);

            //currentSpawnRate -= spawnRateDecrease;

            //if (currentSpawnRate < 1)
            //{
            //    currentSpawnRate = 1f;
            //}
        }

        StopCoroutine(SpawnEnemy());
    }

    private void CalculateSpawnArea(Vector3 stageDimensions)
    {
        _spawnYMinPosition = -Mathf.Abs(stageDimensions.y - (stageDimensions.y * (SPAWN_MARGIN_Y / 2)));
        _spawnYMaxPosition = stageDimensions.y - (stageDimensions.y * (SPAWN_MARGIN_Y / 2));
    }

    private Vector3 GetEnemySpawnPosition()
    {
        if (_stageDimensions.x == 0 && _stageDimensions.y == 0)
        {
            Debug.LogWarning("Stage dimensions are zero.");
        }

        Vector3 spawnPosition = new Vector3();
        spawnPosition.x = _stageDimensions.x + SPAWN_MARGIN_X;
        spawnPosition.y = Random.Range(_spawnYMinPosition, _spawnYMaxPosition);

        return spawnPosition;
    }
}
