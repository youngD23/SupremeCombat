using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] internal float speed;
    [SerializeField] internal float lowJumpForce, highJumpForce;

    public event Action Disruption;

    internal PlayerControls playerControls;
    internal Rigidbody rb;
    internal Animator anim;
    internal States state;
    internal LayerMask charLayer;
    LayerMask groundLayer;
    Vector2 footPosition;

    public GameObject pauseMenuUI;
    public static bool paused;

    internal string controlSetting;
    internal float attackRadius = 0.5f;
    internal float xMovement;
    internal float transitionSpeed = 0.3f;
    internal float acceleration;
    internal float lightValue, heavyValue, upValue, downValue, jumpValue, gaurdValue;
    internal float stunTimer;
    internal int xInput;
    internal int playerNum;
    internal int faceDirection;
    internal bool isGrounded;
    internal bool isStunned;
    internal enum States
    {
        //Movement States
        Idle,
        Running,
        StandingJump,
        RunningJump,
        StandingLand,
        RunningLand,
        Falling,
        FrontFlip,
        FrontFlipFall,
        //Attack States
        RightPunch,
        LeftJab,
        FaceKick,
        BodyKick,
        Uppercut,
        LegSweep,
        RightElbow,
        Knee,
        AirPunch,
        AirDropKick,
        AirDownwardsKick,
        AirBackFlipKick,
        //Damage States
        BodyShot,
        BodyShot1,
        HeadShot,
        HeadShot1,
        BackLaunch,
        FrontLaunch,
        BackGroundSlide,
        FrontGroundSlide,
        LayingFaceUp,
        LayingFaceDown,
        BackAirRecover,
        FrontAirRecover,
        BackGroundRecover,
        FrontGroundRecover,
        Gaurd,
        BodyBlock,
        BodyBlock1,
        HeadBlock,
        HeadBlock1,
        HeavyBlock, 
        SweepFall,
        SweepBlock
    }

    private void Awake() {
        groundLayer = LayerMask.GetMask("Ground");
        charLayer = LayerMask.GetMask("Character");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        playerNum = SceneVariables.currentPlayer;
        if (playerNum == 2) {
            controlSetting = SceneVariables.player2Controls;
            transform.rotation = Quaternion.LookRotation(new Vector3(-90, 0, 0));
            faceDirection = -1;
        } else {
            controlSetting = SceneVariables.player1Controls;
            transform.rotation = Quaternion.LookRotation(new Vector3(90, 0, 0));
            faceDirection = 1;
        }
        Physics.SyncTransforms();
    }
    private void OnEnable() {
        playerControls = new PlayerControls();
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    protected virtual void Update() {
        if (Pause.paused) { return; }

        isGrounded = Physics.CheckSphere(footPosition, 0.05f, groundLayer);
        footPosition = transform.position - transform.up;
        if (stunTimer > 0) {
            stunTimer -= 2 * Time.deltaTime;
            if (stunTimer <= 0) {
                isStunned = false;
                stunTimer = 0;
            }
        }
        GetInput();
    }
    //Make recursive
    /// <summary>
    /// Checks to see if player is able to perform a certain action depending on current state
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    internal bool IsAbleTo(string action) {
        if (Pause.paused) { return false; }
        if (isStunned) { return false; }
        //Damage
        if (action == "recover") {
            if (!Is("disoriented")) {
                return false;
            }
        }
        //Attacks
        if (action == "attack") {
            if (Is("jumping") || Is("landing") || !IsAbleTo("move")) {
                return false;
            }
        }
        //Movement
        if (action == "move") {
            if (Is("attacking") || Is("disoriented") || Is("recovering") || 
                Is("gaurding")) {
                return false;
            }
        }
        if (action == "gaurd") {
            if (!IsAbleTo("move") || Is("falling") || Is("recovering")) {
                return false;
            }
        }
        if (action == "jump") {
            if (!IsAbleTo("move")) {
                return false;
            }
        }
        if (action == "fall") {
            if (Is("disoriented") || Is("recovering") || Is("falling") || Is("attacking")) {
                return false;
            }
        }
        if (action == "land") {
            if (!Is("falling")) {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Checks what condition the player is currently in; Used to determine what player is able to do
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    internal bool Is(string condition) {
        if (condition == "disoriented") {
            if (state == States.BackLaunch || state == States.BackGroundSlide ||
                state == States.FrontLaunch || state == States.FrontGroundSlide ||
                state == States.LayingFaceUp || state == States.LayingFaceDown || 
                state == States.BodyShot || state == States.BodyShot1 ||
                state == States.HeadShot || state == States.HeadShot1 ||
                state == States.SweepFall) {
                return true;
            }
        }
        if (condition == "recovering") {
            if (state == States.BackGroundRecover || state == States.FrontGroundRecover || 
                state == States.BackAirRecover || state == States.FrontAirRecover) {
                return true;
            }
        }
        if (condition == "attacking") {
            if (state == States.RightPunch || state == States.LeftJab ||
                state == States.FaceKick || state == States.LegSweep ||
                state == States.Knee || state == States.RightElbow ||
                state == States.AirPunch || state == States.AirDropKick ||
                state == States.AirDownwardsKick || state == States.AirBackFlipKick ||
                heavyValue > 0 || lightValue > 0) {
                return true;
            }
        }
        if (condition == "gaurding") {
            if (state == States.Gaurd || state == States.BodyBlock ||
                state == States.HeadBlock || state == States.HeavyBlock ||
                state == States.BodyBlock1 || state == States.HeadBlock1 ||
                state == States.SweepBlock) {
                return true;
            }
        }
        if (condition == "falling") {
            if (state == States.Falling || state == States.FrontFlipFall) {
                return true;
            }
        }
        if (condition == "jumping") {
            if (state == States.StandingJump || state == States.RunningJump) {
                return true;
            }
        }
        if (condition == "landing") {
            if (state == States.StandingLand || state == States.RunningLand) {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Returns true if the collider detected belongs to an enemy
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    internal bool IsEnemy(Collider obj) {
        if (obj.GetComponent<Player>().playerNum == playerNum) { return false; }
        return true;
    }
    void GetInput() {
        if (controlSetting == "keyboard") {
            lightValue = playerControls.Keyboard.Light.ReadValue<float>();
            heavyValue = playerControls.Keyboard.Heavy.ReadValue<float>();
            upValue = playerControls.Keyboard.Up.ReadValue<float>();
            jumpValue = playerControls.Keyboard.Up.ReadValue<float>();
            downValue = playerControls.Keyboard.Down.ReadValue<float>();
            gaurdValue = playerControls.Keyboard.Gaurd.ReadValue<float>();
            GetXInput();
        } else if (controlSetting == "gamepad") {
            lightValue = playerControls.Gamepad.Light.ReadValue<float>();
            heavyValue = playerControls.Gamepad.Heavy.ReadValue<float>();
            upValue = playerControls.Gamepad.Up.ReadValue<float>();
            jumpValue = playerControls.Gamepad.Jump.ReadValue<float>();
            downValue = playerControls.Gamepad.Down.ReadValue<float>();
            gaurdValue = playerControls.Gamepad.Gaurd.ReadValue<float>();
            GetXInput();
        }
    }
    /// <summary>
    /// Changes xMovement with a smooth acceleration
    /// </summary>
    void GetXInput() {
        xInput = (int)Input.GetAxisRaw("Horizontal");

        if (isGrounded) {
            acceleration = 3;
        } else {
            acceleration = 1;
        }

        if (xInput > 0) {
            //accelerate right
            if (xMovement < xInput) {
                if (xMovement < 0) {
                    //quick stop
                    xMovement += acceleration * 2 * Time.deltaTime;
                } else {
                    xMovement += acceleration * Time.deltaTime;
                }
                if (xMovement > xInput) {
                    xMovement = xInput;
                }
            }
        } else if (xInput < 0) {
            //accelerate left
            if (xMovement > xInput) {
                if (xMovement > 0) {
                    //quick stop
                    xMovement -= acceleration * 2 * Time.deltaTime;
                } else {
                    xMovement -= acceleration * Time.deltaTime;
                }
                if (xMovement < xInput) {
                    xMovement = xInput;
                }
            }
        } else {
            //decelerate
            if (xMovement > 0) {
                xMovement -= acceleration * Time.deltaTime;
                if (xMovement < 0) {
                    xMovement = 0;
                }
            } else if (xMovement < 0) {
                xMovement += acceleration * Time.deltaTime;
                if (xMovement > 0) {
                    xMovement = 0;
                }
            }
        }
    }
    /// <summary>
    /// Stops all coroutines in other scripts
    /// </summary>
    internal void DisruptActions() {
        Disruption?.Invoke();
    }
}
