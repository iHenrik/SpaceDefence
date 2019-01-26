using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        private const string GAME_MANAGER_ID = "GameManager";

        [SerializeField]
        private float speed = 2f;

        private GameManager gameManager;
        private GameObject player;

        public enum BehaviourType { Normal, Homing };
        public BehaviourType Behaviour;

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

            if (Behaviour == BehaviourType.Normal)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
