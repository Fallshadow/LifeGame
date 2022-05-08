using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class LifeGameChessBoard : MonoBehaviour
{
    // 1 1 0,0    1,2 -0.5,0    
    // 1 1 0,0    2,1 0,0.5
    public int HeightNum;
    public int WidthNum;
    public int BoardType = 0; // 0 数字 1 画板
    public Action UpdateChessBoardCB = null;
    [ReadOnly] public Vector3 StartPos = Vector3.zero;

    public Transform StartTrans = null;
    public GameObject ChunkPrefab = null;
    private List<LifeGameChunk> allChunks = new List<LifeGameChunk>();

    public byte[][] CurDatas = null;
    public bool needReset = false;
    
    public void BornChunks(int h, int w)
    {
        setChessBoardHW(h, w);
        while (HeightNum * WidthNum > allChunks.Count)
        {
            LifeGameChunk chunk = Instantiate(ChunkPrefab, StartTrans).GetComponent<LifeGameChunk>();
            allChunks.Add(chunk);
            chunk.Process(LifeGameChunkType.Dead);
        }

        foreach (var chunk in allChunks)
        {
            chunk.ResetCycle();
        }
        
        while (HeightNum * WidthNum < allChunks.Count)
        {
            for (int index = allChunks.Count - 1; index > HeightNum * WidthNum - 1; --index)
            {
                Destroy(allChunks[index].gameObject);
                allChunks.RemoveAt(index);
            }
        }

        for (int index = 0; index < allChunks.Count; index++)
        {
            int processInt = index;
            int curLine = processInt / WidthNum;
            int column = processInt - curLine * WidthNum;

            float curY = -curLine;
            float curX = column;
            allChunks[index].SetLocalPos(new Vector3(curX, curY, 0));
            allChunks[index].SetNumInfo(curLine, column, index);
        }
        
        GameControllerChessBoard.instance.AdaptiveScreen();
    }

    public void RefreshChunks()
    {
        for (int index = 0; index < allChunks.Count; index++)
        {
            allChunks[index].Process(LifeGameChunkType.Dead);
            allChunks[index].ResetCycle();
        }
        
        needReset = true;
        CurDatas = new byte[HeightNum][];
        for (int index = 0; index < CurDatas.Length; index++)
        {
            CurDatas[index] = new byte[WidthNum];
        }
        UpdateChessBoardCB?.Invoke();
    }

    public void UpdateChunks()
    {
        byte[][] datas = new byte[HeightNum][];
        for (int lineIndex = 0; lineIndex < datas.Length; lineIndex++)
        {
            datas[lineIndex] = new byte[WidthNum];
            for (int column = 0; column < datas[lineIndex].Length; column++)
            {
                int dataIndex = lineIndex * WidthNum + column;
                datas[lineIndex][column] = (byte)allChunks[dataIndex].LifeType;
            }
        }
        LifeGameCore.ProcessData(ref datas,
            GameControllerChessBoard.instance.BornCount,
            GameControllerChessBoard.instance.LiveCount,
            GameControllerChessBoard.instance.LessDeadCount,
            GameControllerChessBoard.instance.MoreDeadCount);
        ProcessChunksByDatas(datas, allChunks);
    }

    public void UpdateChunksColor()
    {
        foreach (var chunk in allChunks)
        {
            if (chunk.LifeType == LifeGameChunkType.Dead && !chunk.isReset)
            {
                chunk.UpdateColor();
            }
        }
    }

    public void ResetChunks()
    {
        foreach (var chunk in allChunks)
        {
            chunk.ResetCycle();
        }
    }

    public void SetChunks(byte[][] datas, bool needResetP)
    {
        if (needResetP)
        {
            SetChunks(datas, ResetChunks);
        }
        else
        {
            SetChunks(datas);
        }
    }
    
    public void SetChunks(byte[][] datas,Action cb = null)
    {
        if (datas.Length == 0) return;
        if (datas.Length * datas[0].Length != WidthNum * HeightNum)
        {
            BornChunks(datas.Length, datas[0].Length);
        }
        cb?.Invoke();
        ProcessChunksByDatas(datas, allChunks);
    }

    private void ProcessChunksByDatas(byte[][] datas, List<LifeGameChunk> chunks)
    {
        for (int lineIndex = 0; lineIndex < datas.Length; lineIndex++)
        {
            for (int column = 0; column < datas[lineIndex].Length; column++)
            {
                int dataIndex = lineIndex * WidthNum + column;
                chunks[dataIndex].Process((LifeGameChunkType)datas[lineIndex][column]);
            }
        }
        CurDatas = datas;
        UpdateChessBoardCB?.Invoke();
    }

    private void setChessBoardHW(int h, int w)
    {
        HeightNum = h;
        WidthNum = w;
        updateStartPos();
    }

    private void updateStartPos()
    {
        StartPos.x = -(WidthNum - 1) * 0.5f;
        StartPos.y = (HeightNum - 1) * 0.5f;
        StartPos.z = 0;
        if (BoardType == 0)
        {
            StartPos.x *= 1.75f;
        }
        else if (BoardType == 1)
        {
            StartPos.x -= (StartPos.x * 1.75f);
        }
        StartTrans.position = StartPos;
    }

//     private void Update()
//     {
// #if UNITY_EDITOR
//         updateStartPos();
// #endif
//     }
}