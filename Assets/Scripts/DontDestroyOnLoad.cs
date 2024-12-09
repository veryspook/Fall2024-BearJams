using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool exists;
    void Start()
    {
        if (!exists){
            exists = true;
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
    }

}
