using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{

    public CapsuleCollider2D capsuleCollider;
    public Rigidbody2D RB;
    public float cooltime = 1.25f;
    public int stage = 0;
    public bool PlayerOn = false;
    public GameObject character;
    private Rigidbody2D characterRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        RB = GetComponent<Rigidbody2D>();
        characterRigidbody = character.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerOn)
        {
            cooltime -= Time.deltaTime;
            if (cooltime <= 0)
            {
                SetStage(stage + 1);
                cooltime = 1.25f;
            }
        }
        else
        {
            cooltime -= Time.deltaTime;
            if (cooltime <= 0)
            {
                SetStage(stage - 1);
                cooltime = 1.25f;
            }
        }
    }

    void SetStage(int n)
    {
        if(n == 0)
        {
            stage = n;
            RB.rotation = 0;
        }
        else if (n == 1)
        {
            stage = n;
            RB.rotation = 10;
        }
        else if (n == 2)
        {
            stage = n;
            RB.rotation = 30;
            capsuleCollider.enabled = true;
        }
        else if (n == 3)
        {
            stage = n;
            RB.rotation = 50;
            capsuleCollider.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Character")
        {
            PlayerOn = true;
            character.transform.SetParent(transform);
            characterRigidbody.gravityScale = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Character")
        {
            PlayerOn = false;
            cooltime = 1.25f;
            character.transform.SetParent(null);
            characterRigidbody.gravityScale = 1;
        }
    }

}
