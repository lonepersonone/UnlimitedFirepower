using MyGame.Data.SO;
using MyGame.Framework.Manager;
using MyGame.Framework.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame.Gameplay.Map
{

    [RequireComponent(typeof(PlanetView))]
    public class PlanetController : MonoBehaviour
    {
        public string GalaxyID; //所属星系id
        public string LocationID; //坐标id

        [SerializeField] private PlanetView view;
        private PlanetModel model;
        private PlanetViewModel viewModel;

        public LevelData LevelData { get; private set; }


        private void Awake()
        {
            // 确保视图引用已设置
            if (view == null)
                view = GetComponent<PlanetView>();
        }

        private void Update()
        {
            if (!GameState.Pauseable)
            {
                Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                viewModel.UpdateCursor(world);
            }
        }

        public void Initialize(int q, int r, LevelData level)
        {
            // 创建模型
            model = new PlanetModel(
                HexgonUtil.HexCoordianteToPixel(q, r, HexMetrics.outerRadius), q, r);

            // 设置控制器属性
            LocationID = model.Key;
            LevelData = level;

            // 创建视图模型并连接各部分
            viewModel = new PlanetViewModel(model, view);
        }

        public void SetType(HexCellType type)
        {
            model.SetType(type);
        }

        public void SetWealth(int value)
        {
            viewModel.SetWealth(value);
        }

        // 外部调用的公共方法 - 转发到视图模型
        public void OnArrived(bool state)
        {
            viewModel.OnArrived(state);
        }

        public void OnExplored()
        {
            viewModel.OnExplored();
        }

        public void OnDestoryed()
        {
            viewModel.OnDestroyed();
        }

        public void OnConquered()
        {
            viewModel.OnConquered();
        }

        public void Viewed(bool state)
        {
            viewModel.SetViewed(state);
        }

        // 访问模型数据的公共方法
        public HexCellState GetState()
        {
            return model.State;
        }

        public Vector3 GetPosition()
        {
            return model.LocalPosition;
        }

        public int[] GetIDByInt()
        {
            return new int[3] { model.Q, model.R, model.S };
        } 

        public HexCellType GetPlanetType()
        {
            return model.Type;
        }

        public int GetWealth()
        {
            return model.Wealth;
        }
    }
}

