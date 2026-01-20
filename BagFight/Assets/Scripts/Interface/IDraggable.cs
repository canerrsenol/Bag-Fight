using System.Collections.Generic;
using UnityEngine;

public interface IDraggable
{
    public Vector2 ItemSize { get; }
    List<Tile> VisitedTiles { get; set; }
    List<Tile> OccupiedTiles { get; set; }
    void OnBeginDrag();
    void OnDrag(Vector3 position);
    void OnEndDrag();
}