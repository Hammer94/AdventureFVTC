using UnityEngine;
using System.Collections;

public static class Inventory
{

    public static int Health = 6;
    public static int Score = 0;
    public static int Lvl1 = 0;
    public static int Lvl2 = 0;
    public static int OnEnter = 0;

    public static void Reset()
    {
        Health = 6;
        Score = 0;
        
    }

    public static void setcurrentscore(int Score)
    {
        OnEnter = Score;
    }
}
