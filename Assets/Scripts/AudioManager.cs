using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); return; }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
