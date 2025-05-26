using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{

    // ��Ƶ����ö��
    public enum AudioType
    {
        Player,      // ������
        Enemy,       // �������
        Environment, // �������
        UI,          // UI���
        System,      // ϵͳ���
        Scene        // �������
    }

    // ��Ƶ��Ϊö��
    public enum AudioAction
    {
        // �����Ϊ
        Shoot,
        Die,
        Pickup,

        // ��������Ϊ
        Attack,
        Hurt,
        Explode,
        Spawn,

        // ������Ϊ
        Wind,
        Rain,
        Thunder,
        DoorOpen,
        DoorClose,

        // UI��Ϊ
        Click,
        Hover,
        Select,
        Error,
        Success,

        // ϵͳ��Ϊ
        Loading,
        Save,
        Load,
        Warning,
        GameOver,
        Conquered,

        // ������Ϊ
        Profound, // ����
        Passion, // ����
    }

    // ��ƵID������
    public static class AudioIDManager
    {
        // ���ֵ䣺[��Ƶ����, [��Ƶ��Ϊ, ��ƵID]]
        private static readonly Dictionary<AudioType, Dictionary<AudioAction, string>> audioIDLookup =
            new Dictionary<AudioType, Dictionary<AudioAction, string>>
        {
        // ��ʼ�������ƵID
        {
            AudioType.Player,
            new Dictionary<AudioAction, string>
            {
                { AudioAction.Shoot, "SFX_Player_Shoot" },
                { AudioAction.Die, "SFX_Player_Die" },
                { AudioAction.Pickup, "SFX_Player_Pickup" }
            }
        },
        
        // ��ʼ��������ƵID
        {
            AudioType.Enemy,
            new Dictionary<AudioAction, string>
            {
                { AudioAction.Shoot, "SFX_Enemy_Shoot" },
                { AudioAction.Attack, "SFX_Enemy_Attack" },
                { AudioAction.Hurt, "SFX_Enemy_Hurt" },
                { AudioAction.Explode, "SFX_Enemy_Explode" },
                { AudioAction.Spawn, "SFX_Enemy_Spawn" }
            }
        },
        
        // ��ʼ��������ƵID
        {
            AudioType.Environment,
            new Dictionary<AudioAction, string>
            {
                { AudioAction.Wind, "SFX_Environment_Wind" },
                { AudioAction.Rain, "SFX_Environment_Rain" },
                { AudioAction.Thunder, "SFX_Environment_Thunder" },
                { AudioAction.DoorOpen, "SFX_Environment_DoorOpen" },
                { AudioAction.DoorClose, "SFX_Environment_DoorClose" }
            }
        },
        
        // ��ʼ��UI��ƵID
        {
            AudioType.UI,
            new Dictionary<AudioAction, string>
            {
                { AudioAction.Click, "UI_Click" },
                { AudioAction.Hover, "UI_Hover" },
                { AudioAction.Select, "UI_Select" },
                { AudioAction.Error, "UI_Error" },
                { AudioAction.Success, "UI_Success" }
            }
        },
        
        // ��ʼ��ϵͳ��ƵID
        {
            AudioType.System,
            new Dictionary<AudioAction, string>
            {
                { AudioAction.Loading, "System_Loading" },
                { AudioAction.Save, "System_Save" },
                { AudioAction.Load, "System_Load" },
                { AudioAction.Warning, "System_Warning" },
                { AudioAction.GameOver, "System_GameOver" },
                { AudioAction.Conquered, "System_Conquered" }
            }
        },

        // ��ʼ��������ƵID
        {
            AudioType.Scene,
            new Dictionary<AudioAction, string>
            {
                { AudioAction.Profound, "BGM_Main_Profound" },
                { AudioAction.Passion, "BGM_Lobby_Passion" },
            }
        },

        };

        // ��ȡ��ƵID
        public static string GetAudioID(AudioType type, AudioAction action)
        {
            if (audioIDLookup.TryGetValue(type, out Dictionary<AudioAction, string> actionDict))
            {
                if (actionDict.TryGetValue(action, out string audioID))
                {
                    return audioID;
                }
            }

            Debug.LogError($"δ�ҵ���ƵID: {type}_{action}");
            return string.Empty;
        }

        // ��ӻ��޸���ƵID
        public static void SetAudioID(AudioType type, AudioAction action, string audioID)
        {
            if (!audioIDLookup.TryGetValue(type, out Dictionary<AudioAction, string> actionDict))
            {
                actionDict = new Dictionary<AudioAction, string>();
                audioIDLookup[type] = actionDict;
            }

            actionDict[action] = audioID;
        }

        // ��ȡĳ���͵�������ƵID
        public static Dictionary<AudioAction, string> GetAllAudioIDs(AudioType type)
        {
            if (audioIDLookup.TryGetValue(type, out Dictionary<AudioAction, string> actionDict))
            {
                return new Dictionary<AudioAction, string>(actionDict);
            }

            return new Dictionary<AudioAction, string>();
        }
    }

}


