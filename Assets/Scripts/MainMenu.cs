using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelData
{
    public LevelData(string levelName)
    {
        string data = PlayerPrefs.GetString(levelName);
        if (data == "")
            return; //fail safe so we dont get error down here
        string[] allData = data.Split('&');
        bestTime = float.Parse(allData[0]);
        SilverTime = float.Parse(allData[1]);
        GoldTime = float.Parse(allData[2]);
    }

    public float bestTime { set; get; }
    public float GoldTime { set; get; }
    public float SilverTime { set; get; }
}

public class MainMenu : MonoBehaviour {
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;
    public GameObject shopButtonPrefab;
    public GameObject shopButtonContainer;
    public Material playerMaterial;
    public Text currencyText;

    private bool nextLevelLocked = false;
    private const float CameraSpeed = 3.0f;
    private Transform cameraTransform;
    private Transform cameraDesiredLook;
    private int[] costs = { 0, 100, 200, 400, 600, 800, 1000, 2000 };

    // Use this for initialization
    private void Start ()
    {
        ChangePlayerSkin(GameManager.Instance.currentSkinIndex);
        cameraTransform = Camera.main.transform;
        currencyText.text = "Currency : " + GameManager.Instance.currency.ToString();
                
        //look inside of resource folder and get every single objects inside of that folder
        Sprite [] thumbnails = Resources.LoadAll<Sprite>("Levels");
        foreach(Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            container.transform.SetParent(levelButtonContainer.transform, false);
            LevelData level = new LevelData(thumbnail.name);

            string minutes = ((int)level.bestTime / 60).ToString("00");
            string seconds = (level.bestTime % 60).ToString("00.00");
            container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (level.bestTime != 0.0f) ? minutes + ":" + seconds : "No record";

            container.transform.GetChild(1).GetComponent<Image>().enabled = nextLevelLocked;
            container.GetComponent<Button>().interactable = !nextLevelLocked;

            if (level.bestTime == 0.0f)
            {
                nextLevelLocked = true; //means level is not completed yet
            }

            string sceneName = thumbnail.name; //name of scene
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
        }

        //thumbnails for skins
        int textureIndex = 0;
        Sprite[] textures = Resources.LoadAll<Sprite>("Player");
        foreach (Sprite texture in textures)
        {
            GameObject container = Instantiate(shopButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = texture;
            container.transform.SetParent(shopButtonContainer.transform, false);

            int index = textureIndex;
            container.GetComponent<Button>().onClick.AddListener(() => ChangePlayerSkin(index));
            container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = costs[index].ToString();
            if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index)
            {
                container.transform.GetChild(0).gameObject.SetActive(false); //return transform of overlay
            }
            textureIndex++;
        }
	}

    private void Update()
    {
        if(cameraDesiredLook != null)
        {
            //slerp which is spherical, using slerp for rotation and lerp for position
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredLook.rotation, CameraSpeed * Time.deltaTime);
        }
    }
	
    //should be call for every button we click
    private void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LookAtMenu(Transform menuTransform)
    {
        cameraDesiredLook = menuTransform;
    }

    private void ChangePlayerSkin(int index)
    {
        if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index) //check if your index number does it contain in the skinavailability
        {
            float x = (index % 4) * 0.25f;
            float y = ((int)index / 4) * 0.25f;

            if (y == 0.0f)
                y = 0.5f;
            else if (y == 0.5f)
                y = 0.0f;

            playerMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));
            GameManager.Instance.currentSkinIndex = index;
            GameManager.Instance.Save(); //saves current skin
        }
        else
        {
            //Incase you do not have the skin, buying mechanics goes here
            int cost = costs[index];
            
            if (GameManager.Instance.currency >= cost)
            {
                GameManager.Instance.currency -= cost;
                GameManager.Instance.skinAvailability += 1 << index;
                GameManager.Instance.Save();
                currencyText.text = "Currency: " + GameManager.Instance.currency.ToString();
                shopButtonContainer.transform.GetChild(index).GetChild(0).gameObject.SetActive(false); //removes overlay if already bought
                ChangePlayerSkin(index);
            }
        }
    }
}
