using SakeShooter;
using UnityEngine;
using SakeShooterSystems;

public class GameSuperviser : MonoBehaviour
{
    public UIController uiController;
    public MasuManager masuManager;
    
    public int _scoreByOneMasu = 100;
    private int _score = 0;

    private const float GameTime = 60.0f;
    private float _elapsedTime = 0.0f;
    private float _timeLeft = GameTime;
    
    private bool _isGameRunning = false;
    
    public void OnMasuExit(MasuExitStatus status)
    {
        switch (status)
        {
            case MasuExitStatus.Success:
                _score += _scoreByOneMasu;
                break;
            case MasuExitStatus.Failure:
                //Debug.Log("SV: Masu Exit Failure");
                break;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        
        if(Input.GetKeyDown(KeyCode.Q) || _timeLeft <= 0.0f)
        {
            EndGame();
        }
        
        if(_isGameRunning) CountDown();
        
        UpdateUI();   
    }
    
    private void StartGame()
    {
        _isGameRunning = true;
        _score = 0;
        _elapsedTime = 0.0f;
        
        masuManager.GameStart();
    }
    
    private void EndGame()
    {
        _isGameRunning = false;
        
        masuManager.GameEnd();
    }

    private void CountDown()
    {
        _elapsedTime += Time.deltaTime;
        _timeLeft = GameTime - _elapsedTime;
        
        if(_timeLeft <= 0.0f) _timeLeft = 0.0f;
    }

    private void UpdateScore()
    {
        uiController.UpdateScore(_score);
    }

    private void UpdateTime()
    {
        uiController.UpdateTime(_timeLeft);
    }
    private void UpdateUI()
    {
        UpdateScore();
        UpdateTime();
    }
}
