using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Synchronizes the height of a ScrollView's content Rect Transform based on its children's height.
/// This is useful because Unity does not do this by default.
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(VerticalLayoutGroup), typeof(RectTransform))]
public class ScrollingHeightSync : MonoBehaviour
{
    private int _lastChildCount;

    [SerializeField]
    private RectTransform _myTransform;
    [SerializeField]
    private VerticalLayoutGroup _layoutGroupSettings;
    
    private void Update()
    {
        int childCount = _myTransform.childCount;
        if (childCount != _lastChildCount)
        {
            _lastChildCount = childCount;

            float newHeight = 0f;

            // loop through the children to get the "catalyst" sizing
            foreach (RectTransform child in _myTransform)
            {
                newHeight += child.rect.height;
            }

            // get the current sizing to assign the new data
            Rect current = _myTransform.rect;
            // We need to calculate the spacing height between the children to appropriately offset the total height
            float itemSpacingOffset = _layoutGroupSettings.spacing * (childCount - 1);
            // We also want to account for the view padding - so we pull the top & bottom
            float viewPadding = _layoutGroupSettings.padding.top + _layoutGroupSettings.padding.bottom;
            // assign the current X delta, since we only care about height
            // add the top & bottom padding with the offset and total children height to have the "real" new total height
            _myTransform.sizeDelta = new Vector2(
                _myTransform.sizeDelta.x,
                newHeight + viewPadding + itemSpacingOffset
            );
        }
    }
}
