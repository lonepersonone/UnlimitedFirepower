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
        public string GalaxyID; //������ϵid
        public string LocationID; //����id

        [SerializeField] private PlanetView view;
        private PlanetModel model;
        private PlanetViewModel viewModel;

        public LevelData LevelData { get; private set; }


        private void Awake()
        {
            // ȷ����ͼ����������
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
            // ����ģ��
            model = new PlanetModel(
                HexgonUtil.HexCoordianteToPixel(q, r, HexMetrics.outerRadius), q, r);

            // ���ÿ���������
            LocationID = model.Key;
            LevelData = level;

            // ������ͼģ�Ͳ����Ӹ�����
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

        // �ⲿ���õĹ������� - ת������ͼģ��
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

        // ����ģ�����ݵĹ�������
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

