using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Edge {

    public enum State
    {
        onHull,
        deleted
    };

    /// <summary>
    /// Statrting point of the oriented edge.
    /// </summary>
    private Vertex head;

    /// <summary>
    /// Every edges knows his prevoius and his next.
    /// </summary>
    private Edge next;
    private Edge prev;

    /// <summary>
    /// All the vertices at the right of this edge not in an other outside set
    /// </summary>
    private List<Vertex> outSet;

    /// <summary>
    /// the center of the edge and his normal.
    /// Since in 2D the normal lays on the same plane.
    /// </summary>
    private Vector2 center;
    private Vector2 normal;

    private State state;

    /// <summary>
    /// Initializes a new instance of an edge.
    /// Different constructor are given.
    /// </summary>
    public Edge () 
    {
        head = null;
        next = prev = null;
		outSet = new List<Vertex>();
		center = Vector2.zero;
		normal = Vector2.zero;
        state = State.onHull;
    }

    public Edge (Vertex v) 
    {
        head = v;
        next = prev = null;
		outSet = new List<Vertex>();
		center = Vector2.zero;
		normal = Vector2.zero;
        state = State.onHull;
    }

    public Edge (Vertex v, Edge n, Edge p)
    {
        head = v;
        next = n;
        prev = p;
		outSet = new List<Vertex>();
        computeCenter();
        computeNormal();
        state = State.onHull;
    }

    /// <summary>
    /// Computes the center and normal of the edges.
    /// </summary>
    public void computeCenterAndNormal ()
	{
		computeCenter();
		computeNormal();
	}

    public void computeCenter ()
	{
		center = (head.Position() + next.head.Position())/2.0f;
	}

    public void computeNormal ()
	{
		Vector3 dir = next.head.Position() - head.Position();
		dir.Normalize();
		Vector3 up = new Vector3(0,0,-1.0f);
		normal = Vector3.Cross(up, dir).normalized;
	}

	/// <summary>
	/// Return the head of the edge.
	/// </summary>
    public Vertex Head ()
	{
		return head;
	}

	/// <summary>
	/// Return the next of the edge.
	/// </summary>
	public Edge Next () 
	{
		return next;
	}

	/// <summary>
	/// Return the previous of the edge.
	/// </summary>
	public Edge Prev ()
	{
		return prev;
	}

	/// <summary>
	/// Sets the head of the edge.
	/// </summary>
	/// <param name="v">Head vertex.</param>
	public void setHead (Vertex v)
	{
		head = v;
	}

	/// <summary>
	/// Sets the next of the edge.
	/// </summary>
	/// <param name="e">Next edge.</param>
	public void setNext (Edge e) 
	{
		next = e;
	}

	/// <summary>
	/// Sets the previous of the edge.
	/// </summary>
	/// <param name="e">Previous edge.</param>
	public void setPrev (Edge e)
	{
		prev = e;
	}

	/// <summary>
	/// Adds a vertex to the outside set.
	/// </summary>
	/// <param name="v">V.</param>
	public void addToOutSet (Vertex v)
	{
		outSet.Add(v);
	}

	/// <summary>
	/// Return the outside set.
	/// </summary>
	/// <returns>The set.</returns>
	public List<Vertex> OutSet ()
	{
		return outSet;
	}

	/// <summary>
	/// Sets the state of the edge.
	/// </summary>
	/// <param name="s">Edge's state.</param>
	public void setState (State s)
	{
		state = s;
	}

	/// <summary>
	/// Check edge's state.
	/// </summary>
	/// <returns><c>true</c>, if edge was ised, <c>false</c> otherwise.</returns>
	public bool isDeleted ()
	{
		return state == State.deleted;
	}

	/// <summary>
	/// Check edge's state.
	/// </summary>
	/// <returns><c>true</c>, if edge is on hull, <c>false</c> otherwise.</returns>
	public bool isOnHull ()
	{
		return state == State.onHull;
	}

}
