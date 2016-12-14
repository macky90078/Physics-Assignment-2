using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour {

	public GameObject m_Main;
	public GameObject m_Instructions;
	public GameObject m_Credits;

	public Text m_CanvasTitle;

	private List<GameObject> m_Canvases;
	private string m_currentCanvas = "Main";

	void Start() {
		m_Canvases = new List<GameObject>();
		m_Canvases.Add(m_Main);

		m_Canvases.Add(m_Instructions);
		m_Canvases.Add(m_Credits);

		for(int i = 1; i < m_Canvases.Count; i++) {
			m_Canvases[i].SetActive(false);
		}
	}

	void Update() {
		if (m_currentCanvas != "Main") {
			m_CanvasTitle.text = m_currentCanvas;
		} else {
			m_CanvasTitle.text = "Physics Assignment 2";
		}
	}

	public void Play() {
		SceneManager.LoadScene(1);
	}

	public void SwitchCanvas(GameObject _target) {
		_target.SetActive(true);
		m_currentCanvas = _target.name;
		foreach(GameObject canvas in m_Canvases) {
			if(canvas.activeInHierarchy && canvas != _target) {
				canvas.SetActive(false);
			}
		}
	}

	public void Quit() {
		Application.Quit();
	}
}
