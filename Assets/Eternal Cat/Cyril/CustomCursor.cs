using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField]
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        Cursor.SetCursor(ResizeCursor(cursorTexture, 100, 100), hotSpot, cursorMode);
    }

    private Texture2D ResizeCursor(Texture2D original, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        RenderTexture.active = rt;
        Graphics.Blit(original, rt);
        Texture2D result = new Texture2D(width, height);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();
        RenderTexture.active = null;
        rt.Release();
        return result;
    }
}
