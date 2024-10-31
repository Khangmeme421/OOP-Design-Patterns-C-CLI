using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Comp1551_Coursewark
{
    // Menu class interface
    public abstract class Menu
    {
        public List<string> MenuItems { get; set; }

        public abstract string DisplayMenu();
    }

    public class MainMenu : Menu
    {
        public MainMenu()
        {
            MenuItems = new List<string> { "Exit Program", "Manage Questions", "Play" };
        }
        public override string DisplayMenu()
        {
            Console.WriteLine("Main Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Choose an option (0-2): ");
            string option = Console.ReadLine();
            List<string> invalidOptions = new List<string> { "0", "1", "2" };
            while (!invalidOptions.Contains(option))
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }
            int optionIndex = int.Parse(option);
            string returnMenu = MenuItems[optionIndex].Replace(" ", "");
            return returnMenu;
        }
    }
    public class ManageQuestionsMenu : Menu
    {
        public ManageQuestionsMenu()
        {
            MenuItems = new List<string> { "Back", "Create New Question", "View All Questions", "Delete Questions", "Edit Questions" };
        }
        public override string DisplayMenu()
        {
            Console.WriteLine("Manage Questions Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Choose an option (0-3): ");
            string option = Console.ReadLine();
            List<string> invalidOptions = new List<string> { "0", "1", "2", "3", "4" };
            while (!invalidOptions.Contains(option))
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }
            int optionIndex = int.Parse(option);
            string returnMenu = MenuItems[optionIndex].Replace(" ", "");
            if (returnMenu == "Back") 
                returnMenu = "MainMenu";
            return returnMenu;
        }
    }

    public class ViewAllQuestionsMenu : Menu
    {
        public ViewAllQuestionsMenu()
        {
            // Initialize the MenuItems list with "Back"
            MenuItems = new List<string> { "Back" };
        }

        public override string DisplayMenu()
        {
            Console.WriteLine("View All Questions Menu:");
            if (DataManagement.Instance.Questions.Count == 0)
            {
                Console.WriteLine("No questions available.\nPress Enter to go back");
                Console.ReadLine();
                return "ManageQuestions";
            }
            for (int i =0; i < DataManagement.Instance.Questions.Count; i++)
            {
                DataManagement.Instance.Questions[i].DisplayQuestion(i + 1);
            }
            Console.Write("Enter (0) to get back: ");
            string option = Console.ReadLine();

            // Validate the input option
            while (option != "0")
            {
                Console.Write("Invalid option. Enter (0) to get back: ");
                option = Console.ReadLine();
            }
            DataManagement.Instance.MenuHistory.RemoveAt(DataManagement.Instance.MenuHistory.Count - 1);
            return "ManageQuestions";
        }
    }

    public class DeleteQuestionsMenu : Menu
    {
        public DeleteQuestionsMenu()
        {
            // Initialize the MenuItems list with "Back"
            MenuItems = new List<string> { "Back" };
        }

        public override string DisplayMenu()
        {
            // Fetch questions from DataManagement and add their titles to the MenuItems
            MenuItems = new List<string> { "Back" };
            var questions = DataManagement.Instance.Questions;
            foreach (var question in questions)
            {
                MenuItems.Add(question.Title + " " + question.QuestionType);
            }

            Console.WriteLine("Delete Questions Menu:");
            if (DataManagement.Instance.Questions.Count == 0)
            {
                Console.WriteLine("No questions available.\nPress Enter to go back");
                Console.ReadLine();
                return "Back";
            }
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Enter question number: ");
            string option = Console.ReadLine();

            // Validate the input option
            int index;
            if (option == "0")
                return "ManageQuestions";
            while (!int.TryParse(option, out index) || index < 0 || index >= MenuItems.Count)
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }
            bool success = DataManagement.Instance.DeleteQuestion(index - 1); // Adjust for "Back" option
            if (success)
                Console.WriteLine("Question deleted successfully.");
            Console.ReadLine();
            DataManagement.Instance.MenuHistory.RemoveAt(DataManagement.Instance.MenuHistory.Count - 1);
            return "ManageQuestions";
        }
    }
    public class EditQuestionsMenu : Menu
    {
        public EditQuestionsMenu()
        {
            // Initialize the MenuItems list with "Back"
            MenuItems = new List<string> { "Back" };
        }

        public override string DisplayMenu()
        {
            // Fetch questions from DataManagement and add their titles to the MenuItems
            Console.WriteLine("Edit Questions Menu:");
            if (DataManagement.Instance.Questions.Count == 0)
            {
                Console.WriteLine("No questions available.\nPress Enter to go back");
                Console.ReadLine();
                return "Back";
            }
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            for (int i = 0; i < DataManagement.Instance.Questions.Count; i++)
            {
                DataManagement.Instance.Questions[i].DisplayQuestion(i + 1);
            }
            Console.Write("Enter question number to edit: ");
            string option = Console.ReadLine();

            // Validate the input option
            int index = int.Parse(option);
            if (option == "0")
                return "ManageQuestions";

            while (!int.TryParse(option, out index) || index < 0 || index > DataManagement.Instance.Questions.Count)
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }

            // Create a new question based on user input
            Question newQuestion = QuestionFactory.CreateQuestion(); 
            if (newQuestion != null)
            {
                // Replace the existing question with the new question
                DataManagement.Instance.Questions[index - 1] = newQuestion; // Update the list with the new question
                Console.WriteLine("Question edited successfully.");
            }
            else
            {
                Console.WriteLine("Failed to edit a question.");
            }

            Console.ReadLine();
            return "ManageQuestions";
        }
    }
    public class CreateQuestionMenu : Menu
    {
        public CreateQuestionMenu()
        {
            MenuItems = new List<string> { "Back" }; // The only option initially is to go back
        }

        public override string DisplayMenu()
        {
            List<string> questionTypes = new List<string>()
        {
            "Open-Ended",
            "Multiple Choice",
            "True/False"
        };

            Console.WriteLine("Create New Question:");
            Console.WriteLine("Select question type:");
            Console.WriteLine("0. Back");
            for (int i = 0; i < questionTypes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {questionTypes[i]}");
            }

            string choice = Console.ReadLine();
            int choice_int;
            while (!int.TryParse(choice, out choice_int) || choice_int < 0 || choice_int > 3)
            {
                Console.Write("Invalid choice. Please enter a number between 0 and 3: ");
                choice = Console.ReadLine();
            }

            if (choice_int == 0) // If the user chooses to go back
            {
                return "ManageQuestions"; // Indicate that the menu user wants to go back
            }

            Console.Write($"({questionTypes[choice_int - 1]}) Enter question title: ");
            string title = Console.ReadLine();

            Question question = null;

            switch (choice_int)
            {
                case 1:
                    Console.Write("Enter correct answer(s) separated by commas: ");
                    string openEndedAnswer = Console.ReadLine();
                    question = new OpenEndedQuestion(title, openEndedAnswer);
                    break;

                case 2:
                    List<string> options = new List<string>();
                    for (int i = 0; i < 4; i++)
                    {
                        Console.Write($"Enter option {i + 1}: ");
                        options.Add(Console.ReadLine());
                    }
                    int index;
                    do
                    {
                        Console.Write("Enter correct answer character (A-D): ");
                        char correctAnswerChar = char.ToUpper(Console.ReadLine()[0]);
                        index = correctAnswerChar - 'A';
                    }
                    while (index < 0 || index > 3);
                    string correctAnswerCharString = ((char)('A' + index)).ToString();
                    question = new MultipleChoiceQuestion(title, options, correctAnswerCharString);
                    break;

                case 3:
                    string trueFalseAnswer;
                    do
                    {
                        Console.Write("Enter correct answer (True/False): ");
                        trueFalseAnswer = Console.ReadLine();
                    }
                    while(!trueFalseAnswer.Equals("True", StringComparison.OrdinalIgnoreCase) && !trueFalseAnswer.Equals("False", StringComparison.OrdinalIgnoreCase));
                    bool tfAnswer = bool.Parse(trueFalseAnswer);
                    question = new TrueFalseQuestion(title, tfAnswer);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    return "ManageQuestions";
            }

            // Add the created question to the DataManagement instance
            DataManagement.Instance.Questions.Add(question);
            DataManagement.Instance.MenuHistory.RemoveAt(DataManagement.Instance.MenuHistory.Count - 1);
            Console.WriteLine("Question created successfully.");
            Console.ReadLine();
            return "CreateNewQuestion"; // After creating a question, go back to the previous menu
        }
    }

    public class PlayMenu : Menu
    {
        public PlayMenu()
        {
            MenuItems = new List<string> { "Back" };
        }
        public override string DisplayMenu()
        {
            Player user = new Player("Player", "Player");
            Console.WriteLine("Play Quiz Menu:");
            if (DataManagement.Instance.Questions.Count == 0)
                Console.WriteLine("No questions available.\nPress Enter to go back");
            else
            {
                DataManagement stopWatch = DataManagement.Instance;
                // Start the stopwatch
                stopWatch.StartStopwatch();
                for (int i = 0; i < DataManagement.Instance.Questions.Count; i++)
                {
                    Console.Clear();
                    Console.WriteLine("Play Quiz Menu:");
                    Question currentQuestion = DataManagement.Instance.Questions[i];
                    currentQuestion.DisplayQuestion(i);
                    string userAnswer = Console.ReadLine();
                    user.AnswerQuestion(currentQuestion, userAnswer);
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                }
                // Stop the stopwatch after the loop
                stopWatch.StopStopwatch();
                Console.WriteLine($"Your score: {user.Score}");
                // Get and print the elapsed time
                string elapsedTime = stopWatch.GetElapsedTime();
                Console.WriteLine($"Total time taken: {elapsedTime}");
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Would you like to view all question's answer (yes/no):");
                string answer = Console.ReadLine();
                if (answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    return "ViewAllAnswersMenu";
            }
            Console.ReadLine();
            return "MainMenu";
        }
    }
    public class ViewAllAnswersMenu : Menu
    {
        public ViewAllAnswersMenu()
        {
            MenuItems = new List<string> { "" };
        }
        public override string DisplayMenu()
        {
            for (int i = 0; i < DataManagement.Instance.Questions.Count; i++)
            {
                Console.Clear();
                Console.WriteLine("View All Answers Menu:");
                Question currentQuestion = DataManagement.Instance.Questions[i];
                currentQuestion.DisplayQuestion(i + 1);
                Console.WriteLine($"Correct answer: {currentQuestion.GetCorrectAnswer()}");
                Console.WriteLine("Press Enter to continue");
                Console.ReadLine();
            }
            return "MainMenu";
        }
    }
    public class ManageUserMenu : Menu
    {
        public ManageUserMenu()
        {
            MenuItems = new List<string> { "Back", "Create new user", "Delete user", "Edit user name" };
        }

        public override string DisplayMenu()
        {
            Console.WriteLine("Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();
            List<string> invalidOptions = new List<string> { "0", "1", "2", "3" };
            while (!invalidOptions.Contains(option))
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }
            int optionIndex = int.Parse(option);
            string returnMenu = MenuItems[optionIndex].Replace(" ", "");
            return returnMenu;
        }
    }

    // User class interface
    public abstract class User
    {
        private string _userName;
        private string _role;
        private int _score;
        public abstract int Score { get; set;}
        public abstract string UserName { get; set; }
        public abstract string Role { get; set; }
        public User(string userName, string role)
        {
            UserName = userName;
            Role = role;
        }
    }

    // Player class is a subclass of User
    public class Player : User
    {
        private string _userName;
        private string _role;
        private int _score;     //Has score attribute in addition
        public override string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public override int Score 
        { 
            get { return _score; } 
            set { _score = value; } 
        }
        public override string Role
        {
            get { return _role; }
            set { _role = value; }
        }
        public Player(string userName, string role) : base(userName, role)
        {
            this.UserName = userName;
            this.Role = role;
            _score = 0;
        }
        public void AnswerQuestion(Question question, string answer)
        {
            if (question.CheckAnswer(answer))
            {
                _score += question.Points;
                Console.WriteLine("Correct!");
            }
            else
            {
                Console.WriteLine("Incorrect!");
            } 
        }
    }

    // Question class
    public abstract class Question
    {
        private string _title;
        private string _questionType;
        private int _points;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string QuestionType
        {
            get { return _questionType; }
            protected set { _questionType = value; }
        }

        public int Points
        {
            get { return _points; }
            set { _points = value > 0 ? value : 1; } // Ensure points are always positive
        }

        public Question(string title)
        {
            Title = title;
            Points = 10; // Default points
        }

        public abstract bool CheckAnswer(string userAnswer);
        public abstract void DisplayQuestion(int questionNumber);
        public abstract string GetCorrectAnswer();
    }
    public class TrueFalseQuestion : Question
    {
        private bool _correctAnswer;

        public bool CorrectAnswer
        {
            get { return _correctAnswer; }
            set { _correctAnswer = value; }
        }

        public TrueFalseQuestion(string title, bool correctAnswer) : base(title)
        {
            CorrectAnswer = correctAnswer;
            QuestionType = "(True/False)";
        }

        public override void DisplayQuestion(int questionNumber)
        {
            Console.WriteLine($"{questionNumber}. {Title} {QuestionType}");
            //Console.WriteLine("Enter True or False:");
        }

        public override bool CheckAnswer(string userAnswer)
        {
            return bool.TryParse(userAnswer, out bool answer) && answer == CorrectAnswer;
        }

        public override string GetCorrectAnswer()
        {
            return CorrectAnswer.ToString();
        }
    }
    public class MultipleChoiceQuestion : Question
    {
        private List<string> _options;
        private string _correctAnswerIndex;

        public List<string> Options
        {
            get { return _options; }
            set { _options = value ?? new List<string>(); }
        }

        public string CorrectAnswerIndex
        {
            get { return _correctAnswerIndex; }
            set { _correctAnswerIndex = value.ToString(); }
        }

        public MultipleChoiceQuestion(string title, List<string> options, string correctAnswerIndex) : base(title)
        {
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
            QuestionType = "(A,B,C,D)";
        }

        public override void DisplayQuestion(int questionNumber)
        {
            Console.WriteLine($"{questionNumber}. {Title} {QuestionType}");
            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine($"{(char)('A' + i)}. {Options[i]}");
            }
        }

        public override bool CheckAnswer(string userAnswer)
        {
            return (userAnswer == _correctAnswerIndex);
        }

        public override string GetCorrectAnswer()
        {
            return _correctAnswerIndex;
        }
    }
    public class OpenEndedQuestion : Question
    {
        private string _acceptableAnswers;
        public string AcceptableAnswers
        {
            get { return _acceptableAnswers; }
            set { _acceptableAnswers = value ?? string.Empty; }
        }

        public OpenEndedQuestion(string title, string acceptableAnswers) : base(title)
        {
            AcceptableAnswers = acceptableAnswers;
            QuestionType = "(Open-ended)";
        }

        public override void DisplayQuestion(int questionNumber)
        {
            Console.WriteLine($"{questionNumber}. {Title} {QuestionType}");
        }
        public override bool CheckAnswer(string userAnswer)
        {
            // Split the acceptable answers by comma and trim whitespace
            var answers = AcceptableAnswers.Split(',')
                                           .Select(answer => answer.Trim())
                                           .ToArray();

            // Check if the user's answer matches any of the acceptable answers
            return answers.Any(answer => answer.Equals(userAnswer.Trim(), StringComparison.OrdinalIgnoreCase));
        }
        public override string GetCorrectAnswer()
        {
            return AcceptableAnswers; // Return the single string of acceptable answers
        }
    }

    public class QuestionFactory
    {
        public static Question CreateQuestion()
        {
            List<string> questionTypes;     //Temporary list of question types
            questionTypes = new List<string>()
            {
                "Back",
                "Open-Ended",
                "Multiple Choice",
                "True/False"
            };
            Console.WriteLine($"Select question type: \n0. Back \n1. {questionTypes[1]} \n2. {questionTypes[2]} \n3. {questionTypes[3]}");
            string choice = Console.ReadLine();
            int choice_int; // temporary choice_int storages the choice as integer
            while (!int.TryParse(choice, out choice_int) || choice_int < 0 || choice_int > 3)
            {
                Console.Write("Invalid choice. Please enter a number between 0 and 3: ");
                choice = Console.ReadLine();
            }
            if (choice_int == 0)
                return null;
            Console.Write($"({questionTypes[choice_int]}) Enter question title: ");
            string title = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter correct answer(s) separated by commas: ");
                    string openEndedAnswer = Console.ReadLine();
                    return new OpenEndedQuestion(title, openEndedAnswer);

                case "2":
                    List<string> options = new List<string>();
                    for (int i = 0; i < 4; i++)
                    {
                        Console.Write($"Enter option {i + 1}: ");
                        options.Add(Console.ReadLine());
                    }
                    int index;
                    do
                    {
                        Console.Write("Enter correct answer character (A-D): ");
                        char correctAnswerChar = char.ToUpper(Console.ReadLine()[0]); // Ensure it's uppercase
                        index = correctAnswerChar - 'A'; // Convert character to index (A=0, B=1, C=2, D=3)

                    }
                    while (index < 0 ||  index > 3);
                    string correctAnswerCharString = ((char)('A' + index)).ToString();
                    return new MultipleChoiceQuestion(title, options, correctAnswerCharString);

                case "3":
                    string trueFalseAnswer;
                    do
                    {
                        Console.Write("Enter correct answer (True/False): ");
                        trueFalseAnswer = Console.ReadLine();
                    }
                    while (!trueFalseAnswer.Equals("True", StringComparison.OrdinalIgnoreCase) && !trueFalseAnswer.Equals("False", StringComparison.OrdinalIgnoreCase));
                    bool tfAnswer = bool.Parse(trueFalseAnswer);
                    return new TrueFalseQuestion(title, tfAnswer);

                default:
                    Console.WriteLine("Invalid choice.");
                    return null;
            }
        }
    }

    public class MenuFactory
    {
        public static Menu CreateMenu(string menuType)
        {
            switch (menuType)
            {
                case "MainMenu":
                    return new MainMenu();
                case "ManageQuestions":
                    return new ManageQuestionsMenu();
                case "ViewAllQuestions":
                    return new ViewAllQuestionsMenu();
                case "DeleteQuestions":
                    return new DeleteQuestionsMenu();
                case "EditQuestions":
                    return new EditQuestionsMenu();
                case "Play":
                    return new PlayMenu();
                case "CreateNewQuestion":
                    return new CreateQuestionMenu();
                case "ViewAllAnswersMenu":
                    return new ViewAllAnswersMenu();
                case "Back":
                    DataManagement.Instance.MenuHistory.RemoveAt(DataManagement.Instance.MenuHistory.Count - 1);
                    return DataManagement.Instance.MenuHistory[DataManagement.Instance.MenuHistory.Count - 1];
                default:
                    return null;
            }
        }
    }
    public class DataManagement
    {
        private static DataManagement _instance;
        private static readonly object _lock = new object();

        public List<Question> Questions { get; private set; } = new List<Question>();
        public List<Menu> MenuHistory { get; private set; } = new List<Menu>();
        private Stopwatch _stopwatch = new Stopwatch();
        private DataManagement() { }

        public static DataManagement Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    { 
                        _instance = new DataManagement();
                    }
                    return _instance;
                }
            }
        }
        public void AddMenuToHistory(Menu menu)
        {
            MenuHistory.Add(menu);
        }
        public bool DeleteQuestion(int index)
        {
            // Validate the index
            if (index < 0 || index >= Questions.Count)
            {
                Console.WriteLine("Invalid index. Unable to delete the question.");
                return false; // Indicate failure
            }
            // Remove the question at the specified index
            Questions.RemoveAt(index);
            Console.WriteLine($"Question at index {index + 1} has been deleted.");
            return true; // Indicate success
        }
        // Method to start the stopwatch
        public void StartStopwatch()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }
        // Method to stop the stopwatch
        public void StopStopwatch()
        {
            _stopwatch.Stop();
        }
        // Method to get formatted elapsed time
        public string GetElapsedTime()
        {
            TimeSpan elapsed = _stopwatch.Elapsed;
            return string.Format("{0:D2}:{1:D2}", (int)elapsed.TotalMinutes, elapsed.Seconds);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = MenuFactory.CreateMenu("MainMenu");
            EndlessMenu(menu);
            Console.ReadLine();
        }
        static void EndlessMenu(Menu menu)
        {
            string option = menu.DisplayMenu();
            if (option == "ExitProgram")
            {
                return;
            }
            Console.Clear();
            DataManagement.Instance.AddMenuToHistory(menu);
            EndlessMenu(MenuFactory.CreateMenu(option));
        }
    }
}
