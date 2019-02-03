using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private const float HORIZONTAL_BOUND = 10f;
    private const float VERTICAL_BOUND = 8f;
    private const string ENEMY_TAG = "Enemy";
    private const string PLAYER_TAG = "Player";
    private const string GAME_MANAGER_ID = "GameManager";

    [SerializeField]
    private float _speed = 30f;

    [SerializeField]
    private GameObject _explosionAnimation;

    [SerializeField]
    private AudioClip _explosionClip;

    [SerializeField]
    public SpriteRenderer SpriteRendererComponent;

    [HideInInspector]
    public enum AmmoUserType { Player, Enemy }

    [HideInInspector]
    public AmmoUserType AmmoUser;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find(GAME_MANAGER_ID).GetComponent<GameManager>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(AmmoUser == AmmoUserType.Player)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        //check if laser is out of game area and destroy
        if(transform.position.x < ( HORIZONTAL_BOUND * -1 ) || transform.position.x > HORIZONTAL_BOUND ||
             transform.position.y < ( VERTICAL_BOUND * -1 ) || transform.position.y > VERTICAL_BOUND)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == ENEMY_TAG && AmmoUser == AmmoUserType.Player)
        {
            if(_gameManager != null)
            {
                _gameManager.AddScore();
            }

            GameObject.Instantiate(_explosionAnimation, collision.gameObject.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);

            //Destroy enemy
            GameObject.Destroy(collision.gameObject);

            //Destroy ammo
            GameObject.Destroy(gameObject);
        }

        if(collision.gameObject.tag == PLAYER_TAG && AmmoUser == AmmoUserType.Enemy && !_gameManager.CurrentPlayer.GetComponent<Player>().HasCooldown)
        {
            GameObject.Instantiate(_explosionAnimation, collision.gameObject.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);

            //Destroy player
            GameObject.Destroy(collision.gameObject);

            //Destroy ammo
            GameObject.Destroy(gameObject);

            _gameManager.PlayerDestroyed();
        }
    }
}
