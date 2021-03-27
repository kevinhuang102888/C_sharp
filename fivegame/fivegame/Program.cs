/*

    請實作 NextStep 函式
    其他地方請勿更改

*/

using System;

namespace gobang
{
    enum Chessboard
    {
        Empty,
        O,
        X
    }
    enum Winner
    {
        No,
        O,
        X
    }
    enum Player
    {
        O,
        X
    }
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //設置棋盤寬度高度
                Console.WriteLine("歡迎來到五子棋遊戲！");
                Console.Write("請輸入棋盤寬度：");
                int width = int.Parse(Console.ReadLine());
                Console.Write("請輸入棋盤高度：");
                int height = int.Parse(Console.ReadLine());
                Chessboard[,] chessboard = new Chessboard[width, height];
                DrawChessboard(chessboard);
               

                //棋盤設置完成，開始遊戲！
                Winner winner = Winner.No;
                Player player = Player.O;
                while (winner == Winner.No)
                {
                    int x, y;
                    while (true)
                    {
                        DrawChessboard(chessboard);
                        Console.WriteLine(chessboard.GetUpperBound(0));
                        Console.WriteLine(chessboard.GetUpperBound(1));
                        Console.WriteLine("輪到 {0} 玩家，請輸入要落子的座標（先輸橫坐標x按enter，再輸入縱座標y按enter）", player);
                        x = int.Parse(Console.ReadLine());
                        y = int.Parse(Console.ReadLine());
                        if (x >= 0 && x < width && y >= 0 && y < height && chessboard[x, y] == Chessboard.Empty)
                            break;
                    }
                    winner = NextStep(player, x, y, chessboard);
                    DrawChessboard(chessboard);
                    player = player == Player.O ? Player.X : Player.O;
                }
                Console.WriteLine(" {0} 玩家獲勝！", winner);

                //是否再玩一次
                char again;
                do
                {
                    Console.Write("再來一場？（請輸入 y/n）");
                    again = char.Parse(Console.ReadLine());
                    Console.Clear();
                } while (again != 'y' && again != 'n');
                if (again == 'n')
                    break;
            }
        }

        static Winner NextStep(Player player, int x, int y, Chessboard[,] chessboard)
        {
            if (player == Player.O)
            {
                chessboard[x, y] = Chessboard.O;
                for(int z = 0; z <= chessboard.GetUpperBound(0)-4; z++)
                {
                    for(int p = 0; p <= chessboard.GetUpperBound(1) -4; p++)
                    {
                        if (chessboard[z, p] == Chessboard.O && chessboard[z + 1, p + 1] == Chessboard.O && chessboard[z + 2, p + 2] == Chessboard.O && chessboard[z + 3, p + 3] == Chessboard.O && chessboard[z + 4, p + 4]== Chessboard.O)
                        {
                            return Winner.O;
                        }
                        else if (chessboard[z, p] == Chessboard.O && chessboard[z , p + 1] == Chessboard.O && chessboard[z , p + 2] == Chessboard.O && chessboard[z, p + 3] == Chessboard.O && chessboard[z, p + 4] == Chessboard.O)
                        {
                            return Winner.O;
                        }
                        else if (chessboard[z, p] == Chessboard.O && chessboard[z + 1, p ] == Chessboard.O && chessboard[z + 2, p ] == Chessboard.O && chessboard[z + 3, p ] == Chessboard.O&& chessboard[z + 4, p ]== Chessboard.O)
                        {
                            return Winner.O;
                        }
                    }
                }
            }
            else
            {
                chessboard[x, y] = Chessboard.X;
                for (int z = 0; z <= chessboard.GetUpperBound(0) - 4; z++)
                {
                    for (int p = 0; p <= chessboard.GetUpperBound(1) - 4; p++)
                    {
                        if (chessboard[z, p] == Chessboard.X && chessboard[z + 1, p + 1] == Chessboard.X && chessboard[z + 2, p + 2] == Chessboard.X && chessboard[z + 3, p + 3] == Chessboard.X && chessboard[z + 4, p + 4] == Chessboard.X)
                        {
                            return Winner.X;
                        }
                        else if (chessboard[z, p] == Chessboard.X && chessboard[z, p + 1] == Chessboard.X && chessboard[z, p + 2] == Chessboard.X && chessboard[z, p + 3] == Chessboard.X && chessboard[z, p + 4] == Chessboard.X)
                        {
                            return Winner.X;
                        }
                        else if (chessboard[z, p] == Chessboard.X && chessboard[z + 1, p] == Chessboard.X && chessboard[z + 2, p] == Chessboard.X && chessboard[z + 3, p] == Chessboard.X && chessboard[z + 4, p] == Chessboard.X)
                        {
                            return Winner.X;
                        }
                    }
                }
            }




            return Winner.No;
        }
        static void DrawChessboard(Chessboard[,] chessboard)
        {
            Console.Clear();
            int width = chessboard.GetLength(0);
            int height = chessboard.GetLength(1);

            Console.Write("   ---");
            for (int j = 1; j < width; j++)
            {
                Console.Write(" ---");
            }
            Console.WriteLine();
            for (int i = 0; i < height; i++)
            {
                i = height - i - 1;
                Console.Write("{0:00}|", i);
                for (int j = 0; j < width; j++)
                {
                    if (chessboard[j, i] == Chessboard.Empty)
                        Console.Write("   |");
                    else if (chessboard[j, i] == Chessboard.O)
                        Console.Write(" O |");
                    else if (chessboard[j, i] == Chessboard.X)
                        Console.Write(" X |");
                }
                Console.WriteLine();
                Console.Write("   ---");
                for (int j = 1; j < width; j++)
                {
                    Console.Write(" ---");
                }
                Console.WriteLine();
                i = height - i - 1;
            }
            Console.Write("   ");
            for (int j = 0; j < width; j++)
            {
                Console.Write(" {0:00} ", j);
            }
            Console.WriteLine();
        }
    }
}