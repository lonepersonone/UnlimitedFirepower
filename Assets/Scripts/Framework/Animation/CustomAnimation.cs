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
        // �����׶�
        public enum AnimationPhase { Show, Hover, Hide }

        // �����׶�����
        [Serializable]
        public class PhaseSettings
        {
            public bool enabled = true;
            public float duration = 1f;
            public float delay = 0f;
            public Ease easeType = Ease.OutQuad;

            // ����ƫ��ֵ��������һ�׶ν���״̬��
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

        [Header("��������")]
        public PhaseSettings showSettings = new PhaseSettings();
        public PhaseSettings hoverSettings = new PhaseSettings();
        public PhaseSettings hideSettings = new PhaseSettings();

        [Header("�Զ���������")]
        public bool playOnEnable = true;
        public bool autoPlayHover = true;
        public bool autoPlayHide = false;
        public float autoHideDelay = 0f;

        [Header("Ŀ���������")]
        [Tooltip("�ֶ�ָ��TextMeshPro���")]
        public TMP_Text targetTextMeshPro;

        [Tooltip("�ֶ�ָ��Image���")]
        public Image targetImage;

        [Tooltip("�ֶ�ָ��Renderer���")]
        public Renderer targetRenderer;

        // �¼��ӿ�
        public Action<AnimationPhase> OnPhaseStarted;
        public Action<AnimationPhase> OnPhaseCompleted;
        public Action OnAllPhasesCompleted;

        private CanvasGroup canvasGroup;
        private Tween currentTween;
        private AnimationPhase currentPhase;
        private bool isPlaying = false;

        // ������һ�׶εĽ���״̬
        private Vector3 lastPosition;
        private Vector3 lastScale;
        private Color lastColor = Color.white;
        private float lastAlpha;
        private Quaternion lastRotation;

        // ��ǰ��Ч����ɫĿ������
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

        // ȷ����ǰ��Ч����ɫĿ������
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
                Debug.LogWarning("CustomAnimation: δ�ҵ���Ч����ɫĿ���������ȷ������ָ��һ��TextMeshPro��Image��Renderer");
            }
        }

        //===== �ⲿ���ƽӿ� =====

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
            sequence.SetLoops(-1, LoopType.Yoyo); // ��ͣĬ��ѭ��
            sequence.OnUpdate(() => {
                // ��ͣ����ÿ֡����
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

        //===== �ڲ�ʵ�� =====

        private void ResetLastState()
        {
            lastPosition = transform.localPosition;
            lastScale = transform.localScale;
            lastRotation = transform.localRotation;
            lastAlpha = canvasGroup.alpha;

            // ���ݵ�ǰ�����������ɫ
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

            // ���ݵ�ǰ����͸�����ɫ
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

            // ���ݵ�ǰ�����Ӧ����ɫ
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
            // ����Ŀ��ֵ��������һ�׶ν���״̬��
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

            // λ�ö���
            if (settings.animatePosition)
            {
                sequence.Append(transform.DOLocalMove(targetPosition, settings.duration));
            }

            // ���Ŷ���
            if (settings.animateScale)
            {
                sequence.Join(transform.DOScale(targetScale, settings.duration));
            }

            // ��ɫ����
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

            // ͸���ȶ���
            if (settings.animateAlpha)
            {
                sequence.Join(canvasGroup.DOFade(targetAlpha, settings.duration));
            }

            // ��ת����
            if (settings.animateRotation)
            {
                sequence.Join(transform.DORotate(targetRotation, settings.duration));
            }

            return sequence;
        }
    }


}


