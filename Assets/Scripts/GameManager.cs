using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay;
    public float timeScale = 1f;
    public static float sunIntensity { get { return Mathf.Clamp01(Mathf.Sin(Mathf.PI * hour / 12f)); } }
    public static int hour;
    private float secs;
    public CursorSpawner spawner;
    public static Dictionary<Creature.CreatureType, List<Creature>> creatures = new Dictionary<Creature.CreatureType, List<Creature>> 
    { { Creature.CreatureType.Jeller, new List<Creature>() }, { Creature.CreatureType.Fish, new List<Creature>() }, { Creature.CreatureType.Lobter, new List<Creature>() } };

    private void Start()
    {
        foreach (KeyValuePair<Creature.CreatureType, List<Creature>> creatureList in creatures)
            creatures[creatureList.Key].Clear();
        hour = 0;
        secs = 0;
    }
    private void Update()
    {
        timeDisplay.text = hour.ToString("00") + ":" + secs.ToString("00");
        secs += Time.deltaTime * timeScale;
        if (secs >= 60) 
        {
            hour++;
            if (hour >= 24)
                hour = 0;
            secs = 0;
        }
    }
}
