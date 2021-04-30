using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public GameObject scrollView;

    public GameObject buttonPrefab;

    string chosenModule = "";

    public Text modInfo;

    // Start is called before the first frame update
    void Start()
    {
        LoadModules();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadModules()
    {
        string path = "C:/Quizzy/";

        foreach (string file in Directory.GetFiles(path))
        {
            if(file.EndsWith(".qzy"))
            {
                string fileName = file.Substring(0, file.Length - 4);
                fileName = fileName.Substring(path.Length);
                GameObject buttonClone = (GameObject)Instantiate(buttonPrefab);
                ModuleButton modButton = buttonClone.GetComponent<ModuleButton>();
                modButton.file = file;
                modButton.main = this;
                modButton.InitButton(fileName);
                buttonClone.transform.SetParent(scrollView.transform);
            }
        }
    }

    public void LoadSpecificModule(string path)
    {
        string line;
        int qs = 0;
        string moduleName = "";
        string grade = "";
        StreamReader reader = new StreamReader(path);
        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("q")) // q = question
            {
                qs++;
            }

            if(line.StartsWith("n"))
            {
                moduleName = line.Substring(2);
            }

            if(line.StartsWith("g"))
            {
                grade = line.Substring(2);
            }
        }

        modInfo.text = moduleName + '\n' + '\n' +
        "Module Questions: " + qs + '\n' + '\n' +
        "Minimum Grade: " + grade;

        chosenModule = path;

    }

    public void PlayModule()
    {
        PlayerPrefs.SetString("Module", chosenModule);
        SceneManager.LoadScene("QuizScene", LoadSceneMode.Single);
    }
}
