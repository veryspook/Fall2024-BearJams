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
    // Start is called before the first frame update
    void Start()
    {
        foreach(Image lifeIcon in lifeIcons) {
            lifeIcon.sprite = healthyLife;
        }
        lives = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoseLife() {
        if (lives >= 1) {
            lifeIcons[lives - 1].sprite = lostLife;
            lives -= 1;
            if (lives <= 0) {
            //trigger gameover
            }
        }
    }
}
