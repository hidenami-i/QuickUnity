using UnityEngine;
using UnityEngine.UI;

namespace QuickUnity.Extensions.Unity
{
    public static class ExRawImage
    {
        public static float GetUvRectX(this RawImage self) => self.uvRect.x;
        public static float GetUvRectY(this RawImage self) => self.uvRect.y;
        public static float GetUvRectWidth(this RawImage self) => self.uvRect.width;
        public static float GetUvRectHeight(this RawImage self) => self.uvRect.height;

        public static void SetUvRectX(this RawImage self, float value)
        {
            Rect uvRect = self.uvRect;
            uvRect.x = value;
            self.uvRect = uvRect;
        }

        public static void SetUvRectY(this RawImage self, float value)
        {
            Rect uvRect = self.uvRect;
            uvRect.y = value;
            self.uvRect = uvRect;
        }

        public static void SetUvRectWidth(this RawImage self, float value)
        {
            Rect uvRect = self.uvRect;
            uvRect.width = value;
            self.uvRect = uvRect;
        }

        public static void SetUvRectHeight(this RawImage self, float value)
        {
            Rect uvRect = self.uvRect;
            uvRect.height = value;
            self.uvRect = uvRect;
        }

        /// <summary>
        /// Modify the size of the RawImage to fit the aspect ratio. The current UI size will be the standard.
        /// </summary>
        /// <see cref="https://github.com/nkjzm/FixAspectSample/tree/master"/>
        public static void FixAspect(this RawImage self)
        {
            self.FixAspect(self.rectTransform.rect.size);
        }

        /// <summary>
        /// Modify the size of RawImage to fit the aspect ratio.
        /// </summary>
        /// <param name="originalSize">Reference UI size</param>
        /// <see cref="https://github.com/nkjzm/FixAspectSample/tree/master"/>
        public static void FixAspect(this RawImage self, Vector3 originalSize)
        {
            Vector2 textureSize = new Vector2(self.texture.width, self.texture.height);

            float heightScale = originalSize.y / textureSize.y;
            float widthScale = originalSize.x / textureSize.x;
            Vector2 rectSize = textureSize * Mathf.Min(heightScale, widthScale);

            Vector2 anchorDiff = self.rectTransform.anchorMax - self.rectTransform.anchorMin;
            Vector2 parentSize = (self.transform.parent as RectTransform).rect.size;
            Vector2 anchorSize = parentSize * anchorDiff;

            self.rectTransform.sizeDelta = rectSize - anchorSize;
        }
    }
}