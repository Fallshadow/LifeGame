using UnityEngine;

[CreateAssetMenu(fileName = "LifeGameConfig", menuName = "Config/LifeGameConfig")]
public class ProjectSetting : ScriptableObject
{
    public Sprite LiveSprite;
    public Sprite DeadSprite;
    public Sprite ObstacleSprite;

    public Sprite Sheep;
    public Sprite Grass;
    public Sprite Tree;
    
    public Color LiveColor;
    public Color deadColor;
    public float cycleTime = 5;
}