using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public Image[] lifeIcons = new Image[3];
    [SerializeField] private Sprite healthyLife;
    [SerializeField] private Sprite lostLife;
    public int lives = 3;

    void Start()
    {
        foreach(Image lifeIcon in lifeIcons) {
            lifeIcon.sprite = healthyLife;
        }
        lives = 3;
    }
    public void LoseLife() {
        if (lives >= 1) {
            lifeIcons[lives - 1].sprite = lostLife;
            lives -= 1;
            AudioManager.instance.PlaySound("Life Timer");
            if (lives <= 0) {
                GameManager.instance.GameOver();
            }
        }
    }
}
