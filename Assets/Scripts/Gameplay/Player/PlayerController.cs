using Cinemachine;
using Michsky.UI.Reach;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MyGame.Gameplay.Player
{
    [RequireComponent(typeof(FireUnitComponent))]
    [RequireComponent(typeof(UAVComponent))]
    [RequireComponent(typeof(MoveComponent))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(ShieldComponent))]
    [RequireComponent(typeof(RebirthComponent))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(RaycastComponent))]
    [RequireComponent(typeof(CinemachineCollisionImpulseSource))]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;

        private CharacterAttribute characterData;
        private List<IAirplaneComponent> airplaneComponents = new List<IAirplaneComponent>();

        public CharacterAttribute CharacterData => characterData;

        public void Initialize(CharacterAttribute characterAttribute)
        {
            this.characterData = characterAttribute;
            ResetComponentsState();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            foreach (var airplaneComponent in airplaneComponents)
            {
                airplaneComponent.UpdateComponent();
            }
        }

        private void ResetComponentsState()
        {
            IAirplaneComponent[] comps = GetComponents<IAirplaneComponent>();
            foreach (var comp in comps)
            {
                if (!airplaneComponents.Contains(comp))
                {
                    comp.Initialize(this);
                    airplaneComponents.Add(comp);
                }
            }
        }

    }
}


