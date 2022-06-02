using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PersistentInfo : MonoBehaviour
{
    //Variables to store between scenes
    public bool m_soundToggle { get; set; }
    public float m_Sound { get; set; }
    public bool m_musicToggle { get; set; }
    public float m_Music { get; set; }
    public float m_FOV { get; set; }
    public int m_MazeWidth { get; set; }
    public int m_MazeHeight { get; set; }
    public int m_Subdivision { get; set; }


    public static PersistentInfo Instance { set; get; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Clear();
        }
    }
    public void Clear()
    {
        //set default
        m_soundToggle = true;
        m_Sound = 100;
        m_musicToggle = true;
        m_Music = 100;
        m_FOV = 60;
        m_MazeWidth = 5;
        m_MazeHeight = 5;
        m_Subdivision = 3;
    }
}