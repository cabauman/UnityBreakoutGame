using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BreakoutGame
{
    public sealed class FadeCanvas : MonoBehaviour
    {
        [SerializeField]
        private Image _fadeImage;

        [SerializeField]
        private float _fadeDuration = 0.5f;

        public static FadeCanvas Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public Coroutine FadeOut() => StartCoroutine(Fade(0f, 1f));
        public Coroutine FadeIn() => StartCoroutine(Fade(1f, 0f));

        private IEnumerator Fade(float from, float to)
        {
            float time = 0f;
            Color c = _fadeImage.color;

            while (time < _fadeDuration)
            {
                time += Time.unscaledDeltaTime;
                c.a = Mathf.Lerp(from, to, time / _fadeDuration);
                _fadeImage.color = c;
                yield return null;
            }

            c.a = to;
            _fadeImage.color = c;
        }
    }
}