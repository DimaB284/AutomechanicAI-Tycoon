using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorUI : MonoBehaviour
{
    public Text errorText;

    public void ShowError(string message, float duration = 2f)
    {
        StopAllCoroutines();
        errorText.text = message;
        errorText.enabled = true;
        StartCoroutine(HideAfterSeconds(duration));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        errorText.enabled = false;
    }
} 