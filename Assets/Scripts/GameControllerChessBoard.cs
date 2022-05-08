using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameControllerChessBoard : SingletonMonoBehaviorNoDestroy<GameControllerChessBoard>
{
    public LifeGameChessBoard ChessBoard;
    public LifeGameChessBoard ChessBoardLook = null;
    public byte LiveCount = 2;
    public byte BornCount = 3;
    public byte LessDeadCount = 2;
    public byte MoreDeadCount = 3;
    public bool StartUpdate = false;
    public float UpdateTimer = 5;
    public float NowTimer = 0;
    public bool paint = false;
    public LifeGameMode GameMode = LifeGameMode.TwoState_LD;

    public Color LiveColor;
    public Color deadColor;
    public int cycleCount = 10;

    [Header("相机相关")] public float MoveSpeed = 10;
    public float MoveSpeedPowerConfig = 10;
    public float MoveSpeedPower = 1;
    public float SizeSpeed = 1;

    public byte GetCurModeTypeCount()
    {
        switch (GameMode)
        {
            case LifeGameMode.TwoState_LD:
                return 2;
            case LifeGameMode.ThreeState_LDO:
                return 3;
        }

        return 0;
    }

    private void Update()
    {
        UpdateCamera();

        if (!StartUpdate ) return;

        NowTimer += Time.deltaTime;
        if (NowTimer >= UpdateTimer)
        {
            ChessBoard.UpdateChunks();
            NowTimer = 0;
        }

        if (ChessBoardLook != null)
        {
            ChessBoardLook.UpdateChunksColor();
        }
    }

    public void RandomChessBoard()
    {
        byte[][] datas = new byte[ChessBoard.HeightNum][];
        for (int line = 0; line < datas.Length; line++)
        {
            datas[line] = new byte[ChessBoard.WidthNum];
            for (int column = 0; column < datas[line].Length; column++)
            {
                switch (GameMode)
                {
                    case LifeGameMode.TwoState_LD:
                        datas[line][column] = (byte) Random.Range(0, 2);
                        break;
                    case LifeGameMode.ThreeState_LDO:
                        datas[line][column] = (byte) Random.Range(0, 3);
                        break;
                }
            }
        }

        ChessBoard.needReset = true;
        ChessBoard.ResetChunks();
        ChessBoard.SetChunks(datas);
    }

    public void SetUpdateTimer(float updateTimer)
    {
        UpdateTimer = updateTimer;
        NowTimer = 0;
    }

    public void SetStartUpdate(bool startOrNot)
    {
        StartUpdate = startOrNot;
    }

    public void AdaptiveScreen()
    {
        if (Camera.main == null) return;
        Camera mainCamera = Camera.main;
        mainCamera.orthographicSize = Mathf.Max(ChessBoard.HeightNum, ChessBoard.WidthNum)* 0.8f;
        var mainCameraTransform = mainCamera.transform;
        var latePos = mainCameraTransform.position;
        // mainCameraTransform.position = new Vector3(-mainCamera.orthographicSize / 2, 0, latePos.z);
        mainCameraTransform.position = new Vector3(0, 0, latePos.z);
    }

    public void UpdateCamera()
    {
        if (Camera.main == null) return;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveSpeedPower = MoveSpeedPowerConfig;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            MoveSpeedPower = 1;
        }

        float moveNowSpeed = MoveSpeed * MoveSpeedPower * Time.deltaTime;
        float sizeNowSpeed = SizeSpeed * MoveSpeedPower * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position += new Vector3(-1 * moveNowSpeed, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position += new Vector3(0, 1 * moveNowSpeed, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position += new Vector3(0, -1 * moveNowSpeed, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position += new Vector3(1 * moveNowSpeed, 0, 0);
        }

        float wheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (wheelValue > 0f)
        {
            Camera.main.orthographicSize -= wheelValue * sizeNowSpeed * Time.deltaTime;
        }
        else if (wheelValue < -0f)
        {
            Camera.main.orthographicSize += -wheelValue * sizeNowSpeed * Time.deltaTime;
        }
    }
}