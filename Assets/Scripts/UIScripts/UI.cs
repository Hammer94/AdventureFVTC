using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace AdventureFVTC
{
    public class UI : MonoBehaviour
    {
        GameObject lifeCounter;
        Text LifeCounter;
        //GameObject scoreCounter;
        //Text ScoreCounter;
        GameObject filled1;
        GameObject filled2;
        GameObject filled3;
        GameObject itemCounter;
        Text ItemCounter;
        GameObject presentsImage;

        // Use this for initialization
        void Start()
        {
            filled1 = GameObject.FindGameObjectWithTag("filled1");
            filled2 = GameObject.FindGameObjectWithTag("filled2");
            filled3 = GameObject.FindGameObjectWithTag("filled3");
            lifeCounter = GameObject.FindGameObjectWithTag("Lives");
            LifeCounter = lifeCounter.GetComponent<Text>();
            LifeCounter.text = "x " + PersistentPlayerStats.LivesLeft.ToString();
            itemCounter = GameObject.FindGameObjectWithTag("Items");
            ItemCounter = itemCounter.GetComponent<Text>();
            ItemCounter.text = "x " + PersistentPlayerStats.GetCurrentItems.ToString();
            presentsImage = GameObject.FindGameObjectWithTag("Presents");
            //scoreCounter = GameObject.FindGameObjectWithTag("Score");
            //ScoreCounter = scoreCounter.GetComponent<Text>();
            //ScoreCounter.text = "Score: " + PersistentPlayerStats.GetScoreTotal.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            LifeCounter.text = "x " + PersistentPlayerStats.LivesLeft.ToString();

            if (Application.loadedLevelName == "Level1") // We're currently on Level1.
            {
                if (PersistentPlayerStats.GetCurrentItems > PersistentPlayerStats.GetLevel1Total)
                    ItemCounter.text = "x " + PersistentPlayerStats.GetCurrentItems.ToString() + "/" + PersistentPlayerStats.GetLevel1Goal.ToString();
                else              
                    ItemCounter.text = "x " + PersistentPlayerStats.GetLevel1Total.ToString() + "/" + PersistentPlayerStats.GetLevel1Goal.ToString();               
            }
            else if (Application.loadedLevelName == "Level2") // We're currently on Level1.
            {
                if (PersistentPlayerStats.GetCurrentItems > PersistentPlayerStats.GetLevel2Total)
                    ItemCounter.text = "x " + PersistentPlayerStats.GetCurrentItems.ToString() + "/" + PersistentPlayerStats.GetLevel2Goal.ToString();
                else
                    ItemCounter.text = "x " + PersistentPlayerStats.GetLevel2Total.ToString() + "/" + PersistentPlayerStats.GetLevel2Goal.ToString();
            }
            else // Else we're on LeveSelect or the Boss Level
            {
                ItemCounter.text = ""; // Hide the text.
                presentsImage.GetComponent<CanvasRenderer>().SetAlpha(0); // Hide the image.
            }

            
            if (Services.Run.Player.Character != null && Services.Run.Player.Character.Health == 3)
            {
                filled1.GetComponent<CanvasRenderer>().SetAlpha(1f);
                filled2.GetComponent<CanvasRenderer>().SetAlpha(1f);
                filled3.GetComponent<CanvasRenderer>().SetAlpha(1f);

            }
            else if(Services.Run.Player.Character != null && Services.Run.Player.Character.Health == 2)
            {
                filled1.GetComponent<CanvasRenderer>().SetAlpha(1f);
                filled2.GetComponent<CanvasRenderer>().SetAlpha(1f);
                filled3.GetComponent<CanvasRenderer>().SetAlpha(0f);
            }
            else if (Services.Run.Player.Character != null && Services.Run.Player.Character.Health == 1)
            {
                filled1.GetComponent<CanvasRenderer>().SetAlpha(1f);
                filled2.GetComponent<CanvasRenderer>().SetAlpha(0f);
                filled3.GetComponent<CanvasRenderer>().SetAlpha(0f);
            }
            else if (Services.Run.Player.Character != null && Services.Run.Player.Character.Health == 0)
            {
                filled1.GetComponent<CanvasRenderer>().SetAlpha(0f);
                filled2.GetComponent<CanvasRenderer>().SetAlpha(0f);
                filled3.GetComponent<CanvasRenderer>().SetAlpha(0f);
            }
        }
    }
}