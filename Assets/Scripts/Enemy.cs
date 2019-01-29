using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        private const string GAME_MANAGER_ID = "GameManager";
        private const float SCREEN_OVER_MARGIN = 2f;

        [SerializeField]
        private GameObject ammo;

        [SerializeField]
        private float fireRate = 1.25f;

        [HideInInspector]
        public bool IsHoming;

        private float speed;
        private GameManager gameManager;
        private GameObject player;
        private float nextFire = 0f;
        private Vector3 _stageDimensions;

        private void Start()
        {
            gameManager = GameObject.Find(GAME_MANAGER_ID).GetComponent<GameManager>();
            player = gameManager.GetPlayer();
        }

        private void Update()
        {
            if (gameManager.CurrentGameState != GameManager.GameState.Game)
            {
                GameObject.Destroy(this.gameObject);
            }

            if (transform.position.x < (_stageDimensions.x + SCREEN_OVER_MARGIN) * -1)
            {
                GameObject.Destroy(this.gameObject);
            }

            Fire();
            Move();
        }

        private void Fire()
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;

                var ammoObject = Instantiate(ammo, transform.position, transform.rotation);
                var ammoScript = ammoObject.GetComponent<Ammo>();
                ammoScript.SpriteRenderer.flipX = true;
                ammoScript.AmmoUser = Ammo.AmmoUserType.Enemy;
            }
        }

        private void Move()
        {
            speed = gameManager.CurrentGameSpeed;

            if (IsHoming && player != null && player.transform.position.x < transform.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                return;
            }

            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        public void SetStageDimensions(Vector3 stageDimensions)
        {
            _stageDimensions = stageDimensions;
        }
    }
}
