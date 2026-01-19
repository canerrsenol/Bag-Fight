using UnityEngine;

public class FitBackgroundToCamera : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        if (srs == null || srs.Length == 0) return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Bounds combined = srs[0].bounds;
        for (int i = 1; i < srs.Length; i++)
        {
            combined.Encapsulate(srs[i].bounds);
        }

        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        Vector3 spriteSize = combined.size;
        if (spriteSize.x == 0f || spriteSize.y == 0f) return;

        transform.localScale = new Vector3(
            screenWidth / spriteSize.x,
            screenHeight / spriteSize.y,
            1f
        );
    }
}
