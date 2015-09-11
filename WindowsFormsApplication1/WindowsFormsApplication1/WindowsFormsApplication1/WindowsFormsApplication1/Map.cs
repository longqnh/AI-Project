using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Map : UserControl
    {
       const int OFFSET = 21;
       float _cellSize;
       Square[,] _cells;
       public List<Point> _path;
       List<Point> wayrun;
       public Point Goal,blackhole;
       
        #region Properties

        public Point Oct1Position
        {
            get;
          set;
        }
        private int _mapSize;
        public int MapSize
        {
            get { return _mapSize; }
            set
            {
                _mapSize = 33;
                InitMap();
                OnResize(null);
            }
        }
        private int _numOfStones;
        public int NumOfStones
        {
            get { return _numOfStones; }
            set
            {
                Random rnd = new Random();
                _numOfStones = rnd.Next(170,250);
                InitMap();
            }
        }
        #endregion

        public Map()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void ShowPath()
        {
          
            Cursor.Current = Cursors.WaitCursor;
            Oct1Position = new Point(Form2.last.X,Form2.last.Y);
            _path = AI.astar(_cells, Oct1Position,Goal.X, Goal.Y);
            wayrun = AI.runpath(_path, _cells, Goal);

            if (_path != null)
            {
                foreach (var item in _path)
                {
                    _cells[item.X, item.Y].Marked = true;

                }
                timer1.Start();
            }
            Invalidate();
        }

        public void InitMap()
        {
            //timer1.Stop();
           
            Oct1Position = new Point(Form2.last.X,Form2.last.Y);
            _cells = new Square[MapSize, MapSize];

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    _cells[i, j] = new Square();
                    _cells[i, j].Marked = false;
                }
            }
            int count = 0;
            if (_numOfStones >= MapSize * MapSize - 8)
                _numOfStones = MapSize * MapSize - 8;
            Random rnd = new Random();
            while (count < NumOfStones)
            {
                int r = rnd.Next(MapSize);

                int c = rnd.Next(MapSize);
                if (r + c <= 2)
                    continue;
                if (_cells[r, c].State != SquareState.Stone)
                {
                    Console.WriteLine(r + "   " + c);
                    _cells[r, c].State = SquareState.Stone;
                    count++;
                }
            }
            while (true)
            {
                int r = rnd.Next(MapSize);
                int c = rnd.Next(MapSize);

                if (r + c <= 2)
                    continue;
                if (_cells[r, c].State == SquareState.Stone)
                    continue;

                _cells[r, c].State = SquareState.Oct1;
                break;
            }
            while (true)
            {
                int r = rnd.Next(MapSize);
                int c = rnd.Next(MapSize);

                if (r + c <= 2)
                    continue;
                if (_cells[r, c].State == SquareState.Stone ||
                    _cells[r, c].State == SquareState.Oct1)
                    continue;
                {
                    _cells[r, c].State = SquareState.Oct2;
                    Goal.X = r;
                    Goal.Y = c;
                
                };
                break;
            }
            Random rand;
            rand = new Random();
            blackhole.X = rand.Next(0, 32);
            blackhole.Y = rand.Next(0, 32);
            while(blackhole.X==Oct1Position.X && blackhole.Y==Oct1Position.Y &&_cells[blackhole.X,blackhole.Y].State!=SquareState.None)
            {
                blackhole.X = rand.Next(0, 32);
                blackhole.Y = rand.Next(0, 32);
            }
            //Oct1Position = new Point();
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {

            if (MapSize == 0)
                MapSize = 4;
            if (this.Width < this.Height)
                this.Height = this.Width;
            else
                this.Width = this.Height;

            _cellSize = (this.Width - OFFSET) / this.MapSize;

            base.OnResize(e);
        }
         protected override void OnPaint(PaintEventArgs e)
        {
            _cells[0, 0].Marked = false;
            e.Graphics.DrawLine(Pens.Black, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(Pens.Black, 0, 0, 0, this.Height);
            e.Graphics.FillRectangle(Brushes.LightSeaGreen, OFFSET, OFFSET, this.Width, this.Height);

            for (int i = 0; i < this.MapSize; i++)
            {

                float left = i * _cellSize + OFFSET;
                SizeF size = e.Graphics.MeasureString("S", this.Font);
                float tLeft = (_cellSize - size.Width) / 2;

                e.Graphics.DrawString(i.ToString(), this.Font, Brushes.Black, left + tLeft, 2);
                e.Graphics.DrawString(i.ToString(), this.Font, Brushes.Black, 2, left + tLeft);

                if (_cells != null)
                {
                    for (int j = 0; j < this.MapSize; j++)
                    {
                        float top = j * _cellSize + OFFSET;
                        if (_cells[i, j].Marked)
                        {
                            e.Graphics.FillRectangle(Brushes.LightSeaGreen, new RectangleF(left, top + 1, _cellSize, _cellSize - 1));
                        }

                        if (_cells[i, j].State == SquareState.Stone && i!=blackhole.X && j!=blackhole.Y)
                        {
                            e.Graphics.DrawImage(Properties.Resources.stone, new RectangleF(left, top, _cellSize, _cellSize));
                        }
                        if(i==blackhole.X && j==blackhole.Y)
                        {
                            e.Graphics.FillRectangle(Brushes.Black, new RectangleF(left, top + 1, _cellSize, _cellSize - 1));
                        }

                    }
                    e.Graphics.DrawLine(Pens.Black, left, 0, left, this.Height);
                    e.Graphics.DrawLine(Pens.Black, 0, left, this.Width, left);
                }
                e.Graphics.DrawLine(Pens.Black, this.Width - 1, 0, this.Width - 1, this.Height - 1);
                e.Graphics.DrawLine(Pens.Black, 0, this.Height - 1, this.Width - 1, this.Height - 1);
                e.Graphics.DrawImage(Properties.Resources.bt, new RectangleF(
                    Oct1Position.X * _cellSize + OFFSET, Oct1Position.Y * _cellSize + OFFSET, _cellSize, _cellSize));
                e.Graphics.DrawImage(Properties.Resources.photo_php, new RectangleF(
                    Goal.X * _cellSize + OFFSET, Goal.Y * _cellSize + OFFSET, _cellSize, _cellSize));
                base.OnPaint(e);
            };
            
        }
     
      
        private void timer1_Tick(object sender, EventArgs e)
         {
             Random rnd;
             Point temp;
            temp= new Point();
            _cells[0, 0].Marked = false;
            if (Goal.X == Oct1Position.X && Goal.Y == Oct1Position.Y)
            {
                timer1.Stop();
                Form2.check = true;
                MessageBox.Show("MISSION FAILED", "GAME OVER");
                return;
            }
            else
            {
                if (Form2.time == 0) timer1.Stop();
            }
            if (_path == null || _path.Count == 0) { return; }; 
            Point p = _path[_path.Count - 1];
            _path.RemoveAt(_path.Count - 1);
            Oct1Position = p;
          
            if (Form2.b5 == true)  // nếu người dùng ấn nút up hoặc W/w
            {
                if(Goal.Y>0 )
                if (_cells[Goal.X, Goal.Y-1].State != SquareState.Stone)
                {
                    Goal.Y -= 1;
                    Oct1Position = Form2.last; // lưu lại vị trí bạch tuộc 1 để vẽ map mới 
                    ShowPath();
                    
                };
                Form2.b5 = false; // vì nỗi nút có giá trị 1 lần di chuyển nên phải gán giá trị nút về lại false sau khi di chuyển 
            }
            if (Form2.left == true)// nếu người dùng ấn nút left hoặc A/a
            {
                if ( Goal.X > 0 )
                    if(_cells[Goal.X-1, Goal.Y ].State != SquareState.Stone)
                {
                    Goal.X -= 1;
                    Oct1Position = Form2.last;
                    ShowPath();

                };
                Form2.left = false; // tương tự 
            }
            if (Form2.right == true)// nếu người dùng ấn nút right hoặc D/d
            {
                if(Goal.X < 32)
                if (_cells[Goal.X + 1, Goal.Y].State != SquareState.Stone   )
                {
                    Goal.X += 1;
                    Oct1Position = Form2.last;
                    ShowPath();

                };
                Form2.right = false; // tương tự 
            }
            if (Form2.down == true) // nếu người dùng ấn nút right hoặc A/a
            {
                if (Goal.Y < 32)
                    if (_cells[Goal.X , Goal.Y+1].State != SquareState.Stone)
                    {
                        Goal.Y += 1;
                        Oct1Position = Form2.last;
                        ShowPath();

                    };
                Form2.down= false; // tương tự 
            }
          
            if (Goal.X == blackhole.X && Goal.Y == blackhole.Y) // nếu bạch tuộc 2 đi vào lỗ đen 
            {
                rnd = new Random();
                Goal.X = rnd.Next(0, 32);
                Goal.Y = rnd.Next(0, 32);
                while (Goal == Oct1Position || _cells[Goal.X, Goal.Y].State == SquareState.Stone)
                {
                    Goal.X = rnd.Next(0, 32);
                    Goal.Y = rnd.Next(0, 32);
                }
            }
            if (Oct1Position.X == blackhole.X && Oct1Position.Y == blackhole.Y) // nếu bạch tuộc 1 đi vào lỗ đen 
            {
                rnd = new Random();
                temp.X = rnd.Next(0, 32);
                temp.Y = rnd.Next(0, 32);
              
                while (Goal == Oct1Position || _cells[Goal.X, Goal.Y].State == SquareState.Stone)
                {
                    temp.X = rnd.Next(0, 32);
                    temp.Y = rnd.Next(0, 32);
                }
                Oct1Position = temp;
            }
            Invalidate();
        }
    }
}
