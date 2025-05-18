using MyGame.Gameplay.Player;
using MyGame.Scene.Main;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame.Gameplay.Prop
{
    public class WealthController : MonoBehaviour, IPropable
    {
        private Transform target;

        private int direction = 1;
        private Vector3 MinEmergePosition = new Vector3(0, 1, 0);
        private Vector3 MaxEmergePosition = new Vector3(2, 5, 0);
        private float EmergeTime = 0.3f;
        private Vector3 MinFadePosition = new Vector3(0, -1, 0);
        private Vector3 MaxFadePosition = new Vector3(2, -5, 0);
        private float FadeTime = 0.3f;

        private void Update()
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, 20 * Time.deltaTime);
                if (Vector3.Distance(transform.position, target.position) < 0.1f) OnPickedDown();

            }
        }

        public void Initialize(WealthData data)
        {
            MinEmergePosition = data.MinEmergePosition;
            MaxEmergePosition = data.MaxEmergePosition;
            EmergeTime = data.EmergeTime;
            MinFadePosition = data.MinFadePosition;
            MaxFadePosition = data.MaxFadePosition;
            FadeTime = data.FadeTime;

            if (Random.Range(0, 2) == 0) direction = 1;
            else direction = -1;

            StartCoroutine(EmergeAnimation(EmergeTime));
        }

        public void OnPickedUp(Transform target)
        {
            this.target = target;
        }

        private void OnPickedDown()
        {
            PlayerExperienceManager.Instance.AddExperience(1);
            Destroy(this.gameObject);
        }

        private IEnumerator EmergeAnimation(float emergeTime)
        {
            float time = 0;
            float offsetX = Random.Range(MinEmergePosition.x, MaxEmergePosition.x) * direction;
            float offsetY = Random.Range(MinEmergePosition.y, MaxEmergePosition.y);
            Vector3 startPos = transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x + offsetX, startPos.y + offsetY, startPos.z);

            while (time <= emergeTime)
            {
                float ratio = time / emergeTime;
                transform.localPosition = Vector3.Lerp(startPos, endPos, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }

            StartCoroutine(FadeAnimation(FadeTime));
        }


        private IEnumerator FadeAnimation(float fadeTime)
        {
            float time = 0;
            float offsetX = Random.Range(MinFadePosition.x, MaxFadePosition.x) * direction;
            float offsetY = Random.Range(MinFadePosition.y, MaxFadePosition.y);
            Vector3 startPos = transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x + offsetX, startPos.y + offsetY, 0);
            while (time <= fadeTime)
            {
                float ratio = time / fadeTime;
                transform.localPosition = Vector3.Lerp(startPos, endPos, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
        }

    }
}


