using System;
using UnityEngine;


public class GameController : SingletonMonoBehaviorNoDestroy<GameController>
{
    
    public ProjectSetting Config;
    public bool StartSync;
    public LifeGameChessBoard CommonChessBoard = null;
    public LifeGameChessBoard PaintChessBoard = null;

    private void Start()
    {
        Config = (ProjectSetting) Resources.Load("ScriptObject/LifeGameConfig");
        if (CommonChessBoard != null && PaintChessBoard != null)
        {
            PaintChessBoard.UpdateChessBoardCB += () =>
            {
                CommonChessBoard.SetChunks(PaintChessBoard.CurDatas);
            };
        }
    }
}