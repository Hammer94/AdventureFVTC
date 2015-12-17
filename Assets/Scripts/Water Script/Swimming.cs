using UnityEngine;
using System.Collections;

public class Swimming : MonoBehaviour {

    float WaterArea;
    ParticleSystem Bubbles;
    bool ActiveUnderwater;
    Color NormalColor;
    Color UnderwaterColor;
    
    

	// Use this for initialization
	void Start ()
    {
        NormalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        UnderwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);
        Bubbles.Stop();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((transform.position.y < WaterArea) != ActiveUnderwater)
        {
            ActiveUnderwater = transform.position.y < WaterArea;
            if (ActiveUnderwater) CheckUnderwater();
            if (!ActiveUnderwater) CheckOnLand();
        }

        CheckOnLand();
        CheckUnderwater();
	}

    void CheckOnLand()
       
        {
            RenderSettings.fogColor = NormalColor;
            RenderSettings.fogDensity = 0.002f;
            Bubbles.Stop();
        }

	   void CheckUnderwater()
        {
            RenderSettings.fogColor = UnderwaterColor;
            RenderSettings.fogDensity = 0.03f;
            Bubbles.Play();
        }
}
