using MyGame.Data.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Effect
{
    public interface IEffectPlayer
    {
        GameObject PlayEffect(EffectConfig config, Vector3 position, Quaternion rotation = default);
        GameObject PlayEffect(EffectConfig config, Transform parent, Vector3 localPosition = default, Quaternion localRotation = default);
        void StopEffect(GameObject effectInstance);
        void StopAllEffects();
    }
}


