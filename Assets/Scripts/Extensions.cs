using UnityEngine;

public static class Extentions {

    public static void SetAngleZ(this Transform transform, float z) {
        Vector3 angle = transform.eulerAngles;
        angle.z = z;
        transform.eulerAngles = angle;
    }

    public static void LookAt2D(this Transform transform, Vector2 target) {
            transform.right = target - (Vector2)transform.position;
    }
}