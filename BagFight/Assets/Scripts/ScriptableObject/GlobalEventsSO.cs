using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalEventsSO", menuName = "ScriptableObjects/GlobalEventsSO", order = 1)]
public class GlobalEventsSO : ScriptableObject
{
    public InventoryEvents InventoryEvents = new InventoryEvents();
}

public class InventoryEvents
{
    public Action<Sprite> OnItemUsed;
}