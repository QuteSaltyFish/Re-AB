using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineUsage : MonoBehaviour {
    public List<int> wineuse;
    public int[,] variety;
    private int maxtry;
    // Use this for initialization
    void Start()
    {
        variety = new int[,] { { 1, 2, 3, 4 }, { 4, 3, 2, 1 } };
    }
	private int Compare()
    {
        int[] test = wineuse.ToArray();
        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                if (test[j] != variety[i, j])
                    break;
                if (j == 3)
                {
                    wineuse.Clear();
                    return i;
                }
            }
        }
        return 0;
    }
	// Update is called once per frame
	void Update () {

        if (wineuse.Count == 4)
            Compare();
	}
}
