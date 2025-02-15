using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DOTweenUIEffect
    : MonoBehaviour
{
    public enum TweenType { Move, Scale, Rotate, Fade }

    [Header("General Settings")]
    public TweenType tweenType = TweenType.Move;
    public Ease easeType = Ease.OutQuad;
    public float duration = 1f;
    public float delay = 0f;
    public bool loop = false;
    public int loopCount = -1; // -1 for infinite
    public LoopType loopType = LoopType.Yoyo;
    public bool playOnStart = true;

    [Header("Move Settings")]
    public Vector3 moveTarget;

    [Header("Scale Settings")]
    public Vector3 scaleTarget = Vector3.one;

    [Header("Rotate Settings")]
    public Vector3 rotateTarget;

    [Header("Fade Settings (Requires CanvasGroup)")]
    [Range(0, 1)] public float fadeTarget = 1f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Tween currentTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Add a CanvasGroup if using fade and none exists
        if (tweenType == TweenType.Fade && canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        if (playOnStart)
        {
            PlayTween();
        }
    }

    public void PlayTween()
    {
        if (currentTween != null) currentTween.Kill(); // Kill any existing tween

        switch (tweenType)
        {
            case TweenType.Move:
                currentTween = rectTransform.DOAnchorPos(moveTarget, duration)
                    .SetEase(easeType)
                    .SetLoops(loop ? loopCount : 0, loopType)
                    .SetDelay(delay);
                break;

            case TweenType.Scale:
                currentTween = rectTransform.DOScale(scaleTarget, duration)
                    .SetEase(easeType)
                    .SetLoops(loop ? loopCount : 0, loopType)
                    .SetDelay(delay);
                break;

            case TweenType.Rotate:
                currentTween = rectTransform.DORotate(rotateTarget, duration, RotateMode.FastBeyond360)
                    .SetEase(easeType)
                    .SetLoops(loop ? loopCount : 0, loopType)
                    .SetDelay(delay);
                break;

            case TweenType.Fade:
                if (canvasGroup != null)
                {
                    currentTween = canvasGroup.DOFade(fadeTarget, duration)
                        .SetEase(easeType)
                        .SetLoops(loop ? loopCount : 0, loopType)
                        .SetDelay(delay);
                }
                break;
        }
    }

    public void StopTween()
    {
        if (currentTween != null) currentTween.Kill();
    }
}
