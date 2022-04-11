using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LifeGamePaintChunk : LifeGameChunk
{
    private float posX = 0;
    private float posY = 0;
    private float posZ = 0;
    private bool UsePaint = false;
    private bool UsePaintNoSheep = false;
    private bool ChangeSheep = false;
    private float timer = 0;

    public override void ResetCycle()
    {
        isReset = true;
        timer = 0;
    }

    public override void SetLocalPos(Vector3 pos)
    {
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
        RandomPos();
    }

    private void Update()
    {
        if (UsePaint)
        {
            timer += Time.deltaTime;
            if (ChangeSheep)
            {
                DisplayPicture.sprite = GameController.instance.Config.Sheep;
                Color later1 = DisplayPicture.color;
                DisplayPicture.color = new Color(later1.r, later1.g, later1.b, 1 - timer / GameController.instance.Config.paintSheepTime);
            }
            else
            {
                switch (LifeType)
                {
                    case LifeGameChunkType.Dead:
                        DisplayPicture.sprite = GameController.instance.Config.Grass;
                        Color later2 = DisplayPicture.color;
                        DisplayPicture.color = new Color(later2.r, later2.g, later2.b, 1);
                        break;
                    case LifeGameChunkType.Obstacle:
                        DisplayPicture.sprite = GetObSprite();
                        Color later3 = DisplayPicture.color;
                        DisplayPicture.color = new Color(later3.r, later3.g, later3.b, 1);
                        break;
                    case LifeGameChunkType.Obstacle2:
                        DisplayPicture.sprite = GetObSprite();
                        Color later4 = DisplayPicture.color;
                        DisplayPicture.color = new Color(later4.r, later4.g, later4.b, 1);
                        break;
                }
            }

            if (timer > GameController.instance.Config.paintSheepTime)
            {
                timer = 0;
                ChangeSheep = false;
                UsePaint = false;
            }
        }
        else
        {
            timer = 0;
        }
    }

    public override void SetNumInfo(int line, int column, int index)
    {
        base.SetNumInfo(line, column, index);
        DisplayPicture.sortingOrder = index;
    }

    public void RandomPos()
    {
        float posRandomX = posX + Random.Range(-0.49f, 0.49f);
        float posRandomY = posY + Random.Range(-0.49f, 0.49f);

        transform.localPosition = new Vector3(posRandomX, posRandomY, posZ);
    }

    public override void UpdateColor()
    {
        return;
    }

    public override void Process(LifeGameChunkType lifeType)
    {
        LifeType = lifeType;
        switch (LifeType)
        {
            case LifeGameChunkType.Live:
                if (!UsePaint || UsePaintNoSheep)
                {
                    return;
                }
                ChangeSheep = true;
                DisplayPicture.sprite = GameController.instance.Config.Sheep;
                RandomPos();
                DisplayPicture.flipX = Random.Range(0, 2) != 0;
                DisplayPicture.sortingOrder = curIndex;
                break;
            case LifeGameChunkType.Dead:
                if (UsePaint)
                {
                    ChangeSheep = false;
                }
                DisplayPicture.sprite = GameController.instance.Config.Grass;
                DisplayPicture.sortingOrder = 0;
                break;
            case LifeGameChunkType.Obstacle:
                if (UsePaint)
                {
                    ChangeSheep = false;
                }
                DisplayPicture.sprite = GetObSprite();
                DisplayPicture.sortingOrder = curIndex;
                break;
            case LifeGameChunkType.Obstacle2:
                if (UsePaint)
                {
                    ChangeSheep = false;
                }
                DisplayPicture.sprite = GetObSprite();
                DisplayPicture.sortingOrder = curIndex;
                break;
        }
    }

    private bool isWa = false;
    public Sprite GetObSprite()
    {
        if (isWa)
        {
            return GameController.instance.Config.Water;
        }
        else
        {
            return GameController.instance.Config.Tree;
        }
    }

    public void OnPointEnter()
    {
        if (GameControllerPaint.instance.PaintBool)
        {
            isWa = GameControllerPaint.instance.isWater;
            UsePaint = true;
            UsePaintNoSheep = GameControllerPaint.instance.curPaintType != LifeGameChunkType.Live;
            Process(GameControllerPaint.instance.curPaintType);
            GameController.instance.PaintChessBoard.CurDatas[curLine][curColumn] = (byte) GameControllerPaint.instance.curPaintType;
            if (GameControllerPaint.instance.curPaintType == LifeGameChunkType.Dead)
            {
                GameController.instance.PaintChessBoard.needReset = true;
            }
            GameController.instance.PaintChessBoard.UpdateChessBoardCB?.Invoke();
        }
    }

    public override void OnPointerClick()
    {
        isWa = GameControllerPaint.instance.isWater;
        UsePaint = true;
        UsePaintNoSheep = GameControllerPaint.instance.curPaintType != LifeGameChunkType.Live;
        Process(GameControllerPaint.instance.curPaintType);
        GameController.instance.PaintChessBoard.CurDatas[curLine][curColumn] = (byte) GameControllerPaint.instance.curPaintType;
        if (GameControllerPaint.instance.curPaintType == LifeGameChunkType.Dead)
        {
            GameController.instance.PaintChessBoard.needReset = true;
        }
        GameController.instance.PaintChessBoard.UpdateChessBoardCB?.Invoke();
    }

    // 开始拖拽 画笔火力全开
    public void OnDragBegin()
    {
        GameControllerPaint.instance.PaintBool = true;
    }

    // 结束拖拽 画笔关闭
    // public void OnPointerUp()
    // {
    //     GameControllerPaint.instance.PaintBool = false;
    // }
}