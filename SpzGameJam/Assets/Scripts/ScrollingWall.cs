using UnityEngine;

public class ScrollingWall : MonoBehaviour
{
    public int direction = 1;

    void Update()
    {
        var scrollingSpeed = GameManager.instance.ScrollingSpeed;
        var offset = new Vector2(Time.time * scrollingSpeed * direction, 0);
        var rend = GetComponent<Renderer>();
        rend.material.mainTextureOffset = offset;
    }
}