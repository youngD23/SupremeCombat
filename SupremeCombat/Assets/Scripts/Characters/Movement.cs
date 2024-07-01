using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Movement : MonoBehaviour
{
    internal Player player;
    
    Quaternion toRotation;
    float rotationSpeed = 1440f;

    int xVelocityHash_p;

    private void Awake() {
        player = GetComponent<Player>();
        xVelocityHash_p = Animator.StringToHash("xVelocity");
        player.state = Player.States.Idle;
    }
    private void OnEnable() {
        if (player != null) {
            player.Disruption += OnDisruptActions;
        }
    }
    private void OnDisable() {
        if (player != null) {
            player.Disruption -= OnDisruptActions;
        }
    }
    private void Start() {
        StartBaseControls();
    }
    private void Update()
    {
        if (Pause.paused) { return; }

        Fall();
        Gaurd();
    }
    private void FixedUpdate() {
        if (player.IsAbleTo("move")) {
            HorizontalMovement();
        }
    }

    void StartBaseControls() {
        if (player.controlSetting == "keyboard") {
            player.playerControls.Keyboard.Up.performed += ctx => JumpCheck();
        } else if (player.controlSetting == "gamepad") {
            player.playerControls.Gamepad.Jump.performed += ctx => JumpCheck();
        }
    }
    internal virtual void HorizontalMovement() {
        player.rb.velocity = new Vector2(player.xMovement * player.speed, player.rb.velocity.y);
        player.anim.SetFloat(xVelocityHash_p, Mathf.Abs(player.xMovement));

        //Set Rotation
        if (player.xInput != 0) {
            toRotation = Quaternion.LookRotation(new Vector3(player.xMovement, 0, 0), Vector3.up);
            if (player.xMovement < 0) {
                player.faceDirection = -1;
            } else if (player.xMovement > 0) {
                player.faceDirection = 1;
            }
        }
        if (transform.rotation != toRotation) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    internal virtual void JumpCheck() {
        if (player.isGrounded && player.IsAbleTo("jump")) {
            StartCoroutine(Jump());
        }
    }
    internal virtual void Fall() {
        //Makes fall faster
        if (Mathf.Abs(player.rb.velocity.y) > 0) {
            player.rb.AddForce(0, -5, 0);
        }
        
        if (!player.isGrounded && player.IsAbleTo("fall")) {
            player.state = Player.States.Falling;
        }
        if (player.isGrounded && player.IsAbleTo("land")) {
            StartCoroutine(Land());
        }
    }
    internal virtual void Gaurd() {
        if (player.gaurdValue > 0 && player.IsAbleTo("gaurd")) {
            if (player.state == Player.States.Idle) {
                player.transitionSpeed = 0.01f;
            } else {
                player.transitionSpeed = 0.1f;
            }
            player.state = Player.States.Gaurd;
        } else if (player.gaurdValue == 0 && player.state == Player.States.Gaurd) {
            player.transitionSpeed = 0.1f;
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator Jump() {
        player.transitionSpeed = 0.01f;
        if (Mathf.Abs(player.rb.velocity.x) > 1) {
            player.state = Player.States.RunningJump;
        } else {
            player.state = Player.States.StandingJump;
        }
        yield return new WaitForSeconds(0.1f);
        if (player.isGrounded) {
            player.transitionSpeed = 0.5f;
            if (player.jumpValue > 0) {
                GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, player.highJumpForce);
            } else {
                GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, player.lowJumpForce);
            }
        }
    }
    internal virtual IEnumerator Land() {
        if (Mathf.Abs(player.rb.velocity.x) > 5) {
            player.state = Player.States.RunningLand;
        } else {
            player.state = Player.States.StandingLand;
        }
        yield return new WaitForSeconds(0.3f);
        if (Mathf.Abs(player.rb.velocity.x) > 5) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    void OnDisruptActions() {
        StopAllCoroutines();
    }
}
