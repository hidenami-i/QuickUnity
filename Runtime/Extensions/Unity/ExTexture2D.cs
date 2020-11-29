using UnityEngine;

namespace QuickUnity.Extensions.Unity
{
    public static class ExTexture2D
    {
        public static Sprite ToSprite(this Texture2D texture2D, Rect rect, Vector2 pivot)
        {
            return Sprite.Create(texture2D, rect, pivot);
        }
    }
}
