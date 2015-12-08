using UnityEngine;

// @author  Ryan
// @date    08 Dec 2015  
namespace AdventureFVTC {
    public static class PersistentPlayerStats {
        private static int HealthOnExit = 3;
        private static int HealthOnEnter = 3;
        private static int Lives = 3;
        private static int ScoreTotal = 100;
        private static int ScoreCurrent = 0;
        private static int Lvl1Goal = 5;
        private static int Lvl2Goal = 5;
        private static int Lvl1Total = 0;
        private static int Lvl2Total = 0;
        private static int Lvl1Current = 0;
        private static int Lvl2Current = 0;
        private static bool hasSnowBallOnExit = false;
        private static bool hasSnowBallCurrent = false;

        public static int LivesLeft {
            get { return Lives; }
        }

        public static int GetScoreTotal
        {
            get { return ScoreTotal; }
        }
        public static void GameOver() { // Occurs when a player dies after having no lives left.
            ResetCurrentHealthAndLives();
            ResetCurrentScore();
            ResetCurrentItems();
            ResetCurrentSnowBall();
        }

        public static bool HasLevel1GoalBeenMet { // Has the player gotten the items they needs from level one?
            get { return (Lvl1Current >= Lvl1Goal || Lvl1Total >= Lvl1Goal); }
        }

        public static bool HasLevel2GoalBeenMet { // Has the player gotten the items they needs from level two?
            get { return (Lvl2Current >= Lvl2Goal || Lvl2Total >= Lvl2Goal); }
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
            Lvl1Current += amount;
        }

        public static void AddToCurrentLevel2(int amount) { // Add to the current level2 items when picking up a level2 item.
            Lvl2Current += amount;
        }

        public static void SetOnEnterHealth() { // Set health on entering a level as the health the player had after exiting the last level.
            HealthOnEnter = HealthOnExit;
        }

        public static void UpdateTotalsOnExit() { // Update the totals when exiting a level so you keep track of the player's progress.
            ScoreTotal += ScoreCurrent;
            Lvl1Total += Lvl1Current;
            Lvl2Total += Lvl2Current;
            hasSnowBallOnExit = hasSnowBallCurrent;
        }

        public static void SetOnExitHealth(int health) { // Set health on exiting a level;
            HealthOnExit = health;
        }

        public static void ResetCurrentScore() { // Reset on entering a level or on a gameover.
            ScoreCurrent = 0;
        }

        public static void ResetCurrentItems() { // Reset on entering a level or on a gameover.
            Lvl1Current = 0;
            Lvl2Current = 0;
        }
        
        public static void ResetCurrentSnowBall() { // Reset on enter a level or on a gameover.
            hasSnowBallCurrent = false;
        }

        private static void ResetCurrentHealthAndLives() { // Reset on a gameover.
            HealthOnEnter = 6;
            Lives = 3;
        }
    }
}
