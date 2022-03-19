using System;
using UnityEngine;

public class LifeGameChunk : MonoBehaviour
{
    public SpriteRenderer DisplayPicture;
    public LifeGameChunkType LifeType;

    public void SetLocalPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public void Process(LifeGameChunkType lifeType)
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

    public void OnPointerClick()
    {
        int offset = (int) LifeType + 1;
        int allEnumCount = GameControllerChessBoard.instance.LifeGameChunkTypeCount;
        int newValue = offset % (allEnumCount - 1);
        LifeGameChunkType changeType = (LifeGameChunkType) newValue;
        Process(changeType);
    }
}