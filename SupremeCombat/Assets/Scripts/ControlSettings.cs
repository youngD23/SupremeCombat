using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSettings : MonoBehaviour
{
    public GameObject P1ControlSelections;
    public GameObject P2ControlSelections;
    public GameObject p1KeyboardOpt, p2KeyboardOpt;
    public GameObject keyboard1, keyboard2;
    public GameObject gamepad1, gamepad2;
    public GameObject cpu1, cpu2;
    //public GameObject bot;

    private void Start() {
        if (SceneVariables.player1Controls.Equals("")) {
            keyboard1.SetActive(false);
            gamepad1.SetActive(false);
            cpu1.SetActive(false);
        } else if (SceneVariables.player1Controls.Equals("keyboard")) {
            keyboard1.SetActive(true);
        } else if (SceneVariables.player1Controls.Equals("gamepad")) {
            gamepad1.SetActive(true);
        } else if (SceneVariables.player1Controls.Equals("cpu")) {
            cpu1.SetActive(true);
        }


        if (SceneVariables.player2Controls.Equals("")) {
            keyboard2.SetActive(false);
            gamepad2.SetActive(false);
            cpu2.SetActive(false);
            //bot.SetActive(false);
        } else if (SceneVariables.player2Controls.Equals("keyboard")) {
            keyboard2.SetActive(true);
        } else if (SceneVariables.player2Controls.Equals("gamepad")) {
            gamepad2.SetActive(true);
        } else if (SceneVariables.player2Controls.Equals("cpu")) {
            cpu2.SetActive(true);
        }
    }
    private void Update() {
        if (SceneVariables.selectP1Controls == true) {
            P1ControlSelections.SetActive(true);
        } else {
            P1ControlSelections.SetActive(false);
        }
        if (SceneVariables.selectP2Controls == true) {
            P2ControlSelections.SetActive(true);
        } else {
            P2ControlSelections.SetActive(false);
        }
        //Only allow one player to use keyboard
        if (SceneVariables.player1Controls.Equals("keyboard")) {
            p2KeyboardOpt.SetActive(false);
        } else {
            p2KeyboardOpt.SetActive(true);
        }
        if (SceneVariables.player2Controls.Equals("keyboard")) {
            p1KeyboardOpt.SetActive(false);
        } else {
            p1KeyboardOpt.SetActive(true);
        }
    }
    public void P1Keyboard() {
        SceneVariables.player1Controls = "keyboard";
        SceneVariables.selectP1Controls = false;
        keyboard1.SetActive(true);
    }
    public void P1Gamepad() {
        SceneVariables.player1Controls = "gamepad";
        SceneVariables.selectP1Controls = false;
        gamepad1.SetActive(true);
    }
    public void P1CPU() {
        SceneVariables.player1Controls = "cpu";
        SceneVariables.selectP1Controls = false;
        cpu1.SetActive(true);
    }
    public void P2Keyboard() {
        SceneVariables.player2Controls = "keyboard";
        SceneVariables.selectP2Controls = false;
        keyboard2.SetActive(true);
    }
    public void P2Gamepad() {
        SceneVariables.player2Controls = "gamepad";
        SceneVariables.selectP2Controls = false;
        gamepad2.SetActive(true);
    }
    public void P2CPU() {
        SceneVariables.player2Controls = "cpu";
        SceneVariables.selectP2Controls = false;
        cpu2.SetActive(true);
    }
    public void ChangeP1Controls() {
        SceneVariables.selectP1Controls = true;
        SceneVariables.player1Controls = "";
        keyboard1.SetActive(false);
        gamepad1.SetActive(false);
        cpu1.SetActive(false);
    }
    public void ChangeP2Controls() {
        SceneVariables.selectP2Controls = true;
        SceneVariables.player2Controls = "";
        keyboard2.SetActive(false);
        gamepad2.SetActive(false);
        cpu2.SetActive(false);
    }

}