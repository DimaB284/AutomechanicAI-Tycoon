using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProblemIndicator : MonoBehaviour
{
    public Image icon;
    public Text problemText;
    public Sprite engineIcon, wheelsIcon, bodyIcon, electronicsIcon, tiresIcon, paintIcon;

    public void SetProblem(Car.DamageType damageType)
    {
        switch (damageType)
        {
            case Car.DamageType.Engine:
                icon.sprite = engineIcon;
                problemText.text = "Двигун";
                break;
            case Car.DamageType.Wheels:
                icon.sprite = wheelsIcon;
                problemText.text = "Шини";
                break;
            case Car.DamageType.Body:
                icon.sprite = bodyIcon;
                problemText.text = "Кузов";
                break;
            case Car.DamageType.Electronics:
                icon.sprite = electronicsIcon;
                problemText.text = "Електроніка";
                break;
            case Car.DamageType.Paint:
                icon.sprite = paintIcon;
                problemText.text = "Фарба";
                break;
        }
    }

    void LateUpdate()
    {
        // Billboard-ефект: завжди дивитися на камеру
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
