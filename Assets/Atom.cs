using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour {
    public string electronsPerShell;
    public string atom;
    public GameObject protonPrefab;
    public GameObject electronPrefab;

    public int shellSmoothRate = 20;

    private List<List<Vector3>> shells = new List<List<Vector3>>();

    private List<int> electronsNumbers = new List<int>();

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for(int i = 0; i < shells.Count; i++)
        {
            for (int j = 0; j < shellSmoothRate; j++)
            {
                Vector3 current = shells[i][j];
                Vector3 next = j == (shellSmoothRate - 1) ? shells[i][0] : shells[i][j + 1];

                Gizmos.DrawSphere(transform.TransformPoint(current), 0.1f);
                Gizmos.DrawLine(transform.TransformPoint(current), transform.TransformPoint(next));
            }
        }

    }

    // Use this for initialization
    void Start () {
        printTitle();

        ParseElectronString();

        CreateShells();

        CreateProton();
        CreateElectrons();
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void CreateShells()
    {
        float interval = 2 * Mathf.PI / shellSmoothRate;

        for(int i = 0; i < electronsNumbers.Count; i++)
        {
            List<Vector3> shell = new List<Vector3>();

            for(int j = 0; j < shellSmoothRate; j++)
            {
                shell.Add(new Vector3(Mathf.Sin(j * interval) * (i + 1) * 5.0f, 0, Mathf.Cos(j * interval) * (i + 1) * 5.0f));
            }

            shells.Add(shell);
        }
    }

    private void CreateProton()
    {
        if(protonPrefab)
        {
            GameObject proton = GameObject.Instantiate(protonPrefab);
            proton.transform.parent = transform;

            //proton.transform.position = transform.InverseTransformPoint(transform.position);
            proton.transform.position = transform.position;
        }
    }

    private void CreateElectrons()
    {
        if(electronPrefab)
        {

            for(int i = 0; i < shells.Count; i++)
            {
                float interval = 2 * Mathf.PI / electronsNumbers[i];
                for(int j = 0; j < electronsNumbers[i]; j++)
                {
                    GameObject electron = GameObject.Instantiate(electronPrefab);
                    electron.transform.parent = transform;
                    electron.transform.position = transform.position + new Vector3(Mathf.Sin(j * interval) * (i + 1) * 5.0f, 0, Mathf.Cos(j * interval) * (i + 1) * 5.0f);
                    electron.GetComponent<FollowPathBehaviour>().shell = shells[i];
                    electron.GetComponent<FollowPathBehaviour>().centre = transform.position;
                }
            }
        }
    }

    private void ParseElectronString()
    {
        char[] seperators = {','};
        string[] numbers = electronsPerShell.Split(seperators);

        for(int i = 0; i < numbers.Length; i++)
        {
            electronsNumbers.Add(int.Parse(numbers[i]));
        }
    }

    private void printTitle()
    {
        transform.GetComponent<TextMesh>().text= atom + "\n" + electronsPerShell;
    }
}
