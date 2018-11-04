//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace A_star
//{
//    class Logic
//    {
//        Square[,] grid;
//        List<Square> open;
//        List<Square> closed;
//        int sourcex; int sourcey;
//        int terminalx; int terminaly;
//        bool ended;
//        const int sleeptime = 100;
//        Options option;

//        public Logic()
//        {
//            grid = new Square[20, 15];
//            BlankGrid();
//        }

//        public void BlankGrid()
//        {
//            ended = true;
//            for (int x = 0; x < 20; x++)
//            {
//                for (int y = 0; y < 15; y++)
//                {
//                    grid[x, y] = new Square(x, y);
//                    this.Controls.Add(grid[x, y].background);
//                }
//            }
//        }

//        public loadBoard()
//        {
//            using (StreamReader reader = new StreamReader(openFileDialog1.FileName))
//            {
//                sourcex = -1; sourcey = -1;
//                terminalx = -1; terminaly = -1;
//                ended = false;
//                open = new List<Square>();
//                closed = new List<Square>();
//                for (int j = 0; j < 15; j++)
//                {
//                    string[] values = Regex.Split(reader.ReadLine(), ",");
//                    for (int i = 0; i < 20; i++)
//                    {
//                        switch (Convert.ToInt32(values[i]))
//                        {
//                            case 0:
//                                grid[i, j].Type = SquareType.Normal;
//                                break;
//                            case 1:
//                                grid[i, j].Type = SquareType.Obstacle;
//                                break;
//                            case 2:
//                                grid[i, j].Type = SquareType.Source;
//                                if (sourcex == -1 && sourcey == -1)
//                                {
//                                    sourcex = i; sourcey = j;
//                                    open.Add(grid[i, j]);
//                                }
//                                else
//                                {
//                                    throw new ArgumentException();
//                                }
//                                break;
//                            case 3:
//                                if (terminalx == -1 && terminaly == -1)
//                                {
//                                    grid[i, j].Type = SquareType.Terminal;
//                                    terminalx = i;
//                                    terminaly = j;
//                                }
//                                else
//                                {
//                                    throw new ArgumentException();
//                                }
//                                break;
//                            default:
//                                throw new ArgumentException();
//                        }
//                    }
//                }
//            }
//            if (sourcex == -1 || sourcey == -1 || terminalx == -1 || terminaly == -1)
//            {
//                throw new Exception();
//            }
//        }

//        public void InsertIntoOpen(Square square, int sourcex, int sourcey)
//        {
//            if (closed.Contains(square) || square.Type == SquareType.Obstacle)
//            {
//                return;
//            }
//            if (open.Contains(square))
//            {
//                if (option == Options.Depth)
//                {
//                    open.Remove(square);
//                }
//                if (option == Options.Breadth)
//                {
//                    return;
//                }
//            }
//            for (int i = 0; i < open.Count; i++)
//            {
//                if (open[i].Weight > square.Weight || option == Options.Depth && open[i].Weight == square.Weight)
//                {
//                    open.Insert(i, square);
//                    open[i].sourcex = sourcex;
//                    open[i].sourcey = sourcey;
//                    return;
//                }
//            }
//            open.Add(square);
//            open.Last().sourcex = sourcex;
//            open.Last().sourcey = sourcey;
//        }

//        public void CalculateWeight(Square square)
//        {
//            if (!square.Calculated && square.Type == SquareType.Normal)
//            {
//                square.WeightToSource = Math.Abs(square.x - sourcex) + Math.Abs(square.y - sourcey);
//                square.WeightToTerminal = Math.Abs(terminalx - square.x) + Math.Abs(terminaly - square.y);
//            }
//        }

//        public void CalculateNeighbors(Square square)
//        {
//            int x = square.x;
//            int y = square.y;
//            if (x > 0)
//            {
//                CalculateWeight(grid[x - 1, y]);
//                InsertIntoOpen(grid[x - 1, y], x, y);
//            }
//            if (y < 14)
//            {
//                CalculateWeight(grid[x, y + 1]);
//                InsertIntoOpen(grid[x, y + 1], x, y);
//            }
//            if (x < 19)
//            {
//                CalculateWeight(grid[x + 1, y]);
//                InsertIntoOpen(grid[x + 1, y], x, y);
//            }
//            if (y > 0)
//            {
//                CalculateWeight(grid[x, y - 1]);
//                InsertIntoOpen(grid[x, y - 1], x, y);
//            }
//        }

//        public void Backtrack(Square currentSquare, bool sleep)
//        {
//            currentSquare.background.BackColor = Color.LimeGreen;
//            this.Update();
//            if (sleep)
//            {
//                Thread.Sleep(sleeptime);
//            }
//            if (currentSquare.sourcex > -1 || currentSquare.sourcey > -1)
//            {
//                Backtrack(grid[currentSquare.sourcex, currentSquare.sourcey], sleep);
//            }
//        }
//    }
//}
