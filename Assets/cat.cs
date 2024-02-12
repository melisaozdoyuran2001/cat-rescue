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
    private GameObject character;

    void Start()
    {
        winText = GameObject.Find("WinText").GetComponent<TextMeshProUGUI>();
        winText.text = "";
        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.gameObject.SetActive(false);
        character = GameObject.Find("Character");
        restartButton.onClick.AddListener(RestartGame);

    }

    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       //isCatTouched = true;
       character.GetComponent<Rigidbody2D>().gravityScale = 0;
        character.transform.position = new Vector2(-1.06f, 46.32f);
        character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        winText.text = "You Won!";
        restartButton.gameObject.SetActive(true);
        character.GetComponent<Character>().endGame = true;
    }

    void RestartGame()
    {
       

        //character.GetComponent<>

        winText.text = "";
        restartButton.gameObject.SetActive(false);
        //isCatTouched = false;
        character.GetComponent<Rigidbody2D>().gravityScale = 1;
        character.GetComponent<Rigidbody2D>().isKinematic = true;
        while (character.transform.position.y > -3.184443f)
        {
            character.transform.Translate(Vector3.down * Time.deltaTime); // Fall to ground
            
        }
        character.GetComponent<Rigidbody2D>().isKinematic = false;
        character.GetComponent<Rigidbody2D>().velocity = Vector2.one;
        character.GetComponent<Character>().endGame = false;
    }
}

//y = -3.184443