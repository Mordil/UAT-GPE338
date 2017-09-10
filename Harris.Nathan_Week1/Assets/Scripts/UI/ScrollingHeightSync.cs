using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(VerticalLayoutGroup), typeof(RectTransform))]
public class ScrollingHeightSync : MonoBehaviour
{
    private int _lastChildCount;

    [SerializeField]
    private RectTransform _myTransform;
    [SerializeField]
    private float _paddingTop = 0f;
    [SerializeField]
    private float _paddingBottom = 0f;
    [SerializeField]
    private float _itemSpacing = 0f;

    private void Awake()
    {
        if (_myTransform == null)
        {
            _myTransform = GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        int childCount = _myTransform.childCount;
        if (childCount != _lastChildCount)
        {
            _lastChildCount = childCount;

            float newHeight = 0f;

            foreach (RectTransform child in _myTransform)
            {
                newHeight += child.rect.height;
            }

            Rect current = _myTransform.rect;
            float itemSpacingOffset = _itemSpacing * (childCount - 1);
            _myTransform.sizeDelta = new Vector2(0, newHeight + _paddingBottom + _paddingTop + itemSpacingOffset);
        }
    }
}
