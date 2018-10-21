using UnityEngine;

public class ScrollingWall : MonoBehaviour
{
    public int direction = 1;

    private Renderer m_renderer = null;
    private Renderer ImageRenderer{get{return m_renderer ?? ( m_renderer = GetComponent<Renderer>()); }}
    void Update()
    {
        var scrollingSpeed = GameManager.instance.ScrollingSpeed;
        var offset = new Vector2(Time.time * scrollingSpeed * direction, 0);
        ImageRenderer.material.mainTextureOffset = offset;
    }
}