using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    void Start() {
        gameObject.SetActive(true);
    }
    public Animator anim;
    public void SwapScenes(){
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            SceneManager.LoadScene("Main");
        } else {
            SceneManager.LoadScene("MainMenu");
        }
    }    
    public void Exit() {
        gameObject.SetActive(true);
        anim.SetTrigger("Exit");
    }
    public void Disable() {
        gameObject.SetActive(false);
    }
}
