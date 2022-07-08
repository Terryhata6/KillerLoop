using System;

public class LevelEvents
{
        public static LevelEvents Current = new LevelEvents();

        public Action OnLevelStart;
        public void LevelStart()
        {
                OnLevelStart?.Invoke();
        }

        public Action OnLevelRestart;
        public void LevelRestart()
        { 
                OnLevelRestart?.Invoke();
        }

        public Action OnLevelFinish;
        public void LevelFinish()
        { 
                OnLevelFinish?.Invoke();
        }
        
        public Action OnLevelLose;
        public void LevelLose()
        { 
                OnLevelLose?.Invoke();
        }
        
        public Action OnChangeLevel;
        public void ChangeLevel()
        {
                OnChangeLevel?.Invoke();
        }

        public Action OnLevelChanged;
        public void LevelChanged()
        {
                OnLevelChanged?.Invoke();
        }
}