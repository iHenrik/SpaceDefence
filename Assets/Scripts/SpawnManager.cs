using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private const float SPAWN_MARGIN_X = 1f;
    private const float SPAWN_MARGIN_Y = 0.2f; //percentage of the screen height

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private float spawnRate = 5f;

    private Vector3 stageDimensions;
    private float spawnYMinPosition;
    private float spawnYMaxPosition;

    void Start()
    {
        stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        spawnYMinPosition = -Mathf.Abs(stageDimensions.y - (stageDimensions.y * (SPAWN_MARGIN_Y / 2)));
        spawnYMaxPosition = stageDimensions.y - (stageDimensions.y * (SPAWN_MARGIN_Y / 2));
    }

    public void StartEnemySpawning()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (gameManager.CurrentGameState == GameManager.GameState.Game)
        {
            Vector3 spawnPosition = GetEnemySpawnPosition();

            var enemyObject = GameObject.Instantiate(enemy, spawnPosition, Quaternion.identity);
            var enemyScript = enemyObject.GetComponent<Enemy>();
            enemyScript.Behaviour = Enemy.BehaviourType.Homing;

            yield return new WaitForSeconds(spawnRate);

            //currentSpawnRate -= spawnRateDecrease;

            //if (currentSpawnRate < 1)
            //{
            //    currentSpawnRate = 1f;
            //}
        }

        StopCoroutine(SpawnEnemy());
    }

    private Vector3 GetEnemySpawnPosition()
    {
        if (stageDimensions.x == 0 && stageDimensions.y == 0)
        {
            Debug.LogWarning("Stage dimensions are zero.");
        }

        Vector3 spawnPosition = new Vector3();
        spawnPosition.x = stageDimensions.x + SPAWN_MARGIN_X;
        spawnPosition.y = Random.Range(spawnYMinPosition, spawnYMaxPosition);

        return spawnPosition;
    }
}
