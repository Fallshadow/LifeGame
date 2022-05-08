using UnityEngine;

public class GameControllerPaint : SingletonMonoBehaviorNoDestroy<GameControllerPaint>
{
    public GameObject BackGround;
    public LifeGameChunkType curPaintType = LifeGameChunkType.Dead;
    public bool isWater = false;
    public bool PaintBool = false;
    public Vector3 StartPos = Vector3.zero;
    
    public void ResizeBackGround(int height, int width)
    {
        BackGround.transform.localScale = new Vector3(width + 2, height + 2, 1);
        StartPos.x = -(width - 1) * 0.5f;
        StartPos.y = 0;
        StartPos.z = 0;
        StartPos.x *= 1.75f / 2.3f;
        BackGround.transform.position = StartPos;
    }
}