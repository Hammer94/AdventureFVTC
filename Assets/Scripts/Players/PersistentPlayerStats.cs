using UnityEngine;

// @author  Ryan
// @date    14 Dec 2015  
namespace AdventureFVTC {
    public static class PersistentPlayerStats {
        private static int HealthOnExit = 3;
        private static int HealthOnEnter = 3;
        private static int Lives = 3;
        private static int ScoreTotal = 0;
        private static int ScoreCurrent = 0;
        private static int lvl1Goal = 5;
        private static int lvl2Goal = 5;
        private static int lvl1ItemTotal = 0;
        private static int lvl2ItemTotal = 0;
        private static int currentItems = 0;
        private static bool hasSnowBallOnExit = false;
        private static bool hasSnowBallCurrent = false;

        public static int GetHealthOnExit
        {
            get { return HealthOnExit; }
        }

        public static int LivesLeft {
            get {
                return Lives;
            }
            set {
                Lives = value;
                if (Lives < 0)
                    Lives = 0;
            }
        }

        public static int GetScoreTotal
        {
            get { return ScoreTotal; }
        }

        public static int GetCurrentItems
        {
            get { return currentItems; }
        }

        public static int GetLevel1Total
        {
            get { return lvl1ItemTotal; }
        }

        public static int GetLevel2Total
        {
            get { return lvl2ItemTotal; }
        }

        public static int GetLevel1Goal
        {
            get { return lvl1Goal; }
        }

        public static int GetLevel2Goal
        {
            get { return lvl2Goal; }
        }

        public static bool HasLevel1GoalBeenMet { // Has the player gotten the items they needs from level one?
            get { return (currentItems >= lvl1Goal || lvl1ItemTotal >= lvl1Goal); }
        }

        public static bool HasLevel2GoalBeenMet { // Has the player gotten the items they needs from level two?
            get { return (currentItems >= lvl2Goal || lvl2ItemTotal >= lvl2Goal); }
        }

        public static void GotSnowBall() { // When the player picks up the snowball, let them throw it.
            hasSnowBallCurrent = true;
        }

        public static bool HasSnowBall { // Has the player picked up the snowball yet?  
                get { return (hasSnowBallCurrent || hasSnowBallOnExit); }
        }

        public static void AddToCurrentScore(int amount) { // Add to the current score when picking up a coin.
            ScoreCurrent += amount;
        }

        public static void AddToCurrentLevel1(int amount) { // Add to the current level1 items when picking up a level1 item.
            currentItems += amount;
        }

        public static void AddToCurrentLevel2(int amount) { // Add to the current level2 items when picking up a level2 item.
            currentItems += amount;
        }

        public static void ResetCurrentsOnExit() { // Update the currents when entering a level so they start with no progress on the level.
            SetOnEnterHealth();
            ResetCurrentScore();
            ResetCurrentItems();
            ResetCurrentSnowBall();
        }

        public static void UpdateTotalsOnExit(string level, int health) { // Update the totals when exiting a level so you keep track of the player's progress.
            ScoreTotal += ScoreCurrent;
            if (level == "Level1")
                lvl1ItemTotal += currentItems;
            else if (level == "Level2")
                lvl2ItemTotal += currentItems;
            hasSnowBallOnExit = hasSnowBallCurrent;
            HealthOnExit = health;
        }

        public static void GameOver() { // Occurs when a player dies after having no lives left.
            ResetCurrentHealthAndLives();
            ResetCurrentItems();
            ResetCurrentScore();
            ResetCurrentSnowBall();
        }

        private static void SetOnEnterHealth() { // Set health on entering a level as the health the player had after exiting the last level.
            HealthOnEnter = HealthOnExit;
        }

        private static void ResetCurrentHealthAndLives() { // Reset on a gameover.
            HealthOnEnter = 3;
            Lives = 3;
        }

        private static void ResetCurrentItems() { // Reset on entering a level or on a gameover.
            currentItems = 0;
        }

        private static void ResetCurrentScore() { // Reset on entering a level or on a gameover.
            ScoreCurrent = 0;
        }

        private static void ResetCurrentSnowBall() { // Reset on enter a level or on a gameover.
            hasSnowBallCurrent = false;
        }      
    }
}
