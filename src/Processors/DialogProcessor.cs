using Draw.src.Model;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Избран елемент.
		/// </summary>
		private List<Shape> selection= new List<Shape>();
		public List<Shape> Selection {
			get { return selection; }
			set { selection = value; }
		}
		
		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}
		private List<Shape> selectionTemp = new List<Shape>();
		public List<Shape> SelectionTemp
		{
			get { return selectionTemp; }
			set { selectionTemp = value; }
		}
		private List<Shape> selectionTemp2 = new List<Shape>();
		public List<Shape> SelectionTemp2
		{
			get { return selectionTemp2; }
			set { selectionTemp2 = value; }
		}
		private List<Shape> selectionC = new List<Shape>();
		public List<Shape> SelectionC
		{
			get { return selectionC; }
			set { selectionC = value; }
		}
		#endregion

		
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = Color.White;
			rect.StrokeColor = Color.Black;
			rect.BorderWidth = 2;

			ShapeList.Add(rect);
		}/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void DrawLine()
		{
			Random rnd = new Random();
			int x1 = rnd.Next(100, 1000);
			int y1 = rnd.Next(100, 600);
			LineShape line = new LineShape(new Rectangle(x1, y1, 300, 300));
			line.StrokeColor= Color.Black;
			line.BorderWidth = 2;
			ShapeList.Add(line);
		}


		public void AddRandomEllipse()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
			ellipse.FillColor = Color.White;
			ellipse.BorderWidth = 2;
			ellipse.StrokeColor = Color.Black;
			

			ShapeList.Add(ellipse);
		}


		public void AddPolygon()
		{
			Random rnd = new Random();
			int firstX = rnd.Next(50, 550);
			int firstY = rnd.Next(150, 500);

			PointF one = new PointF(firstX, firstY);
			PointF two = new PointF(firstX - 30, firstY - 85);
			PointF three = new PointF(firstX + 50, firstY - 140);
			PointF four = new PointF(firstX + 125, firstY - 85);
			PointF five = new PointF(firstX + 100, firstY);
			PointF six = new PointF(firstX, firstY);

			PolygonShape polygon = new PolygonShape(one, two, three, four, five, six);
			polygon.FillColor =Color.White;
			polygon.StrokeColor = Color.Black;
			polygon.BorderWidth = 2;

			ShapeList.Add(polygon);
		}

		public void RotateShape(float rotateAngle)
		{
			if (Selection.Count > 0)
			{
				foreach (Shape item in Selection)
				{
					if (item.GetType().Name == "GroupShape")
					{
						selectionTemp.Add(item);
						foreach (GroupShape gs in SelectionTemp)
						{
							foreach (Shape item2 in gs.SubShapes)
							{
								item2.ShapeAngle = rotateAngle;
								
							}
						}
						selectionTemp.Remove(item);
					}
					else
					{
						item.ShapeAngle = rotateAngle;
						
					}
					
				}
				
			}
		}
		public void AddRandomSquare()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			SquareShape square = new SquareShape(new Rectangle(x, y, 100, 100));

			square.FillColor = Color.White;
			square.StrokeColor = Color.Black;
			square.BorderWidth = 2;
			ShapeList.Add(square);
		}

		public void AddRandomFigFive()
		{

			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			FigFive five = new FigFive(new Rectangle(x, y, 100, 100));


			five.FillColor = Color.White;
			five.StrokeColor = Color.Black;
			five.BorderWidth = 2;
			five.ShapeAngle = 90;

			ShapeList.Add(five);
		}

		public void DeleteSelected()
        {
			foreach(Shape item in selection)
            {
				selectionTemp2.Add(item);
            }
			foreach(Shape item in selectionTemp2)
            {
				selection.Remove(item);
				ShapeList.Remove(item);
            }
			selectionTemp2.Clear();
        }
		public void UnGroupShapes()
		{
			List<Shape> allShapesInGroup = new List<Shape>();
			List<Shape> allShapeMMsInGroup = new List<Shape>();

			foreach (GroupShape groupShape in Selection)
			{
				foreach (var shape in groupShape.SubShapes)
				{
					allShapesInGroup.Add(shape);
				}
				groupShape.SubShapes.Clear();
				allShapeMMsInGroup.Add(groupShape);


				foreach (var shape in allShapesInGroup)
				{
					Selection.Remove(shape);
					ShapeList.Add(shape);
				}
			}
			foreach (Shape item in allShapeMMsInGroup)
			{
				Selection.Remove(item);
			}
			allShapeMMsInGroup.Clear();

		}
		public void GroupShapes()
		{
			//checking if at least 2 shapeMs are selected
			if (Selection.Count < 2) return;

			float minimalX = 10000;
			float minimalY = 10000;
			float maximalX = -10000;
			float maximalY = -10000;
			foreach (var shape in Selection)
			{
				if (maximalX < shape.Location.X + shape.Width)
				{
					maximalX = shape.Location.X + shape.Width;
				}
				if (maximalY < shape.Location.Y + shape.Height)
				{
					maximalY = shape.Location.Y + shape.Height;
				}

				if (minimalX > shape.Location.X)
				{
					minimalX = shape.Location.X;
				}
				if (minimalY > shape.Location.Y)
				{
					minimalY = shape.Location.Y;
				}
			}

			var groupShape = new GroupShape(new RectangleF(minimalX, minimalY, maximalX - minimalX, maximalY - minimalY));
			groupShape.SubShapes= Selection;
			foreach (var shape in Selection)
			{
				ShapeList.Remove(shape);
			}

			Selection = new List<Shape>();
			ShapeList.Add(groupShape);
			Selection.Add(groupShape);

		}
		public void CopySelected()
		{
			selectionC.Clear();
			foreach (Shape item in selection)
			{
				selectionC.Add(item);
			}
			
		}
		public void PasteSelected()
		{
			
			foreach (Shape item in selectionC)
			{
                if (item.GetType().Name == "EllipseShape")
                {
					AddRandomEllipse();
                }
				if (item.GetType().Name == "SquareShape")
				{
					AddRandomSquare();
				}
				if (item.GetType().Name == "RectangleShape")
				{
					AddRandomRectangle();
				}
				if (item.GetType().Name == "GroupShape")
				{
					selectionTemp.Add(item);
					foreach(Shape item2 in selectionTemp)
                    {
						if (item2.GetType().Name == "EllipseShape")
						{
							AddRandomEllipse();
						}
						if (item2.GetType().Name == "SquareShape")
						{
							AddRandomSquare();
						}
						if (item2.GetType().Name == "RectangleShape")
						{
							AddRandomRectangle();
						}
					}
					selectionTemp.Remove(item);
				}
                
			}

		}

		//Serialization method
		public void SerializeFile(object currentObject, string path = null)
		{

			Stream stream;
			IFormatter binaryFormatter = new BinaryFormatter();
			if (path == null)
			{
				stream = new FileStream("DrawFile.asd", FileMode.Create, FileAccess.Write, FileShare.None);
			}
			else
			{
				string preparePath = path + ".asd";
				stream = new FileStream(preparePath, FileMode.Create);

			}
			binaryFormatter.Serialize(stream, currentObject);
			stream.Close();
		}


		//Deserialization method
		public object DeSerializeFile(string path = null)
		{
			object currentObject;

			Stream stream;
			IFormatter binaryFormatter = new BinaryFormatter();
			if (path == null)
			{
				stream = new FileStream("DrawFile.asd", FileMode.Open);

			}
			else
			{
				stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
			}
			currentObject = binaryFormatter.Deserialize(stream);
			stream.Close();
			return currentObject;
		}

		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
				
						
					return ShapeList[i];
				}	
			}
			return null;
		}
		
		/// <summary>
		/// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
		/// </summary>
		/// <param name="p">Вектор на транслация.</param>
		public void TranslateTo(PointF p)
		{
			if (selection.Count>0)
			{
				foreach(Shape item in Selection)
                {
                    Console.WriteLine(item.GetType().ToString());
					item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);
                    if (item.GetType().Name == "GroupShape")
                    {
						selectionTemp.Add(item);
						foreach (GroupShape gs in SelectionTemp)
						{
							foreach (Shape item2 in gs.SubShapes)
							{
								item2.Location = new PointF(item2.Location.X + p.X - lastLocation.X, item2.Location.Y + p.Y - lastLocation.Y);

							}
						}
						selectionTemp.Remove(item);
					}
				}
				
				lastLocation = p;
			}
		}

		public void TranslateToPoint(PointF p,int n,int j,int g)
		{
			

			if (selection.Count > 0)
			{
				if(selection[g].GetType().Name != "GroupShape")
                {
					for (int i = n; i < j; i++)
					{
						selection[i].Location = new PointF(selection[i].Location.X + p.X - lastLocation.X, selection[i].Location.Y + p.Y - lastLocation.Y);

					}
				}
				else
                {
					foreach(GroupShape gs in selectionTemp)
                    {
						for (int i = n; i < j; i++)
						{
							gs.SubShapes[i].Location = new PointF(gs.SubShapes[i].Location.X + p.X - lastLocation.X,
								gs.SubShapes[i].Location.Y + p.Y - lastLocation.Y);

						}
					}
					
				}					
				
				lastLocation = p;
			}
		}

		public override void DrawShape(Graphics grfx, Shape item)
        {
            base.DrawShape(grfx, item);

            if (selection.Contains(item))
            {
				grfx.DrawRectangle(new Pen(Color.Gray),
					item.Location.X-3,
					item.Location.Y-3,
					item.Width+6,
					item.Height+6);
				
			}

		}
		public void SelectAll()
		{
			foreach (Shape item in ShapeList)
			{
				if (!selection.Contains(item))
				{
					selection.Add(item);
				}
			}
		}
		public void DeSelectAll()
		{
			foreach (Shape item in ShapeList)
			{
				if (selection.Contains(item))
				{
					selection.Remove(item);
				}
			}
		}
		public void Scale(int w,int h,int n,int j,int g)
		{
			int m = j;
			for (int i=n ; i < j; i++)
			{
				if(selection[g].GetType().Name!="GroupShape")
                {
					selection[i].Width = w;
					selection[i].Height = h;
                }
                else
                {


					selectionTemp2.Add(selection[g]);
						foreach (GroupShape gs in SelectionTemp)
						{
							for (int p = n; p < m; p++)
							{
								gs.SubShapes[p].Width = w;
								gs.SubShapes[p].Height = h;
							
							}
						}
					selectionTemp2.Remove(selection[g]);
					
					
					
				}
				
			}
			
		}

	}
}
