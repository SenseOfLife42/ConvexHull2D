using UnityEngine;
using System.Collections;

public class Utility {

	/// <summary>
	/// Tolerance.
	/// </summary>
	private static float epsilon;

	/// <summary>
	/// Checks if a vertex is at left of a given edge.
	/// </summary>
	/// <returns><c>true</c>, if vertex is at the left, <c>false</c> otherwise.</returns>
	/// <param name="v">Vertex.</param>
	/// <param name="e">Edge.</param>
	public static bool atLeft (Vertex v, Edge e)
	{
		return ccw(e.Head(), e.Next().Head(), v) > epsilon;
	}

	/// <summary>
	/// Checks if a vertex is at right of a given edge..
	/// </summary>
	/// <returns><c>true</c>, if vertex is at the right, <c>false</c> otherwise.</returns>
	/// <param name="v">Vertex.</param>
	/// <param name="e">Edge.</param>
	public static bool atRight (Vertex v, Edge e)
	{
		return ccw(e.Head(), e.Next().Head(), v) < -epsilon;
	}

	/// <summary>
	/// Checks if a vertex is inside of a closed polygon given an edge of it.
	/// If a vertex is at left of all his edges than it's inside of the polygon.
	/// </summary>
	/// <param name="v">Vertex.</param>
	/// <param name="e0">Starting edge.</param>
	public static bool inside (Vertex v, Edge e0)
	{
		Edge e = e0;
		bool inside = true;
		do {
			inside = atLeft(v, e);
			e = e.Next();
		} while (inside && (e != e0));
		return inside;
	}

	/// <summary>
	/// Assign the specified vertex to a face of a polygon. Needs to be sure that the veretx is outside the polygon.
	/// </summary>
	/// <param name="v">Vertex.</param>
	/// <param name="e0">Starting edge.</param>
	public static void assign (Vertex v, Edge e0)
	{
		Edge e = e0;
		bool assigned = false;
		do {
			assigned = atRight(v, e);
			if (assigned) {
				v.setState(Vertex.State.processed);
				e.addToOutSet(v);
			}
			e = e.Next();
		} while (!assigned);
	}

	/// <summary>
	/// Finds the furthest vertex from an edge's outside set to that edge.
	/// </summary>
	/// <returns>Furthest vertex.</returns>
	/// <param name="e">Edge.</param>
	public static Vertex furthestFrom (Edge e)
	{
		Vertex EP = e.OutSet () [0];
		float maxA = Mathf.Abs (ccw (e.Head (), e.Next ().Head (), EP)); 
		float a = maxA;

		foreach (Vertex v in e.OutSet()) {
			a = Mathf.Abs (ccw (e.Head (), e.Next ().Head (), v));
			if (a > maxA) {
				maxA = a;
				EP = v;
			}
		}

		return EP;
	}

	/// <summary>
	/// Auxilary method. Calcuate double of the signed area of the triangle formed by the 3 vertices.
	/// </summary>
	/// <param name="v1">Vertex.</param>
	/// <param name="v2">Vertex.</param>
	/// <param name="v3">Vertex.</param>
	public static float ccw (Vertex v1, Vertex v2, Vertex v3)
	{
	 return (v2.X() - v1.X()) * (v3.Y() - v1.Y()) - (v2.Y() - v1.Y()) * (v3.X() - v1.X());
	}

	public static void setEpsilon (float e)
	{
		epsilon = e;
	}

}
