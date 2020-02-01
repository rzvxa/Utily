using UnityEngine;
using System.Collections;

namespace Utils.Unity.Runtime.Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _speed;
        [SerializeField] private bool _autoPlay;

        private SpriteRenderer _spriteRenderer;
        private int _spriteIndex;

        private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

        private void OnEnable()
        {
            if(_autoPlay)
                Animate();
        }

        public void Animate() => StartCoroutine(Routine());

        public void Stop() => StopAllCoroutines();

        private IEnumerator Routine()
        {
            while(true)
            {
                _spriteRenderer.sprite = _sprites[_spriteIndex];
                _spriteIndex = (_spriteIndex + 1) % (_sprites.Length - 1);
                yield return new WaitForSeconds(_speed);
            }
        }

    }
}
