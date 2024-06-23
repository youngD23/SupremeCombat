using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //Implement Scene Manager
    public static void GameScene() {
        SceneManager.LoadScene(1);
    }
    public static void CharacterScene() {
        SceneManager.LoadScene(0);
    }
}
