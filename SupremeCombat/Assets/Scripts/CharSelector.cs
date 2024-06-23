using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelector : MonoBehaviour
{
    public GameObject beginButton;
    public GameObject bot, goat;
    GameObject player1, player2;
    Vector3 p1Origin = new Vector3(-5, 1, 0);
    Vector3 p2Origin = new Vector3(5, 1, 0);

    private void Awake() {
        SceneVariables.matchInProgress = false;
        SceneVariables.currentPlayer = 1;
    }
    private void Update() {
        if (SceneVariables.currentPlayer > 2 && !beginButton.activeInHierarchy) {
            beginButton.SetActive(true);
        } else if (SceneVariables.currentPlayer <= 2 && beginButton.activeInHierarchy) {
            beginButton.SetActive(false);
        }
    }
    public void SelectBot() {
        if (SceneVariables.currentPlayer == 1) {
            SceneVariables.player1Character = "bot";
            player1 = Instantiate(bot, p1Origin, Quaternion.identity);
            SceneVariables.currentPlayer++;
        } else if (SceneVariables.currentPlayer == 2) {
            SceneVariables.player2Character = "bot";
            player2 = Instantiate(bot, p2Origin, Quaternion.identity);
            SceneVariables.currentPlayer++;
        }
    }
    public void SelectGoat() {
        if (SceneVariables.currentPlayer == 1) {
            SceneVariables.player1Character = "goat";
            player1 = Instantiate(goat, p1Origin, Quaternion.identity);
            SceneVariables.currentPlayer++;
        } else if (SceneVariables.currentPlayer == 2) {
            SceneVariables.player2Character = "goat";
            player2 = Instantiate(goat, p2Origin, Quaternion.identity);
            SceneVariables.currentPlayer++;
        }
    }
    public void Back() {
        if (SceneVariables.currentPlayer == 3) {
            Destroy(player2);
            SceneVariables.player2Character = "";
            SceneVariables.currentPlayer = 2;
        } else if (SceneVariables.currentPlayer == 2){
                Destroy(player1);
                SceneVariables.player1Character = "";
                SceneVariables.currentPlayer = 1;
        }
    }
}
