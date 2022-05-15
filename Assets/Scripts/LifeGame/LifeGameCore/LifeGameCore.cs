using System.Collections.Generic;

public static class LifeGameCore
{
    private static List<byte> checkList = new List<byte>();
    private static Dictionary<byte, byte> checkDict = new Dictionary<byte, byte>();

    /// <summary>
    /// 最基础的生命游戏参数格式（康威为 Live = 3  Continue = 2 Less = 2 More = 3） 其中数据格式一定是！More >= Live >= Continue >= Less
    /// </summary>
    /// <param name="data">棋盘数据</param>
    /// <param name="aroundLiveCount">周围有固定几个的时候出生</param>
    /// <param name="aroundContinueCount">周围有固定几个的时候维持状态</param>
    /// <param name="aroundLessDeadCount">周围少于几个的时候死亡</param>
    /// <param name="aroundMoreDeadCount">周围多余几个的时候死亡</param>
    public static void ProcessData(ref byte[][] data, byte aroundLiveCount, byte aroundContinueCount, byte aroundLessDeadCount, byte aroundMoreDeadCount)
    {
        if (data.Length == 0) return;

        if (aroundMoreDeadCount < aroundLiveCount || aroundLiveCount < aroundContinueCount || aroundContinueCount < aroundLessDeadCount) return;
        
        int allLine = data.Length;
        int allColumn = data[0].Length;
        
        byte[][] dataNew = new byte[allLine][];
        for (int line = 0; line < dataNew.Length; line++)
        {
            dataNew[line] = new byte[allColumn];
            for (int column = 0; column < dataNew[line].Length; column++)
            {
                dataNew[line][column] = 0;
            }
        }

        for (int line = 0; line < allLine; line++)
        {
            for (int column = 0; column < allColumn; column++)
            {
                if (data[line][column] == (int) LifeGameChunkType.Obstacle || data[line][column] == (int) LifeGameChunkType.Obstacle2)
                {
                    dataNew[line][column] = 2;
                    continue;
                }
                
                if (data[line][column] == (int) LifeGameChunkType.DeadObstacle)
                {
                    dataNew[line][column] = 4;
                    continue;
                }
                
                int live = 0;
                int liveAdd, deadAdd, obstacleAdd = 0;
                JudgeState(allLine, allColumn, line - 1, column - 1, data, out  liveAdd, out  deadAdd, out  obstacleAdd);
                live += liveAdd;
                JudgeState(allLine, allColumn, line - 1, column, data, out  liveAdd, out  deadAdd, out  obstacleAdd);
                live += liveAdd;
                JudgeState(allLine, allColumn, line - 1, column + 1, data, out  liveAdd, out  deadAdd, out  obstacleAdd);
                live += liveAdd;
                JudgeState(allLine, allColumn, line, column - 1, data, out  liveAdd, out  deadAdd, out  obstacleAdd);
                live += liveAdd;
                JudgeState(allLine, allColumn, line, column + 1, data, out  liveAdd, out  deadAdd, out  obstacleAdd);
                live += liveAdd;
                JudgeState(allLine, allColumn, line + 1, column - 1, data, out  liveAdd, out  deadAdd, out  obstacleAdd);
                live += liveAdd;
                JudgeState(allLine, allColumn, line + 1, column, data, out  liveAdd, out  deadAdd, out  obstacleAdd);
                live += liveAdd;
                JudgeState(allLine, allColumn, line + 1, column + 1, data, out  liveAdd, out  deadAdd, out  obstacleAdd);
                live += liveAdd;

                if (live > aroundMoreDeadCount || live < aroundLessDeadCount)
                {
                    dataNew[line][column] = 0;
                }
                else if (live == aroundLiveCount)
                {
                    dataNew[line][column] = 1;
                }
                else if (live == aroundContinueCount)
                {
                    dataNew[line][column] = data[line][column];
                }
            }
        }

        data = dataNew;
    }

    private static bool checkIndex(int allLine, int allColumn, int line, int column)
    {
        bool lineSafe = line >= 0 && line < allLine;
        bool columnSafe = column >= 0 && column < allColumn;
        return lineSafe && columnSafe;
    }

    private static void JudgeState(int allLine, int allColumn, int line, int column, byte[][] datas, out int live, out int dead, out int obstacle)
    {
        live = 0;
        dead = 0;
        obstacle = 0;
        if (!checkIndex(allLine, allColumn, line, column)) return;
        switch ((LifeGameChunkType) datas[line][column])
        {
            case LifeGameChunkType.Live:
                live = 1;
                break;
            case LifeGameChunkType.Dead:
                dead = 1;
                break;
            case LifeGameChunkType.Obstacle:
                obstacle = 1;
                break;
            case LifeGameChunkType.Obstacle2:
                obstacle = 1;
                break;
            case LifeGameChunkType.DeadObstacle:
                obstacle = 1;
                break;
        }
    }
}