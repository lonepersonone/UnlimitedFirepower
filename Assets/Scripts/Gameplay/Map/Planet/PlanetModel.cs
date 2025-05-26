using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public enum HexCellState
    {
        Hidden, // ������
        Explored, // ��̽��
        NotAnalysed, // δ��������ϵ��Ϣ
        Fighting, // ս����
        Conquered, // ������
        Destory // ����
    }

    public enum HexCellType
    {
        Default, // Ĭ�����ͣ�ֻ�е��ˣ���������
        Life, // �������򣬾�ס�������˿�
        Trap, // ����
        Wonder, // ���
        Channel //ͨ��
    }

    // ������ģ��
    public class PlanetModel
    {
        // ʹ�����Զ��ǹ����ֶ�
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

        // ״̬����¼�
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

        // �����������ڰ�ȫ���޸�״̬
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


