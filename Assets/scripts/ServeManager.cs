using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ServeType {
	public float upForce;
	public float hitForce;
}

public class ServeManager : MonoBehaviour {
	public ServeType lob;
	public ServeType hard;
}


