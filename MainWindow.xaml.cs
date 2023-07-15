using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace SudokuWPF_test1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private TextBox[,] textBoxes;
    private int[,]? puzzle;
    private int[,]? puzzleSolution;

    public MainWindow()
    {
        InitializeComponent();
        this.Title = "Sudoku";

        // Create Grid
        Grid grid = new Grid();
        for (int i = 0; i < 10; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        // Create TextBoxes
        textBoxes = new TextBox[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                textBoxes[i, j] = new TextBox();
                textBoxes[i, j].MaxLength = 1;
                textBoxes[i, j].FontSize = 20;
                textBoxes[i, j].TextAlignment = TextAlignment.Center;
                textBoxes[i, j].PreviewTextInput += TextBox_PreviewTextInput;

                Border border = new Border();
                border.Child = textBoxes[i, j];
                border.BorderThickness = new Thickness(
                    j % 3 == 0 ? 2 : 0,
                    i % 3 == 0 ? 2 : 0,
                    j % 3 == 2 ? 2 : 1,
                    i % 3 == 2 ? 2 : 1);
                border.BorderBrush = Brushes.Black;

                Grid.SetRow(border, i);
                Grid.SetColumn(border, j);
                grid.Children.Add(border);
            }
        }

        // Add Solve button
        Button solveButton = new Button();
        solveButton.Content = "Solve";
        solveButton.Click += SolveButton_Click;
        Grid.SetRow(solveButton, 9);
        Grid.SetColumn(solveButton, 0);
        Grid.SetColumnSpan(solveButton, 3);
        grid.Children.Add(solveButton);

        // Add Reset button
        Button resetButton = new Button();
        resetButton.Content = "Reset";
        resetButton.Click += ResetButton_Click;
        Grid.SetRow(resetButton, 9);
        Grid.SetColumn(resetButton, 3);
        Grid.SetColumnSpan(resetButton, 3);
        grid.Children.Add(resetButton);

        // Add Check button
        Button checkButton = new Button();
        checkButton.Content = "Check";
        checkButton.Click += CheckButton_Click;
        Grid.SetRow(checkButton, 9);
        Grid.SetColumn(checkButton, 6);
        Grid.SetColumnSpan(checkButton, 3);
        grid.Children.Add(checkButton);

        // Add Grid to window
        this.Content = grid;

        // Generate sudoku puzzle
        GenerateNewPuzzle();
    }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Only allow digits as input
        if (!char.IsDigit(e.Text[0]))
            e.Handled = true;
    }
    
    private void SolveButton_Click(object sender, RoutedEventArgs e)
    {
        // Solve the puzzle
        SolvePuzzle();
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        // Generate a new puzzle
        GenerateNewPuzzle();
    }

    private void CheckButton_Click(object sender, RoutedEventArgs e)
    {
         // Check if solution is correct and highlight incorrect answers
         if (IsSolutionCorrect())
             MessageBox.Show("Correct!");
         else
             HighlightIncorrectAnswers();
    }

    private void SolvePuzzle()
    {
         // Populate TextBoxes with solution
         for (int i = 0; i < 9; i++)
             for (int j = 0; j < 9; j++)
                 textBoxes[i, j].Text = puzzleSolution[i, j].ToString();
    }

    private void HighlightIncorrectAnswers()
    {
         for (int i = 0; i < 9; i++)
             for (int j = 0; j < 9; j++)
                 if (textBoxes[i,j].Text.Length > 0 && int.Parse(textBoxes[i,j].Text) != puzzleSolution[i,j])
                     textBoxes[i,j].Background = Brushes.Red;
                 else
                     textBoxes[i,j].Background = Brushes.White;
    }

    private bool IsSolutionCorrect()
    {
         // Check if all cells are filled
         for (int i = 0; i < 9; i++)
             for (int j = 0; j < 9; j++)
                 if (textBoxes[i,j].Text.Length == 0)
                     return false;

         // Check if all rows, columns and subgrids contain the numbers 1-9
         for (int i=0;i<9;i++){
             bool[] row=new bool[9];
             bool[] col=new bool[9];
             bool[] subgrid=new bool[9];
             for(int j=0;j<9;j++){
                 int num=int.Parse(textBoxes[i,j].Text)-1;
                 if(row[num])
                     return false;
                 row[num]=true;

                 num=int.Parse(textBoxes[j,i].Text)-1;
                 if(col[num])
                     return false;
                 col[num]=true;

                 num=int.Parse(textBoxes[i/3*3+j/3,i%3*3+j%3].Text)-1;
                 if(subgrid[num])
                     return false;
                 subgrid[num]=true;
             }
         }

         return true;
    }

    private void GenerateNewPuzzle()
    {
        // Clear TextBoxes
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                textBoxes[i, j].Clear();

        // Generate sudoku puzzle
        puzzleSolution = SudokuGenerator.GenerateSudokuPuzzle();
        puzzle = (int[,])puzzleSolution.Clone();
        RemoveDigits(puzzle);

        // Populate TextBoxes with puzzle
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (puzzle[i, j] != 0)
                    textBoxes[i, j].Text = puzzle[i, j].ToString();
    }

    private static void RemoveDigits(int[,] grid)
    {
    // Removing digits from the generated Sudoku puzzle
    // You can change the number of digits to remove based on the difficulty level
    int count = SudokuGenerator.RandomGenerator(45,50);
    while(count!=0){
        int cellId=SudokuGenerator.RandomGenerator(0,81);
        int i=(cellId/9);
        int j=cellId%9;
        if(grid[i,j]!=0){
            count--;
            grid[i,j]=0;
        }
    }
    }

}