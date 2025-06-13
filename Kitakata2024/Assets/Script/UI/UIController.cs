using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIDocument sakeAmountBar;
    [SerializeField] private UIDocument scoreText;
    [SerializeField] private UIDocument time;

    [SerializeField] private TextMeshPro scoreBoardText;
    [SerializeField] private TextMeshPro highScoreText;
    [SerializeField] private TextMeshPro highScoreBoardText;
    
    private const string ScoreMessage = "現在のすこあ：　";
    private const string HighScoreMessage = "はいすこあ！";
    private const string CelebrateMessage = "おめでとうございます";
    
    private ProgressBar _bar;

    private Label _label;
    private Label _score;
    private Label _difficulty;
    private Label _time;
    private Label _upgrade;
    
    private void Start()
    {
        _bar = sakeAmountBar.rootVisualElement.Q<ProgressBar>("SakeAmount");
        
        _label = scoreText.rootVisualElement.Q<Label>("Label");
        _upgrade = sakeAmountBar.rootVisualElement.Q<Label>("Upgrade");
        
        _score = scoreText.rootVisualElement.Q<Label>("Score");
        _time = time.rootVisualElement.Q<Label>("time");
        _difficulty = scoreText.rootVisualElement.Q<Label>("Difficulty");
        
        _difficulty.text = "";
        _upgrade.text = "";
        
        _score.text = "0";
        _time.text = "スペースキーで開始！";
    }

    public void ChangeUpgradeMessage(string m)
    {
        _upgrade.text = m;
    }
    public void ShowDifficulty(string d)
    {
        if (!string.IsNullOrEmpty(d)) d += "\n D: Change Difficulty\n Q: Quit Application\n ";
        else d = "";
            _difficulty.text = d;
    }
    public void UpdateTime(string t)
    {
        _time.text = t;
    }
    public void UpdateSakeAmount(float v)
    {
        _bar.value = v;
    }
    
    public void UpdateHighScore(string v)
    {
        Debug.Log("Updating High Score: " + v);
		highScoreText.text = "";
        highScoreText.text = v;
    }
    
    public void UpdateScoreBoard(string v)
    {
        scoreBoardText.text = v;
    }
    public void UpdateScore(string v)
    {
        scoreBoardText.text = v;
        _score.text = v;
    }
    
    public async void UpdateHighScoreBoard()
    {
        highScoreBoardText.text = HighScoreMessage;
        await UniTask.Delay(TimeSpan.FromSeconds(7.0f));
        
        highScoreBoardText.text = CelebrateMessage;
        
        await UniTask.Delay(TimeSpan.FromSeconds(5.0f));
        highScoreBoardText.text = ScoreMessage;
    }
    
    
}
