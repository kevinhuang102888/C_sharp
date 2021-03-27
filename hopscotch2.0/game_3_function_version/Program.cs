using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace game
{
    class Program
    {
        static void Main(string[] args)
        {
            //最初遊戲設定(局數、最初位置)
            int count = 1;
            int[,] Order = new int[2, 2];
            int C_row = 0;
            int C_column = 0;
            int P_row ;
            int P_column;
            int survived_P ;
            int survived_C ;
            string Attacker;
            string Deffender;
            int max_row;
            int max_col;
            int points = 100;
            int limit = 1;
            //遊戲初始化
            set_game(out Attacker, out Deffender, out survived_C, out survived_P, out max_row, out max_col);
            P_row = max_row-1;
            P_column = max_col-1;
            string[,] structure=initial_game(max_row,max_col);
            //遊戲開始
            while (true)
            {
                Console.WriteLine("Run：{0}", count);
                // 電腦隨意跳
                Random rand = new Random();
                int max_row_c = (C_row+1>max_row)?max_row-1: C_row + 1;
                int min_row_c = (C_row -1 < 0) ? 0 : C_row - 1;
                int max_column_c = (C_column+1>max_col)?max_col-1: C_column + 1;
                int min_column_c = (C_column - 1 <0) ? 0 : C_column - 1;
                C_row = rand.Next(min_row_c,max_row_c+1);
                C_column = rand.Next(min_column_c, max_column_c+1) ;
                // 玩家選擇位置
                Input_new_position(ref P_row, ref P_column,max_row,max_col,ref points,ref limit,ref survived_P);
                Console.WriteLine("你輸入的位置({0},{1})", P_row + 1, P_column + 1);
                Console.WriteLine("電腦輸入的位置：({0},{1})", C_row + 1, C_column + 1);
                // 制定誰是攻方、守方，如果是第一局，不用設定
                who_is_attacker(ref Order,  C_row,  C_column,  P_row,  P_column, ref Attacker, ref Deffender, count);
                // 攻守方都換新位置(先消除上一局的紀錄，重新設置位置)
                change_position(ref structure, Order, Attacker, Deffender,max_row,max_col);
                // 判斷是否為同行同列
                if(is_same_position(ref structure, Order, Deffender, max_row, max_col))
                {
                    if (Deffender == "C") --survived_C; 
                    else --survived_P;
                }
                // 判斷遊戲是否結束
                if (Is_END_GAME(ref survived_P, ref survived_C, ref Deffender, ref Attacker,ref count,ref max_col,ref max_col)) break; 
                ++count;
                points += 100;
            }
        }

         static bool Is_END_GAME(ref int survived_P,ref int survived_C,ref string Deffender,ref string Attacker,ref int count,ref int max_row,ref int max_col)
        {
            if (survived_C == 0)
            {   Console.WriteLine("玩家贏了"); 
            }
            else if (survived_P == 0)
            {   Console.WriteLine("電腦贏了");
            }
            else
            {
                //更新遊戲現況
                Console.WriteLine("====================");
                Console.WriteLine("目前是{0}為攻方", Deffender);
                Console.WriteLine("目前玩家還剩下多少命：{0}", survived_P);
                Console.WriteLine("目前電腦還剩下多少命：{0}", survived_C);
                return false;
            }
            Console.Write("是否還想要再玩?(yes/no)");
            bool is_again = Console.ReadLine() == "yes" ? true : false;
            if (is_again)
            {
                Console.WriteLine("====================");
                Console.Clear();
                Console.WriteLine("遊戲重新開始!!!");
                string[,] structure = initial_game(max_row,max_col);
                set_game(out Attacker, out Deffender, out survived_C, out survived_P,out max_row,out max_col);
                count = 0;
                return false;
            }
            else
            {
                Console.WriteLine("====================");
                Console.WriteLine("遊戲結束");
                return true;
            }
        }

        private static void Input_new_position(ref int P_row, ref int P_column,int max_row,int max_col,ref int point,ref int limit,ref int survived_P)
        {
            int pre_row = P_row;
            int pre_col = P_column;
            Console.WriteLine("你目前有{0}個遊戲點數", point);
            Console.WriteLine("你想要用1000點數來買生命藥水(增命)嗎?(yes/no)", point);
            if (Console.ReadLine() == "yes")
            {
                if (point >= 1000)
                {
                    Console.WriteLine("恭喜你多一條命，要繼續加油喔~");
                    survived_P++;
                }
                else { Console.WriteLine("說謊是不好的行為喔~"); }
            }
            Console.WriteLine("你想要用800點數來買彈跳彈簧(多跳一格遠)嗎?(yes/no)", point);
            if (Console.ReadLine() == "yes")
            {
                if (point >= 800)
                {
                    if (limit > 4) { Console.WriteLine("你已經可以跳很遠了，不能再買了QQ"); }
                    else
                    {
                        point -= 800;
                        limit += 1;
                        Console.WriteLine("恭喜你可以跳更遠~");
                    }
                }
                else { Console.WriteLine("再玩幾局，就可以多賺點數。"); }
            }
            //玩家選擇位置
            while (true)
            {
                Console.Write("請輸入想要跳到第幾列(1~{0}列)：",max_row);
                P_row = (int.Parse(Console.ReadLine())) - 1;
                Console.Write("請輸入想要跳到第幾行(1~{0}行)：",max_col);
                P_column = (int.Parse(Console.ReadLine())) - 1;
                if (P_row > max_row-1 | P_row < 0 | P_column>max_col-1|P_column<0)
                {
                    Console.WriteLine("你輸入錯誤，請輸入正確的「數字」範圍");
                    continue;
                }
                if ((P_row > pre_row + limit | P_row < pre_row-limit ) | (P_column > pre_col + limit | P_column < pre_col-limit ))
                {
                    Console.WriteLine("已經超出你能跳的極限了QQ，你可以跳{0}宮格",(limit+2)* (limit + 2));
                    continue;
                }
                break;
            }
        }

        static void who_is_attacker(ref int[,] Order, int C_row, int C_column, int P_row, int P_column, ref string Attacker, ref string Deffender,int count)
        {
            Order[0, 0] = C_row; Order[0, 1] = C_column;
            Order[1, 0] = P_row; Order[1, 1] = P_column;
            if (Attacker == "P"&count>1)
            {
                Attacker = "C"; Deffender = "P";
            }
            else if (Attacker == "C" & count > 1)
            {
                Attacker = "P"; Deffender = "C";
            }
        }

        static void set_game(out string Attacker,out string Deffender, out int survived_C,out int survived_P,out int max_row,out int max_col)
        {
            Console.WriteLine();
            Console.Write("歡迎來到「ㄎㄎ跳格子」");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("【遊戲規則】在M X N的格子中，玩家(P,Player)跟電腦(C,cumputer)進行遊戲，每次跳躍為一局(Run)，玩家須從全部格子中"
              + "，選擇跳到某一格，也可以選擇不跳(原地不動)，其中一方為攻方，另一方為守方，" +
              "當攻方跟守方佔在同行或同列時，則遊戲結束，由攻方贏，輪流攻守交替，直到遊戲結束。");
            Console.WriteLine("【玩家初設定】一開始只能跳九宮格以內，且開始位置為右下角，電腦則為左上角。");
            Console.WriteLine("【遊戲商店】一開始玩家皆有100點數，每一局將增加100點數，目前有「生命藥水」跟「彈跳彈簧」，歡迎採購。");
            Console.WriteLine("【註】 「列」為水平的格子，「行」為垂直的格子。");
            Console.WriteLine(" "); 
            Console.WriteLine("< 遊戲設定 >");
            Console.Write("(1)請問是否想要先攻(yes/no)：");
            if (Console.ReadLine() == "yes")
            {
                Attacker = "P";
                Deffender = "C";
            }
            else
            {
                Attacker = "C";
                Deffender = "P";
            }
            Console.Write("(2)請問想要每一隊有幾條命？(最多4條命)：");
            int survive_count = int.Parse(Console.ReadLine());
            survived_C = survive_count;
            survived_P = survive_count;
            Console.WriteLine("(3)輸入跳格子遊戲的格子");
            Console.Write("row有幾個(輸入數字)：");
            max_row = int.Parse(Console.ReadLine());
            Console.Write("col有幾個(輸入數字)：");
            max_col = int.Parse(Console.ReadLine());
            Console.WriteLine("");
            Console.Clear();
            Console.WriteLine(" < 遊戲開始 >");
            Console.WriteLine(" 首局是{0}為攻方", Attacker);
        }

        static string[,] initial_game(int max_row,int max_col)
        {
            string[,] structure = new string[max_row, max_col];
            for(int i = 0; i < max_row; i++)
            {
                for(int j = 0; j < max_col; j++)
                {
                    structure[i, j] = " ";
                }
            }
            structure[0, 0] = "C";
            structure[max_row - 1, max_col - 1]="P";
            print_gameboard(structure, max_row, max_col);
            return structure;
        }

        static bool is_same_position(ref string[,] structure, int[,] Order, string Deffender,int max_row,int max_col)
        {
            // 判斷攻守方是否為同行同列
            if (Order[1, 0] != Order[0, 0] && Order[1, 1] != Order[0, 1])
            {   
                print_gameboard(structure,max_row,max_col);
                Console.WriteLine("          ");
                Console.WriteLine("請繼續玩，現在換{0}攻。", Deffender);
                return false;
            }
            // case2 守方跟攻方的新位置為同一格
            else if (Order[1, 0] == Order[0, 0] && Order[1, 1] == Order[0, 1])
                {
                    print_gameboard(structure,max_row,max_col);
                    Console.WriteLine("{0}輸了QQ (同一格)", Deffender);
                    return true;
                }
            // case3 守方跟攻方的新位置為同一列或同一行
            else
            {
                print_gameboard(structure, max_row, max_col);
                Console.WriteLine("{0}輸了QQ (同行或同列)", Deffender);
                return true;
            }

        }

        static void change_position(ref string[,] structure, int[,] Order, string attacker, string deffender,int max_row,int max_col)
        {
            structure = new string[max_row, max_col];
            for (int i = 0; i < max_row; i++)
            {
                for (int j = 0; j < max_col; j++)
                {
                    structure[i, j] = " ";
                }
            }
            if (attacker=="C")
            {
                structure[Order[1, 0], Order[1, 1]] = deffender;
                structure[Order[0, 0], Order[0, 1]] = attacker;
            }
            if (attacker == "P")
            {
                structure[Order[0, 0], Order[0, 1]] = deffender;
                structure[Order[1, 0], Order[1, 1]] = attacker;
            }
        }
        static void print_gameboard(string[,] structure,int max_row,int max_col)
        {
            Console.WriteLine();
            Console.Write("  ");
            for (int x = 0; x < max_col; x++)
            {
                Console.Write("{0} ", x+1);
            }
            Console.WriteLine();
            Console.Write(" -");
            for (int x = 0; x < max_col; x++)
            {
                Console.Write("--");
            }
            for (int h = 0; h < max_row; ++h)
            {
                Console.WriteLine();
                Console.Write("{0}", h + 1);
                for (int y = 0; y < max_col; ++y)
                {
                    Console.Write("|{0}", structure[h, y]);
                }
                Console.Write("|");
                Console.WriteLine();
                Console.Write(" -");
                for (int z = 0; z < max_col; z++)
                {
                    Console.Write("--");
                }
            }
            Console.WriteLine();
        }
    }
}