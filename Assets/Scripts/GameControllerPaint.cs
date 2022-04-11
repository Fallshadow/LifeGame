using UnityEngine;

public class GameControllerPaint : SingletonMonoBehaviorNoDestroy<GameControllerPaint>
{
    public GameObject BackGround;
    public LifeGameChunkType curPaintType = LifeGameChunkType.Dead;
    public bool isWater = false;
    public bool PaintBool = false;

    public void ResizeBackGround(int height, int width)
    {
        BackGround.transform.localScale = new Vector3(width + 2, height + 2, 1);
    }
}