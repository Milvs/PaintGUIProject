using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw.src.Model
{
	[Serializable]
	public class LineShape : Shape
	{
		public LineShape(RectangleF line) : base(line)
		{
		}
		public LineShape(RectangleShape line2) : base(line2) { }

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		/// 

		//проверява дали п3 лежи на пътеката между п1 и п2
		bool IsOnTheLine(PointF p1, PointF p2, PointF p3, int width = 5) {
			using (var path = new GraphicsPath())
			{
				using (var pen = new Pen(Brushes.Black, width))
				{
					path.AddLine(p1, p2);
					return path.IsOutlineVisible(p3, pen);
				}
			}
		}
		public override bool Contains(PointF p)
		{
			if (base.Contains(p)) { 
			float varOne = Width / 2;
			float varTwo = Height / 2;
			float x1 = Location.X + varOne;
			float y1 = Location.Y + varTwo;
			bool isItOnTheLine = Math.Pow((p.X - x1) / varOne, 2) + Math.Pow((p.Y - y1) / varTwo, 2) - BorderWidth <= 0;
			return isItOnTheLine;
		}else{
				return false;
		}
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			Pen pen = new Pen(StrokeColor, BorderWidth);
			base.DrawSelf(grfx);
			base.RotateShape(grfx);
			PointF point1 = new PointF(Rectangle.X, Rectangle.Y);
			PointF point2 = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Width);
			grfx.DrawLine(pen, point1, point2);	
			grfx.ResetTransform();

		}
	}
}


