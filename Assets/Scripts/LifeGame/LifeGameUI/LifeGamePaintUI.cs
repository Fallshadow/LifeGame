
using UnityEngine.UI;

public class LifeGamePaintUI : LifeGameUI
{
    public Toggle Grass;
    public Toggle Sheep;
    public Toggle Tree;
    
    protected override void Born()
    {
        base.Born();
        GameControllerPaint.instance.ResizeBackGround(Height, Width);
    }

    protected override void UpdateGameMode(int index)
    {
        switch (index)
        {
            case 0:
                GameControllerChessBoard.instance.GameMode = LifeGameMode.TwoState_LD;
                Tree.gameObject.SetActive(false);
                break;
            case 1:
                GameControllerChessBoard.instance.GameMode = LifeGameMode.ThreeState_LDO;
                Tree.gameObject.SetActive(true);
                break;
        }
    }

    protected override void InitUi()
    {
        base.InitUi();
        
        Grass.onValueChanged.AddListener((bool toggleValue) =>
        {
            if (toggleValue)
            {
                toggleType(LifeGameChunkType.Dead);
                Sheep.isOn = false;
                Tree.isOn = false;
            }
        });
        
        Sheep.onValueChanged.AddListener((bool toggleValue) =>
        {
            if (toggleValue)
            {
                toggleType(LifeGameChunkType.Live);
                Grass.isOn = false;
                Tree.isOn = false;
            }
        });
        
        Tree.onValueChanged.AddListener((bool toggleValue) =>
        {
            if (toggleValue)
            {
                toggleType(LifeGameChunkType.Obstacle);
                Sheep.isOn = false;
                Grass.isOn = false;
            }
        });
    }

    private void toggleType(LifeGameChunkType lgct)
    {
        GameControllerPaint.instance.curPaintType = lgct;
    }
}
