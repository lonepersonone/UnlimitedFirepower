using MyGame.Data.SO;
using MyGame.Framework.Event;
using MyGame.Framework.Manager;
using MyGame.Framework.Utilities;
using MyGame.Gameplay.Enemy;
using MyGame.Gameplay.Player;
using MyGame.Scene.BattleRoom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MyGame.Gameplay.Level 
{
    public class LevelManager : GameSystemBase
    {
        public static LevelManager Instance;
        
        public GameObject CanvasUpgrade;
        public GameObject LevelTitleObj;

        public TextMeshProUGUI WaveText;
        public TextMeshProUGUI SettlementText;

        private LevelAttribute levelData;

        private List<GameObject> enemyUnits = new List<GameObject>();
        private int waveIndex = 0;
        private int waveCount = 0;

        private void Awake()
        {
            Instance = this;
        }

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            levelData = BattleDataManager.Instance.LevelAttribute;

            Debug.Log("Level ID: " + levelData.ID + "Wave Count: " + levelData.Waves.Count);

            waveCount = levelData.Waves.Count;

            GameEventManager.RegisterListener(GameEventType.BattleStarted, EnableAssault);
            GameEventManager.RegisterListener(GameEventType.GameOver, GameOver);

            await Task.Delay(100);

            IsReady = true;
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.BattleStarted, EnableAssault);
            GameEventManager.UnregisterListener(GameEventType.GameOver, GameOver);
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (CanvasUpgrade.activeSelf)
                {
                    Time.timeScale = 1;
                    CanvasUpgrade.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0;
                    CanvasUpgrade.SetActive(true);
                }
            }

        }

  
        /// <summary>
        /// 激活进攻流程
        /// </summary>
        public void EnableAssault()
        {
            StartCoroutine(EnableAssaultCoroutine());
        }

        private IEnumerator EnableAssaultCoroutine()
        {
            UpdateWaveText();
            StartCoroutine(Assault(levelData.Waves[waveIndex].Enemies));

            Debug.Log("已生成敌人，下一波到达时间：" + levelData.Waves[waveIndex].Delay);

            yield return new WaitForSeconds(levelData.Waves[waveIndex].Delay); 
            waveIndex++;
            

            if(waveIndex < levelData.Waves.Count) StartCoroutine(EnableAssaultCoroutine());
        }

        /// <summary>
        /// 激活具体EnemyAI
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerator Assault(List<EnemySpawnData> enemies)
        {
            foreach (var enemy in enemies)
            {
                for (int i = 0; i < enemy.Count; i++)
                {
                    CharacterAttribute enemyData = new CharacterAttribute(enemy.Enemy);

                    GameObject instance = Instantiate(enemyData.CharacterPrefab);
                    List<Vector3> assaultPos = TransformUtil.GetRingGridPositions(PlayerController.Instance.transform.localPosition, 60, 60, 1);
                    instance.transform.localPosition = assaultPos[UnityEngine.Random.Range(0, assaultPos.Count)];

                    IEnemy unit = instance.GetComponent<IEnemy>();
                    unit.Initialize(enemyData);

                    AddEnemy(instance);
                }
            }

            yield return null;
        }

        public void ReduceEnemy(GameObject enemy)
        {
            enemyUnits.Remove(enemy);
            Destroy(enemy);
            if (waveIndex == levelData.Waves.Count && enemyUnits.Count == 0) StartCoroutine(EndBattleGame());
        }

        public void AddEnemy(GameObject enemy)
        {
            enemyUnits.Add(enemy);
        }

        private IEnumerator EndBattleGame()
        {
            yield return new WaitForSeconds(2f);

            GameEventManager.ClearListener(GameEventType.BattleStarted);

            SceneController.Instance.BattleVictory();           
        }
        
        private void UpdateWaveText()
        {
            waveCount--;
            if (waveCount == 0)
            {
                LevelTitleObj.SetActive(false);
                return;
            }           
            WaveText.text = waveCount.ToString();           
        }

        private void GameOver()
        {
            SettlementText.gameObject.SetActive(true);
            LevelTitleObj.SetActive(false);
        }

    }
}


