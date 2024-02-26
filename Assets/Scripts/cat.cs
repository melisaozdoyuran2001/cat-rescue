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
    public GameObject character;
    public GameObject nextScene;
    public Timer timer;
    public float winCharPosX = -1.06f;
    public float winCharPosY = 46.32f;

    void Start()
    {
        winText = GameObject.Find("WinText").GetComponent<TextMeshProUGUI>();
        winText.text = "";
        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(NextScene);

    }

    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //isCatTouched = true;
        if (other.gameObject == character) {
            character.GetComponent<Rigidbody2D>().gravityScale = 0;
            character.transform.position = new Vector2(winCharPosX, winCharPosY);
            character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            winText.text = "You Won!";
            restartButton.gameObject.SetActive(true);
            character.GetComponent<Character>().endGame = true;
        }
    }

    void NextScene()
    {
       

    }
}

//y = -3.184443