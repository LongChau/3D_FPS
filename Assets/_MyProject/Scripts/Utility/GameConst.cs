using UnityEngine;

public static class GameConst
{
    /// <summary>
    ///  SAVE_PATH = Application.persistentDataPath + "/Save/userInfo.gd"
    /// </summary>
    public static readonly string SAVE_PATH = Application.persistentDataPath + "/Save/userInfo.gd";
}

public static class GameTags
{
    public static readonly string PLAYER = "Player";
    public static readonly string LAND = "Land";
    public static readonly string OBSTACLE = "Obstacle";
    public static readonly string UI = "UI";
}

public static class GameScenes 
{
    public const string MENU_SCENE = "MenuScene";
    public const string MAIN_SCENE = "MainScene";
    public const string LOADING_SCENE = "LoadingScene";
    public const string BATTLE_SCENE = "BattleScene";
    public const string LEVEL_SCENE = "Level_";
}