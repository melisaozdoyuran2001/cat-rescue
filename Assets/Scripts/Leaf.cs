using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{

    public CapsuleCollider2D capsuleCollider;
    public Rigidbody2D RB;
    public static float cooltime = .5f;
    public float cur_cooltime = cooltime;
    public int stage = 0;
    public bool PlayerOn = false;
    public GameObject character;
    private Rigidbody2D characterRigidbody;
    public bool left_side_drops;
    public SpriteRenderer sprite;
    private Color default_green = new Color32(102, 176, 105, 255);
    private Color player_green = new Color32(124, 173, 24, 255);
    private Color yelllow_ = new Color32(235, 207, 24, 255);
    private Color orange_ = new Color32(235, 122, 24, 255);
    private Color red_ = new Color32(168, 49, 24, 255);

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        RB = GetComponent<Rigidbody2D>();
        characterRigidbody = character.GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerOn)
        {
            cur_cooltime -= Time.deltaTime;
            if (cur_cooltime <= 0)
            {
                SetStage(stage + 1);
                if (stage != 3)
                {
                    cur_cooltime = cooltime;
                }
                else
                {
                    cur_cooltime = cooltime * 2;
                }

            }
        }
        else
        {
            cur_cooltime -= Time.deltaTime;
            if (cur_cooltime <= 0)
            {
                SetStage(stage - 1);
                cur_cooltime = cooltime;
            }
        }
    }

    void SetStage(int n)
    {
        if (n == 0)
        {
            stage = n;
            if (left_side_drops)
            {
                RB.rotation = 0;
            }
            else
            {
                RB.rotation = 0;
            }
            if (PlayerOn)
            {
                sprite.color = player_green;
            }
            else
            {
                sprite.color = default_green;
            }
        }
        else if (n == 1)
        {
            stage = n;
            if (left_side_drops)
            {
                RB.rotation = 10;
            }
            else
            {
                RB.rotation = -10;
            }
            sprite.color = yelllow_;
        }
        else if (n == 2)
        {
            stage = n;
            if (left_side_drops)
            {
                RB.rotation = 30;
            }
            else
            {
                RB.rotation = -30;
            }
            capsuleCollider.enabled = true;
            sprite.color = orange_;
        }
        else if (n == 3)
        {
            stage = n;
            if (left_side_drops)
            {
                RB.rotation = 50;
            }
            else
            {
                RB.rotation = -50;
            }
            capsuleCollider.enabled = false;
            sprite.color = red_;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Character")
        {
            PlayerOn = true;
            if (stage == 0)
            {
                sprite.color = player_green;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Character")
        {
            PlayerOn = false;
            cur_cooltime = cooltime;
            character.transform.SetParent(null);
            characterRigidbody.gravityScale = 1;
            if (stage == 0)
            {
                sprite.color = default_green;
            }
        }
    }

}
