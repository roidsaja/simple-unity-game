using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    public GameObject pauseMenu;
    public Transform respawnPoint;
    public Text timertext;
    public Text endtimertext;
    public GameObject endVictory;

    private GameObject player;
    private float startTime;
    private float levelDuration;
    public float silverTime;
    public float goldTime;

    private void Start()
    {
        instance = this;
        pauseMenu.SetActive(false);
        endVictory.SetActive(false);
        startTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = respawnPoint.position;
    }

    private void Update()
    {
        if (player.transform.position.y < -10.0f) //if player is below certain y axis, it will respawn back to origin point
            Death();
        levelDuration = Time.time - startTime;
        string minutes = ((int)levelDuration / 60).ToString("00");
        string seconds = (levelDuration % 60).ToString("00.00");
        timertext.text = minutes + ":" + seconds;
    }
    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf); //boolean paremeter
        Time.timeScale = (pauseMenu.activeSelf) ? 0 : 1;
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Victory()
    {
        foreach(Transform t in endVictory.transform.parent)
        {
            t.gameObject.SetActive(false);
        }
        endVictory.SetActive(true);
        player.GetComponent<Rigidbody>();
        Rigidbody rigid = player.GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezePosition;

        levelDuration = Time.time - startTime;
        string minutes = ((int)levelDuration / 60).ToString("00");
        string seconds = (levelDuration % 60).ToString("00.00");
        endtimertext.text = minutes + ":" + seconds;

        float levelDurationn = Time.time - startTime; //duration of the whole level
        if (levelDuration < goldTime)
        {
            GameManager.Instance.currency += 50; //if user beats goldtime, he gets 50 coins
            endtimertext.color = Color.yellow;
        }
        else if (levelDuration < silverTime)
        {
            GameManager.Instance.currency += 25; //if user beats silvertime, he gets 25 coins
            endtimertext.color = Color.black;
        }
        else
        {
            GameManager.Instance.currency += 10; //if belw those time, only 10
            endtimertext.color = new Color(0.8f, 0.5f, 0.2f,1.0f); //float rgb colour of bronze
        }
        GameManager.Instance.Save();
        string saveString = "";
        LevelData level = new LevelData(SceneManager.GetActiveScene().name);
        saveString += (level.bestTime > levelDuration || level.bestTime == 0.0f) ? levelDuration.ToString() : level.bestTime.ToString();
        saveString += '&';
        saveString += silverTime.ToString();
        saveString += '&';
        saveString += goldTime.ToString();
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, saveString);
        
    }

    public void Death()
    {
        player.transform.position = respawnPoint.position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
