using System;
using UnityEngine;

public class LifeGameChunk : MonoBehaviour
{
    public SpriteRenderer DisplayPicture;
    public LifeGameChunkType LifeType;
    public int allLine;
    public int allColumn;
    public int curLine;
    public int curColumn;
    public int curIndex;
    public bool isReset = true;
    public float timer = 0;
    
    public virtual void SetLocalPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public virtual void ResetCycle()
    {
        isReset = true;
        timer = 0;
        transform.localScale = Vector3.one;
        DisplayPicture.color = GameController.instance.Config.deadColor;
    }
    
    public virtual void SetNumInfo(int line,int column,int index)
    {
        // allLine = line;
        // allColumn = column;
        curLine = line;
        curColumn = column;
        curIndex = index;
        // curLine = (index + 1) / (line) - 1;
        // curColumn = index - curLine * (column);
    }

    public virtual void UpdateColor()
    {
        timer += Time.deltaTime;
        float process = timer / GameController.instance.Config.cycleTime;
        float processSize = timer / GameController.instance.Config.sizeTime;
        if (process >= 1 && processSize >= 1)
        {
            ResetCycle();
            timer = 0;
        }

        transform.localScale = Vector3.Lerp(Vector3.one, GameController.instance.Config.sizeScale, processSize);
        
        if (process >= 0f && process <= 0.2f)
        {
            process = ProcessValue(process,0.2f);
            DisplayPicture.color = Color.Lerp(GameController.instance.Config.LiveColor1, GameController.instance.Config.LiveColor2, process);
        }
        else if (process >= 0.2f && process <= 0.4f)
        {
            process = ProcessValue(process,0.4f);
            DisplayPicture.color = Color.Lerp(GameController.instance.Config.LiveColor2, GameController.instance.Config.LiveColor3, process);
        }
        else if (process >= 0.4f && process <= 0.6f)
        {
            process = ProcessValue(process,0.6f);
            DisplayPicture.color = Color.Lerp(GameController.instance.Config.LiveColor3, GameController.instance.Config.LiveColor4, process);
        }
        else if (process >= 0.6f && process <= 0.8f)
        {
            process = ProcessValue(process,0.8f);
            DisplayPicture.color = Color.Lerp(GameController.instance.Config.LiveColor4, GameController.instance.Config.LiveColor5, process);
        }
        else
        {
            process = ProcessValue(process,1f);
            DisplayPicture.color = Color.Lerp(GameController.instance.Config.LiveColor5, GameController.instance.Config.deadColor, process);
        }
    }

    private float ProcessValue(float process,float maxF)
    {
        // process = process / 0.4f;
        return Mathf.PingPong(process, maxF) / maxF;
    }

    private bool isUsed2 = false;
    public virtual void Process(LifeGameChunkType lifeType)
    {
        LifeType = lifeType;
        switch (LifeType)
        {
            case LifeGameChunkType.Live:
                isUsed2 = false;
                DisplayPicture.sprite = GameController.instance.Config.LiveSprite;
                DisplayPicture.color = GameController.instance.Config.LiveColor1;
                transform.localScale = Vector3.one;
                isReset = false;
                break;
            case LifeGameChunkType.Dead:
                isUsed2 = false;
                DisplayPicture.sprite = GameController.instance.Config.DeadSprite;
                if (isReset)
                {
                    DisplayPicture.color = GameController.instance.Config.deadColor;
                }
                transform.localScale = Vector3.one;
                break;
            case LifeGameChunkType.Obstacle:
                if (isUsed2)
                {
                    DisplayPicture.sprite = GameController.instance.Config.ObstacleSprite;
                    DisplayPicture.color = GameController.instance.Config.WaterColor;
                    transform.localScale = Vector3.one;
                    break;
                }
                DisplayPicture.sprite = GameController.instance.Config.ObstacleSprite;
                DisplayPicture.color = GameController.instance.Config.TreeColor;
                transform.localScale = Vector3.one;
                break;
            case LifeGameChunkType.Obstacle2:
                isUsed2 = true;
                DisplayPicture.sprite = GameController.instance.Config.ObstacleSprite;
                DisplayPicture.color = GameController.instance.Config.WaterColor;
                transform.localScale = Vector3.one;
                break;
            case LifeGameChunkType.DeadObstacle:
                isUsed2 = false;
                DisplayPicture.sprite = GameController.instance.Config.DeadObstacleSprite;
                DisplayPicture.color = GameController.instance.Config.DeadObstacleColor;
                transform.localScale = Vector3.one;
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