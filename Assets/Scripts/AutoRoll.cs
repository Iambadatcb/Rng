using UnityEngine;
using System.Collections;

public class AutoRoller : MonoBehaviour
{
    public RngWeighted rngWeighted;
    public float rollInterval = 1f; // Time between rolls in seconds

    private Coroutine autoRollCoroutine;

    // Call this method from your AutoRoll UI Button OnClick
    public void StartAutoRolling()
    {
        if (autoRollCoroutine == null)
            autoRollCoroutine = StartCoroutine(AutoRollRoutine());
    }

    public void StopAutoRolling()
    {
        if (autoRollCoroutine != null)
        {
            StopCoroutine(autoRollCoroutine);
            autoRollCoroutine = null;
        }
    }

    private IEnumerator AutoRollRoutine()
    {
        while (true)
        {
            rngWeighted.RollOnClick();
            yield return new WaitForSeconds(rollInterval);
        }
    }
}

