using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundHost : MonoBehaviour
{
    private const string GAME_MANAGER_ID = "GameManager";
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private GameObject _explosionAnimation;

    [SerializeField]
    private AudioClip _explosionClip;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find(GAME_MANAGER_ID).GetComponent<GameManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if(collision.gameObject.tag == PLAYER_TAG && !_gameManager.CurrentPlayer.GetComponent<Player>().HasCooldown)
        {
            GameObject.Instantiate(_explosionAnimation, collision.gameObject.transform.position, Quaternion.identity);

            //Destroy player
            GameObject.Destroy(collision.gameObject);

            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);

            _gameManager.PlayerDestroyed();
        }
    }
}
