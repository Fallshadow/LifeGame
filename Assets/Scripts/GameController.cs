using System;
using UnityEngine;


public class GameController : SingletonMonoBehaviorNoDestroy<GameController>
{
    public ProjectSetting Config;

    private void Start()
    {
        Config = (ProjectSetting) Resources.Load("ScriptObject/LifeGameConfig");
    }
}