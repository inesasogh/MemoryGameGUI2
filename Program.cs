using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class MemoryGame1 : Form
{
    private List<string> icons;
    private List<Button> buttons;
    private Button firstButton;
    private Button secondButton;
    private Timer gameTimer;
    //private Timer timer;
    private int pairsFound = 0;
    private int timeLeft = 100;
    private int totalTime = 100;
    private int currentLevel = 1;
    private int[] levelPairs = { 4, 6, 7 };
    private int totalPairs;
    private Label timeLabel;
    private Label levelLabel;
    public MemoryGame1()
    {
        Text = "Memory Game";
        Size = new System.Drawing.Size(500, 600);
        icons = new List<string>
        {
            "Apple", "Apple", "Button", "Button", "Click", "Click", "Drive", "Drive",
            "Enter", "Enter", "File", "File", "Game", "Game", "Hour", "Hour", "Apple", "Apple", "Button", "Button", "Click", "Click", "Drive", "Drive",
            "Enter", "Enter", "File", "File", "Game", "Game"


        };

        buttons = new List<Button>();
        gameTimer = new Timer();
        gameTimer.Interval = 2000;  // Update every 1 second
        gameTimer.Tick += GameTimer_Tick;

        timeLabel = new Label
        {
            Text = "Time: " + timeLeft + "s",
            Location = new System.Drawing.Point(10, 10),
            AutoSize = true
        };
        Controls.Add(timeLabel);
        levelLabel = new Label
        {
            Text = "Level: " + currentLevel,
            Location = new System.Drawing.Point(10, 30),
            AutoSize = true
        };
        Controls.Add(levelLabel);
        ResetGame();
    }

    private void ResetGame()
    {

        ClearButtonText();
        ShuffleIcons();
        CreateButtons();
        pairsFound = 0;
        timeLeft = totalTime;
        totalPairs = levelPairs[currentLevel - 1];
        UpdateButtonState(true);
        gameTimer.Start();
        UpdateLevelLabel();
    }
    private void UpdateLevelLabel()
    {
        levelLabel.Text = "Level: " + currentLevel;
    }
    private void ClearButtonText()
    {
        foreach (Button button in buttons)
        {
            button.Text = "";
        }
    }
    private void UpdateButtonState(bool enable)
    {
        foreach (Button button in buttons)
        {
            button.Text = enable ? " " : button.Tag.ToString();
            button.Enabled = enable;
        }
    }

    private void ShuffleIcons()
    {
        Random rand = new Random();
        icons = icons.OrderBy(i => rand.Next()).ToList();
    }

    private void CreateButtons()
    {
        int row = 1;
        int col = 0;

        buttons.Clear(); // Clear existing buttons before creating new ones
        int totalButtons = totalPairs * 2;
        for (int i = 0; i < icons.Count; i++)
        {
            Button button = new Button
            {
                Text = "",
                Tag = icons[i],
                Width = 60,
                Height = 60,
                Top = row * 70 + 20,
                Left = col * 70 + 20
            };

            button.Click += Button_Click;
            buttons.Add(button);
            Controls.Add(button);

            col++;
            if (col == 6)
            {
                col = 0;
                row++;
            }
        }
    }

    private void Button_Click(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;

        if (firstButton == null)
        {
            firstButton = clickedButton;
            firstButton.Text = firstButton.Tag.ToString();
            firstButton.Enabled = false;
        }
        else if (secondButton == null && clickedButton != firstButton)
        {
            secondButton = clickedButton;
            secondButton.Text = secondButton.Tag.ToString();
            secondButton.Enabled = false;

            if (firstButton.Tag.Equals(secondButton.Tag))
            {
                pairsFound++;
                if (pairsFound == totalPairs)
                {
                    if (currentLevel < levelPairs.Length)
                    {
                        MessageBox.Show($"Congratulations! You've completed Level {currentLevel}.\nStarting Level {currentLevel+1}.");
                        currentLevel++;
                        ClearButtonText();
                        ResetGame();
                    }
                    else
                    {
                        MessageBox.Show("Congratulations! You've won the game.");
                        Close();
                    }
                }
                firstButton = null;
                secondButton = null;
            }
            /*else
            {
                gameTimer.Start();
            }*/
        }
    }

    private void GameTimer_Tick(object sender, EventArgs e)
    {
        totalTime--; // Decrement the time left
        timeLabel.Text = "Time: " + totalTime + "s";  // Update the time label
        if (totalTime <= 0)
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over! You ran out of time.");
            Close();
        }

       if (firstButton != null)
        {
            firstButton.Text = "";
            firstButton.Enabled = true;
        }
        if (secondButton != null)
        {
            secondButton.Text = "";
            secondButton.Enabled = true;
        }

        firstButton = null;
        secondButton = null;
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new MemoryGame1());
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();
        // 
        // MemoryGame1
        // 
        this.ClientSize = new System.Drawing.Size(338, 369);
        this.Name = "MemoryGame1";
        this.ResumeLayout(false);

    }
}
