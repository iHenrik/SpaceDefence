using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BackgroundRepeat : MonoBehaviour
    {
        private float _backgroundWidth;

        private void Start()
        {
            _backgroundWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
        }

        private void Update()
        {
            if (transform.position.x < -_backgroundWidth * 2)
            {
                RepositionBackground();
            }
        }

        private void RepositionBackground()
        {
            Vector2 groundOffSet = new Vector2(_backgroundWidth * 4f, 0);
            transform.position = (Vector2)transform.position + groundOffSet;
        }
    }
}
