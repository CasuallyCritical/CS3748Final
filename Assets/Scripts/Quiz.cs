using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    //Initialization, these are used for setup
    int questionCount = 0;
    public List<Question> questions = new List<Question>();
    //How many questions are needed to pass?
    public float percentage_Pass = 0;

    public int questionsAsked = 0;
    public int correctAnswers = 0;

    string defaultFolder = "C:/Quizzy/";

    string fileName = "Demo.qzy";

    public GameObject quizResults;
    public GameObject quizPanel;

    Question currentQuestion;

    //Quiz Panel items

    public InputField answerField;
    public Text questionField;
    public Text hint;

    //Quiz results
    public Text qAskedText;
    public Text questonsCorrect;
    public Text score;
    public Text title;

    Question GetQuestion(int num)
    {
        if(questions.Count > 0)
        {
            foreach (Question q in questions) // sort through all of our existing questions
            {
                if (q.qNumber == num) // did the number we want already get made? prevent duplicates
                {
                    return q;
                }
            }
        }
        return null;
    }

    public void SubmitAnswer()
    {
        bool correct = currentQuestion.checkAnswer(answerField.text);

        if(questionCount + 1 >= questions.Count)
        {
            questionCount++;
            FinishQuiz();
        } else
        {
            questionCount++;
            currentQuestion = questions[questionCount];
            LoadQuestion();
        }
    }

    public void FinishQuiz()
    {
        correctAnswers = 0;
        foreach(Question q in questions)
        {
            if(q.correct)
            {
                correctAnswers++;
            }
        }

        float grade = (float)(correctAnswers / questionCount);
        score.text = "Final Grade: " + (grade * 100) + "%";
        qAskedText.text = "Questions Asked: " + questionCount;
        questonsCorrect.text = "Questions Correct: " + correctAnswers;
        title.text = title.text + " - " + (grade >= percentage_Pass ? "Passed!" : "Failed!");
        quizResults.SetActive(true);
        quizPanel.SetActive(false);
    }

    private void Start()
    {
        string savedModule = PlayerPrefs.GetString("Module");
        SetupQuiz(savedModule);
        LoadQuestion();
    }

    public void MenuReturn()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    void LoadQuestion()
    {
        currentQuestion.ReadQuestion();
        questionField.text = currentQuestion.question;
        hint.text = currentQuestion.hint;
        answerField.text = "";
    }

    public void SetupQuiz(string location)
    {
        quizResults.SetActive(false);
        quizPanel.SetActive(true);
        string line;
        StreamReader reader = new StreamReader(location);
        while ((line = reader.ReadLine()) != null) {
            if(line.StartsWith("q")) // q = question
            {
                Question temp = GetQuestion(int.Parse(line.Substring(1, 2)));
                if(temp != null) // If the question was already made, just alter it
                {
                    temp.question = line.Substring(3);
                } else // else make the question
                {
                    temp = new Question();
                    temp.question = line.Substring(3);
                    temp.qNumber = int.Parse(line.Substring(1, 2));
                    questions.Add(temp);
                }
            }

            if(line.StartsWith("a")) // a = answer
            {
                Question temp = GetQuestion(int.Parse(line.Substring(1, 2)));
                if (temp != null)
                {
                    temp.answer = line.Substring(3);
                }
                else
                {
                    temp = new Question();
                    temp.answer = line.Substring(3);
                    temp.qNumber = int.Parse(line.Substring(1, 2));
                    questions.Add(temp);
                }
            }

            if (line.StartsWith("h")) // h = hint
            {
                Question temp = GetQuestion(int.Parse(line.Substring(1, 2)));
                if (temp != null)
                {
                    temp.hint = line.Substring(3);
                }
                else
                {
                    temp = new Question();
                    temp.hint = line.Substring(3);
                    temp.qNumber = int.Parse(line.Substring(1, 2));
                    questions.Add(temp);
                }
            }

            if (line.StartsWith("n")) // n = Name
            {
                title.text = line.Substring(2);
            }

            if(line.StartsWith("g")) // g = Grade
            {
                percentage_Pass = float.Parse(line.Substring(2)) / 100;
            }
        }

        foreach(Question q in questions){

            //Correct the formatting, in case we have a whitespace in front of the question, or answer

            if(q.question.StartsWith(" "))
            {
                q.question = q.question.Substring(0);
            }

            if (q.answer.StartsWith(" "))
            {
                q.answer = q.answer.Substring(0);
            }

            if(q.hint.StartsWith(" "))
            {
                q.hint = q.hint.Substring(0);
            }
        }

        currentQuestion = questions[0];
    }

}
