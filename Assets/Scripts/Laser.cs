using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private const float HORIZONTAL_BOUND = 10f;
    private const float VERTICAL_BOUND = 8f;
    private const string ENEMY_TAG = "Enemy";
    private const string GAME_MANAGER_ID = "GameManager";

    [SerializeField]
    private float speed = 20f;

    void Start()
    {
        //gameManager = GameObject.Find(GAME_MANAGER_ID).GetComponent<GameManager>();
    }

    void Update()
    {
        Move();   
    }

    private void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        //check if laser is out of game area and destroy
        if (transform.position.x < (HORIZONTAL_BOUND * -1) || transform.position.x > HORIZONTAL_BOUND ||
             transform.position.y < (VERTICAL_BOUND * -1) || transform.position.y > VERTICAL_BOUND)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ENEMY_TAG)
        {
            //if (gameManager != null)
            //{
            //    gameManager.AddScore();
            //}
            
            //GameObject.Instantiate(explosionAnimation, collision.gameObject.transform.position, Quaternion.identity);

            //Destroy enemy
            GameObject.Destroy(collision.gameObject);

            //Destroy laser
            GameObject.Destroy(gameObject);
        }
    }
}
