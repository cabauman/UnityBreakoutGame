using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCtor.DevToolbox
{
    /// <summary>
    /// Place this component on the <see cref="RectTransform"/> you want to be clickable.
    /// </summary>
    public sealed class UIDragComponent : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [Tooltip(
            "The RectTransform that you want to be dragged. Can be the same as this " +
            "component if the whole RectTransform is clickable.")]
        [SerializeField]
        private RectTransform _panelRt;

        private Vector2 _offset;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            _offset = (Vector2)_panelRt.position - eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            _panelRt.position = eventData.position + _offset;
        }
    }
}
