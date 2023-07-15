namespace SudokuWPF_test1;

public static class SudokuGenerator
{
    public static int[,] GenerateSudokuPuzzle()
    {
        int[,] grid = new int[9, 9];

        // Fill the diagonal 3x3 matrices
        FillDiagonalMatrices(grid);

        // Fill remaining blocks
        FillRemaining(0, 3, grid);

        return grid;
    }

    private static void FillDiagonalMatrices(int[,] grid)
    {
        for (int i = 0; i < 9; i += 3)
            FillMatrix(i, i, grid);
    }

    private static void FillMatrix(int row, int col, int[,] grid)
    {
        int num;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                do
                {
                    num = RandomGenerator(1, 10);
                }
                while (!IsSafeToPutNumInMatrix(row, col, grid, num));

                grid[row + i, col + j] = num;
            }
        }
    }

    private static bool IsSafeToPutNumInMatrix(int row, int col, int[,] grid, int num)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (grid[row + i, col + j] == num)
                    return false;

        return true;
    }

    private static bool FillRemaining(int i, int j, int[,] grid)
    {
        if (j >= 9 && i < 8)
        {
            i++;
            j = 0;
        }

        if (i >= 9 && j >= 9)
            return true;

        if (i < 3)
        {
            if (j < 3)
                j = 3;
        }
        else if (i < 6)
        {
            if (j == (int)(i / 3) * 3)
                j += 3;
        }
        else
        {
            if (j == 6)
            {
                i++;
                j = 0;
                if (i >= 9)
                    return true;
            }
        }

        for (int num = 1; num <= 9; num++)
        {
            if (IsSafeToPutNum(i, j, grid, num))
            {
                grid[i, j] = num;
                if (FillRemaining(i, j + 1, grid))
                    return true;

                grid[i, j] = 0;
            }
        }

        return false;
    }

    private static bool IsSafeToPutNum(int i, int j, int[,] grid, int num)
    {
        return IsSafeInRow(i, grid, num) &&
            IsSafeInCol(j, grid, num) &&
            IsSafeInMatrix(i - i % 3, j - j % 3, grid, num);
    }

    private static bool IsSafeInRow(int i,int[,] grid,int num)
    {
    for(int col=0;col<9;col++)
        if(grid[i,col]==num)
            return false;
    return true;
    }

    private static bool IsSafeInCol(int j,int[,] grid,int num)
    {
    for(int row=0;row<9;row++)
            if(grid[row,j]==num)
                return false;
        return true;
        }

        private static bool IsSafeInMatrix(int row,int col,int[,] grid,int num)
        {
        for(int r=0;r<3;r++)
            for(int c=0;c<3;c++)
                if(grid[row+r,col+c]==num)
                    return false;
        return true;
        }

    public static int RandomGenerator(int min,int max){
    Random random=new Random();
    return random.Next(min,max);
    }

}

