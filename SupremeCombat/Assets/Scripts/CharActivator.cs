using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharActivator : MonoBehaviour
{
    public GameObject vcam;
    public CinemachineTargetGroup targetGroup;
    public GameObject bot, goat;
    GameObject player1, player2;
    Vector3 p1Origin = new Vector3(-5, 1, 0);
    Vector3 p2Origin = new Vector3(5, 1, 0);

    private void Start() {
        //Don't instantiate players in test mode
        if (SceneVariables.player1Character != "") {
            InstantiateCharacters();
            SetCamera();
        }
        SceneVariables.matchInProgress = true;
    }
    void InstantiateCharacters() {
        SceneVariables.currentPlayer = 1;
        if (SceneVariables.player1Character == "bot") {
            player1 = Instantiate(bot, p1Origin, Quaternion.identity);
        } else if (SceneVariables.player1Character == "goat") {
            player1 = Instantiate(goat, p1Origin, Quaternion.identity);
        }
        SceneVariables.currentPlayer = 2;
        if (SceneVariables.player2Character == "bot") {
            player2 = Instantiate(bot, p2Origin, Quaternion.identity);
        } else if (SceneVariables.player2Character == "goat") {
            player2 = Instantiate(goat, p2Origin, Quaternion.identity);
        }
    }
    void SetCamera() {
        if (player1) {
            targetGroup.AddMember(player1.transform, 1f, 1f);
        }
        if (player2) {
            targetGroup.AddMember(player2.transform, 1f, 1f);
        }
        vcam.GetComponent<CinemachineVirtualCamera>().Follow = targetGroup.transform;
    }
}
