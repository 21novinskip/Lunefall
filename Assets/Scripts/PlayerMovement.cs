using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    
    private bool settingUp = true;
    
    public GameObject gamemanager;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    public PLAYERSTATES PlayerState;
    private PLAYERSTATES LastState;
    public enum PLAYERSTATES
    {
        FREE,
        DIALOGUE,
        PAUSED,
    }

    public ROLES MyRole;
    public enum ROLES
    {
        WITCH,
        FIGHTER,
        TANK,
    }

    public GameObject Witch;
    public GameObject Fighter;
    public GameObject Tank;
    public TRAINPOSITIONS TrainPosition;
    private TRAINPOSITIONS WitchPosition;
    private TRAINPOSITIONS FighterPosition;
    private TRAINPOSITIONS TankPosition;
    private TRAINPOSITIONS OG_Position;
    private float Following_Distance = 1.5f;
    public enum TRAINPOSITIONS
    {
        FIRST,
        SECOND,
        THIRD,
    }
    private GameObject First_Player;
    private GameObject Second_Player;
    private GameObject Third_Player;

    public Camera MyCamera;
    private Camera WitchCamera;
    private Camera FighterCamera;
    private Camera TankCamera;

    private bool I_am_walking = false;
    private bool walkSoundPlaying = false;
    private AudioSource snd_walk_sound;

    private float pullDistance = 0.5f; // Max distance to attach for pulling
    private string boxTag = "Box"; // Tag for pullable boxes
    private KeyCode pullKey = KeyCode.Space;

    private Rigidbody2D playerRb;
    private FixedJoint2D joint;
    private Collider2D currentBox;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        snd_walk_sound = GetComponent<AudioSource>();
        StartCoroutine(FindPositions());

        joint = gameObject.AddComponent<FixedJoint2D>();
        joint.enabled = false; // Initially disabled
    }

    IEnumerator FindPositions()
    {
        yield return new WaitForSeconds(0.1f);
        gamemanager = GameObject.FindWithTag("GameController");
        Witch = GameObject.FindWithTag("witchOW");
        Fighter = GameObject.FindWithTag("fighterOW");
        Tank = GameObject.FindWithTag("tankOW");

        if (WitchCamera == null){
            WitchCamera = Witch.GetComponentInChildren<Camera>();
        }
        if (TankCamera == null){
            TankCamera = Tank.GetComponentInChildren<Camera>();
        }
        if (FighterCamera == null){
            FighterCamera = Fighter.GetComponentInChildren<Camera>();
        }

        //This code is so unbelievably braindead but if it aint broke...
        WitchPosition = Witch.GetComponent<PlayerMovement>().TrainPosition;
        TankPosition = Tank.GetComponent<PlayerMovement>().TrainPosition;
        FighterPosition = Fighter.GetComponent<PlayerMovement>().TrainPosition;
        //find Witch position
        if (WitchPosition == TRAINPOSITIONS.FIRST){
            First_Player = Witch;
        }
        else if (WitchPosition == TRAINPOSITIONS.SECOND){
            Second_Player = Witch;
        }
        else if (WitchPosition == TRAINPOSITIONS.THIRD){
            Third_Player = Witch;
        }
        //find Tank position
        if (TankPosition == TRAINPOSITIONS.FIRST){
            First_Player = Tank;
        }
        else if (TankPosition == TRAINPOSITIONS.SECOND){
            Second_Player = Tank;
        }
        else if (TankPosition == TRAINPOSITIONS.THIRD){
            Third_Player = Tank;
        }
        //find Fighter position
        if (FighterPosition == TRAINPOSITIONS.FIRST){
            First_Player = Fighter;
        }
        else if (FighterPosition == TRAINPOSITIONS.SECOND){
            Second_Player = Fighter;
        }
        else if (FighterPosition == TRAINPOSITIONS.THIRD){
            Third_Player = Fighter;
        }
        //Debug.Log("First is " + First_Player.name);
        //Debug.Log("Second is: " + Second_Player.name);
        //Debug.Log("Third is: " + Third_Player.name);

        
        if (TrainPosition == TRAINPOSITIONS.FIRST)
        {
            WitchCamera.enabled = false;
            TankCamera.enabled = false;
            FighterCamera.enabled = false;
            MyCamera.enabled = true;
            AudioListener[] listeners = FindObjectsOfType<AudioListener>();
            foreach (AudioListener listener in listeners)
            {
                listener.enabled = false;
            }
            MyCamera.GetComponent<AudioListener>().enabled = true;
        }

        settingUp = false;
        yield return null;
    }


    private void OnMovement(InputValue value)
    {
        if (PlayerState == PLAYERSTATES.FREE)
        {
            if (TrainPosition == TRAINPOSITIONS.FIRST)
            {
                movement = value.Get<Vector2>();
            }
            if (movement.x != 0 || movement.y != 0)
            {
                animator.SetFloat("X", movement.x);
                animator.SetFloat("Y", movement.y);

                animator.SetBool("isWalking", true);
                I_am_walking = true;
            }
            else
            {
                animator.SetBool("isWalking", false);
                I_am_walking = false;
            }
        }
        else
        {
            movement = Vector2.zero;
            animator.SetBool("isWalking", false);
            I_am_walking = false;
        }
    }

    private void FixedUpdate()
    {
        if (settingUp == true)
        {

        }
        else
        {
            if (PlayerState == PLAYERSTATES.FREE)
            {
                if (TrainPosition == TRAINPOSITIONS.SECOND)
                {
                    float distance = Vector2.Distance(First_Player.transform.position, transform.position);

                    if (distance >= Following_Distance)
                    {
                        movement = (First_Player.transform.position - transform.position).normalized;
                        I_am_walking = true;
                    }
                    else
                    {
                        movement = Vector2.zero;
                        I_am_walking = false;
                    }
                }
                else if (TrainPosition == TRAINPOSITIONS.THIRD)
                {
                    float distance = Vector2.Distance(Second_Player.transform.position, transform.position);

                    if (distance >= Following_Distance)
                    {
                        movement = (Second_Player.transform.position - transform.position).normalized;
                        I_am_walking = true;

                    }
                    else
                    {
                        movement = Vector2.zero;
                        I_am_walking = false;
                    }
                }
                if (movement.x != 0 || movement.y != 0)
                {
                    animator.SetFloat("X", movement.x);
                    animator.SetFloat("Y", movement.y);

                    animator.SetBool("isWalking", true);
                }
                else
                {
                    animator.SetBool("isWalking", false);
                }
                rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            }
            else
            {
                //uhhhhh
            }
        }
    }
    
    private void Update()
    {
        if (settingUp == true)
        {

        }
        else
        {
            if (gamemanager.GetComponent<GameManager>().GameState == GameManager.GAMESTATES.PAUSE & PlayerState != PLAYERSTATES.PAUSED)
            {
                LastState = PlayerState;
                PlayerState = PLAYERSTATES.PAUSED;
            }
            else if (gamemanager.GetComponent<GameManager>().GameState != GameManager.GAMESTATES.PAUSE & PlayerState == PLAYERSTATES.PAUSED)
            {
                PlayerState = LastState;
            }

            if (I_am_walking && !walkSoundPlaying)
            {
                snd_walk_sound.loop = true;
                snd_walk_sound.Play();
                walkSoundPlaying = true;
            }
            if (!I_am_walking && walkSoundPlaying)
            {
                snd_walk_sound.Stop();
                walkSoundPlaying = false;
            }
    /*
            if (TrainPosition != OG_Position)
            {
                FindPositions();
                OG_Position = TrainPosition;
            }
    */

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Witch.GetComponent<PlayerMovement>().TrainPosition = TRAINPOSITIONS.FIRST;
                Tank.GetComponent<PlayerMovement>().TrainPosition = TRAINPOSITIONS.SECOND;
                Fighter.GetComponent<PlayerMovement>().TrainPosition = TRAINPOSITIONS.THIRD;
                StartCoroutine(FindPositions());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Tank.GetComponent<PlayerMovement>().TrainPosition = TRAINPOSITIONS.FIRST;
                Fighter.GetComponent<PlayerMovement>().TrainPosition = TRAINPOSITIONS.SECOND;
                Witch.GetComponent<PlayerMovement>().TrainPosition = TRAINPOSITIONS.THIRD;
                StartCoroutine(FindPositions());
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Fighter.GetComponent<PlayerMovement>().TrainPosition = TRAINPOSITIONS.FIRST;
                Witch.GetComponent<PlayerMovement>().TrainPosition = TRAINPOSITIONS.SECOND;
                Tank.GetComponent<PlayerMovement>().TrainPosition = TRAINPOSITIONS.THIRD;
                StartCoroutine(FindPositions());
            }
            if ((TrainPosition == TRAINPOSITIONS.FIRST) && (PlayerState == PLAYERSTATES.FREE))
            {
                // Check if the player is near a box
                if (Input.GetKeyDown(pullKey))
                {
                    Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, pullDistance);

                    foreach (Collider2D collider in nearbyColliders)
                    {
                        if (collider.CompareTag(boxTag))
                        {
                            currentBox = collider;
                            AttachToBox();
                            break;
                        }
                    }
                }

                // Detach when the key is released
                if (Input.GetKeyUp(pullKey) && joint.enabled)
                {
                    DetachFromBox();
                }
            }
        }
    }

    void AttachToBox()
    {
        if (currentBox != null)
        {
            Rigidbody2D boxRb = currentBox.GetComponent<Rigidbody2D>();
            if (boxRb != null)
            {
                joint.connectedBody = boxRb;
                joint.enabled = true;
                Debug.Log("Box gwabbed");
            }
        }
    }

    void DetachFromBox()
    {
        joint.enabled = false;
        joint.connectedBody = null;
        currentBox = null;
        Debug.Log("Box wet go");
    }
}
