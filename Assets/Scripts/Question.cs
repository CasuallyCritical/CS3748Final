using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question
{

    public string question = "";

    public int qNumber = 0;

    //We can check this to see if we are correct
    public string answer = "";

    //Optional: Give the user a hint to help mark it correctly, such as "Use A, B, C, or D to denote the answer." or "Round decimal to nearest hundredth"
    public string hint = "";

    //This is contained for later purposes
    public bool correct = false;

    public void ReadQuestion()
    {
        //Purely for debugging, you'll never see this outside the Editor
        Debug.Log(question + '\n' + answer + '\n' + hint);
    }

    public bool checkAnswer(string a)
    {
        correct = (answer.ToLower()).Contains(a.ToLower()); // Check the answers (force formatting to lowercase so they can match easier)
        return correct;
    }

}
