using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffin : MonoBehaviour
{
    public DragCorpse corpse;

    public void ShutCorpse()
    {
        corpse.Slam();
    }
}
