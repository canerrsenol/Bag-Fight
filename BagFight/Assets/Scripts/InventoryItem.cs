using UnityEngine;

public class InventoryItem : MonoBehaviour, IDraggable
{
    public void OnBeginDrag()
    {
        Debug.Log("Drag started on " + gameObject.name);
    }

    public void OnDrag(Vector3 position)
    {
        // Transform'u güncellenmiş pozisyona taşı
        transform.position = position;
    }

    public void OnEndDrag()
    {
        Debug.Log("Drag ended on " + gameObject.name);
    }
}
