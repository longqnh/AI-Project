using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    enum SquareState
    {
        None, Stone, Oct1, Oct2
    }

    struct Square
    {
        public SquareState State { get; set; }
        public bool Marked { get; set; }
    }
    public struct pos
    {
        public int x, y, f, h, g;

        public int xpre, ypre;
    };
    class AI
    {

        static void h(ref pos k, int x, int y)
        {
            k.h = Math.Max(Math.Abs(k.x - x), Math.Abs(k.y - y));
        }
        public static List<Point> astar(Square[,] cost, Point star ,int xgoal, int ygoal)
        {

            int i, j, next = 0, r, n, m, dem1, dem2, co = -1, cc = -1, ci;//co mean checkopen cc mean checkclose
            int xstart = star.X, ystart = star.Y;
            m = cost.GetLength(0);
            n = cost.GetLength(1);
            bool found = false;
            int[] dx = new int[] { 0, -1, -1, -1, 0, 1, 1, 1 };
            int[] dy = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };
            List<pos> open, close;
            pos temp, temp2;
            open = new List<pos>();
            close = new List<pos>();
            temp = new pos();
            temp.x = xstart;
            temp.y = ystart;
            temp.f = 0;
            temp.g = 0;
            open.Add(temp);
            while (open.Count != 0)
            {
                for (i = 0; i < open.Count; i++) if (open[i].f < open[next].f) next = i;// choicing best
                //-- next is current pos
                if (open[next].x == xgoal && open[next].y == ygoal)
                {
                    found = true;
                    break;
                };
                for (r = 0; r < 8; r++)
                {

                    i = open[next].x + dx[r];
                    j = open[next].y + dy[r];

                    if (i >= 0 && j >= 0 && i < n && j < m)
                        if (cost[i, j].State == SquareState.None || cost[i, j].State == SquareState.Oct2)
                        {
                            for (dem1 = 0; dem1 < open.Count; dem1++) if (open[dem1].x == i && open[dem1].y == j) co = dem1;// check pos x in list open
                            for (dem2 = 0; dem2 < close.Count; dem2++) if (close[dem2].x == i && close[dem2].y == j) cc = dem2;// check pos y in close;
                            temp.x = i;
                            temp.y = j;
                            h(ref temp, xgoal, ygoal);
                            if (co != -1)
                            {
                                if (open[co].f > temp.h + open[next].g + 1)
                                {
                                    temp.g = open[next].g + 1;
                                    temp.f = temp.h + temp.g;
                                    temp.xpre = open[next].x;
                                    temp.ypre = open[next].y;

                                    open[co] = temp;
                                };

                            }
                            else
                                if (cc == -1) // chua có trong open và close
                                {
                                    temp.g = open[next].g + 1;
                                    temp.f = temp.h + temp.g;
                                    temp.xpre = open[next].x;
                                    temp.ypre = open[next].y;

                                    open.Add(temp);
                                }
                                else// n?u có trong close 
                                    if (close[cc].f > temp.h + open[next].g + 1)
                                    {
                                        temp.g = open[next].g + 1;
                                        temp.f = temp.h + temp.g;
                                        temp.xpre = open[next].x;
                                        temp.ypre = open[next].y;

                                        open.Add(temp);
                                        close.RemoveAt(cc);
                                    };


                        }

                    co = -1;
                    cc = -1;

                };
                close.Add(open[next]);
                open.RemoveAt(next);
                next = 0;

            }
            List<Point> path;
            path = new List<Point>();
            path.Add(new Point(0, 0));
            Point tg;
            tg = new Point();
            if (found)
            {

                temp2 = open[next];
                while (temp2.x != xstart || temp2.y != ystart)
                {
                    tg.X = temp2.x;
                    tg.Y = temp2.y;
                    path.Add(tg);
                    for (i = 0; i < close.Count; i++)
                    {
                        if (temp2.xpre == close[i].x && temp2.ypre == close[i].y)
                        {
                            temp2 = close[i];
                            break;
                        }
                    }
                }
                return path;
            }
            else return null;

        }
        static void vl(Point oct2, int x, int y, ref double kq)
        {
            kq = Math.Sqrt((oct2.X - x) * (oct2.X - x) + (oct2.Y - y) * (oct2.X - y));
        }
        public  static void run(Square[,] cost, Point oct1, Point oct2,ref Point kq)
        {
       
            kq = new Point();
            int[] dx = new int[] { 0, -1, -1, -1, 0, 1, 1, 1 };
            int[] dy = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };
            int i, j, r, n, m;
            double x = 1, lastk = -1;
            m = cost.GetLength(0);
            n = cost.GetLength(1);
            for (r = 0; r < 8; r++)
            {
                i = oct2.X + dx[r];
                j = oct2.Y + dy[r];
                if (i >= 0 && j >= 0 && i < n && j < m)
                    if (cost[i, j].State == SquareState.None && cost[i, j].State != SquareState.Oct1)
                    {
                        vl(oct1, i, j, ref x);
                        if (x > lastk) { lastk = x; kq.X = i; kq.Y = j; };
                    }
            }
            
        }

        public static List<Point> runpath(List<Point> path,Square[,] cost ,Point oct )
        {
            List<Point> kq;
            kq = new List<Point>();
            Point temp;
            temp= new Point();
            temp = oct;
            kq.Add(temp);
            if(path!=null)
            foreach (var item in path)
            {
                run(cost, item, kq[kq.Count-1],ref temp);
                kq.Add(temp);
             
            }
       
            return kq;
        }
    }
}