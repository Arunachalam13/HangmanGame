using System.ComponentModel;
using System.Diagnostics;

namespace Hangman;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    #region UI properties

    public string Spotlight
    {
        get { return spotlight; }
        set 
        { 
            spotlight = value;
            OnPropertyChanged(nameof(Spotlight));
        }
    }

    public List<char> Letters
    {
        get { return letters; }
        set 
        { 
            letters = value;
            OnPropertyChanged(nameof(Letters));
        }
    }

    public string Message
    {
        get { return message; }
        set 
        { 
            message = value;
            OnPropertyChanged();
        }
    }

    public string GameStatus
    {
        get { return gamestatus; }
        set 
        { 
            gamestatus = value;
            OnPropertyChanged();
        }
    }

    public string CurrentImage
    {
        get { return currentimage; }
        set 
        { 
            currentimage = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Fields
    List<string> words = new List<string>()
    {
        "python",
        "javascript",
        "maui",
        "csharp",
        "mongodb",
        "sql",
        "xaml",
        "word",
        "excel",
        "powerpoint",
        "code",
        "hotreload",
        "snippets"
    };
    string answer = "";
    private string spotlight;
    List<char> guessed = new List<char>();
    private List<char> letters = new List<char>();
    private string message;
    int mistakes = 0;
    private string gamestatus;
    int maxWrong = 6;
    private string currentimage = "img0.jpg";
    #endregion

    public MainPage()
	{
		InitializeComponent();
        Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
        BindingContext = this;
        PickWord();
        CalculateWord(answer, guessed);
    }

    #region Game Engine
    private void PickWord()
    {
        answer = words[new Random().Next(words.Count)];
        Debug.WriteLine(answer);
    }

    private void CalculateWord(string answer, List<char> guessed)
    {
        var temp = answer.Select(x => guessed.IndexOf(x) >= 0 ? x :'_').ToArray();
        Spotlight = string.Join(' ', temp);
    }
    #endregion

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if(btn != null)
        {
            var letter = btn.Text;
            btn.IsEnabled = false;
            HandleGuess(letter[0]);
        }
    }

    private void HandleGuess(char letter)
    {
        if(answer.IndexOf(letter) >= 0)
        {
            guessed.Add(letter);
            CalculateWord(answer, guessed);
            CheckIfGameWon();
        }
        else if(answer.IndexOf(letter) == -1)
        {
            mistakes++;
            UpdateStatus();
            CheckIfGameLost();
            CurrentImage = $"img{mistakes}.jpg";
        }
    }

    private void CheckIfGameLost()
    {
        if(mistakes==maxWrong)
        {
            Message = "You Lost!";
            DisableLetters();
        }
    }

    private void DisableLetters()
    {
        foreach(var element in lettersContainer.Children)
        {
            var btn = element as Button;
            if(btn != null )
            {
                btn.IsEnabled = false;
            }
        }
    }

    private void CheckIfGameWon()
    {
        if(Spotlight.Replace(" ","") == answer)
        {
            Message = "You Won!";
            DisableLetters();
        }
    }

    private void UpdateStatus()
    {
        GameStatus = $"Errors: {mistakes} of {maxWrong}";
    }

    private void ResetClicked(object sender, EventArgs e)
    {
        mistakes = 0;
        guessed = new List<char>();
        PickWord();
        CalculateWord(answer, guessed);
        Message = "";
        UpdateStatus();
        CurrentImage = "img0.jpg";
        EnableLetters();
    }

    private void EnableLetters()
    {
        foreach (var element in lettersContainer.Children)
        {
            var btn = element as Button;
            if (btn != null)
            {
                btn.IsEnabled = true;
            }
        }
    }
}

