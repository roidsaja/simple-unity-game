using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour {
    
    private void OnTriggerEnter (Collider col)
    {
        
        if (col.tag == "Player") //makes sure the tag is player
        {
            //victory code
            LevelManager.Instance.Victory();
        }
    }
	
}
