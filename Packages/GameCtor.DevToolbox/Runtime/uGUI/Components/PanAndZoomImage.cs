using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCtor.DevToolbox
{
    public sealed class PanAndZoomImage : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [Range(0.1f, 1f)]
        private float _zoomStep = 0.2f;

        [SerializeField]
        private Button _zoomInBtn;

        [SerializeField]
        private Button _zoomOutBtn;

        private RectTransform _rt;
        private Canvas _canvas;
        private RectTransform _parent;
        private Vector2 _offset;
        private Vector3 _max;
        private Vector3 _min;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _parent = transform.parent.GetComponent<RectTransform>();
        }

        private void Start()
        {
            _zoomInBtn.onClick.AddListener(ZoomIn);
            _zoomOutBtn.onClick.AddListener(ZoomOut);
        }

        private void OnEnable()
        {
            ResetParams();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            _offset = (Vector2)_rt.position - eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            UpdatePosition(eventData.position + _offset);
        }

        private void AdjustBounds()
        {
            var diff = 0.5f * _canvas.scaleFactor * (_rt.rect.size * _rt.localScale - _parent.rect.size);
            _max = (Vector2)_parent.position + diff;
            _min = (Vector2)_parent.position - diff;
        }

        private void UpdatePosition(Vector2 newPosition)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, _min.x, _max.x);
            newPosition.y = Mathf.Clamp(newPosition.y, _min.y, _max.y);
            _rt.position = newPosition;
        }

        public void ZoomIn()
        {
            var value = Mathf.Clamp(_rt.localScale.x + _zoomStep, 1f, 10f);
            _rt.localScale = new Vector3(value, value, 1f);
            AdjustBounds();
        }

        public void ZoomOut()
        {
            var value = Mathf.Clamp(_rt.localScale.x - _zoomStep, 1f, 10f);
            _rt.localScale = new Vector3(value, value, 1f);
            AdjustBounds();
            UpdatePosition(_rt.position);
        }

        public void ResetParams()
        {
            _rt.localScale = Vector3.one;
            _rt.anchoredPosition = Vector3.zero;
            AdjustBounds();
        }
    }
}
