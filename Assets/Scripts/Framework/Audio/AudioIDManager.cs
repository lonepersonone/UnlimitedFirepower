using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{

    // 音频类型枚举
    public enum AudioType
    {
        Player,      // 玩家相关
        Enemy,       // 敌人相关
        Environment, // 环境相关
        UI,          // UI相关
        System,      // 系统相关
        Scene        // 场景相关
    }

    // 音频行为枚举
    public enum AudioAction
    {
        // 玩家行为
        Shoot,
        Die,
        Pickup,

        // 敌人类行为
        Attack,
        Hurt,
        Explode,
        Spawn,

        // 环境行为
        Wind,
        Rain,
        Thunder,
        DoorOpen,
        DoorClose,

        // UI行为
        Click,
        Hover,
        Select,
        Error,
        Success,

        // 系统行为
        Loading,
        Save,
        Load,
        Warning,
        GameOver,
        Conquered,

        // 场景行为
        Profound, // 深邃
        Passion, // 激情
    }

    // 音频ID管理器
    public static class AudioIDManager
    {
        // 主字典：[音频类型, [音频行为, 音频ID]]
        private static readonly Dictionary<AudioType, Dictionary<AudioAction, string>> audioIDLookup =
            new Dictionary<AudioType, Dictionary<AudioAction, string>>
        {
        // 初始化玩家音频ID
        {
            AudioType.Player,
            new Dictionary<AudioAction, string>
            {
                { AudioAction.Shoot, "SFX_Player_Shoot" },
                { AudioAction.Die, "SFX_Player_Die" },
                { AudioAction.Pickup, "SFX_Player_Pickup" }
            }
        },
        
        // 初始化敌人音频ID
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
        
        // 初始化环境音频ID
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
        
        // 初始化UI音频ID
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
        
        // 初始化系统音频ID
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

        // 初始化场景音频ID
        {
            AudioType.Scene,
            new Dictionary<AudioAction, string>
            {
                { AudioAction.Profound, "BGM_Main_Profound" },
                { AudioAction.Passion, "BGM_Lobby_Passion" },
            }
        },

        };

        // 获取音频ID
        public static string GetAudioID(AudioType type, AudioAction action)
        {
            if (audioIDLookup.TryGetValue(type, out Dictionary<AudioAction, string> actionDict))
            {
                if (actionDict.TryGetValue(action, out string audioID))
                {
                    return audioID;
                }
            }

            Debug.LogError($"未找到音频ID: {type}_{action}");
            return string.Empty;
        }

        // 添加或修改音频ID
        public static void SetAudioID(AudioType type, AudioAction action, string audioID)
        {
            if (!audioIDLookup.TryGetValue(type, out Dictionary<AudioAction, string> actionDict))
            {
                actionDict = new Dictionary<AudioAction, string>();
                audioIDLookup[type] = actionDict;
            }

            actionDict[action] = audioID;
        }

        // 获取某类型的所有音频ID
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


