using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatMovement : Movement
{
    internal override void Fall() {
        if (!player.isGrounded && player.state == Player.States.FrontFlip) {
            player.state = Player.States.FrontFlipFall;
        }
        base.Fall();
    }
    internal override IEnumerator Jump() {
        player.transitionSpeed = 0.02f;
        if (Mathf.Abs(player.rb.velocity.x) > 1) {
            player.state = Player.States.RunningJump;
        } else {
            player.state = Player.States.StandingJump;
        }
        yield return new WaitForSeconds(0.1f);
        if (player.isGrounded) {
            player.transitionSpeed = 0.5f;
            if (player.upValue > 0) {
                player.transitionSpeed = 0.05f;
                player.state = Player.States.FrontFlip;
                GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, player.highJumpForce);
            } else {
                GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, player.lowJumpForce);
            }
        }
    }
}
