using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public enum HexCellState
    {
        Hidden, // 被隐藏
        Explored, // 已探索
        NotAnalysed, // 未解析出星系信息
        Fighting, // 战斗中
        Conquered, // 已征服
        Destory // 已损坏
    }

    public enum HexCellType
    {
        Default, // 默认类型，只有敌人，无特殊性
        Life, // 生命星球，居住有特殊人口
        Trap, // 陷阱
        Wonder, // 奇观
        Channel //通道
    }

    // 纯数据模型
    public class PlanetModel
    {
        // 使用属性而非公有字段
        public HexCellState State { get; private set; }
        public HexCellType Type { get; private set; }
        public DifficultyType Difficulty { get; private set; }

        public int Q { get; }
        public int R { get; }
        public int S { get; }
        public string Key { get; }
        public Vector3 LocalPosition { get; }

        public int Wealth { get; private set; }
        public int Scale { get; private set; }

        // 状态变更事件
        public event Action<HexCellState> OnStateChanged;

        public PlanetModel(Vector3 localPosition, int q, int r)
        {
            LocalPosition = localPosition;
            Q = q;
            R = r;
            S = -Q - R;
            Key = $"{Q}/{R}/{S}";

            SetState(HexCellState.Hidden);

            Difficulty = DifficultyType.Normal;
        }

        // 公共方法用于安全地修改状态
        public void SetState(HexCellState newState)
        {
            if (State != newState)
            {
                State = newState;
                OnStateChanged?.Invoke(newState);
            }
        }

        public void SetWealth(int value)
        {
            Wealth = value;
        }

        public void SetScale(int value)
        {
            Scale = value;
        }

        public void SetType(HexCellType type)
        {
            Type = type;
        }

        public void SetDifficulty(DifficultyType difficulty)
        {
            Difficulty = difficulty;
        }
    }
}


