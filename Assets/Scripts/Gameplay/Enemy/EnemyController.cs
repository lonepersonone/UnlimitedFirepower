using Cinemachine;
using MyGame.Framework.Audio;
using MyGame.Framework.Record;
using MyGame.Gameplay.Level;
using MyGame.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MyGame.Gameplay.Enemy 
{
    [RequireComponent(typeof(WealthComponentE))]
    [RequireComponent(typeof(MoveComponentE))]
    [RequireComponent(typeof(DynamicTextComponentE))]
    [RequireComponent(typeof(EffectComponentE))]
    [RequireComponent(typeof(HealthComponentE))]
    [RequireComponent(typeof(RaycastComponentE))]
    [RequireComponent(typeof(AttackComponentE))]
    [RequireComponent(typeof(MinimapComponentE))]
    public class EnemyController : MonoBehaviour, IEnemy
    {
        public Transform[] FirePoints;

        protected IEnemyState currentState;

        protected CharacterAttribute characterData;

        protected List<IBehavoir> behaviors = new List<IBehavoir>();

        protected List<IShipComponentE> shipComponentEs = new List<IShipComponentE>();
        protected WealthComponentE wealthComponentE;
        protected MoveComponentE moveComponentE;
        protected DynamicTextComponentE dynamicTextComponentE;
        protected EffectComponentE effectComponentE;
        protected HealthComponentE healthComponentE;
        protected RaycastComponentE raycastComponentE;
        protected AttackComponentE attackComponentE;
        protected MinimapComponentE minimapComponentE;

        public IEnemyState CurrentState => currentState;
        public CharacterAttribute CharacterData => characterData;
        public WealthComponentE WealthComponentE => wealthComponentE;
        public MoveComponentE MoveComponentE => moveComponentE;
        public DynamicTextComponentE DynamicTextComponentE => dynamicTextComponentE;
        public EffectComponentE EffectComponentE => effectComponentE;
        public HealthComponentE HealthComponentE => healthComponentE;
        public RaycastComponentE RaycastComponentE => raycastComponentE;
        public AttackComponentE AttackComponentE => attackComponentE;
        public MinimapComponentE MinimapComponentE => minimapComponentE;

        void Update()
        {
            currentState?.Update();

            foreach (var shipComponentE in shipComponentEs)
            {
                shipComponentE.UpdateComponent();
            }
        }

        public virtual void Initialize(CharacterAttribute characterAttribute)
        {
            this.characterData = characterAttribute;
            ResetComponentsState();
            SetState(new IdleStateE(this));
            effectComponentE.CreateSpawnEffect(transform.position);
            minimapComponentE.CreateMinimapIcon();
        }

        public virtual void Attack() { }

        public virtual void Die()
        {
            effectComponentE.CreateDieEffect(transform.position);
            wealthComponentE.CreateWealth();
            //TriggerBehaviors(BehaviorType.Splite);
            minimapComponentE.DestroyMinimapIcon();

            AudioHelper.PlayOneShot(gameObject, AudioIDManager.GetAudioID(Framework.Audio.AudioType.Enemy, AudioAction.Explode), transform.position);

            LevelManager.Instance.ReduceEnemy(this.gameObject);

            RecordDataManager.Instance.UpdateEnemiesKilled(1);           
        }

        public void SetState(IEnemyState state)
        {
            currentState?.Exit();
            currentState = state;
            currentState?.Enter();
        }

        public void AddBehavior(IBehavoir behavoir)
        {
            behaviors.Add(behavoir);
        }

        public void RemoveBehavior(BehaviorType type)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                if (behaviors[i].BehaviorType == type) behaviors.Remove(behaviors[i]);
            }
        }

        public void PerformBehaviors(BehaviorType type)
        {
            foreach (var behavior in behaviors)
            {
                if (behavior.BehaviorType == type)
                    behavior.ExecuteBehavior(this);
            }
        }

        public void TriggerBehaviors(BehaviorType type)
        {
            PerformBehaviors(type);
        }

        private void ResetComponentsState()
        {
            moveComponentE = RegisterComponent<MoveComponentE>();
            wealthComponentE = RegisterComponent<WealthComponentE>();
            dynamicTextComponentE = RegisterComponent<DynamicTextComponentE>();
            effectComponentE = RegisterComponent<EffectComponentE>();
            healthComponentE = RegisterComponent<HealthComponentE>();
            raycastComponentE = RegisterComponent<RaycastComponentE>();
            attackComponentE = RegisterComponent<AttackComponentE>();
            minimapComponentE = RegisterComponent<MinimapComponentE>();
        }

        private T RegisterComponent<T>() where T : MonoBehaviour, IShipComponentE
        {
            T component = GetComponent<T>();
            component.Initialize(this);
            shipComponentEs.Add(component); // 添加到接口列表
            return component; // 返回具体类型
        }


    }

}



