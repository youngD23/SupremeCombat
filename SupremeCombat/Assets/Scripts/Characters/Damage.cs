using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    internal Player player;

    float groundSlideBuffer;

    internal virtual void Awake() {
        player = GetComponent<Player>();
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
    internal virtual void Start() {
        StartBaseControls();
    }
    internal virtual void Update() {
        if (Pause.paused) { return; }

        GroundSlideCheck();
    }

    void StartBaseControls() {
        if (player.controlSetting == "keyboard") {
            player.playerControls.Keyboard.Up.performed += ctx => RecoverCheck();
            player.playerControls.Keyboard.Light.performed += ctx => RecoverCheck();
            player.playerControls.Keyboard.Heavy.performed += ctx => RecoverCheck();
        }
    }
    /// <summary>
    /// Cancels all actions and plays damage animation if not blocking
    /// </summary>
    /// <param name="region"></param>
    internal virtual void TakeDamage(int direction, string region) {
        player.DisruptActions();
        player.transitionSpeed = 0.02f;
        if (player.Is("gaurding") && player.faceDirection != direction) { 
            if (region == "head") {
                StartCoroutine(HeadBlock());
            } else {
                StartCoroutine(BodyBlock());
            }
        } else {
            if (region == "head") {
                StartCoroutine(HeadShot());
            } else {
                StartCoroutine(BodyShot());
            }
        }
    }
    internal virtual void Launch(int direction, string trajectory) {
        player.DisruptActions();
        player.transitionSpeed = 0.05f;
        if (player.Is("gaurding") && player.faceDirection != direction) {
            StartCoroutine(HeavyBlock());
        } else {
            groundSlideBuffer = 0.03f;
            if (player.faceDirection == direction) {
                player.state = Player.States.FrontLaunch;
            } else {
                player.state = Player.States.BackLaunch;
            }
            player.isStunned = true;
            //Add function based on health for stun timer
            player.stunTimer = 1f;
            //Add function based on damage and health for launch force
            if (trajectory == "up") {
                player.rb.AddForce(new Vector3(direction * 600, 800, 0));
            } else {
                player.rb.AddForce(new Vector3(direction * 800, 600, 0));
            }
        }
    }
    internal virtual void Sweep(int direction) {
        player.DisruptActions();
        player.transitionSpeed = 0.02f;
        if (player.Is("gaurding") && player.faceDirection != direction) {
            StartCoroutine(SweepBlock());
        } else {
            StartCoroutine(SweepFall());
        }
    }
    /// <summary>
    /// Changes player from Launch state to GroundSlide and LayingFaceUp states
    /// </summary>
    internal virtual void GroundSlideCheck() {
        //Makes sure player doesn't switch to ground slide animation 
        //immediately after player is launched but still grounded
        if (groundSlideBuffer > 0) {
            groundSlideBuffer -= 0.1f * Time.deltaTime;
        }

        if (player.state == Player.States.BackLaunch && player.isGrounded && groundSlideBuffer <= 0) {
            //BackLaunch to GroundSlide
            player.transitionSpeed = 1f;
            player.state = Player.States.BackGroundSlide;
        } else if (player.state == Player.States.FrontLaunch && player.isGrounded && groundSlideBuffer <= 0) {
            //FrontLaunch to GroundSlide
            player.transitionSpeed = 1f;
            player.state = Player.States.FrontGroundSlide;
        }
        if (player.state == Player.States.BackGroundSlide && Mathf.Abs(player.rb.velocity.x) < 5) {
            //GroundSlide to FaceUp
            player.transitionSpeed = 1f;
            player.state = Player.States.LayingFaceUp;
        } else if (player.state == Player.States.FrontGroundSlide && Mathf.Abs(player.rb.velocity.x) < 5) {
            //GroundSlide to FaceDown
            player.transitionSpeed = 1f;
            player.state = Player.States.LayingFaceDown;
        }
        if (player.state == Player.States.LayingFaceUp && Mathf.Abs(player.rb.velocity.x) > 5) {
            //FaceUp to GroundSlide
            player.transitionSpeed = 1f;
            player.state = Player.States.BackGroundSlide;
        } else if (player.state == Player.States.LayingFaceDown && Mathf.Abs(player.rb.velocity.x) > 5) {
            //FaceDown to GroundSlide
            player.transitionSpeed = 1f;
            player.state = Player.States.FrontGroundSlide;
        }
        if (player.state == Player.States.LayingFaceUp || player.state == Player.States.BackGroundSlide) {
            if (!player.isGrounded) {
                //FaceUp or Sliding to BackLaunch
                player.state = Player.States.BackLaunch;
            }
        } else if (player.state == Player.States.LayingFaceDown || player.state == Player.States.FrontGroundSlide) {
            if (!player.isGrounded) {
                //FaceDown or Sliding to FrontLaunch
                player.state = Player.States.FrontLaunch;
            }
        }
    }
    internal virtual void RecoverCheck() {
        if (player.IsAbleTo("recover")) {
            StartCoroutine(Recover());
        }
    }
    internal virtual void Counter() {
        //Counter disrupt enemy combo and leave them open for a second
    }
    internal virtual IEnumerator BodyShot() {
        player.isStunned = true;
        if (player.state == Player.States.BodyShot) {
            player.state = Player.States.HeadShot;
        } else {
            player.state = Player.States.BodyShot;
        }
        yield return new WaitForSeconds(0.7f);
        player.isStunned = false;
        if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator HeadShot() {
        player.isStunned = true;
        player.state = Player.States.HeadShot;
        yield return new WaitForSeconds(0.7f);
        player.isStunned = false;
        player.state = Player.States.Idle;
    }
    internal virtual IEnumerator SweepFall() {
        player.isStunned = true;
        player.state = Player.States.SweepFall;
        yield return new WaitForSeconds(0.5f);
        player.stunTimer = 0.7f; //base on health
        player.transitionSpeed = 0.2f;
        player.state = Player.States.LayingFaceUp;
    }
    internal virtual IEnumerator HeadBlock() {
        player.isStunned = true;
        player.state = Player.States.HeadBlock;
        yield return new WaitForSeconds(0.5f);
        player.isStunned = false;
        player.state = Player.States.Gaurd;
    }
    internal virtual IEnumerator BodyBlock() {
        player.isStunned = true;
        player.state = Player.States.BodyBlock;
        yield return new WaitForSeconds(0.5f);
        player.isStunned = false;
        player.state = Player.States.Gaurd;
    }
    internal virtual IEnumerator HeavyBlock() {
        player.isStunned = true;
        player.state = Player.States.HeavyBlock;
        yield return new WaitForSeconds(0.5f);
        player.isStunned = false;
        player.state = Player.States.Gaurd;
    }
    internal virtual IEnumerator SweepBlock() {
        yield return new WaitForSeconds(0.5f);
    }
    internal virtual IEnumerator Recover() {
        float recoverTime = 0.3f;
        if (player.state == Player.States.BackLaunch) {
            player.transitionSpeed = 0.1f;
            player.state = Player.States.BackAirRecover;
            recoverTime = 0.3f;
        } else if (player.state == Player.States.BackGroundSlide || player.state == Player.States.LayingFaceUp){
            player.transitionSpeed = 5f;
            player.state = Player.States.BackGroundRecover;
            recoverTime = 0.9f;
        } else if (player.state == Player.States.FrontLaunch) {
            player.transitionSpeed = 0.1f;
            player.state = Player.States.FrontAirRecover;
            recoverTime = 0.3f;
        } else {
            player.transitionSpeed = 5f;
            player.state = Player.States.FrontGroundRecover;
            recoverTime = 0.7f;
        }
        yield return new WaitForSeconds(recoverTime);
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
