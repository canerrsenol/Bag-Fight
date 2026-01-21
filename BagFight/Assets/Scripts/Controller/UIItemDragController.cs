using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemDragController : MonoBehaviour
{
    private IDraggable currentDraggable;
    private float fingerDownTime;
    private bool isDragging = false;
    private bool isHolding = false;

    private const float dragStartHoldTime = .15f;

    void OnEnable()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp += HandleFingerUp;
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        fingerDownTime = Time.time;
        isDragging = false;
        isHolding = true;

        CastForDraggable(finger);
    }

    private void CastForDraggable(LeanFinger finger)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = finger.ScreenPosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            currentDraggable = results[0].gameObject.GetComponentInParent<IDraggable>();
        }
    }

    private void HandleFingerUp(LeanFinger finger)
    {
        if (isDragging && currentDraggable != null)
        {
            currentDraggable.OnEndDrag();
        }

        isDragging = false;
        isHolding = false;
        currentDraggable = null;
        fingerDownTime = 0f;
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        if (!isHolding)
            return;

        if (!isDragging)
        {
            CastForDraggable(finger);

            if (currentDraggable == null)
            {
                fingerDownTime = Time.time;
                return;
            }

            float holdTime = Time.time - fingerDownTime;

            if (holdTime < dragStartHoldTime)
                return;

            isDragging = true;
            currentDraggable.OnBeginDrag();
        }

        currentDraggable.OnDrag(finger.ScreenPosition);
    }
}
