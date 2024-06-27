using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    internal Player player;

    bool canStringTilt = true; //Prevents tilt combos from continuing indefinitely

    private void Awake() {
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
    private void Start() {
        StartBaseControls();
    }

    void StartBaseControls() {
        if (player.controlSetting == "keyboard") {
            player.playerControls.Keyboard.Light.performed += ctx => LightCheck();
            player.playerControls.Keyboard.Heavy.performed += ctx => HeavyCheck();
        }
    }
    internal virtual void LightCheck() {
        if (player.IsAbleTo("attack")) {
            if (!player.isGrounded) {
                StartCoroutine(LightAttackAir());
            } else {
                StartCoroutine(LightAttackGround());
            }
        }
    }
    internal virtual void HeavyCheck() {
        if (player.IsAbleTo("attack")) {
            if (!player.isGrounded) {
                StartCoroutine(HeavyAttackAir());
            } else {
                StartCoroutine(HeavyAttackGround());
            }
        }
    }
    internal virtual void HitCheck(string frontRegion, string backRegion) {
        Vector3 attackPoint = transform.position + new Vector3(0.6f * player.faceDirection, 1.5f, 0);
        Collider[] colliders = Physics.OverlapSphere(attackPoint, player.attackRadius, player.charLayer);
        foreach (Collider enemy in colliders) {
            if (player.IsEnemy(enemy)) {
                //Scoot enemy back if too close
                if (Mathf.Abs(transform.position.x - enemy.transform.position.x) < 0.7) {
                    enemy.transform.position += transform.forward * 0.2f;
                }
                if (enemy.GetComponent<Player>().faceDirection == player.faceDirection) {
                    //Back hit
                    enemy.GetComponent<Damage>().TakeDamage(player.faceDirection, backRegion);
                } else {
                    enemy.GetComponent<Damage>().TakeDamage(player.faceDirection, frontRegion);
                }
            }
        }
    }
    internal virtual void LaunchCheck(string trajectory) {
        Vector3 attackPoint;
        Collider[] colliders;
        if (trajectory == "up") {
            attackPoint = transform.position + new Vector3(0.5f * player.faceDirection, 1.75f, 0);
            colliders = Physics.OverlapSphere(attackPoint, player.attackRadius, player.charLayer);
            foreach (Collider enemy in colliders) {
                if (player.IsEnemy(enemy)) {
                    if (Mathf.Abs(transform.position.x - enemy.transform.position.x) < 0.8) {
                        enemy.transform.position += transform.forward * 0.3f;
                    }
                    enemy.GetComponent<Damage>().Launch(player.faceDirection, trajectory);
                }
            }
        } else {
            attackPoint = transform.position + new Vector3(1f * player.faceDirection, 1.5f, 0);
            colliders = Physics.OverlapSphere(attackPoint, player.attackRadius, player.charLayer);
            foreach (Collider enemy in colliders) {
                if (player.IsEnemy(enemy)) {
                    //Scoot enemy back if too close
                    if (Mathf.Abs(transform.position.x - enemy.transform.position.x) < 0.8) {
                        enemy.transform.position += transform.forward * 0.3f;
                    }
                    enemy.GetComponent<Damage>().Launch(player.faceDirection, trajectory);
                }
            }
        }
    }
    internal virtual void SweepCheck() {
        Vector3 attackPoint = transform.position + new Vector3(1f * player.faceDirection, 1.5f, 0);
        Collider[] colliders = Physics.OverlapSphere(attackPoint, player.attackRadius, player.charLayer);
        foreach (Collider enemy in colliders) {
            if (player.IsEnemy(enemy)) {
                //Scoot enemy back if too close
                if (Mathf.Abs(transform.position.x - enemy.transform.position.x) < 0.9) {
                    enemy.transform.position += transform.forward * 0.3f;
                }
                enemy.GetComponent<Damage>().Sweep(player.faceDirection);
            }
        }
    }
    internal virtual IEnumerator LightUpTiltGround() {
        player.state = Player.States.RightElbow;
        yield return new WaitForSeconds(0.2f);
        HitCheck("head1", "body1");
        yield return new WaitForSeconds(0.2f);
        if (player.downValue > 0 && canStringTilt) {
            canStringTilt = false;
            player.transitionSpeed = 0.05f;
            StartCoroutine(LightDownTiltGround());
            yield break;
        } else {
            canStringTilt = true;
        }
        if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator LightUpTiltAir() {
        player.state = Player.States.RightElbow;
        yield return new WaitForSeconds(0.2f);
        HitCheck("head1", "body1");
        yield return new WaitForSeconds(0.2f);
        if (player.downValue > 0 && canStringTilt) {
            canStringTilt = false;
            player.transitionSpeed = 0.05f;
            StartCoroutine(LightDownTiltGround());
            yield break;
        } else {
            canStringTilt = true;
        }
        if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator LightDownTiltGround() {
        player.state = Player.States.Knee;
        yield return new WaitForSeconds(0.2f);
        HitCheck("body1", "head1");
        yield return new WaitForSeconds(0.2f);
        if (player.upValue > 0 && canStringTilt) {
            canStringTilt = false;
            player.transitionSpeed = 0.05f;
            StartCoroutine(LightUpTiltGround());
            yield break;
        } else {
            canStringTilt = true;
        }
        if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator LightDownTiltAir() {
        player.state = Player.States.Knee;
        yield return new WaitForSeconds(0.2f);
        HitCheck("body1", "head1");
        yield return new WaitForSeconds(0.2f);
        if (player.upValue > 0 && canStringTilt) {
            canStringTilt = false;
            player.transitionSpeed = 0.05f;
            StartCoroutine(LightUpTiltGround());
            yield break;
        } else {
            canStringTilt = true;
        }
        if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator LightAttackGround() {
        yield return new WaitForSeconds(0.05f); //Tilt buffer
        player.transitionSpeed = 0.02f;
        if (player.upValue > 0) {
            StartCoroutine(LightUpTiltGround());
            yield break;
        } else if (player.downValue > 0) {
            StartCoroutine(LightDownTiltGround());
            yield break;
        }
        player.state = Player.States.RightPunch;
        yield return new WaitForSeconds(0.2f);
        HitCheck("body", "head");
        yield return new WaitForSeconds(0.05f);
        if (player.lightValue > 0) {
            if (player.upValue > 0) {
                StartCoroutine(LightUpTiltGround());
                yield break;
            } else if (player.downValue > 0) {
                StartCoroutine(LightDownTiltGround());
                yield break;
            }
            player.state = Player.States.LeftJab;
            yield return new WaitForSeconds(0.2f);
            HitCheck("head", "body");
            yield return new WaitForSeconds(0.05f);
            if (player.lightValue > 0) {
                if (player.upValue > 0) {
                    StartCoroutine(LightUpTiltGround());
                    yield break;
                } else if (player.downValue > 0) {
                    StartCoroutine(LightDownTiltGround());
                    yield break;
                }
                player.state = Player.States.RightPunch;
                yield return new WaitForSeconds(0.2f);
                HitCheck("body", "head");
            }
        }
        yield return new WaitForSeconds(0.2f);
        if (player.upValue > 0) {
            StartCoroutine(LightUpTiltGround());
            yield break;
        } else if (player.downValue > 0) {
            StartCoroutine(LightDownTiltGround());
            yield break;
        }
        if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator LightAttackAir() {
        yield return new WaitForSeconds(0.05f); //Tilt buffer
        player.transitionSpeed = 0.02f;
        if (player.upValue > 0) {
            StartCoroutine(LightUpTiltAir());
            yield break;
        } else if (player.downValue > 0) {
            StartCoroutine(LightDownTiltAir());
            yield break;
        }
        player.state = Player.States.AirPunch;
        yield return new WaitForSeconds(0.2f);
        HitCheck("body", "head");
        yield return new WaitForSeconds(0.1f);
        if (player.lightValue > 0) {
            if (player.upValue > 0) {
                StartCoroutine(LightUpTiltAir());
                yield break;
            } else if (player.downValue > 0) {
                StartCoroutine(LightDownTiltAir());
                yield break;
            }
        }
        yield return new WaitForSeconds(0.1f);
        if (!player.isGrounded) {
            player.state = Player.States.Falling;
        } else if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator HeavyAttackGround() {
        yield return new WaitForSeconds(0.05f); //tilt buffer
        player.transitionSpeed = 0.02f;
        if (player.upValue > 0) {
            player.state = Player.States.Uppercut;
            yield return new WaitForSeconds(0.3f);
            LaunchCheck("up");
        } else if (player.downValue > 0) {
            player.state = Player.States.LegSweep;
            yield return new WaitForSeconds(0.4f);
            SweepCheck();
        } else {
            player.state = Player.States.FaceKick;
            yield return new WaitForSeconds(0.35f);
            LaunchCheck("forward");
        }
        yield return new WaitForSeconds(0.7f);
        if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator HeavyAttackAir() {
        yield return new WaitForSeconds(0.05f); //tilt buffer
        player.transitionSpeed = 0.02f;
        if (player.upValue > 0) {
            player.state = Player.States.Uppercut;
            yield return new WaitForSeconds(0.3f);
            LaunchCheck("up");
        } else if (player.downValue > 0) {
            player.state = Player.States.LegSweep;
            yield return new WaitForSeconds(0.4f);
            SweepCheck();
        } else {
            player.state = Player.States.AirDropKick;
            yield return new WaitForSeconds(0.2f);
            LaunchCheck("forward");
        }
        yield return new WaitForSeconds(0.3f);
        if (!player.isGrounded) {
            player.state = Player.States.Falling;
        } else if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    void OnDisruptActions() {
        StopAllCoroutines();
    }
    /*private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector2 attackPoint = transform.position + new Vector3(0.5f * player.faceDirection, 1.75f, 0);
        Gizmos.DrawWireSphere(attackPoint, player.attackRadius);
    }*/
}
