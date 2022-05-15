
using UnityEngine.UI;

public class LifeGamePaintUI : LifeGameUI
{
    public Toggle DeadGrass;
    public Toggle Grass;
    public Toggle Sheep;
    public Toggle Tree;
    public Toggle Water;
    
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
                Water.gameObject.SetActive(false);
                break;
            case 1:
                GameControllerChessBoard.instance.GameMode = LifeGameMode.ThreeState_LDO;
                Tree.gameObject.SetActive(true);
                Water.gameObject.SetActive(true);
                break;
        }
    }

    protected override void InitUi()
    {
        base.InitUi();
        
        DeadGrass.onValueChanged.AddListener((bool toggleValue) =>
        {
            if (toggleValue)
            {
                toggleType(LifeGameChunkType.DeadObstacle);
                Sheep.isOn = false;
                Tree.isOn = false;
                Water.isOn = false;
                Grass.isOn = false;
            }
        });
        
        Grass.onValueChanged.AddListener((bool toggleValue) =>
        {
            if (toggleValue)
            {
                toggleType(LifeGameChunkType.Dead);
                Sheep.isOn = false;
                Tree.isOn = false;
                Water.isOn = false;
                DeadGrass.isOn = false;
            }
        });
        
        Sheep.onValueChanged.AddListener((bool toggleValue) =>
        {
            if (toggleValue)
            {
                toggleType(LifeGameChunkType.Live);
                Grass.isOn = false;
                Tree.isOn = false;
                Water.isOn = false;
                DeadGrass.isOn = false;
            }
        });
        
        Tree.onValueChanged.AddListener((bool toggleValue) =>
        {
            if (toggleValue)
            {
                toggleType(LifeGameChunkType.Obstacle);
                toggleWaterType(false);
                Sheep.isOn = false;
                Grass.isOn = false;
                Water.isOn = false;
                DeadGrass.isOn = false;
            }
        });
        
        Water.onValueChanged.AddListener((bool toggleValue) =>
        {
            if (toggleValue)
            {
                toggleType(LifeGameChunkType.Obstacle2);
                toggleWaterType(true);
                Sheep.isOn = false;
                Grass.isOn = false;
                Tree.isOn = false;
                DeadGrass.isOn = false;
            }
        });
    }

    private void toggleType(LifeGameChunkType lgct)
    {
        GameControllerPaint.instance.curPaintType = lgct;
    }
    
    private void toggleWaterType(bool isW)
    {
        GameControllerPaint.instance.isWater = isW;
    }
}
