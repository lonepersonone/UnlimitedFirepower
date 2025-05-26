using Michsky.UI.Reach;
using MyGame.Framework.Event;
using MyGame.Framework.Manager;
using MyGame.Framework.Notification;
using MyGame.Gameplay.Effect;
using MyGame.Scene.BattleRoom;
using MyGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class PlayerManager : GameSystemBase
    {
        public static PlayerManager Instance;

        [SerializeField] private string[] rankUpMessages = new string[100];

        [Header("UI")]
        public ProgressBar RankBar;
        public ProgressBar HealthBar;
        public ProgressBar ShieldBar;
        public ProgressBar ShieldProgressBar;
        public ProgressBar ThrusterBar;

        public TextMeshProUGUI MoveSpeedText;
        public TextMeshProUGUI CoolTimeText;
        public TextMeshProUGUI ThrusterRatioText;
        public TextMeshProUGUI ThrusterDurationText;
        public TextMeshProUGUI DamageReductionText;
        public TextMeshProUGUI DamageText;
        public TextMeshProUGUI CriticalProbabilityText;
        public TextMeshProUGUI CriticalRatioText;
        public TextMeshProUGUI RangeText;
        public TextMeshProUGUI FireRateText;
        public TextMeshProUGUI AttenuationRatioText;

        private GameObject currentShip;
        private CharacterAttribute characterData;
        private PlayerAttribute playerData;

        private void Awake()
        {
            Instance = this;         
        }

        private void Update()
        {
            if (currentShip != null && characterData != null) UpdateText();

            if (Input.GetKeyDown(KeyCode.N))
            {
                PlayerExperienceManager.Instance.AddExperience(500);
            }
        }
      
        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            playerData = BattleDataManager.Instance.PlayerAttribute;
            characterData = BattleDataManager.Instance.PlayerAttribute.ShipAttribute;
            GameEventManager.RegisterListener(GameEventType.BattleStarted, SpawnPlayerAnimation);
            GameEventManager.RegisterListener(GameEventType.BattleInitial, CreateAirplane);

            PlayerExperienceManager.Instance.OnRankUp += HandleRankUp;

            await Task.Delay(100);

            IsReady = true;
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.BattleInitial, CreateAirplane);
            GameEventManager.UnregisterListener(GameEventType.BattleStarted, SpawnPlayerAnimation);

            PlayerExperienceManager.Instance.OnRankUp -= HandleRankUp;
        }

        public void CreateAirplane()
        {
            currentShip = UnityEngine.Object.Instantiate(characterData.CharacterPrefab);
            currentShip.GetComponent<PlayerController>().Initialize(characterData);
            currentShip.gameObject.SetActive(false);
            BattleCameraManager.Instance.SetPlayerCamera(currentShip.transform);
            MinimapManager.Instance.Initialize(currentShip.transform);
        }

        private void SpawnPlayerAnimation()
        {
            EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("SpawnFrigate"), currentShip.transform.position);
            currentShip.gameObject.SetActive(true);
        }

        private void UpdateText()
        {
            MoveSpeedText.text = currentShip.GetComponent<MoveComponent>().CurrentSpeed.ToString();
            CoolTimeText.text = characterData.CoolTime.ToString();
            ThrusterRatioText.text = characterData.ThrusterRate.ToString("F2");
            ThrusterDurationText.text = characterData.ThrusterDuration.ToString("F2");
            DamageReductionText.text = characterData.DamageReduction.ToString("P1");
            DamageText.text = characterData.WeaponData.Damage.ToString();
            CriticalProbabilityText.text = characterData.WeaponData.CriticalProbability.ToString("P1");
            CriticalRatioText.text = characterData.WeaponData.CriticalRatio.ToString("P1");
            RangeText.text = characterData.WeaponData.Range.ToString();
            FireRateText.text = characterData.WeaponData.AttackSpeed.ToString("F2");
            AttenuationRatioText.text = characterData.WeaponData.AttenuationRatio.ToString("P1");
        }

        private void HandleRankUp(object sender, RankChangedEventArgs e)
        {
           
            if (e.newRank < rankUpMessages.Length && !string.IsNullOrEmpty(rankUpMessages[e.newRank]))
            {
                Debug.Log(rankUpMessages[e.newRank]);
            }

            if (currentShip.GetComponent<FireUnitComponent>().IsFull())
            {
                currentShip.GetComponent<FireUnitComponent>().AddFireUnit();
                characterData.FireUnitCount++;

                Framework.Notification.NotificationManager.Instance.ShowFeedNotification("FireLevelUp");
            }
            else
            {
                ChangeShip();

                Framework.Notification.NotificationManager.Instance.ShowFeedNotification("ShipLevelUp");
            }           
        }

        private void ChangeShip()
        {
            if (playerData.ChangeShip())
            {
                Vector3 lastPos = currentShip.transform.position;
                Destroy(currentShip.gameObject);
                CreateAirplane();
                currentShip.transform.position = lastPos;
                SpawnPlayerAnimation();
            }
        }

        
    }
}


