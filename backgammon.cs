using System;
using System.Collections.Generic;
using System.IO;
class Solution
{
    static List<List<Tuple<int, int, int>>> moves = new List<List<Tuple<int, int, int>>>();  //from, to, priority 
    static int me;
    static int rotate = 4;
    static void Main(String[] args)
    {

        // Processing input

        List<Tuple<int, int>> board = new List<Tuple<int, int>>();
        List<int> dice = new List<int>();
        string line = Console.ReadLine();
        if (line == "1") { me = 1; }
        else { me = 2; }
        for (int i = 0; i < 26; i++)
        {
            line = Console.ReadLine();
            if (line == "0") { board.Add(new Tuple<int, int>(0, 0)); }
            else { board.Add(new Tuple<int, int>(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]))); }
        }
        if (me == 2) { board.Reverse(); }
        line = Console.ReadLine();
        int bar1 = Convert.ToInt32(line);
        line = Console.ReadLine();
        int bar2 = Convert.ToInt32(line);
        line = Console.ReadLine();
        while ((line = Console.ReadLine()) != null)
        {
            dice.Add(Convert.ToInt32(line));
        }
        dice.Sort();
        // Place bar on 0th position

        if (me == 1 && bar1 > 0) { board[0] = new Tuple<int, int>(bar1, me); }
        else if (me == 2 && bar2 > 0) { board[0] = new Tuple<int, int>(bar2, me); }
        else { board[0] = new Tuple<int, int>(0, 0);}
        // Mapping possibilities

        CollectMoves(dice, board, new List<Tuple<int, int, int>>());

            List<int> priorities = new List<int>();

            for (int i = 0; i<moves.Count; i++)
            {
                priorities.Add(0);
                foreach (Tuple<int, int, int> _move in moves[i])
                {
                    priorities[i] += _move.Item3;
                }
            }
            int max = -50000;
            for (int x = 0; x < priorities.Count; x++)
            {
                if (priorities[x] > max) max = priorities[x];
            }
            int maxitem = priorities.IndexOf(max);
            foreach (Tuple<int, int, int> move in moves[maxitem])
            {
                int item1 = Normalize(move.Item1);
                int item2 = Normalize(move.Item2);
                Console.WriteLine(item1 + " " + item2);
            }
    }
    private static int Normalize(int move)
    {
        if (move == 0) return -1;
        else if (me == 2) return 25 - move;
        else return move;

    }
    private static void CollectMoves(List<int> dice, List<Tuple<int, int>> board, List<Tuple<int, int, int>> possibleMoves)
    {
        if (dice.Count == 0 || board[25].Item1 == 15)
        {
            moves.Add(possibleMoves);
        }
        else
        {
            int currentDice = dice[0];
            List<int> diceNew = new List<int>(dice);
            List<Tuple<int, int, int>> possibleMovesNew;
            int from, fromType;
            int to, toType;
            int priority;
            int start = 0;
            for (int i = start; i < 25; i++)
            {
                from = i;
                fromType = GetPointType(me, board[i]);
                if (fromType == 1 || fromType == 2 || fromType == 5)
                {
                    to = from + currentDice;
                    if (to > 24)
                    {
                        if (!CanIGoOut(board)) continue;
                        if (to > 25 && !CanITakeThisOff(from, board)) continue;
                        to = 25;
                    }
                    toType = GetPointType(me, board[to]);
                    if (toType != 3)
                    {
                        priority = GetPriority(from, to, fromType, toType, board);
                        possibleMovesNew = new List<Tuple<int, int, int>>(possibleMoves);
                        possibleMovesNew.Add(new Tuple<int, int, int>(from, to, priority));
                        diceNew = new List<int>(dice);
                        diceNew.RemoveAt(0);
                        CollectMoves(diceNew, BoardMove(from, to, board), possibleMovesNew);
                    }
                 }
                if (AmIOnBar(board[0]))
                {
                    break;
                }
            }
            if (diceNew.Count == dice.Count)
            {
                if (rotate != 0)
                {
                    --rotate;
                    diceNew = new List<int>(dice);
                    diceNew.Add(diceNew[0]);
                    diceNew.RemoveAt(0);
                    CollectMoves(diceNew, board, possibleMoves);
            
                }
                else
                {
                    diceNew = new List<int>(dice);
                    diceNew.RemoveAt(0);
                    CollectMoves(diceNew, board, possibleMoves);
                }
            }
        }
    }
    // 0 nothing
    // 5 mine more than 2
    // 1 mine 2
    // 2 mine only one
    // 3 enemy full
   private static int GetPriority(int from, int to, int fromType, int toType, List<Tuple<int, int>> board)
    {
        int prio = 0;
        if (to == 25)
        {
            prio = 15;
        }
        else if (fromType == 5)
        {
            if (toType == 5)
            {
                prio = 10;
            }
            else if (toType == 1)
            {
                prio = 10;
            }
            else if (toType == 2)
            {
                prio = 13;
                prio = prio - (HowManyEnemies(board, to));
            }
            else if (toType == 4)
            {
                prio = 13;
                prio = prio - (HowManyEnemies(board, to));
            }
            else if (toType == 0)
            {
                prio = 7;
                prio = prio - (HowManyEnemies(board, to));
            }
        }
        else if (fromType == 1)
        {
            if (toType == 5)
            {
                prio = 7;
            }
            else if (toType == 1)
            {
                prio = 7;
            }
            else if (toType == 2)
            {
                prio = 10;
                prio = prio - (HowManyEnemies(board, to));
            }
            else if (toType == 4)
            {
                prio = 10;
                prio = prio - (HowManyEnemies(board, to));
            }
            else if (toType == 0)
            {
                prio = 7;
                prio = prio - (HowManyEnemies(board, to));
            }
        }
        else if (fromType == 2)
        {
            prio = prio + (HowManyEnemies(board, from));
            if (toType == 5)
            {
                prio = 13;
            }
            else if (toType == 1)
            {
                prio = 13;
            }
            else if (toType == 2)
            {
                prio = 16;
                prio = prio - (HowManyEnemies(board, to));
            }
            else if (toType == 4)
            {
                prio = 13;
                prio = prio - (HowManyEnemies(board, to));
            }
            else if (toType == 0)
            {
                prio = 10;
                prio = prio - (HowManyEnemies(board, to));
            }
        }
        if (from < 7) { prio += 10; }
        if (from < 13) { prio += 3; }
        if (from < 19) { prio += 3; }
        return prio;
    }
    private static int HowManyEnemies(List<Tuple<int, int>> board, int to)
    {
        int count = 0;
        for (int i = (to + 1); i < (to + 7) && i < 24; i++) {
            if (board[i].Item2 != me && board[i].Item2 != 0) 
            { 
                ++count;
            }
        }
        return count;
    }
    private static List<Tuple<int, int>> BoardMove(int from, int to, List<Tuple<int, int>> board)
    {
        List<Tuple<int, int>> boardNew = new List<Tuple<int, int>>(board);
        if (boardNew[from].Item1 == 1) { boardNew[from] = new Tuple<int, int>(boardNew[from].Item1 - 1, 0); }
        else { boardNew[from] = new Tuple<int, int>(boardNew[from].Item1 - 1, me); }
        if (boardNew[to].Item2 == me || boardNew[to].Item2 == 0)
        {
            boardNew[to] = new Tuple<int, int>(boardNew[to].Item1 + 1, me);
        }
        else
        {
            boardNew[to] = new Tuple<int, int>(1, me);
        }
        return boardNew;
    }
     private static bool CanITakeThisOff(int from, List<Tuple<int, int>> board)
    {
        for (int i = 19; i < from; i++)
        {
            if (board[i].Item2 == me) { return false; }
        }
        return true;
    }
 	private static bool CanIGoOut(List<Tuple<int, int>> board) 
    {
        for (int i = 0; i < 19; i++)
        {
            if (board[i].Item2 == me) { return false; }
        }
        return true;
    }
    private static bool AmIOnBar(Tuple<int, int> bar) 
    {
        return bar.Item1 > 0;
    }
    // 0 nothing
    // 5 mine more than 2
    // 1 mine 2
    // 2 mine only one
    // 3 enemy full
    // 4 enemy one
    private static int GetPointType(int me, Tuple<int, int> point)
    {
        if (point.Item1 == 0)
        {
            return 0;
        }
        else if (point.Item2 == me && point.Item1 == 2)
        {
            return 1;
        }
        else if (point.Item2 == me && point.Item1 == 1)
        {
            return 2;
        }
        else if (point.Item2 == me && point.Item1 > 2)
        {
            return 5;
        }
        else if (point.Item2 != me && point.Item1 > 1)
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }

}