using UnityEngine;

[CreateAssetMenu(fileName = "LifeGameConfig", menuName = "Config/LifeGameConfig")]
public class ProjectSetting : ScriptableObject
{
    public Sprite LiveSprite;
    public Sprite DeadSprite;
    public Sprite ObstacleSprite;

    public Sprite Sheep;
    public Sprite Grass;
    public Sprite GrassEmpty;
    public Sprite Tree;
    public Sprite Water;
    
    public Color LiveColor1;
    public Color LiveColor2;
    public Color LiveColor3;
    public Color LiveColor4;
    public Color LiveColor5;
    public Color deadColor;
    public Color TreeColor;
    public Color WaterColor;

    public float cycleTime = 5;
    public Vector3 sizeScale = Vector3.one;
    public float sizeTime = 5;
    
    [Header("羊死的时间")]
    public float paintSheepTime = 2;
}