using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int critChance;

    public int _critChance;
    
	void Start ()
    {
        critChance = _critChance;
	}
}
