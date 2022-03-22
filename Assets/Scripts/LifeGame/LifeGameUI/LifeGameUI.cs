using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifeGameUI : MonoBehaviour
{
    public Text LineText;
    public InputField LineInput;
    
    public Text ColumnText;
    public InputField ColumnInput;
    
    public Button BornBtn;
    public Button RefreshBtn;
    public Button RandomBtn;
    public Button AdaptiveScreenBtn;

    public Text UpdateSpeedText;
    public InputField UpdateSpeedInput;
    public Slider UpdateSpeedSlider;

    public Button PauseBtn;
    public Button ContinueBtn;

    public int Height = 10;
    public int Width = 10;

    public Dropdown GameMode;
    public Text BornText;
    public InputField BornInput;
    public Text LiveText;
    public InputField LiveInput;
    public Text LessDeadText;
    public InputField LessDeadInput;
    public Text MoreDeadText;
    public InputField MoreDeadInput;
    
    public byte LiveCount = 2;
    public byte BornCount = 3;
    public byte LessDeadCount = 2;
    public byte MoreDeadCount = 3;
    
    
    private void Start()
    {
        LineInput.onEndEdit.AddListener(UpdateHeight);
        ColumnInput.onEndEdit.AddListener(UpdateWidth);
        
        LiveInput.onEndEdit.AddListener(UpdateLive);
        BornInput.onEndEdit.AddListener(UpdateBorn);
        MoreDeadInput.onEndEdit.AddListener(UpdateMoreDead);
        LessDeadInput.onEndEdit.AddListener(UpdateLessDead);
        
        BornBtn.onClick.AddListener(Born);
        RefreshBtn.onClick.AddListener(() => {GameControllerChessBoard.instance.ChessBoard.RefreshChunks();});
        RandomBtn.onClick.AddListener(() => {GameControllerChessBoard.instance.RandomChessBoard();});
        AdaptiveScreenBtn.onClick.AddListener(() => { GameControllerChessBoard.instance.AdaptiveScreen();});
        
        PauseBtn.onClick.AddListener(() => { GameControllerChessBoard.instance.SetStartUpdate(false);});
        ContinueBtn.onClick.AddListener(() => { GameControllerChessBoard.instance.SetStartUpdate(true);});
        
        UpdateSpeedInput.onEndEdit.AddListener(UpdateSpeed);
        UpdateSpeedSlider.onValueChanged.AddListener(UpdateSpeed);
        
        GameMode.onValueChanged.AddListener(UpdateGameMode);
        
        UpdateHeight("10");
        UpdateWidth("10");


        UpdateLive(GameControllerChessBoard.instance.LiveCount.ToString());
        UpdateBorn(GameControllerChessBoard.instance.BornCount.ToString());
        UpdateLessDead(GameControllerChessBoard.instance.LessDeadCount.ToString());
        UpdateMoreDead(GameControllerChessBoard.instance.MoreDeadCount.ToString());
        
        UpdateSpeed(GameControllerChessBoard.instance.UpdateTimer.ToString());
        UpdateGameMode(0);
        InitUi();
    }

    protected virtual void InitUi()
    {
        
    }
    
    protected virtual void Born()
    {
        GameControllerChessBoard.instance.ChessBoard.BornChunks(Height, Width);
    }

    protected virtual void UpdateGameMode(int index)
    {
        switch (index)
        {
            case 0:
                GameControllerChessBoard.instance.GameMode = LifeGameMode.TwoState_LD;
                break;
            case 1:
                GameControllerChessBoard.instance.GameMode = LifeGameMode.ThreeState_LDO;
                break;
        }
    }
    
    private void UpdateHeight(string inputContent)
    {
        Height = int.Parse(inputContent);
        LineText.text = $"行数:{Height}";
    }
    
    private void UpdateWidth(string inputContent)
    {
        Width = int.Parse(inputContent);
        ColumnText.text = $"列数:{Width}";
    }

    private void UpdateLive(string inputContent)
    {
        LiveCount = byte.Parse(inputContent);
        LiveText.text = $"生存基数:{LiveCount}";
        LiveInput.text = inputContent;
        GameControllerChessBoard.instance.LiveCount = LiveCount;
    }
    
    private void UpdateLessDead(string inputContent)
    {
        LessDeadCount = byte.Parse(inputContent);
        LessDeadText.text = $"Less消亡:{LessDeadCount}";
        LessDeadInput.text = inputContent;
        GameControllerChessBoard.instance.LessDeadCount = LessDeadCount;
    }
    
    private void UpdateMoreDead(string inputContent)
    {
        MoreDeadCount = byte.Parse(inputContent);
        MoreDeadText.text = $"More消亡:{MoreDeadCount}";
        MoreDeadInput.text = inputContent;
        GameControllerChessBoard.instance.MoreDeadCount = MoreDeadCount;
    }
    
    private void UpdateBorn(string inputContent)
    {
        BornCount = byte.Parse(inputContent);
        BornText.text = $"繁衍基数:{BornCount}";
        BornInput.text = inputContent;
        GameControllerChessBoard.instance.BornCount = BornCount;
    }

    private void UpdateSpeed(string inputContent)
    {
        float speed = float.Parse(inputContent);
        UpdateSpeedText.text = $"更新速度{speed}秒";
        UpdateSpeedSlider.value = speed;
        GameControllerChessBoard.instance.SetUpdateTimer(speed);
    }
    
    private void UpdateSpeed(float speed)
    {
        UpdateSpeedText.text = $"更新速度{speed}秒";
        GameControllerChessBoard.instance.SetUpdateTimer(speed);
    }
    
    private void Update()
    {
        
    }
}
