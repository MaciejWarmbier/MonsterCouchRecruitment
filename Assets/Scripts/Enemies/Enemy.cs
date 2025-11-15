using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] Color _activeColor;
        [SerializeField] Color _frozenColor;

        public void SetFreeze(bool isFrozen)
        {
            _spriteRenderer.color = isFrozen ? _frozenColor : _activeColor;
        }
    }
}
