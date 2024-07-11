using SakeShooter;
using UnityEngine;
using SakeShooterSystems;
using UnityEngine.InputSystem.Layouts;

namespace SakeShooter
{
    public class GameSupervisor : MonoBehaviour
    {
        [Header("-------- 難易度設定 ----------")]
        public DifficultySetting difficultySetting;
        
        [Header("---------- Managers ----------")]
        public UIController uiController;
        public MasuManager masuManager;
        
        private int _score = 0;
        
        private float[] _maxSpawnInterval = { 7.0f, 5.0f, 3.0f };
        private float[] _minSpawnInterval = { 4.0f, 2.0f, 0.5f };
        // enumにした方がいいかも？
        // こっちのdifficultyは全体の難易度ではなく相対的
        private int _currentDifficultyLevel = 0;
        private int _scoreByOneMasu = 100;

        private const float GameTime = 60.0f;
        private float _elapsedTime = 0.0f;
        private float _timeLeft = GameTime;

        private bool _isGameRunning = false;
        private bool _isGamePaused = false;

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
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                TogglePauseGame();
            }

            if (_isGameRunning)
            {
                CountDown();
                if (_currentDifficultyLevel == 0 && _elapsedTime >= 15.0f) ChangeDifficulty();
                if (_currentDifficultyLevel == 1 && _elapsedTime >= 30.0f) ChangeDifficulty();
            }

            UpdateUI();
        }

        private void SetParameters()
        {
            Parameters parameters = difficultySetting.GetParameters();

            _scoreByOneMasu = parameters.Score;
            _maxSpawnInterval = parameters.MaxWaitTimes;
            _minSpawnInterval = parameters.MinWaitTimes;
            
            // masuManager -----------------
            masuManager.masuSpeed = parameters.MasuSpeed;
            masuManager.masuAcceleration = parameters.MasuAcceleration;
            masuManager.tawaraSpeed = parameters.TawaraSpeed;
            masuManager.tawaraAcceleration = parameters.TawaraAcceleration;
            // ------------------------------
            
            
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
            masuManager.ChangeSpawnRate(_maxSpawnInterval[_currentDifficultyLevel],
                _minSpawnInterval[_currentDifficultyLevel]);
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
        
        private void TogglePauseGame()
        {
            _isGamePaused = !_isGamePaused;

            if (_isGamePaused)
            {
                // Logic to pause the game
                Time.timeScale = 0f;
            }
            else
            {
                // Logic to resume the game
                Time.timeScale = 1f;
            }
        }
    }
}
