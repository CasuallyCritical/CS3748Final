using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleButton : MonoBehaviour
{

    public string file = "";

    public Menu main;

    public Text btnText;

    public void InitButton(string text)
    {
        btnText.text = text;
    }

    public void ButtonClick()
    {
        main.LoadSpecificModule(file);
    }

}
