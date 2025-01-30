using System;
using Cysharp.Threading.Tasks;
using SakeShooter;
using UnityEngine;
using SakeShooterSystems;

namespace SakeShooter
{
    public class GameMain : MonoBehaviour
    {
        [Header("-------- 難易度設定 ----------")]
        public DifficultySetting difficultySetting;
        
        [Header("---------- Managers ----------")]
        public UIController uiController;
        public SoundEffectManager soundEffectManager;
        public MasuSpawnManager masuSpawnManager;
        public ScoreRecorder scoreRecorder;
        public SakeGun sakeGun;

        [Header("-------- 雰囲気 ----------")] 
        public Light normalLight;
        public Light nightLight;
        
        private int _score = 0;
        
        private float[] _maxSpawnInterval = { 7.0f, 5.0f, 3.0f };
        private float[] _minSpawnInterval = { 4.0f, 2.0f, 0.5f };
        
        // ゲーム開始から時間がたつとスポーンする升の量が増える
        private int _masuSpawnIntervalLevel = 0;
        private int _scoreByOneMasu = 100;

        private const float GameTime = 70.0f;
        private float _elapsedTime = 0.0f;
        private float _timeLeft = GameTime;

        private bool _isGameRunning = false;
        private bool _isGamePaused = false;
        
        private bool _isMasuSpawning = false;

        private bool _isScoreRecorded = true;
        
        private void Start()
        {
            UpdateHighScore();
            PlayMusic();
            ToggleNormalLight();
        }
        
        private void Update()
        {
            ProcessInput();
             
            if (_isGamePaused)
            {
                ProcessInputPaused();
            }

            if (_isGameRunning)
            {
                CountDown();
                UpdateUI();
                if (_masuSpawnIntervalLevel == 0 && _elapsedTime >= 15.0f) ChangeMasuSpawnLevel();
                if (_masuSpawnIntervalLevel == 1 && _elapsedTime >= 30.0f) ChangeMasuSpawnLevel();
            }
            
        }
        
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

        
        //　応急措置
        private async void PlayMusic()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
            soundEffectManager.PlayRestMusic();
        }
        
        private void ProcessInput()
        {
            // 升のスポーン開始（チュートリアル用）
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (!_isMasuSpawning)
                {
                    masuSpawnManager.StartSpawning();
                    _isMasuSpawning = true;
                }
                else
                {
                    masuSpawnManager.EndSpawning();
                    _isMasuSpawning = false;
                }
            }
            
            // ゲーム開始
            if (Input.GetKeyDown(KeyCode.Space) && !_isGameRunning)
            {
                StartGame();
            }
            
            // ゲーム終了（アプリケーションは終了しない）
            if (Input.GetKeyDown(KeyCode.Q) || _timeLeft <= 0.0f)
            {
                if(_isGameRunning)  EndGame();
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                TogglePauseGame();
            }
        }
        
        private void ProcessInputPaused()
        {
            if (Input.GetKeyDown(KeyCode.D))　SetDifficulty();
            if (Input.GetKeyDown(KeyCode.P)) _isScoreRecorded = false;
                
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ToggleNormalLight();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ToggleNightLight();
            }
            
            if(Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.C))
            {
                scoreRecorder.ResetAllScores();
                UpdateHighScore();
            }
        }
        
        // 雰囲気を変える
        private void ToggleNormalLight()
        {
            normalLight.enabled = true;
            nightLight.enabled = false;
        }
        
        // 雰囲気を変える
        private void ToggleNightLight()
        {
            // Enable the night light and disable the normal light
            nightLight.enabled = true;
            normalLight.enabled = false;
        }
        private void SetDifficulty()
        {
           
                Difficulty d = difficultySetting.GetNextDifficulty();
                difficultySetting.SetDifficulty(d);
                
                uiController.ShowDifficulty(d.ToString());
                
                SetParameters();
            
        }
        
        // 難易度設定によってパラメータを設定
        private void SetParameters()
        {
            Parameters parameters = difficultySetting.GetParameters();

            _scoreByOneMasu = parameters.Score;
            _maxSpawnInterval = parameters.MaxWaitTimes;
            _minSpawnInterval = parameters.MinWaitTimes;
            
            // masuSpawnManager -----------------
            masuSpawnManager.masuSpeed = parameters.MasuSpeed;
            masuSpawnManager.masuAcceleration = parameters.MasuAcceleration;
            masuSpawnManager.tawaraSpeed = parameters.TawaraSpeed;
            masuSpawnManager.tawaraAcceleration = parameters.TawaraAcceleration;
            masuSpawnManager.fillAmount = parameters.MasuFillAmount;
            // ------------------------------
        }
        private void StartGame()
        {
            
            SetParameters();

            sakeGun.ResetUpgradeStatus();
            _isScoreRecorded = true;
            _timeLeft = GameTime;
            _isGameRunning = true;
            _score = 0;
            _elapsedTime = 0.0f;
            _masuSpawnIntervalLevel = 0;
            
            ChangeSpawnRate();
            
            masuSpawnManager.StartSpawning();
            soundEffectManager.PlayGameStartSound();
            soundEffectManager.PlayGameMusic();
        }

        private void EndGame()
        {
            _isGameRunning = false;

            masuSpawnManager.EndSpawning();
            soundEffectManager.PlayGameEndSound();
            
            soundEffectManager.PlayRestMusic();

            if (_isScoreRecorded)
            {
                if (scoreRecorder.RecordTopThreeScores(_score))
                {
                    uiController.UpdateHighScoreBoard();
                }
            }
            
            UpdateHighScore();
            uiController.UpdateTime("スペースで開始");
        }

        private void CountDown()
        {
            _elapsedTime += Time.deltaTime;
            _timeLeft = GameTime - _elapsedTime;

            if (_timeLeft <= 0.0f) _timeLeft = 0.0f;
        }
        
        private void ChangeSpawnRate()
        {
            masuSpawnManager.ChangeSpawnRate(_maxSpawnInterval[_masuSpawnIntervalLevel],
                _minSpawnInterval[_masuSpawnIntervalLevel]);
        }
        private void ChangeMasuSpawnLevel()
        {
            _masuSpawnIntervalLevel++;
            if(_masuSpawnIntervalLevel > 3) _masuSpawnIntervalLevel = 3;
            ChangeSpawnRate();
        }

        private void UpdateScore()
        {
            uiController.UpdateScoreBoard("現在のすこあ: " + _score.ToString());
            uiController.UpdateScore(_score.ToString());
        }

        private void UpdateTime()
        {
            uiController.UpdateTime(_timeLeft.ToString("F2"));
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
                soundEffectManager.PlayPauseSound();
                
                Difficulty d = difficultySetting.GetCurrentDifficulty();
                uiController.ShowDifficulty(d.ToString());
            }
            else
            {
                // Logic to resume the game
                Time.timeScale = 1f;
                soundEffectManager.PlayResumeSound();
                
                uiController.ShowDifficulty("");
                SetParameters();
            }
        }

        public async void NotifyUpgrade()
        {
            uiController.ChangeUpgradeMessage("あっぷぐれえど！");

            await UniTask.Delay(TimeSpan.FromSeconds(2.0f));
            
            uiController.ChangeUpgradeMessage("");
        }
        
        private void UpdateHighScore()
        {
            int[] scores = scoreRecorder.GetTopThreeScores();
            
            string highScores = scores[0] + "\n" + scores[1] + "\n" + scores[2];
            uiController.UpdateHighScore(highScores);
        }
        
       
    }
}
