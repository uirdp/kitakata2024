using System;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooter
{
    [CreateAssetMenu(fileName = "DifficultySetting", menuName = "DifficultySetting")]
    public class DifficultySetting : ScriptableObject
    {
        public List<Parameters> parameters;

        private Difficulty _currentDifficulty = Difficulty.Normal;
        
        public void SetDifficulty(Difficulty difficulty)
        {
            _currentDifficulty = difficulty;
        }
        
        public Difficulty GetNextDifficulty()
        {
            return (Difficulty)(((int)_currentDifficulty + 1) % Enum.GetValues(typeof(Difficulty)).Length);
        }
        
        public Difficulty GetCurrentDifficulty()
        {
            return _currentDifficulty;
        }
        
        public Parameters GetParameters()
        {
            return parameters[(int)_currentDifficulty];
        }
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
    
    
    [Serializable]
    public class Parameters
    {
        public Difficulty difficulty = Difficulty.Normal;
        
        [Header("------ 升の速度と加速度 ------")]
        [SerializeField] private float masuSpeed;
        [SerializeField] private float masuAcceleration;
        
        [Header("------ 升の生成間隔 ------")]
        [SerializeField] private float[] maxWaitTimes = { 0f, 0f, 0f };
        [SerializeField] private float[] minWaitTimes = { 0f, 0f, 0f };
        
        [Header("------ 升に溜まる量 ------")]
        [SerializeField] private float masuFillAmount;
        
        [Header("------ 俵の速度と加速度 ------")]
        [SerializeField] private float tawaraSpeed;
        [SerializeField] private float tawaraAcceleration;
        
        [Header("------ 成功時のスコア ------")]
        [SerializeField] private int score;
        
        public float MasuSpeed => masuSpeed;
        public float MasuAcceleration => masuAcceleration;
        public float TawaraSpeed => tawaraSpeed;
        public float TawaraAcceleration => tawaraAcceleration;
        public float[] MaxWaitTimes => maxWaitTimes;
        public float[] MinWaitTimes => minWaitTimes;
        public int Score => score;
        
        public float MasuFillAmount => masuFillAmount;
    }
}