using UnityEngine;
using System.Collections;

public class Vertex {

	/// <summary>
	/// Extreme state is for those 4 vertex found by the golin's elimination algorithm. 
	/// </summary>
	public enum State {
		onHull,
		processed,
		notProcessed,
		preDeleted,
		deleted,
		extreme
	};

	/// <summary>
	/// Visualization object.
	/// </summary>
	public GameObject obj;

	private Vector2 pnt;

	private State state;

	/// <summary>
	/// Initializes a new instance of a vertex.
    /// Different constructor are given.
	/// </summary>
	public Vertex ()
	{
		pnt = new Vector2();
		state = State.notProcessed;
	}

	public Vertex (Vector2 v)
	{
		pnt = v;
		state = State.notProcessed;
	}

	public Vertex (float x, float y)
	{
		pnt = new Vector2(x, y);
	}

	/// <summary>
	/// Returns the position of the vertex.
	/// </summary>
	public Vector3 Position ()
	{
		return pnt;
	}

	/// <summary>
	/// Returns the x coordinate of the position.
	/// </summary>
	public float X ()
	{
		return pnt.x;
	}

	/// <summary>
	/// Returns the y coordinate of the position.
	/// </summary>
	public float Y ()
	{
		return pnt.y;
	}

	/// <summary>
	/// Returns the i-th coordinate of the position if exists.
	/// </summary>
	/// <returns>The coordinate.</returns>
	/// <param name="i">i-th coordinate.</param>
	public float getCoord (int i)
	{
		switch (i) {
			case 0: return pnt.x;
			case 1:	return pnt.y;
			default:
				new UnityException("Unexpected coordinate number: " + i);
				return float.NaN;
		}
	}

	/// <summary>
	/// Change the color of the visualization object based on the state.
	/// </summary>
	public void visualizeState()
	{
		if (state == State.deleted)
			obj.GetComponent<Renderer> ().material.color = Color.red;
		else if (state == State.preDeleted)
			obj.GetComponent<Renderer> ().material.color = Color.black;
		else if (state == State.onHull)
			obj.GetComponent<Renderer> ().material.color = Color.green;
		else if (state == State.extreme)
			obj.GetComponent<Renderer> ().material.color = Color.blue;
		else
			obj.GetComponent<Renderer> ().material.color = Color.gray;
	}

	/// <summary>
	/// Sets the position of the vertex.
	/// </summary>
	/// <param name="v">2D Vector.</param>
	public void setPosition(Vector2 v) 
	{
		pnt = v;
	}

	/// <summary>
	/// Sets the position f the vertex
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public void setPosition (float x, float y)
	{
		pnt.x = x;
		pnt.y = y;
	}

	/// <summary>
	/// Sets the state of the vertex
	/// </summary>
	/// <param name="s">the state.</param>
	public void setState (State s)
	{
		state = s;
	}

	/// <summary>
	/// Checks vertex state.
	/// </summary>
	/// <returns><c>true</c>, if vertex is on hull, <c>false</c> otherwise.</returns>
	public bool isOnHull ()
	{
		return state == State.onHull || state == State.extreme;
	}

	/// <summary>
	/// Checks vertex state.
	/// </summary>
	/// <returns><c>true</c>, if vertex is processed, <c>false</c> otherwise.</returns>
	public bool isProcessed ()
	{
		return state == State.processed;
	}

	/// <summary>
	/// Checks vertex state.
	/// </summary>
	/// <returns><c>true</c>, if vertex is not on hull, <c>false</c> otherwise.</returns>
	public bool isNotProcessed ()
	{
		return state == State.notProcessed;
	}

	/// <summary>
	/// Checks vertex state.
	/// </summary>
	/// <returns><c>true</c>, if vertex is deleted, <c>false</c> otherwise.</returns>
	public bool isDeleted ()
	{
		return state == State.deleted || state == State.preDeleted;
	}

	/// <summary>
	/// Sets the object for visualization.
	/// </summary>
	/// <param name="o">The object.</param>
	public void setObj (GameObject o)
	{
		obj = o;
	}

}
