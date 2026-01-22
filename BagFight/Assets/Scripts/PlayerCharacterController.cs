using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    private Animator animator;

    void OnEnable()
    {
        globalEventsSO.InventoryEvents.OnItemUsed += OnItemUsed;
    }

    void OnDisable()
    {
        globalEventsSO.InventoryEvents.OnItemUsed -= OnItemUsed;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnItemUsed(Sprite sprite)
    {
        animator.SetTrigger("Throw");
        Debug.Log("Item used: " + sprite.name);
    }
}
