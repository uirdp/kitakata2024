using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIDocument sakeAmountBar;
    [SerializeField] private UIDocument scoreText;
    [SerializeField] private UIDocument time;
    
    private ProgressBar _bar;

    private Label _label;
    private Label _score;
    private Label _difficulty;
    private Label _time;
    
    private void Start()
    {
        _bar = sakeAmountBar.rootVisualElement.Q<ProgressBar>("SakeAmount");
        
        _label = scoreText.rootVisualElement.Q<Label>("Label");
        _score = scoreText.rootVisualElement.Q<Label>("Score");
        _time = time.rootVisualElement.Q<Label>("time");
        _difficulty = scoreText.rootVisualElement.Q<Label>("Difficulty");

        _difficulty.text = "";
        _score.text = "0";
        _time.text = "Stopped";
    }

    public void ShowDifficulty(string d)
    {
        _difficulty.text = d;
    }
    public void UpdateTime(float t)
    {
        _time.text = t.ToString("F2");
    }
    public void UpdateSakeAmount(float v)
    {
        _bar.value = v;
    }
    
    public void UpdateScore(int v)
    {
        _score.text = v.ToString();
    }
}
