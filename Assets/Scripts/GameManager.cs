using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance { get {return instance;} }

    public int currentSkinIndex = 0;
    public int currency = 0;
    public int skinAvailability = 1;

    private void Awake() //it is being called before start and load all of these values
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        //check if we ever saved data/play the game before
        if (PlayerPrefs.HasKey("CurrentSkin"))
        {
            //We had a previous session and load the previously saved game
            currentSkinIndex = PlayerPrefs.GetInt("CurrentSkin");
            currency = PlayerPrefs.GetInt("Currency");
            skinAvailability = PlayerPrefs.GetInt("SkinAvailability");
        }
        else
        {
            Save();
        }

    }
	
    public void Save()
    {
        //being our new game and load these up
        PlayerPrefs.SetInt("CurrentSkin", currentSkinIndex);
        PlayerPrefs.SetInt("Currency", currency);
        PlayerPrefs.SetInt("SkinAvailability", skinAvailability);

    }
}
