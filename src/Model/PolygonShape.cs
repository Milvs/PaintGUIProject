using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    internal class PolygonShape : Shape
    {
		#region Constructor
		PointF point1 = new PointF();
		PointF point2 = new PointF();
		PointF point3= new PointF();
		PointF point4 = new PointF();
		PointF point5 = new PointF();
		PointF point6 = new PointF();
		PointF[] arrayOfPoints;
		public PolygonShape(PointF p1, PointF p2, PointF p3, PointF p4, PointF p5, PointF p6)
		{
			point1 = p1;
			point2 = p2;
			point3 = p3;
			point4 = p4;
			point5 = p5;
			point6 = p6;
		}		

		public PolygonShape(PolygonShape polygon) : base(polygon)
		{


			polygon.arrayOfPoints = new PointF[] { new PointF(polygon.point1.X, polygon.point1.Y), new PointF(polygon.point2.X, polygon.point2.Y), new PointF(polygon.point3.X, polygon.point3.Y), new PointF(polygon.point4.X, polygon.point4.Y), new PointF(polygon.point5.X, polygon.point5.Y) };
		}

		#endregion

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		/// 



		public override bool Contains(PointF point)
		{
			if (base.Contains(point))
				// Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
				// В случая на правоъгълник - директно връщаме true
				return true;
			else
				// Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
				return false;


		}

		
		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{

			base.DrawSelf(grfx);
			this.Location = this.point1;
			grfx.FillPolygon(new SolidBrush(Color.White), new PointF[] { point1,point2,point3,point4,point5});
			grfx.DrawPolygon(new Pen(StrokeColor, BorderWidth), new PointF[] { point1,point2,point3,point4,point5 });




		}
	}

}

