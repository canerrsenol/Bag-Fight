using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemDragController : MonoBehaviour
{
    private IDraggable currentDraggable;
    private float fingerDownTime;
    private bool isDragging = false;

    private const float dragStartHoldTime = 1f;

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
        // Basılı tutma saatini başlat
        fingerDownTime = Time.time;
        isDragging = false;

        // Parmağın bulunduğu konumdaki UI elemanını al (EventSystem ile)
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = finger.ScreenPosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            currentDraggable = results[0].gameObject.GetComponent<IDraggable>();
        }
    }

    private void HandleFingerUp(LeanFinger finger)
    {
        // Eğer drag sürüyorsa OnEndDrag çağır
        if (isDragging && currentDraggable != null)
        {
            currentDraggable.OnEndDrag();
        }

        // Drag durumunu sıfırla
        isDragging = false;
        currentDraggable = null;
        fingerDownTime = 0f;
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        if (currentDraggable == null)
            return;

        float holdTime = Time.time - fingerDownTime;

        if (holdTime < dragStartHoldTime)
            return;

        if (!isDragging)
        {
            isDragging = true;
            currentDraggable.OnBeginDrag();
        }

        currentDraggable.OnDrag(finger.ScreenPosition);
    }
}
