using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        private const string GAME_MANAGER_ID = "GameManager";
        private const string PLAYER_TAG = "Player";
        private const float SCREEN_OVER_MARGIN = 2f;

        [SerializeField]
        private float _normalSpeed = 3f;

        [SerializeField]
        private GameObject _ammo;

        [SerializeField]
        private GameObject _barrelTip;

        [SerializeField]
        private float _fireRate = 0.1f;

        [SerializeField]
        private float _fireBurstLength = 0.5f;

        [SerializeField]
        private float _fireBurstCooldown = 1f;

        [SerializeField]
        private GameObject _explosionAnimation;

        [SerializeField]
        private AudioClip _machineGunClip;

        [SerializeField]
        private AudioClip _explosionClip;

        [HideInInspector]
        public bool IsHoming;

        private GameManager _gameManager;
        private GameObject _player;
        private float _nextFireBurst = 0f;
        private Vector3 _stageDimensions;
        private float _nextMachineGunAudioPlay = 0;

        private void Start()
        {
            _gameManager = GameObject.Find(GAME_MANAGER_ID).GetComponent<GameManager>();
            _player = _gameManager.GetPlayer();

            StartCoroutine(FireBurst());
        }

        private void Update()
        {
            if(_gameManager.CurrentGameState != GameManager.GameState.Game)
            {
                GameObject.Destroy(this.gameObject);
            }

            if(transform.position.x < ( _stageDimensions.x + SCREEN_OVER_MARGIN ) * -1)
            {
                GameObject.Destroy(this.gameObject);
            }

            Move();
        }

        IEnumerator FireBurst()
        {
            float nextFireBurstEnd = Time.time + _fireBurstLength;
            
            while(_gameManager.CurrentGameState == GameManager.GameState.Game)
            {
                if(Time.time < nextFireBurstEnd)
                {
                    var ammoObject = Instantiate(_ammo, _barrelTip.transform.position, transform.rotation);
                    var ammoScript = ammoObject.GetComponent<Ammo>();
                    ammoScript.SpriteRendererComponent.flipX = true;
                    ammoScript.AmmoUser = Ammo.AmmoUserType.Enemy;

                    if(Time.time > _nextMachineGunAudioPlay)
                    {
                        _nextMachineGunAudioPlay = Time.time + _machineGunClip.length;
                        AudioSource.PlayClipAtPoint(_machineGunClip, transform.position);
                    }

                    yield return new WaitForSeconds(_fireRate);
                }
                else
                {
                    nextFireBurstEnd = Time.time + _fireBurstLength + _fireBurstCooldown;
                    yield return new WaitForSeconds(_fireBurstCooldown);
                }
            }
            StopCoroutine(FireBurst());
        }

        private void Move()
        {
            if(IsHoming && _player != null && _player.transform.position.x < transform.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, ( _normalSpeed * _gameManager.CurrentGameSpeedPercentage ) * Time.deltaTime);
                return;
            }

            transform.Translate(Vector3.left * ( _normalSpeed * _gameManager.CurrentGameSpeedPercentage ) * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == PLAYER_TAG && !_gameManager.CurrentPlayer.GetComponent<Player>().HasCooldown)
            {
                GameObject.Instantiate(_explosionAnimation, collision.gameObject.transform.position, Quaternion.identity);
                GameObject.Instantiate(_explosionAnimation, gameObject.transform.position, Quaternion.identity);

                AudioSource.PlayClipAtPoint(_explosionClip, transform.position);

                //Destroy player
                GameObject.Destroy(collision.gameObject);

                //Destroy enemy
                GameObject.Destroy(gameObject);

                _gameManager.PlayerDestroyed();
            }
        }

        public void SetStageDimensions(Vector3 stageDimensions)
        {
            _stageDimensions = stageDimensions;
        }
    }
}
