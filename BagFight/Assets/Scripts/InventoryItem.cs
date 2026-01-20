using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IDraggable
{
    [SerializeField] private float cooldownTime = 1.0f;
    [SerializeField] private Vector2 itemSize = new Vector2(1, 1);
    private float lastUsedTime = -Mathf.Infinity;
    public Vector2 ItemSize => itemSize;

    public List<Tile> VisitedTiles { get; set; } = new List<Tile>();
    public List<Tile> OccupiedTiles { get; set; } = new List<Tile>();

    private Image image;
    private RectTransform rectTransform;

    private Vector3[] corners;
    private const float tileSize = 100f; // Assuming each tile is 100x100 pixels

    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = transform.parent.GetComponent<RectTransform>();
        corners = new Vector3[(int)(itemSize.x * itemSize.y)];
    }

    public void OnBeginDrag()
    {
        transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0), 0.3f, 1, 1);

        // Clear occupied tiles
        if(OccupiedTiles.Count > 0)
        {
            foreach (Tile tile in OccupiedTiles)
            {
                tile.ClearTile();
            }
            OccupiedTiles.Clear();
        }
    }

    public void OnDrag(Vector3 position)
    {
        transform.parent.position = position;

        
        for (int i = 0; i < itemSize.x; i++)
        {
            for (int j = 0; j < itemSize.y; j++)
            {
                int index = (int)(i * itemSize.y + j);
                corners[index] = new Vector3(
                    rectTransform.position.x - (itemSize.x * tileSize) / 2 + (i + 0.5f) * tileSize,
                    rectTransform.position.y - (itemSize.y * tileSize) / 2 + (j + 0.5f) * tileSize,
                    0);
            }
        }

        List<Tile> currentTiles = new List<Tile>();
        foreach (Vector3 corner in corners)
        {
            Tile tile = RaycastForTile(corner);
            if (tile != null && !currentTiles.Contains(tile))
            {
                currentTiles.Add(tile);
            }
        }

        // Add new tiles and call TileVisited
        foreach (Tile tile in currentTiles)
        {
            if (!VisitedTiles.Contains(tile))
            {
                VisitedTiles.Add(tile);
                tile.TileVisited();
            }
        }

        // Remove tiles that are no longer visited and call TileVisitedExit
        List<Tile> tilesToRemove = new List<Tile>();
        foreach (Tile tile in VisitedTiles)
        {
            if (!currentTiles.Contains(tile))
            {
                tilesToRemove.Add(tile);
            }
        }

        foreach (Tile tile in tilesToRemove)
        {
            VisitedTiles.Remove(tile);
            tile.TileVisitedExit();
        }
    }

    private Tile RaycastForTile(Vector3 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Tile tile = result.gameObject.GetComponent<Tile>();
            if (tile != null)
            {
                return tile;
            }
        }

        return null;
    }

    public void OnEndDrag()
    {
        // Check if item can be placed on visited tiles if so occupy them
        // Otherwise return to original position

        if(VisitedTiles.Count == 0)
        {
            // No tiles visited, return to original position
            return;
        }

        if(CanPlaceItem())
        {
            transform.parent.DOMove(VisitedTiles[0].transform.position, 0.1f);

            foreach (Tile tile in VisitedTiles)
            {
                tile.OccupyTile(this);
                OccupiedTiles.Add(tile);
            }

            VisitedTiles.Clear();
        }
    }

    private bool CanPlaceItem()
    {
        foreach (Tile tile in VisitedTiles)
        {
            if (tile.IsOccupied())
                return false;
        }
        return true;
    }
}
