using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utils.Unity.Runtime.Components
{
    public class FadeMe : MonoBehaviour
    {
        [SerializeField] private bool FadeInOnStart;
        [SerializeField] private bool FadeOutOnStart;

        [SerializeField] private float FadeInSpeed;
        [SerializeField] private float FadeOutSpeed;

        private SpriteRenderer _sprite;
        private Image _image;

        private float Alpha
        {
            get =>
                _sprite == null ?
                    _image == null ?
                        throw new NullReferenceException()
                        : _image.color.a
                    : _sprite.color.a;

            set
            {
                Color color;
                if (_sprite == null)
                {
                    if (_image == null)
                        throw new NullReferenceException();
                    color = _image.color;
                    color.a = value;
                    _image.color = color;
                }
                else
                {
                    color = _sprite.color;
                    color.a = value;
                    _sprite.color = color;
                }
            }
        }

        private void Start()
        {
            _image = GetComponent<Image>();
            _sprite = GetComponent<SpriteRenderer>();

            if(FadeInOnStart)
                FadeIn();
            else FadeOut();
        }

        public void FadeIn()
        {
            StopCoroutine(nameof(FadeRoutine));
            StartCoroutine(FadeRoutine(1, FadeInSpeed));
        }

        public void FadeOut()
        {
            StopCoroutine(nameof(FadeRoutine));
            StartCoroutine(FadeRoutine(0, FadeOutSpeed));
        }

        private IEnumerator FadeRoutine(float targetAlpha, float speed)
        {
            while (Alpha != targetAlpha)
            {
                Alpha = Mathf.MoveTowards(Alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

        }

    }
}
