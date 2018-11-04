using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace A_star
{
	enum SquareType { Normal, Obstacle, Source, Terminal }
	class Square
	{
		public int x { get; private set; }
		public int y { get; private set; }
		public int sourcex;
		public int sourcey;

		public PictureBox background;
		Label weightLabelShort;
		Label weightLabelLong;
		SquareType type;
		public bool Calculated { get; private set; }
		int weighttosource;
		int weighttoterminal;
		public int step;

		public SquareType Type
		{
			get { return type; }
			set
			{
				type = value;
				sourcex = -1;
				sourcey = -1;
				switch(value)
				{
					case SquareType.Normal:
						Calculated = false;
						background.BackColor = Color.White;
						weightLabelShort.Text = "";
						weightLabelLong.Text = "";
						step = 0;
						break;
					case SquareType.Obstacle:
						background.BackColor = Color.Black;
						weightLabelShort.Text = "";
						weightLabelLong.Text = "";
						step = 0;
						break;
					case SquareType.Source:
						background.BackColor = Color.Blue;
						weightLabelShort.Text = "S";
						weightLabelLong.Text = "";
						step = 1;
						break;
					case SquareType.Terminal:
						background.BackColor = Color.Green;
						weightLabelShort.Text = "F";
						weightLabelLong.Text = "";
						step = 0;
						break;
				}
			}
		}

		public int Weight
		{
			get { return weighttosource + weighttoterminal; }
		}
		public int WeightToSource
		{
			get { return weighttosource; }
			set
			{
				weighttosource = value;
				Calculate();
			}
		}
		public int WeightToTerminal
		{
			get { return weighttoterminal; }
			set
			{
				weighttoterminal = value;
				Calculate();
			}
		}


		public Square(int x, int y)
		{
			this.x = x;
			this.y = y;
			sourcex = -1;
			sourcey = -1;
			int xCoord = 50 + (x * 40);
			int yCoord = 30 + (y * 40);

			background = new PictureBox();
			background.BackColor = Color.White;
			background.BorderStyle = BorderStyle.FixedSingle;
			background.Location = new System.Drawing.Point(xCoord, yCoord);
			background.Name = String.Format("pictureBox{0}_{1}", xCoord, yCoord);
			background.Size = new System.Drawing.Size(40, 40);
			background.TabIndex = 0;
			background.TabStop = false;


			weightLabelShort = new Label();
			weightLabelShort.Parent = background;
			weightLabelShort.BackColor = System.Drawing.Color.Transparent;
			weightLabelShort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			weightLabelShort.Name = String.Format("labelShort{0}_{1}", xCoord, yCoord);
			weightLabelShort.Size = new System.Drawing.Size(30, 20);
			weightLabelShort.Location = new System.Drawing.Point(5, 5);
			weightLabelShort.TabIndex = 1;
			weightLabelShort.Text = "";

			weightLabelLong = new Label();
			weightLabelLong.Parent = background;
			weightLabelLong.BackColor = System.Drawing.Color.Transparent;
			weightLabelLong.Name = String.Format("labelLong{0}_{1}", xCoord, yCoord);
			weightLabelLong.Size = new System.Drawing.Size(36, 13);
			weightLabelLong.Location = new System.Drawing.Point(2, 27);
			weightLabelLong.TabIndex = 1;
			weightLabelLong.Text = "";
		}

		private void Calculate()
		{
			if (weighttosource > 0 && weighttoterminal > 0)
			{
				//weightLabelLong.Text = String.Format("{0}+{1}", weighttosource, weighttoterminal);
				weightLabelShort.Text = String.Format("{0}", weighttosource + weighttoterminal);
				Calculated = true;
			}
		}
	}
}
