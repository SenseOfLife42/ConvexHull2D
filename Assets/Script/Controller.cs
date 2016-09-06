using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public int pointNumber = 20;

	public bool rect = true;

	public float width = 50;
	public float hight = 50;

	public float radius = 50;

	public GameObject vis;

	private ConvexHull cHull;
	private PointGenerator pointGen;

	private Vertex[] vertices;
	private Edge[] edges;

	bool a = true;

	// Use this for initialization
	void Start ()
	{
		if (rect)
			cHull = new ConvexHull (PointGenerator.createPointInRectangle (pointNumber, width, hight));
		else
			cHull = new ConvexHull (PointGenerator.createPointInCircle (pointNumber, radius));


		cHull.buildHull ();

		vertices = cHull.getVertices ();

		foreach (Vertex v in vertices) {
			GameObject sphere = GameObject.Instantiate(vis);
			sphere.transform.position = v.Position();
			v.setObj(sphere);
			v.visualizeState ();
		}

		Debug.Log("Start ended");
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Nothing to do here
	}

}
