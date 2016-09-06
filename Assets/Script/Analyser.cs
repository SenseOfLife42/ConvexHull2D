using UnityEngine;
using System.Collections;
using System.IO;

public class Analyser : MonoBehaviour {

	private int pointNumber = 20;

	private float width = 50;

	private int left;

	private ConvexHull_Analyser cHull;
	private PointGenerator pointGen;

	private Vertex[] vertices;

	private string data = "";
	private string averageData = "";

	private string dataFile = "data.txt";
	private string dataFileAverage = "dataAverage.txt";

	private int wStep = 10;
	private int pStep = 10;

	private int rep = 10;
	private int average;
	private int averagePercent;

	// Use this for initialization
	void Start ()
	{
		if (File.Exists (dataFile)) {
			File.Delete (dataFile);
		}

		if (File.Exists (dataFileAverage)) {
			File.Delete (dataFileAverage);
		}

		for (int j = 1; j <= wStep; j++) {
			//Debug.Log ("Computing...");
			averagePercent = 0;
			for (int i = 1; i <= pStep; i++) {
				average = 0;
				for (int k = 0; k < rep; k++) {
					
					cHull = new ConvexHull_Analyser (PointGenerator.createPointInRectangle (pointNumber * i * j, width * j, width * j));

					left = cHull.buildHull ();

					data += "Dimension: " + (width * j) + "; Point Number: " + (pointNumber * i * j) + "; Left point: " + left + System.Environment.NewLine;
					average += left;
				}
				average /= rep;
				averagePercent += (pointNumber * i * j - left) * 100 / (pointNumber * i * j);
				//averageData += "Dimension: " + (width * j) + "; Point Number: " + (pointNumber * i * j) + "; Left point average: " + average + "; Elimination : " + (pointNumber * i * j - left) * 100 / (pointNumber * i * j) + "%" + System.Environment.NewLine;
			}
			averagePercent /= pStep;
			averageData += "Series " + j + System.Environment.NewLine;
			averageData += "Dimension: " + (width * j) + "; Min points: " + pointNumber * j + "; Max points: " + pointNumber * j * pStep + System.Environment.NewLine;
			averageData += "Average elimination percentage: " + averagePercent + "%" + System.Environment.NewLine;
			averageData += "---------------------------------" + System.Environment.NewLine;
		}

		//Debug.Log("Writing on file...");

		File.AppendAllText(dataFile, data);

		File.AppendAllText(dataFileAverage, averageData);

		Debug.Log("Done.");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
