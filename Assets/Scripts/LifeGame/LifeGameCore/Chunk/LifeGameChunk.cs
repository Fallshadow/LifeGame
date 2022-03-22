using System;
using UnityEngine;

public class LifeGameChunk : MonoBehaviour
{
    public SpriteRenderer DisplayPicture;
    public LifeGameChunkType LifeType;
    public int allLine;
    public int allColumn;
    public int curIndex;
    public bool isReset = true;
    public float timer = 0;
    
    public virtual void SetLocalPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public void ResetCycle()
    {
        isReset = true;
        timer = 0;
    }
    
    public virtual void SetNumInfo(int line,int column,int index)
    {
        allLine = line;
        allColumn = column;
        curIndex = index;
    }

    public virtual void UpdateColor()
    {
        timer += Time.deltaTime;
        float process = timer / GameController.instance.Config.cycleTime;
        if (process >= 1)
        {
            ResetCycle();
            timer = 0;
        }

        DisplayPicture.color = Color.Lerp(GameController.instance.Config.LiveColor, GameController.instance.Config.deadColor, process);
    }

    public virtual void Process(LifeGameChunkType lifeType)
    {
        LifeType = lifeType;
        switch (LifeType)
        {
            case LifeGameChunkType.Live:
                DisplayPicture.sprite = GameController.instance.Config.LiveSprite;
                DisplayPicture.color = GameController.instance.Config.LiveColor;
                isReset = false;
                break;
            case LifeGameChunkType.Dead:
                DisplayPicture.sprite = GameController.instance.Config.DeadSprite;
                if (isReset)
                {
                    DisplayPicture.color = GameController.instance.Config.deadColor;
                }
                break;
            case LifeGameChunkType.Obstacle:
                DisplayPicture.sprite = GameController.instance.Config.ObstacleSprite;
                break;
        }
    }

    public virtual void OnPointerClick()
    {
        int offset = (int) LifeType + 1;
        int allEnumCount = GameControllerChessBoard.instance.GetCurModeTypeCount();
        int newValue = offset % allEnumCount;
        LifeGameChunkType changeType = (LifeGameChunkType) newValue;
        Process(changeType);
    }
}