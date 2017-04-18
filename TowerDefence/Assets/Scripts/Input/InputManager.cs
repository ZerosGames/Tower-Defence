using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static bool isShiftPressed;
    public static bool isMouseButtonLeftPressed;
    public static bool isMouseButtonRightPressed;
    public static bool isBuildButtonPressed;

    void Update ()
    {
        if (GameManager.gameManager.gameState == GameManager.GameState.Playing)
        {
            isShiftPressed = (Input.GetAxis("Shift") == 1) ? true : false;
            isMouseButtonLeftPressed = (Input.GetAxis("MouseButton 0") == 1) ? true : false;
            isMouseButtonRightPressed = (Input.GetAxis("MouseButton 1") == 1) ? true : false;
            isBuildButtonPressed = (Input.GetAxis("BuildMode") == 1) ? true : false;
        }
    }

    public static bool GetShiftButtonPressed()
    {
        return isShiftPressed;
    }

    public static bool GetMouseButtonLeftPressed()
    {
        return isMouseButtonLeftPressed;
    }

    public static bool GetMouseButtonRightPressed()
    {
        return isMouseButtonRightPressed;
    }

    public static bool GetBuildModePressed()
    {
        return isBuildButtonPressed;
    }
}
