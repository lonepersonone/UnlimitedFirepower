using Michsky.UI.Reach;
using MyGame.Gameplay.Prop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class CollectComponent : MonoBehaviour, IAirplaneComponent
    {
        private Collider2D[] hits = new Collider2D[60];

        private ProgressBar rankBar;

        public void Initialize(PlayerController player)
        {
            rankBar = PlayerManager.Instance.RankBar;

            rankBar.SetRange(0, PlayerExperienceManager.Instance.GetRequiredExperienceForNextRank());

            PlayerExperienceManager.Instance.OnExperienceChanged += OnExperienceChanged;
            PlayerExperienceManager.Instance.OnRankChanged += OnRankChanged;
        }

        public void UpdateComponent()
        {
            CircleRaycast();
        }

        private void CircleRaycast()
        {
            int count = CircleCastNonAlloc(hits);
            for (int i = 0; i < count; i++)
            {
                IPropable propable = hits[i].GetComponent<IPropable>();
                propable.OnPickedUp(transform);
            }
        }

        private int CircleCastNonAlloc(Collider2D[] hits)
        {
            return Physics2D.OverlapCircleNonAlloc(transform.position, 60f, hits, LayerMask.GetMask("Prop"));
        }

        private void OnExperienceChanged(object sender, int value)
        {
            rankBar.SetValue(value);
        }

        private void OnRankChanged(object sender, RankChangedEventArgs e)
        {
            rankBar.SetRange(0, e.requiredExperience);
        }

    }
}


