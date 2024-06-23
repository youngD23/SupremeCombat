using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    internal Player player;

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
            StartCoroutine(LightAttack());
        }
    }
    internal virtual void HeavyCheck() {
        if (player.IsAbleTo("attack")) {
            StartCoroutine(HeavyAttack());
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
                if (Mathf.Abs(transform.position.x - enemy.transform.position.x) < 0.8) {
                    enemy.transform.position += transform.forward * 0.3f;
                }
                enemy.GetComponent<Damage>().Sweep(player.faceDirection);
            }
        }
    }
    internal virtual IEnumerator LightAttack() {
        player.transitionSpeed = 0.02f;
        player.state = Player.States.RightPunch;
        yield return new WaitForSeconds(0.2f);
        HitCheck("body", "head");
        yield return new WaitForSeconds(0.05f);
        if (player.lightValue > 0) {
            player.state = Player.States.LeftJab;
            yield return new WaitForSeconds(0.2f);
            HitCheck("head", "body");
            yield return new WaitForSeconds(0.05f);
            if (player.lightValue > 0) {
                player.state = Player.States.RightPunch;
                yield return new WaitForSeconds(0.2f);
                HitCheck("body", "head");
            }
        }
        yield return new WaitForSeconds(0.2f);
        if (Mathf.Abs(player.xInput) > 0) {
            player.state = Player.States.Running;
        } else {
            player.state = Player.States.Idle;
        }
    }
    internal virtual IEnumerator HeavyAttack() {
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
    void OnDisruptActions() {
        StopAllCoroutines();
    }
    /*private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector2 attackPoint = transform.position + new Vector3(0.5f * player.faceDirection, 1.75f, 0);
        Gizmos.DrawWireSphere(attackPoint, player.attackRadius);
    }*/
}
