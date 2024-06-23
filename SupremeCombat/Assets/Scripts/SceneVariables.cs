using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneVariables : MonoBehaviour
{
    public static string player1Character = "";
    public static string player2Character = "";
    public static string player1Controls = "keyboard";
    public static string player2Controls = "cpu";
    public static int currentPlayer;
    public static bool matchInProgress = false;
    public static bool selectP1Controls;
    public static bool selectP2Controls;
}
