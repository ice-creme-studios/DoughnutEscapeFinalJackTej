using UnityEngine;
using UnityEngine.InputSystem; // needed for Mouse.current
using System.Collections;

public class SwordScript : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float angle = 30f;
    public float duration = 0.08f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine rotateRoutine;
    private bool isRotating = false;

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isRotating) // ignore new clicks while already swinging
            {
                rotateRoutine = StartCoroutine(RotateTapRoutine());
            }
        }
    }

    private IEnumerator RotateTapRoutine()
    {
        isRotating = true;

        Quaternion start = transform.localRotation;
        Quaternion target = start * Quaternion.Euler(axis.normalized * angle);

        // go to target
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float e = ease.Evaluate(Mathf.Clamp01(t));
            transform.localRotation = Quaternion.Slerp(start, target, e);
            yield return null;
        }

        // go back
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float e = ease.Evaluate(Mathf.Clamp01(t));
            transform.localRotation = Quaternion.Slerp(target, start, e);
            yield return null;
        }

        isRotating = false;
        rotateRoutine = null;
    }
}
