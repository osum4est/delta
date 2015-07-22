using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager {

    public void UpdateHealth()
    {
        Image[] imgs = Globals.i.hudHealth.GetComponentsInChildren<Image>();
        for (int i = 0; i < imgs.Length; i++)
        {
            Color color = imgs[i].color;
            if (i < Globals.i.playerStats.currentHealth)
            {
                color.a = 1;
            }
            else
            {
                color.a = 0;
            }
            imgs[i].color = color;
        }
    }

    public void UpdateScore()
    {
        Globals.i.hudScore.GetComponent<Text>().text = Globals.i.playerStats.score.ToString();
    }
}
