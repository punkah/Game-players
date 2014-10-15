using System;
using System.Collections.Generic;
using System.IO;
class Solution {
    static void Main(String[] args) {
        string line = Console.ReadLine();
        if (line == "INIT"){
            Console.WriteLine("9 8");
            Console.WriteLine("9 7");
            Console.WriteLine("0 5:1 5");
            Console.WriteLine("0 4:1 4");
            Console.WriteLine("0 3:2 3");
            Console.WriteLine("0 2:3 2");
            Console.WriteLine("0 1:4 1");
        }
        else if (line != null)
        {
            char[,] matrix = new char[10,10];
            int x,y;
            x = 0;
            for(x=0;x<10;x++)
            {
                line = Console.ReadLine();
                {
                    for(y=0;y<10;y++)
                    {
                        matrix[x,y] = line[y];
                    }
                }
            }
            for (x = 0; x < 10; x++)
            {
                for (y = 0; y < 10; y++)
                {
                    if (matrix[x, y] == 'h')
                    {
                        int dir = HasNeighbourH(x, y, matrix);
                        if (GuessNeighbourOfH(x, y, matrix, dir)==1)
                        {
                            return;
                        }
                    }
                }
            }
            int[,] count = new int[10,10];
            
            
            for (x = 0; x < 10; x++)
            {
               for (y = 0; y < 10; y++)
                {
                    if (matrix[x, y] == '-') 
                    {
                        int len1 = GetLength(x,y,matrix,1);
                        int len2 = GetLength(x,y,matrix,2);
                        int len3 = GetLength(x,y,matrix,3);
                        int len4 = GetLength(x,y,matrix,4);
                        count[x,y]=len1*len3*len2*len4;
                    }   
                }
            }
            int max = 1;
            for (x = 0; x < 10; x++) 
            {
                for (y = 0;y < 10; y++) 
                {
                   if(count[x,y] > max) max = count[x,y]; 
                }
            }
            var list = new List<Tuple<int, int>>();
            for (x = 0; x < 10; x++) 
            {
                for (y = 0;y < 10; y++) 
                {
                    if (max == count[x,y])
                    {
                       list.Add(new Tuple<int,int>(x,y));
                    }
                }
            }
            Random r = new Random();
            int next = r.Next(list.Count);
            Tuple<int,int> t = list[next];
            GuessCell(t.Item1,t.Item2);
            
        }
    }
 
    static int HasNeighbourH(int x, int y, char[,] matrix)
    {
        if (GetCell(x + 1, y, matrix) == ('h')) return 1;
        else if (GetCell(x, y + 1, matrix) == ('h')) return 2;
        else if (GetCell(x - 1, y, matrix) == ('h')) return 1;
        else if (GetCell(x, y - 1, matrix) == ('h')) return 2;
        else return 0;
    }
    static int GetNeighbourM(int x, int y, char[,] matrix)
    {
        int i = 0;
        if (GetCell(x + 1, y, matrix) == ('m')) i++;
        if (GetCell(x, y + 1, matrix) == ('m')) i++;
        if (GetCell(x - 1, y, matrix) == ('m')) i++;
        if (GetCell(x, y - 1, matrix) == ('m')) i++;
        return i;
    }
    static int GuessNeighbourOfH(int x, int y, char[,] matrix, int dir)
    {
        if (dir == 1)
        {
            if (GetCell(x + 1, y, matrix) == ('-')) {GuessCell(x + 1, y); return 1;}
            if (GetCell(x - 1, y, matrix) == ('-')) {GuessCell(x - 1, y); return 1;}
        }
        else if (dir == 2)
        {
            if (GetCell(x, y - 1, matrix) == ('-')) {GuessCell(x, y - 1);return 1;}
            if (GetCell(x, y + 1, matrix) == ('-')) {GuessCell(x, y + 1);return 1;}
            
        }
        if (GetCell(x + 1, y, matrix) == ('-')) {GuessCell(x + 1, y);return 1;}
        if (GetCell(x - 1, y, matrix) == ('-')) {GuessCell(x - 1, y);return 1;}
        if (GetCell(x, y - 1, matrix) == ('-')) {GuessCell(x, y - 1);return 1;}
        if (GetCell(x, y + 1, matrix) == ('-')) {GuessCell(x, y + 1);return 1;}
        return 0;
    }
    static void GuessCell(int x, int y)
    {
        Console.WriteLine(x + " " + y);
    }
 
    static char GetCell(int x, int y, char[,] matrix)
    {
        if (x >= 0 && x < 10 && y >= 0 && y < 10) return matrix[x, y];
        else return 'e';
    }
    static int GetLength(int x, int y, char[,] matrix, int dir)
    {
      if(dir == 1 && GetCell(x+1,y,matrix)=='-') return (GetLength(x+1,y,matrix,1) + 1);
      else if(dir == 2 && GetCell(x,y+1,matrix)=='-') return (GetLength(x,y+1,matrix,2) + 1);
      else if(dir == 3 && GetCell(x-1,y,matrix)=='-') return (GetLength(x-1,y,matrix,3) + 1);
      else if(dir == 4 && GetCell(x,y-1,matrix)=='-') return (GetLength(x,y-1,matrix,4) + 1);
      else return 1;
    }
}