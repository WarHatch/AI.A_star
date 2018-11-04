using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace A_star
{
	public enum Options { Depth, Breadth } //FIXME: Always on default value
	public partial class Form1 : Form
	{
		Square[,] grid;
		List<Square> open;
		List<Square> closed;
		int sourcex; int sourcey;
		int terminalx; int terminaly;
		bool ended;
		const int sleeptime = 100;
		Options option;

		public Form1()
		{
			InitializeComponent();
			grid = new Square[20, 15];
			BlankGrid();
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog()== DialogResult.OK)
			{
				try
				{
					using (StreamReader reader = new StreamReader(openFileDialog1.FileName))
					{
						sourcex = -1; sourcey = -1;
						terminalx = -1; terminaly = -1;
						ended = false;
						open = new List<Square>();
						closed = new List<Square>();
						for (int j = 0; j < 15; j++)
						{
							string[] values = Regex.Split(reader.ReadLine(), ",");
							for (int i = 0; i < 20; i++)
							{
								switch(Convert.ToInt32(values[i]))
								{
									case 0:
										grid[i, j].Type = SquareType.Normal;
										break;
									case 1:
										grid[i, j].Type = SquareType.Obstacle;
										break;
									case 2:
										grid[i, j].Type = SquareType.Source;
										if(sourcex == -1 && sourcey == -1)
										{
											sourcex = i; sourcey = j;
											open.Add(grid[i, j]);
										}
										else
										{
											throw new ArgumentException();
										}
										break;
									case 3:
										if (terminalx == -1 && terminaly == -1)
										{
											grid[i, j].Type = SquareType.Terminal;
											terminalx = i;
											terminaly = j;
										}
										else
										{
											throw new ArgumentException();
										}
										break;
									default:
										throw new ArgumentException();
								}
							}
						}
					}
					if(sourcex == -1 || sourcey == -1 || terminalx == -1 || terminaly == -1)
					{
						throw new Exception();
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
					BlankGrid();
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
					{
						for (int y = 0; y < 15; y++)
						{
							for (int x = 0; x < 19; x++)
							{
								switch(grid[x,y].Type)
								{
									case SquareType.Normal:
										writer.Write("0,");
										break;
									case SquareType.Obstacle:
										writer.Write("1,");
										break;
									case SquareType.Source:
										writer.Write("2,");
										break;
									case SquareType.Terminal:
										writer.Write("3,");
										break;
								}
							}
							switch(grid[19,y].Type)
							{
								case SquareType.Normal:
									writer.WriteLine("0");
									break;
								case SquareType.Obstacle:
									writer.WriteLine("1");
									break;
								case SquareType.Source:
									writer.WriteLine("2");
									break;
								case SquareType.Terminal:
									writer.WriteLine("3");
									break;
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void BlankGrid()
		{
			ended = true;
			for (int x = 0; x < 20; x++)
			{
				for (int y = 0; y < 15; y++)
				{
					grid[x, y] = new Square(x, y);
					this.Controls.Add(grid[x, y].background);
				}
			}
		}
		private void InsertIntoOpen(Square square, int sourcex, int sourcey)
		{
			if(closed.Contains(square) || square.Type == SquareType.Obstacle)
			{
				return;
			}
			if(open.Contains(square))
			{
				if(option == Options.Depth)
				{
					open.Remove(square);
				}
				if(option == Options.Breadth)
				{
					return;
				}
			}		
			for (int i = 0; i < open.Count; i++)
			{
				if (open[i].Weight > square.Weight || option == Options.Depth && open[i].Weight == square.Weight)
				{
					open.Insert(i, square);
					open[i].sourcex = sourcex;
					open[i].sourcey = sourcey;
					return;
				}
			}
			open.Add(square);
			open.Last().sourcex = sourcex;
			open.Last().sourcey = sourcey;
		}

		private void CalculateWeight(Square square)
		{
			if(!square.Calculated && square.Type == SquareType.Normal)
			{
				square.WeightToSource = Math.Abs(square.x - sourcex) + Math.Abs(square.y - sourcey);
				square.WeightToTerminal = Math.Abs(terminalx - square.x) + Math.Abs(terminaly - square.y);
			}
		}

		private void CalculateNeighbors(Square square)
		{
			int x = square.x;
			int y = square.y;
			if (x > 0)
			{
				CalculateWeight(grid[x - 1, y]);
				InsertIntoOpen(grid[x - 1, y], x, y);
			}
			if (y < 14)
			{
				CalculateWeight(grid[x, y + 1]);
				InsertIntoOpen(grid[x, y + 1], x, y);
			}
			if (x < 19)
			{
				CalculateWeight(grid[x + 1, y]);
				InsertIntoOpen(grid[x + 1, y], x, y);
			}
			if (y > 0)
			{
				CalculateWeight(grid[x, y - 1]);
				InsertIntoOpen(grid[x, y - 1], x, y);
			}
		}

		private void advanceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Run(true, false);
		}

		private void runToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Run(false, false);
		}
		private void advanceStepToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Run(false, true);
		}

		private void Run(bool sleep, bool once)
		{
			if (ended)
			{
				MessageBox.Show("Program already ended! Load new scenario if you wish to try again.");
				return;
			}
			do
			{
				if (!open.Any())
				{
					MessageBox.Show("Program failed to find path from source to terminal.");
					ended = true;
					return;
				}
				Square currentSquare = open[0];
				open.Remove(currentSquare);
				closed.Add(currentSquare);
				currentSquare.background.BackColor = Color.LightBlue;
				if (currentSquare.Type == SquareType.Terminal)
				{
					Backtrack(currentSquare, sleep);
					MessageBox.Show(String.Format("Program Suceeded."));
					ended = true;
					return;
				}
				CalculateNeighbors(currentSquare);

				this.Update();
				if (sleep)
				{
					Thread.Sleep(sleeptime);
				}
			} while (!once);
		}

		private void Backtrack(Square currentSquare, bool sleep)
		{
			currentSquare.background.BackColor = Color.LimeGreen;
			this.Update();
			if (sleep)
			{
				Thread.Sleep(sleeptime);
			}
			if (currentSquare.sourcex > -1 || currentSquare.sourcey > -1)
			{
				Backtrack(grid[currentSquare.sourcex, currentSquare.sourcey], sleep);
			}
		}
        
		private void KeyPressEvent(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)32)
			{
				Run(false, true);
			}
		}
    }
}
