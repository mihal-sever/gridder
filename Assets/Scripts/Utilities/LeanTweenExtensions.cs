using System;
using UnityEngine;

namespace Sever.Gridder
{
    public static class LeanTweenExtensions
    {
        public static void Move(this RectTransform rect, Vector2 from, Vector2 to, float duration = 0, LeanTweenType easeType = LeanTweenType.notUsed,
            Action callback = null, float delay = 0)
        {
            LeanTween.value(rect.gameObject, from, to, duration).setEase(easeType).setOnUpdate(
                (Vector2 val) => { rect.anchoredPosition = val; }).setDelay(delay).setOnComplete(() => { callback?.Invoke(); });
        }
    }
}