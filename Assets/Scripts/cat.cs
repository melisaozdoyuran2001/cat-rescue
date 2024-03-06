using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class cat : MonoBehaviour
{
    //private bool isCatTouched = false;
    private TextMeshProUGUI winText;
    private Button restartButton;
    public Button bonusButton;
    public GameObject character;
    public GameObject nextScene;
    public Timer timer;
    public float winCharPosX = -1.06f;
    public float winCharPosY = 46.32f;
    private bool noWinYet = true;
    public AudioClip meow;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        winText = GameObject.Find("WinText").GetComponent<TextMeshProUGUI>();
        winText.text = "";
        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.gameObject.SetActive(false);
        if (bonusButton != null)
        {
            print("button");
            bonusButton.gameObject.SetActive(false);
        }
        restartButton.onClick.AddListener(NextScene);
        noWinYet = true;
       
    }

    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (noWinYet)
        {
            if (other.gameObject == character)
            {
                noWinYet = false;
                audioSource.PlayOneShot(meow, 1);
                character.GetComponent<Rigidbody2D>().gravityScale = 0;
                character.transform.position = new Vector2(winCharPosX, winCharPosY);
                character.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                winText.text = "You Won!";
                restartButton.gameObject.SetActive(true);
                character.GetComponent<Character>().endGame = true;
                if(bonusButton != null)
                {
                    bonusButton.gameObject.SetActive(true);
                }
            }
        }
    }

    void NextScene()
    {
       

    }
}

//y = -3.184443