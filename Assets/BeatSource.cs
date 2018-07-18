using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BeatSource : MonoBehaviour {
    public enum Grade
    {
        PERFECT,
        GOOD,
        MISS
    }

    public AudioSource audio;
    public int audioBpm;
    public Transform beatIndicator;

    private double audioStart;
    private double secondsPerBeat;

    private double getOffset(double timePoint)
    {
        double beats = timePoint / secondsPerBeat;
        double left = Math.Floor(beats) * secondsPerBeat;
        double right = Math.Ceiling(beats) * secondsPerBeat;
        if (timePoint - left < right - timePoint)
        {
            return left - timePoint;
        }
        else
        {
            return right - timePoint;
        }
    }

    private double songTime { get { return AudioSettings.dspTime - audioStart; } }

    private double getCurrentOffset()
    {
        return getOffset(songTime);
    }

    public double getBeatPercent()
    {
        double beats = songTime / secondsPerBeat;
        return beats - Math.Floor(beats);
    }

    public Grade getCurrentGrade()
    {
        double off = Math.Abs(getCurrentOffset());
        Grade res;
        if (off < 0.06f)
        {
            res = Grade.PERFECT;
        }
        else if (off < 0.15f)
        {
            res = Grade.GOOD;
        }
        else
        {
            res = Grade.MISS;
        }
        Debug.Log(res.ToString() + " " + off.ToString() + "ms");
        return res;
    }

    // Use this for initialization
    void Start () {
        audio.Play();
        //audio.PlayDelayed(0.05f);
        audioStart = AudioSettings.dspTime - 0.18;
        secondsPerBeat = 60.0 / audioBpm;
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    Debug.Log(offset(AudioSettings.dspTime - audioStart));
        float beat = (float)getBeatPercent();
        beatIndicator.transform.localScale = new Vector3(beat, beat, 1);
	}

    void OnGUI()
    {
        return;
        Event e = Event.current;

        if (e.isKey)
        {
            EventType type = e.type;

            switch (e.type)
            {
                case EventType.KeyDown:
          //          if (e.keyCode == KeyCode.Space)
         //               Debug.Log(offset(AudioSettings.dspTime - audioStart));
                    break;
            }
        }
    }

    void FixedUpdate()
    {
    }
}
