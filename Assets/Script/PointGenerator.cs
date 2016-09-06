using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointGenerator {

	/// <summary>
	/// Creates the random distributed points inside of a rectangle.
	/// </summary>
	/// <returns>List of points.</returns>
	/// <param name="n">Number of points.</param>
	/// <param name="maxX">Half the width of the rectangle.</param>
	/// <param name="maxY">Half the height of the rectangle.</param>
	public static List<Vector2> createPointInRectangle (int n, float maxX, float maxY)
	{
		List<Vector2> points = new List<Vector2> ();

		float x, y;

		for (int i = 0; i < n; i++) {
			x = Random.Range(-maxX, maxX);
			y = Random.Range(-maxY, maxY);

			points.Add(new Vector2(x, y));
		}

		return points;
	}

	/// <summary>
	/// Creates the random distributed points in a circle.
	/// If used than golin's algorithm won't be as usefull, but we can expect better performances from don't useing it.
	/// </summary>
	/// <returns>List of points.</returns>
	/// <param name="n">Number of points.</param>
	/// <param name="r">Radius of the circle.</param>
	public static List<Vector2> createPointInCircle (int n, float r)
	{
		List<Vector2> points = new List<Vector2> ();

		for (int i = 0; i < n; i++) {
			points.Add((Random.insideUnitCircle) * r);
		}

		return points;
	}
	
}
