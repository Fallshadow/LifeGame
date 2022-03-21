using System;
using UnityEngine;

public class LifeGameChunk : MonoBehaviour
{
    public SpriteRenderer DisplayPicture;
    public LifeGameChunkType LifeType;
    public int allLine;
    public int allColumn;
    public int curIndex;
    
    public virtual void SetLocalPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }
    
    public virtual void SetNumInfo(int line,int column,int index)
    {
        allLine = line;
        allColumn = column;
        curIndex = index;
    }
    

    public virtual void Process(LifeGameChunkType lifeType)
    {
        LifeType = lifeType;
        switch (LifeType)
        {
            case LifeGameChunkType.Live:
                DisplayPicture.sprite = GameController.instance.Config.LiveSprite;
                break;
            case LifeGameChunkType.Dead:
                DisplayPicture.sprite = GameController.instance.Config.DeadSprite;
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