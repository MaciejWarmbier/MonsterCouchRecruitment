using System;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public Action<Vector2> OnPlayerMoved;
        public Vector2 SpawnPosition => _spawnPosition;

        [SerializeField] private float _speed = 5f;
        [SerializeField] private Vector2 _spawnPosition = Vector2.zero;

        private Vector2 _lastPosition;
        private Vector2 _screenBounds;

        public void Init(Vector2 screenBounds)
        {
            transform.position = _spawnPosition;
            _lastPosition = transform.position;

            _screenBounds = screenBounds;
        }

        void Update()
        {
            Vector2 input = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (input.sqrMagnitude > 0f)
            {
                Vector2 newPos = (Vector2)transform.position + input.normalized * (_speed * Time.deltaTime);

                newPos.x = Mathf.Clamp(newPos.x, -_screenBounds.x, _screenBounds.x);
                newPos.y = Mathf.Clamp(newPos.y, -_screenBounds.y, _screenBounds.y);

                transform.position = newPos;

                if ((Vector2)transform.position != _lastPosition)
                {
                    OnPlayerMoved?.Invoke(transform.position);
                    _lastPosition = transform.position;
                }
            }
        }
    }
}
