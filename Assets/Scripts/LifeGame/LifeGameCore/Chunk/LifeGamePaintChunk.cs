using UnityEngine;
using Random = UnityEngine.Random;

public class LifeGamePaintChunk : LifeGameChunk
{
    private float posX = 0;
    private float posY = 0;
    private float posZ = 0;
    
    public override void SetLocalPos(Vector3 pos)
    {
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
        RandomPos();
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

    public override void Process(LifeGameChunkType lifeType)
    {
        LifeType = lifeType;
        switch (LifeType)
        {
            case LifeGameChunkType.Live:
                DisplayPicture.sprite = GameController.instance.Config.Sheep;
                RandomPos();
                DisplayPicture.flipX = Random.Range(0, 2) != 0;
                DisplayPicture.sortingOrder = curIndex;
                break;
            case LifeGameChunkType.Dead:
                DisplayPicture.sprite = GameController.instance.Config.Grass;
                DisplayPicture.sortingOrder = 0;
                break;
            case LifeGameChunkType.Obstacle:
                DisplayPicture.sprite = GameController.instance.Config.Tree;
                DisplayPicture.sortingOrder = curIndex;
                break;
        }
    }

    public override void OnPointerClick()
    {
        Process(GameControllerPaint.instance.curPaintType);
    }
}