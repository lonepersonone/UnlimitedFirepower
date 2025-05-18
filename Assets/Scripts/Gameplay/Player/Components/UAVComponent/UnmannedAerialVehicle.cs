using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


namespace MyGame.Gameplay.Player
{
    [RequireComponent(typeof(MoveComponentU))]
    [RequireComponent(typeof(RaycastComponentU))]
    [RequireComponent(typeof(AttackComponentU))]
    [RequireComponent(typeof(HealthComponentU))]
    public class UnmannedAerialVehicle : MonoBehaviour
    {
        private CharacterAttribute characterData;

        private IUAVState currentState;

        protected List<IUAVComponent> uavComponents = new List<IUAVComponent>();
        protected MoveComponentU moveComponentU;
        protected RaycastComponentU raycastComponentU;
        protected AttackComponentU attackComponentU;
        protected HealthComponentU healthComponentU;

        public CharacterAttribute CharacterData => characterData;

        public MoveComponentU MoveComponentU => moveComponentU;
        public RaycastComponentU RaycastComponentU => raycastComponentU;
        public AttackComponentU AttackComponentU => attackComponentU;
        public HealthComponentU HealthComponentU => healthComponentU;

        private void Update()
        {
            currentState?.Update();

            foreach (var com in uavComponents) com.UpdateComponent();            
        }

        public void Initialize(CharacterAttribute data)
        {
            this.characterData = data;

            ResetComponentsState();

            SetState(new IdleStateU(this));
        }

        public void SetState(IUAVState state)
        {
            currentState?.Exit();
            currentState = state;
            currentState?.Enter();
        }

        private void ResetComponentsState()
        {
            moveComponentU = RegisterComponent<MoveComponentU>();
            raycastComponentU = RegisterComponent<RaycastComponentU>();
            attackComponentU = RegisterComponent<AttackComponentU>();
            healthComponentU = RegisterComponent<HealthComponentU>();
        }

        private T RegisterComponent<T>() where T : MonoBehaviour, IUAVComponent
        {
            T component = GetComponent<T>();
            component.Initialize(this);
            uavComponents.Add(component);
            return component;
        }

    }
}



