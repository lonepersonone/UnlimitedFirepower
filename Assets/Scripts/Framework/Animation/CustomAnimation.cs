using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace MyGame.Framework.Animation
{
    using UnityEngine;
    using DG.Tweening;
    using System;
    using TMPro;
    using UnityEngine.UI;

    [RequireComponent(typeof(CanvasGroup))]
    public class CustomAnimation : MonoBehaviour
    {
        // 动画阶段
        public enum AnimationPhase { Show, Hover, Hide }

        // 动画阶段配置
        [Serializable]
        public class PhaseSettings
        {
            public bool enabled = true;
            public float duration = 1f;
            public float delay = 0f;
            public Ease easeType = Ease.OutQuad;

            // 动画偏移值（基于上一阶段结束状态）
            public bool animatePosition = true;
            public Vector3 positionOffset = Vector3.zero;

            public bool animateScale = true;
            public Vector3 scaleMultiplier = Vector3.one;

            public bool animateColor = true;
            public Color colorOffset = Color.white;

            public bool animateAlpha = true;
            [Range(0f, 1f)] public float alphaOffset = 0f;

            public bool animateRotation = false;
            public Vector3 rotationOffset = Vector3.zero;
        }

        [Header("动画配置")]
        public PhaseSettings showSettings = new PhaseSettings();
        public PhaseSettings hoverSettings = new PhaseSettings();
        public PhaseSettings hideSettings = new PhaseSettings();

        [Header("自动播放设置")]
        public bool playOnEnable = true;
        public bool autoPlayHover = true;
        public bool autoPlayHide = false;
        public float autoHideDelay = 0f;

        [Header("目标组件引用")]
        [Tooltip("手动指定TextMeshPro组件")]
        public TMP_Text targetTextMeshPro;

        [Tooltip("手动指定Image组件")]
        public Image targetImage;

        [Tooltip("手动指定Renderer组件")]
        public Renderer targetRenderer;

        // 事件接口
        public Action<AnimationPhase> OnPhaseStarted;
        public Action<AnimationPhase> OnPhaseCompleted;
        public Action OnAllPhasesCompleted;

        private CanvasGroup canvasGroup;
        private Tween currentTween;
        private AnimationPhase currentPhase;
        private bool isPlaying = false;

        // 保存上一阶段的结束状态
        private Vector3 lastPosition;
        private Vector3 lastScale;
        private Color lastColor = Color.white;
        private float lastAlpha;
        private Quaternion lastRotation;

        // 当前生效的颜色目标类型
        private enum ColorTargetType { None, TextMeshPro, Image, Renderer }
        private ColorTargetType activeColorTargetType = ColorTargetType.None;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            DetermineActiveColorTarget();
            ResetLastState();
        }

        void OnEnable()
        {
            if (playOnEnable && !isPlaying)
                PlayShowPhase();
        }

        void OnDisable()
        {
            StopAllAnimations();
        }

        // 确定当前生效的颜色目标类型
        private void DetermineActiveColorTarget()
        {
            if (targetTextMeshPro != null)
            {
                activeColorTargetType = ColorTargetType.TextMeshPro;
                lastColor = targetTextMeshPro.color;
            }
            else if (targetImage != null)
            {
                activeColorTargetType = ColorTargetType.Image;
                lastColor = targetImage.color;
            }
            else if (targetRenderer != null)
            {
                activeColorTargetType = ColorTargetType.Renderer;
                lastColor = targetRenderer.material.color;
            }
            else
            {
                activeColorTargetType = ColorTargetType.None;
                Debug.LogWarning("CustomAnimation: 未找到有效的颜色目标组件，请确保至少指定一个TextMeshPro、Image或Renderer");
            }
        }

        //===== 外部控制接口 =====

        public void PlayShowPhase()
        {
            if (!showSettings.enabled)
            {
                OnPhaseCompleted?.Invoke(AnimationPhase.Show);
                if (autoPlayHover) PlayHoverPhase();
                return;
            }

            StopAllAnimations();
            currentPhase = AnimationPhase.Show;
            isPlaying = true;
            OnPhaseStarted?.Invoke(currentPhase);

            Sequence sequence = CreatePhaseSequence(showSettings);
            sequence.OnComplete(() => {
                UpdateLastState();
                isPlaying = false;
                OnPhaseCompleted?.Invoke(currentPhase);

                if (autoPlayHover)
                    Invoke(nameof(PlayHoverPhase), autoHideDelay);
            });

            currentTween = sequence;
        }

        public void PlayHoverPhase()
        {
            if (!hoverSettings.enabled)
            {
                OnPhaseCompleted?.Invoke(AnimationPhase.Hover);
                if (autoPlayHide) PlayHidePhase();
                return;
            }

            StopAllAnimations();
            currentPhase = AnimationPhase.Hover;
            isPlaying = true;
            OnPhaseStarted?.Invoke(currentPhase);

            Sequence sequence = CreatePhaseSequence(hoverSettings);
            sequence.SetLoops(-1, LoopType.Yoyo); // 悬停默认循环
            sequence.OnUpdate(() => {
                // 悬停动画每帧更新
            });
            sequence.OnComplete(() => {
                UpdateLastState();
                isPlaying = false;
                OnPhaseCompleted?.Invoke(currentPhase);

                if (autoPlayHide)
                    Invoke(nameof(PlayHidePhase), autoHideDelay);
            });

            currentTween = sequence;
        }

        public void PlayHidePhase()
        {
            if (!hideSettings.enabled)
            {
                OnPhaseCompleted?.Invoke(AnimationPhase.Hide);
                OnAllPhasesCompleted?.Invoke();
                return;
            }

            StopAllAnimations();
            currentPhase = AnimationPhase.Hide;
            isPlaying = true;
            OnPhaseStarted?.Invoke(currentPhase);

            Sequence sequence = CreatePhaseSequence(hideSettings);
            sequence.OnComplete(() => {
                UpdateLastState();
                isPlaying = false;
                OnPhaseCompleted?.Invoke(currentPhase);
                OnAllPhasesCompleted?.Invoke();
            });

            currentTween = sequence;
        }

        public void StopAllAnimations()
        {
            if (currentTween != null)
            {
                currentTween.Kill();
                currentTween = null;
            }

            isPlaying = false;
        }

        public void ResetToInitialState()
        {
            ResetLastState();
            ApplyLastState();
        }

        //===== 内部实现 =====

        private void ResetLastState()
        {
            lastPosition = transform.localPosition;
            lastScale = transform.localScale;
            lastRotation = transform.localRotation;
            lastAlpha = canvasGroup.alpha;

            // 根据当前活动类型重置颜色
            switch (activeColorTargetType)
            {
                case ColorTargetType.TextMeshPro:
                    lastColor = targetTextMeshPro.color;
                    break;
                case ColorTargetType.Image:
                    lastColor = targetImage.color;
                    break;
                case ColorTargetType.Renderer:
                    lastColor = targetRenderer.material.color;
                    break;
                default:
                    lastColor = Color.white;
                    break;
            }
        }

        private void UpdateLastState()
        {
            lastPosition = transform.localPosition;
            lastScale = transform.localScale;
            lastRotation = transform.localRotation;
            lastAlpha = canvasGroup.alpha;

            // 根据当前活动类型更新颜色
            switch (activeColorTargetType)
            {
                case ColorTargetType.TextMeshPro:
                    lastColor = targetTextMeshPro.color;
                    break;
                case ColorTargetType.Image:
                    lastColor = targetImage.color;
                    break;
                case ColorTargetType.Renderer:
                    lastColor = targetRenderer.material.color;
                    break;
            }
        }

        private void ApplyLastState()
        {
            transform.localPosition = lastPosition;
            transform.localScale = lastScale;
            transform.localRotation = lastRotation;
            canvasGroup.alpha = lastAlpha;

            // 根据当前活动类型应用颜色
            switch (activeColorTargetType)
            {
                case ColorTargetType.TextMeshPro:
                    targetTextMeshPro.color = lastColor;
                    break;
                case ColorTargetType.Image:
                    targetImage.color = lastColor;
                    break;
                case ColorTargetType.Renderer:
                    targetRenderer.material.color = lastColor;
                    break;
            }
        }

        private Sequence CreatePhaseSequence(PhaseSettings settings)
        {
            // 计算目标值（基于上一阶段结束状态）
            Vector3 targetPosition = settings.animatePosition ? lastPosition + settings.positionOffset : lastPosition;
            Vector3 targetScale = settings.animateScale ?
                new Vector3(
                    lastScale.x * settings.scaleMultiplier.x,
                    lastScale.y * settings.scaleMultiplier.y,
                    lastScale.z * settings.scaleMultiplier.z
                ) : lastScale;

            Color targetColor = settings.animateColor ? lastColor + settings.colorOffset : lastColor;
            float targetAlpha = settings.animateAlpha ? Mathf.Clamp01(lastAlpha + settings.alphaOffset) : lastAlpha;
            Vector3 targetRotation = settings.animateRotation ? lastRotation.eulerAngles + settings.rotationOffset : lastRotation.eulerAngles;

            Sequence sequence = DOTween.Sequence();
            sequence.SetDelay(settings.delay);
            sequence.SetEase(settings.easeType);

            // 位置动画
            if (settings.animatePosition)
            {
                sequence.Append(transform.DOLocalMove(targetPosition, settings.duration));
            }

            // 缩放动画
            if (settings.animateScale)
            {
                sequence.Join(transform.DOScale(targetScale, settings.duration));
            }

            // 颜色动画
            if (settings.animateColor && activeColorTargetType != ColorTargetType.None)
            {
                switch (activeColorTargetType)
                {
                    case ColorTargetType.TextMeshPro:
                        sequence.Join(targetTextMeshPro.DOColor(targetColor, settings.duration));
                        break;
                    case ColorTargetType.Image:
                        sequence.Join(targetImage.DOColor(targetColor, settings.duration));
                        break;
                    case ColorTargetType.Renderer:
                        sequence.Join(targetRenderer.material.DOColor(targetColor, settings.duration));
                        break;
                }
            }

            // 透明度动画
            if (settings.animateAlpha)
            {
                sequence.Join(canvasGroup.DOFade(targetAlpha, settings.duration));
            }

            // 旋转动画
            if (settings.animateRotation)
            {
                sequence.Join(transform.DORotate(targetRotation, settings.duration));
            }

            return sequence;
        }
    }


}


