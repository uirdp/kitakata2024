using SakeShooter;
using UnityEngine;
using SakeShooterSystems;
using UnityEngine.InputSystem.Layouts;

namespace SakeShooter
{
    public class GameSuperviser : MonoBehaviour
    {
        public UIController uiController;
        public MasuManager masuManager;

        public int _scoreByOneMasu = 100;
        private int _score = 0;
        
        [Header("マスの生成スピードの調整：maxとminの間でランダムに生成される")]
        public float[] maxSpawnInterval = { 7.0f, 5.0f, 3.0f };
        public float[] minSpawnInterval = { 4.0f, 2.0f, 0.5f };
        // enumにした方がいいかも？
        private int _currentDifficultyLevel = 0;

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
            if (Input.GetKeyDown(KeyCode.T))
            {
                masuManager.StartSpawning();
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && !_isGameRunning)
            {
                StartGame();
            }

            if (Input.GetKeyDown(KeyCode.Q) || _timeLeft <= 0.0f)
            {
                EndGame();
            }

            if (_isGameRunning)
            {
                CountDown();
                if (_currentDifficultyLevel == 0 && _elapsedTime >= 15.0f) ChangeDifficulty();
                if (_currentDifficultyLevel == 1 && _elapsedTime >= 30.0f) ChangeDifficulty();
            }

            UpdateUI();
        }
        
        private void StartGame()
        {
            _timeLeft = GameTime;
            _isGameRunning = true;
            _score = 0;
            _elapsedTime = 0.0f;
            _currentDifficultyLevel = 0;
            
            ChangeSpawnRate();

            masuManager.StartSpawning();
        }

        private void EndGame()
        {
            _isGameRunning = false;

            masuManager.EndSpawning();
        }

        private void CountDown()
        {
            _elapsedTime += Time.deltaTime;
            _timeLeft = GameTime - _elapsedTime;

            if (_timeLeft <= 0.0f) _timeLeft = 0.0f;
        }
        
        private void ChangeSpawnRate()
        {
            masuManager.ChangeSpawnRate(maxSpawnInterval[_currentDifficultyLevel], minSpawnInterval[_currentDifficultyLevel]);
        }
        private void ChangeDifficulty()
        {
            _currentDifficultyLevel++;
            if(_currentDifficultyLevel > 3) _currentDifficultyLevel = 3;
            ChangeSpawnRate();
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
}
