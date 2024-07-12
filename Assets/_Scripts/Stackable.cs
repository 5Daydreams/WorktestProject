using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour
{
    private Stackable next;

    public Stackable Next
    {
        get => next;
        set
        {
            if(next == null) 
            {
                next = value;
            }
        }
    }


}
