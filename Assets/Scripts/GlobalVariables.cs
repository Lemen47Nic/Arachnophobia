using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    bool ended = false;
    public bool Ended
    {
        get
        {
            return ended;
        }
        set
        {
            ended = value;
        }
    }

    int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    int totCharacters = 0;
    int readyCharacters = 0;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void ImHere()
    {
        if (!ended)
        {
            totCharacters++;
        }
        else
        {
            readyCharacters++;
            if (readyCharacters == totCharacters)
            {
                ended = false;
            }
        }
    }

    public void ZeroCharacters()
    {
        readyCharacters = 0;
    }
}
