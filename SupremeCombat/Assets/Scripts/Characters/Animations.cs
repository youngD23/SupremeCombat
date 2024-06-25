using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    Player player;

    int currentState;

    private void Start() {
        player = GetComponent<Player>();
    }
    void Update()
    {
        ChangeAnimationState(GetAnimation());
    }
    int GetAnimation() {
        /** Damage **/
        if (player.state == Player.States.SweepFall) {
            return Animator.StringToHash("SweepFall");
        }
        if (player.state == Player.States.SweepBlock) {
            return Animator.StringToHash("SweepBlock");
        }
        if (player.state == Player.States.BodyShot) {
            return Animator.StringToHash("BodyShot");
        }
        if (player.state == Player.States.BodyShot1) {
            return Animator.StringToHash("BodyShot1");
        }
        if (player.state == Player.States.HeadShot) {
            return Animator.StringToHash("HeavyHeadShot");
        }
        if (player.state == Player.States.HeadShot1) {
            return Animator.StringToHash("HeadShot1");
        }
        if (player.state == Player.States.BackLaunch) {
            return Animator.StringToHash("BackLaunch");
        }
        if (player.state == Player.States.FrontLaunch) {
            return Animator.StringToHash("FrontLaunch");
        }
        if (player.state == Player.States.BackGroundSlide) {
            return Animator.StringToHash("BackGroundSlide");
        }
        if (player.state == Player.States.FrontGroundSlide) {
            return Animator.StringToHash("FrontGroundSlide");
        }
        if (player.state == Player.States.LayingFaceUp) {
            return Animator.StringToHash("LayingFaceUp");
        }
        if (player.state == Player.States.LayingFaceDown) {
            return Animator.StringToHash("LayingFaceDown");
        }
        if (player.state == Player.States.BackAirRecover) {
            return Animator.StringToHash("BackAirRecover");
        }
        if (player.state == Player.States.FrontAirRecover) {
            return Animator.StringToHash("FrontAirRecover");
        }
        if (player.state == Player.States.BackGroundRecover) {
            return Animator.StringToHash("BackGroundRecover");
        }
        if (player.state == Player.States.FrontGroundRecover) {
            return Animator.StringToHash("FrontGroundRecover");
        }
        /** Attacks **/
        if (player.state == Player.States.Knee) {
            return Animator.StringToHash("Knee");
        }
        if (player.state == Player.States.RightElbow) {
            return Animator.StringToHash("RightElbow");
        }
        if (player.state == Player.States.LegSweep) {
            return Animator.StringToHash("LegSweep");
        }
        if (player.state == Player.States.RightPunch) {
            return Animator.StringToHash("RightPunch");
        }
        if (player.state == Player.States.LeftJab) {
            return Animator.StringToHash("LeftJab");
        }
        if (player.state == Player.States.FaceKick) {
            return Animator.StringToHash("FaceKick");
        }
        if (player.state == Player.States.BodyKick) {
            return Animator.StringToHash("BodyKick");
        }
        if (player.state == Player.States.Uppercut) {
            return Animator.StringToHash("Uppercut");
        }
        /** Movement **/
        if (player.state == Player.States.Gaurd) {
            return Animator.StringToHash("Gaurd");
        }
        if (player.state == Player.States.HeadBlock) {
            return Animator.StringToHash("HeadBlock");
        }
        if (player.state == Player.States.HeadBlock1) {
            return Animator.StringToHash("HeadBlock1");
        }
        if (player.state == Player.States.BodyBlock) {
            return Animator.StringToHash("BodyBlock");
        }
        if (player.state == Player.States.BodyBlock1) {
            return Animator.StringToHash("BodyBlock1");
        }
        if (player.state == Player.States.HeavyBlock) {
            return Animator.StringToHash("HeavyBlock");
        }
        if (player.state == Player.States.StandingJump) {
            return Animator.StringToHash("StandingJump");
        }
        if (player.state == Player.States.RunningJump) {
            return Animator.StringToHash("RunningJump");
        }
        if (player.state == Player.States.StandingLand) {
            return Animator.StringToHash("StandingLand");
        }
        if (player.state == Player.States.RunningLand) {
            return Animator.StringToHash("RunningLand");
        }
        if (player.state == Player.States.Falling) {
            return Animator.StringToHash("Falling");
        }
        if (player.state == Player.States.FrontFlip) {
            return Animator.StringToHash("FrontFlip");
        }
        if (player.state == Player.States.FrontFlipFall) {
            return Animator.StringToHash("FrontFlip");
        }
        if (player.state == Player.States.Running) {
            return Animator.StringToHash("Movement Blend Tree");
        }
        return Animator.StringToHash("Idle");
    }
    void ChangeAnimationState(int newState) {
        if (currentState == newState) return;
        player.anim.CrossFade(newState, player.transitionSpeed);
        currentState = newState;
        player.transitionSpeed = 0.3f;
    }
}
