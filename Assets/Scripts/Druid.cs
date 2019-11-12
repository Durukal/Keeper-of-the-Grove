using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druid : MonoBehaviour
{
    private Rigidbody2D myBody;
    private Animator anim;
    float skillWaitTime = 1f;
    float walkWaitTime = 0.44f;
    bool skillUsed = false;
    bool isWalking = false;
    public AudioClip MoveDruid, SkillSound1, SkillSound2, SkillSound3;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (skillUsed)
            skillWaitTime -= Time.deltaTime;

        if (isWalking)
            walkWaitTime -= Time.deltaTime;

        if (skillWaitTime <= 0)
            anim.SetBool("Skill Usage", false);

        if (walkWaitTime <= 0)
        {
            isWalking = false;
            walkWaitTime = 0.44f;
        }

        if (skillUsed)
            anim.enabled = true;
        else if (myBody.velocity == Vector2.zero)
            anim.enabled = false;
        else
            anim.enabled = true;

        Movement.SetAnimations(myBody, anim);

        PlayerWalkKeyboard();
        PlayerSkillInput();

        if (myBody.velocity.x != 0 || myBody.velocity.y != 0)
        {
            if (!isWalking)
            {
                AudioSource.PlayClipAtPoint(MoveDruid, Vector3.zero);
            }
            isWalking = true;
        }

    }

    void SkillDefault()
    {
        skillUsed = true;
        skillWaitTime = 0.5f;
        walkWaitTime = 0.44f;
    }

    void PlayerWalkKeyboard()
    {
        Movement.Move(myBody, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void PlayerSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetBool("Skill Usage", true);
            Bury();
            if (!skillUsed)
                AudioSource.PlayClipAtPoint(SkillSound1, Vector3.zero);
            SkillDefault();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetBool("Skill Usage", true);
            GrowTree();
            if (!skillUsed)
                AudioSource.PlayClipAtPoint(SkillSound2, Vector3.zero);
            SkillDefault();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetBool("Skill Usage", true);
            Fear();
            if (!skillUsed)
                AudioSource.PlayClipAtPoint(SkillSound3, Vector3.zero);
            SkillDefault();
        }
    }

    void GrowTree()
    {
        GameObject[] treeResults = GameObject.FindGameObjectsWithTag("Tree");

        foreach (var tree in treeResults)
        {
            if ((tree.GetComponent<GameTree>().isChopped || tree.GetComponent<GameTree>().isDead) && Vector2.Distance(tree.transform.position, transform.position) < 2f)
            {
                tree.GetComponent<SpriteRenderer>().sprite = tree.GetComponent<GameTree>().originalSprite;
                tree.GetComponent<GameTree>().isDead = false;
                tree.GetComponent<GameTree>().isChopped = false;
                tree.GetComponent<GameTree>().age = Random.Range(0, 5);
                tree.GetComponent<GameTree>().IncreaseHP();
                GameManager.woodBalance++;
            }
        }
    }

    void Fear()
    {
        GameObject[] hunterResults = GameObject.FindGameObjectsWithTag("Hunter");
        GameObject[] chopperResults = GameObject.FindGameObjectsWithTag("Chopper");

        foreach (var hunter in hunterResults)
        {
            if (Vector2.Distance(hunter.transform.position, transform.position) < 2f)
            {
                hunter.GetComponent<Human>().Flee();
                GameManager.meatBalance++;
            }
        }

        foreach (var chopper in chopperResults)
        {
            if (Vector2.Distance(chopper.transform.position, transform.position) < 2f)
            {
                chopper.GetComponent<Human>().Flee();
                GameManager.woodBalance++;
            }
        }
    }

    void Bury()
    {
        GameObject[] animalResults = GameObject.FindGameObjectsWithTag("Animal");

        foreach (var animal in animalResults)
        {
            if (animal.GetComponent<Animal>().isDead && Vector2.Distance(animal.transform.position, transform.position) < 2f)
            {
                GameManager.meatBalance++;
                animal.SetActive(false);
            }
        }
    }

}