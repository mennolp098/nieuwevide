﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayButtonPressed()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
