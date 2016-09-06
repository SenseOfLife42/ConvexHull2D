using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvexHull_Analyser {

	public static float epsilon;

	private Vertex[] vertices;

	public ConvexHull_Analyser ()
	{

	}

	public ConvexHull_Analyser (List<Vector2> verts)
	{
		vertices = new Vertex[verts.Count];

		for (int i = 0; i < verts.Count; i++) {
			vertices[i] = new Vertex(verts[i]);
		}
	}

	public int buildHull ()
	{
		setEpsilon ();
		Utility.setEpsilon (epsilon);

		int left = eliminatePoints ();
		return left;
	}

	private int eliminatePoints ()
	{
		int n = vertices.Length;
		int i1, i2, i3, i4, j;
		float a1, a2, a3, a4, highX, lowX, highY, lowY;

		i1 = i2 = i3 = i4 = 0;
		a1 = vertices [0].X () + vertices [0].Y ();
		a3 = a1;
		a2 = vertices [0].X () - vertices [0].Y ();
		a4 = a2;

		for (j = 1; j < n; j++) {
			if (vertices [j].X () + vertices [j].Y () < a1) {
				i1 = j;
				a1 = vertices [j].X () + vertices [j].Y ();
			} else if (vertices [j].X () + vertices [j].Y () > a4) {
				i4 = j;
				a4 = vertices [j].X () + vertices [j].Y ();
			}

			if (vertices [j].X () - vertices [j].Y () < a2) {
				i2 = j;
				a2 = vertices [j].X () - vertices [j].Y ();
			} else if (vertices [j].X () - vertices [j].Y () > a3) {
				i3 = j;
				a3 = vertices [j].X () - vertices [j].Y ();
			}
		}

		vertices [i1].setState (Vertex.State.extreme);
		vertices [i2].setState (Vertex.State.extreme);
		vertices [i3].setState (Vertex.State.extreme);
		vertices [i4].setState (Vertex.State.extreme);

		Edge e1 = new Edge (vertices [i1]);
		Edge e2 = new Edge (vertices [i3]);
		Edge e3 = new Edge (vertices [i4]);
		Edge e4 = new Edge (vertices [i2]);

		lowX = Mathf.Max (vertices [i1].X (), vertices [i2].X ());
		highX = Mathf.Min (vertices [i3].X (), vertices [i4].X ());
		lowY = Mathf.Max (vertices [i1].Y (), vertices [i3].Y ());
		highY = Mathf.Min (vertices [i2].Y (), vertices [i4].Y ());

		float x, y;
		Vertex temp;
		int m = -1;
		j = n - 1;
		while (j > m) {
			x = vertices [j].X ();
			y = vertices [j].Y ();
			if (lowX < x && x < highX && lowY < y && y < highY) {
				vertices [j].setState (Vertex.State.preDeleted);
				j--;
			} else {
				m++;
				temp = vertices [m];
				vertices [m] = vertices [j];
				vertices [j] = temp;
			}
		}

		return (m + 1);
	}

	private void setEpsilon ()
	{
		if (vertices [0] == null)
			return;

		float maxX, maxY;
		maxX = maxY = 0;

		float x, y;

		foreach (Vertex v in vertices) {
			if ((x = Mathf.Abs(v.X())) > maxX)
				maxX = x;
			if ((y = Mathf.Abs(v.Y())) > maxY)
				maxY = y;
		}

		epsilon = 2 * (maxX + maxY) * float.Epsilon;
	}
}
