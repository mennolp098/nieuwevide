using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	private int _taintedSouls = 0;
	private int _savedSouls = 0;
	private int _badReputation = 0;

	public int taintedSouls
	{
		get { return _taintedSouls; }
		set { _taintedSouls = value; }
	}

	public int savedSouls
	{
		get { return _savedSouls; }
		set { _savedSouls = value; }
	}

	public int badRep
	{
		get { return _badReputation; }
		set { _badReputation = value; }
	}

	public void AddTainted()
	{
		_taintedSouls += 1;
	}

	public void AddSaved()
	{
		_savedSouls += 1;
	}

	public void AddRep()
	{
		_badReputation += 1;
	}
}
