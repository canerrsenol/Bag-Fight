using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private Image tileImage;
    private InventoryItem currentItem;

    private void Awake()
    {
        tileImage = GetComponent<Image>();
    }

    public bool IsOccupied()
    {
        return currentItem != null;
    }

    public void OccupyTile(InventoryItem item)
    {
        currentItem = item;
        tileImage.color = Color.red;
    }

    public void ClearTile()
    {
        currentItem = null;
        tileImage.color = Color.white;
    }

    public void TileVisited()
    {
        if(currentItem != null) return;
        tileImage.color = Color.yellow;
    }

    public void TileVisitedExit()
    {
        if(currentItem != null) return;
        tileImage.color = Color.white;
    }
}
