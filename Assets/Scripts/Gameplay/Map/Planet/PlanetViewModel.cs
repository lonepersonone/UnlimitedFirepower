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

            // 订阅模型状态变更事件
            model.OnStateChanged += HandleStateChanged;

            // 初始化视图
            UpdateViewFromModel();
        }

        private void HandleStateChanged(HexCellState newState)
        {
            UpdateViewFromModel();
        }

        private void UpdateViewFromModel()
        {
            // 根据模型状态更新视图
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

            // 更新其他视图元素
            view.SetWealthText(model.Wealth.ToString());
            view.SetPosition(model.LocalPosition);
        }

        // 外部调用的公共方法
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
            // 仅在玩家未到达时更新视图框
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


