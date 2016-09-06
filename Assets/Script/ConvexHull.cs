using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvexHull {

	public static float epsilon;

	private Vertex[] vertices;
	private Edge[] edges;
	private Edge[] finalEdges;
	private int edgeCount;

	public ConvexHull ()
	{

	}

	/// <summary>
	/// Initializes convexhull's data structures.
	/// </summary>
	/// <param name="verts">List o vertices.</param>
	public ConvexHull (List<Vector2> verts)
	{
		vertices = new Vertex[verts.Count];
		edges = new Edge[2 * verts.Count];

		for (int i = 0; i < verts.Count; i++) {
			vertices[i] = new Vertex(verts[i]);
		}
	}

	/// <summary>
	/// Calculate an epsilon value to be used as tolerance. Eliminate most of the points surely inside 
	/// the final hull and than builds it with the remaining.
	/// </summary>
	public void buildHull ()
	{
		setEpsilon ();
		Utility.setEpsilon (epsilon);

		eliminatePoints ();
		quickhull ();
	}

	/// <summary>
	/// Gets the vertices array.
	/// </summary>
	/// <returns>The vertices array.</returns>
	public Vertex[] getVertices ()
	{
		return vertices;
	}

	/// <summary>
	/// Gets the boundary of the convexhull.
	/// </summary>
	/// <returns>The edges array.</returns>
	public Edge[] getEdge ()
	{
		int i = 0;
		foreach (Edge e in edges) {
			if (e == null)
				break;
			i++;
		}
		finalEdges = new Edge[i];
		for (int j = 0; j < edges.Length; j++) {
			if (edges[j] == null)
				break;
			finalEdges[j] = finalEdges[j];
		}
		return finalEdges;
	}

	/// <summary>
	/// Implementation of the golin's elimination algorithm.
	/// Here only for demostration pourpose, the next algorithm will be used instead.
	/// </summary>
	private void golinEliminate ()
	{
		int n = vertices.Length;
		int i1, i2, i3, i4, j;
		float a1, a2, a3, a4, highX, lowX, highY, lowY;

		i1 = i2 = i3 = i4 = 0;
		a1 = vertices [0].X() + vertices [0].Y();
		a3 = a1;
		a2 = vertices [0].X() - vertices [0].Y();
		a4 = a2;

		for (j = 1; j < n; j++) {
			if (vertices [j].X() + vertices [j].Y() < a1) {
				i1 = j;
				a1 = vertices [j].X() + vertices [j].Y();
			} else if (vertices [j].X() + vertices [j].Y() > a4) {
				i4 = j;
				a4 = vertices [j].X() + vertices [j].Y();
			}

			if (vertices [j].X() - vertices [j].Y() < a2) {
				i2 = j;
				a2 = vertices [j].X() - vertices [j].Y();
			} else if (vertices [j].X() - vertices [j].Y() > a3) {
				i3 = j;
				a3 = vertices [j].X() - vertices [j].Y();
			}
		}

		lowX = Mathf.Max (vertices [i1].X(), vertices [i2].X());
		highX = Mathf.Min (vertices [i3].X(), vertices [i4].X());
		lowY = Mathf.Max (vertices [i1].Y(), vertices [i3].Y());
		highY = Mathf.Min (vertices [i2].Y(), vertices [i4].Y());

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
				temp = vertices[m];
				vertices[m] = vertices[j];
				vertices[j] = temp;
			}
		}
	}

	/// <summary>
	/// Implementation of the standard golin's elimination algorithm,
	/// but also builds the initial hull using the vertices that minimize/maximize used function.
	/// </summary>
	private void eliminatePoints ()
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

		Debug.Log ("Remaining vertices: " + (m + 1));

		e1.setNext (e2);
		e1.setPrev (e4);

		e2.setNext (e3);
		e2.setPrev (e1);

		e3.setNext (e4);
		e3.setPrev (e2);

		e4.setNext (e1);
		e4.setPrev (e3);

		e1.computeCenterAndNormal ();
		e2.computeCenterAndNormal ();
		e3.computeCenterAndNormal ();
		e4.computeCenterAndNormal ();

		for (int k = 0; k < m; k++) {
			if (vertices[k].isOnHull())
				continue;
			if (Utility.inside(vertices[k], e1))
				vertices[k].setState(Vertex.State.deleted);
		}

		edges[0] = e1;
		edges[1] = e2;
		edges[2] = e3;
		edges[3] = e4;
		edgeCount = 4;
	}

	/// <summary>
	/// Quickhull algorithm implementation.
	/// </summary>
	public void quickhull ()
	{
		foreach (Vertex v in vertices) {
			if (!v.isNotProcessed ())
				continue;
			Utility.assign (v, edges [0]);
		}

		int i = 0;
		Edge wEdge;
		Vertex EP;
		List<Edge> visibleSet = new List<Edge> ();
		Edge[] bounds = new Edge[2];
		List<Vertex> unassignedVertex = new List<Vertex> ();

		while (i < edgeCount) {
			wEdge = edges [i];
			if (wEdge.OutSet ().Count <= 0 || wEdge.isDeleted ()) {
				i++;
				continue;
			}

			EP = Utility.furthestFrom (wEdge);
			EP.setState(Vertex.State.onHull);

			bounds [0] = null;
			bounds [1] = null;

			wEdge.setState (Edge.State.deleted);
			unassignedVertex.Clear();
			findBounds (EP, wEdge, ref unassignedVertex, ref bounds);

			Edge e1 = new Edge ();
			e1.setHead (bounds [0].Head ());

			Edge e2 = new Edge ();
			e2.setHead (EP);

			e1.setPrev (bounds [0].Prev ());
			e1.setNext (e2);

			e2.setPrev (e1);
			e2.setNext (bounds [1].Next ());

			foreach (Vertex v in unassignedVertex) {
				if (Utility.atRight (v, e1)) {
					e1.addToOutSet (v);
				} else if (Utility.atRight (v, e2)) {
					e2.addToOutSet (v);
				} else {
					v.setState(Vertex.State.deleted);
				}
			}

			edges[edgeCount++] = e1;
			edges[edgeCount++] = e2;
			i++;
		}
	}

	/// <summary>
	/// Ausilary function for the quickhull routine. Finds the visible edges (bounds) from an eye-points (vertex).
	/// </summary>
	/// <param name="v">Eye-point.</param>
	/// <param name="e0">Starting edges.</param>
	/// <param name="vrts">Collects all vertices in the bounds outside's vertex sets.</param>
	/// <param name="bounds">Visible edges from the eye-point.</param>
	private void findBounds (Vertex v, Edge e0, ref List<Vertex> vrts, ref Edge[] bounds)
	{
		Edge e = e0;
		bounds[0] = e;
		bounds[1] = e;

		foreach (Vertex u in e.OutSet()) {
			if (u != v)
				vrts.Add (u);
		}

		e = e.Prev ();
		while (Utility.atRight (v, e)) {
			foreach (Vertex u in e.OutSet()) {
				vrts.Add (u);
			}
			e.setState(Edge.State.deleted);
			bounds [0] = e;
			e = e.Prev ();
		}

		e = e0.Next ();
		while (Utility.atRight (v, e)) {
			foreach (Vertex u in e.OutSet()) {
				vrts.Add(u);
			}
			e.setState(Edge.State.deleted);
			bounds[1] = e;
			e = e.Next();
		}
	}

	/// <summary>
	/// Sets the value used as tolerance.
	/// </summary>
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
