using UnityEngine;

// https://www.youtube.com/watch?v=6gAmI5fOELY
public static class RectTransformExtensions
{
    public static void SetWidth(this RectTransform self, float width)
    {
        self.sizeDelta = new Vector2(width, self.rect.height);
    }
}
