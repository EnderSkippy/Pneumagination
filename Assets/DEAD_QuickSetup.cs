using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEAD_QuickSetup : MonoBehaviour
{
    [Header("Create New Showtape")]
    public string showName;
    public string author;
    public string description;
    public string animatronicsUsedFor;
    public float endOfTapeTime;
    [Space(10)]
    [Header("Save, then reselect to reload.")]
    public bool createShow;
    [Header("Select audio, save to show to inject it. Then reselect to reload.")]
    public bool injectAudio;
    [Space(30)]
    public int showtapeSlot;
    [Space(10)]
    //public bool playShow;
    [Space(10)]
    //public bool stopShow;
    [Space(10)]
    //public bool recordShow;
    [Space(30)]
    
    //public bool saveRecording;


    [HideInInspector]
    public DEAD_GUI_File_Saver saver;
    [HideInInspector]
    public DEAD_Recorder recorder;
    [HideInInspector]
    public DEAD_DTU_Tester DTU;
    [HideInInspector]
    public DEAD_Interface Interface;

    //[HideInInspector]
    //public string play;
    //[HideInInspector]
    //public DEAD_InterfaceCommands.DEAD_InterfaceFunctionList pause;





    // Update is called once per frame
    private void Update()
    {
        if (createShow)
        {
            createShow = false;
            CreateShow();
        }
        if (injectAudio)
        {
            injectAudio = false;
            InjectAudio();
        }
        //if (playShow)
        {
            //playShow = false;
            //PlayShow(); //Add Play Function
        }
        //if (stopShow)
        {
            //stopShow = false;
            //StopShow(); //Add Pause function
        }
        //if (recordShow)
        {
            //recordShow = false;
            //RecordShow(); //Add Play Function
        }
        //if (saveRecording)
        {
            //saveRecording = false;
            //SaveRecording();
        }
    }








    void CopyFields()
    {
        saver.showtapeSlot = showtapeSlot; //Move to load and save? Or play and record?
        saver.name = showName;
        saver.author = author;
        saver.description = description;
        saver.animatronicsUsedFor = animatronicsUsedFor;
        saver.endOfTapeTime = endOfTapeTime;
    }
    void CreateShow()
    {
        CopyFields();
        saver.SaveFile(true);
        saver.LoadFile();
    }
    void InjectAudio()
    {
        CopyFields(); //In case they were changed.
        saver.InjectAudioData();
        saver.SaveFile(false);
        saver.LoadFile();
    }
    void PlayShow()
    {
        saver.ClearData();
        //DTU.overrideDTU = false;
        saver.showtapeSlot = showtapeSlot;
        saver.LoadFile();
        //Add copying fields to Quick Setup
        saver.InjectIntoInterface();
        //Interface.SendCommand(play, false); //Add actual play command
    }
    void StopShow()
    {
        //Interface.SendCommand(pause, false); //Add actual pause command
    }

    void RecordShow()
    {
        saver.ClearData();
        //DTU.overrideDTU = true;
        saver.showtapeSlot = showtapeSlot;
        saver.LoadFile();
        //Add copying fields to Quick Setup
        saver.InjectIntoInterface();
        //Interface.SendCommand(play, false); //Add actual play command
    }
    void SaveRecording()
    {
        saver.SaveFile(false);
        recorder.ApplyRecordingToTape();
        recorder.SaveTapeToFile();
    }
}
