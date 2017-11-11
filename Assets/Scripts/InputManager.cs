using UnityEngine;

public static class InputManager {

    private static InputType[] inputTypes;

    // public static void SetInputs(InputType p1) {

    // }

    public static void SetInputs(params InputType[] inputTypes) {
        InputManager.inputTypes = inputTypes;
    }

    public static bool IsShooting(int player) {
        if (player >= inputTypes.Length) {
            Debug.LogError("Player " + player + " does not exist!");
            Application.Quit();
        }

        if (inputTypes[player].IsKeyboard())
            return Input.GetMouseButton(2);
        else
            return true;
        
    }
}

public enum InputType {
    Keyboard,
    Joy1,
    Joy2,
    Joy3,
    Joy4
}

internal static class InputTypeExtensions {
    internal static bool IsKeyboard(this InputType inputType) {
        return inputType == InputType.Keyboard;
    }

    internal static int GetJoyNum(this InputType inputType) {
        switch(inputType) {
            case InputType.Keyboard:
                return -1;
            case InputType.Joy1:
                return 1;
            case InputType.Joy2:
                return 2;
            case InputType.Joy3:
                return 3;
            case InputType.Joy4:
                return 4;
            default:
                Debug.LogError("Bad input type. This should never happen...");
                return -2;
        }
    }
}