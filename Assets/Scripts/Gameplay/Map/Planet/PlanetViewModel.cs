using MyGame.Framework.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public class PlanetViewModel
    {
        private readonly PlanetModel model;
        private readonly PlanetView view;

        public PlanetViewModel(PlanetModel model, PlanetView view)
        {
            this.model = model;
            this.view = view;

            // ����ģ��״̬����¼�
            model.OnStateChanged += HandleStateChanged;

            // ��ʼ����ͼ
            UpdateViewFromModel();
        }

        private void HandleStateChanged(HexCellState newState)
        {
            UpdateViewFromModel();
        }

        private void UpdateViewFromModel()
        {
            // ����ģ��״̬������ͼ
            switch (model.State)
            {
                case HexCellState.Hidden:
                    view.SetHidden();
                    break;
                case HexCellState.Explored:
                    view.SetExplored();
                    break;
                case HexCellState.Conquered:
                    view.SetConquered();
                    break;
                case HexCellState.Destory:
                    view.SetDestroyed();
                    break;
                case HexCellState.Fighting:
                    view.SetFighting();
                    break;
                case HexCellState.NotAnalysed:
                    view.SetNotAnalysed();
                    break;
            }

            // ����������ͼԪ��
            view.SetWealthText(model.Wealth.ToString());
            view.SetPosition(model.LocalPosition);
        }

        // �ⲿ���õĹ�������
        public void OnArrived(bool isArrived)
        {
            view.SetPlayerFrameActive(isArrived);
            //view.SetViewFrameActive(isArrived);
        }

        public void OnExplored()
        {
            if (model.State == HexCellState.Hidden)
            {
                model.SetState(HexCellState.Explored);
            }
        }

        public void OnDestroyed()
        {
            model.SetState(HexCellState.Destory);
        }

        public void OnConquered()
        {
            model.SetState(HexCellState.Conquered);
        }

        public void SetViewed(bool isViewed)
        {
            // �������δ����ʱ������ͼ��
            if (!view.IsPlayerFrameActive())
            {
                view.SetViewFrameActive(isViewed);
            }
        }

        public void UpdateCursor(Vector3 worldPos)
        {
            bool isHovered = model.Key == HexgonUtil.WorldToLocationID(worldPos);
            SetViewed(isHovered);
        }

        public void SetWealth(int value)
        {
            model.SetWealth(value);
        }
    }
}


