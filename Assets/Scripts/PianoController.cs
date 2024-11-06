using System.IO.Ports;
using UnityEngine;
using System;

public class PianoController : MonoBehaviour
{
    public AudioClip[] noteClips; // Array de AudioClips para las notas (DO a DO)
    private AudioSource audioSource;
    private SerialPort arduinoPort;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        arduinoPort = new SerialPort("COM3", 9600); // Cambiar "COM3" al puerto adecuado
        arduinoPort.Open();
    }

    void Update()
    {
        if (arduinoPort.IsOpen)
        {
            try
            {
                string noteIndexString = arduinoPort.ReadLine();
                int noteIndex;
                if (int.TryParse(noteIndexString, out noteIndex))
                {
                    PlayNoteSound(noteIndex);
                }
            }
            catch (System.Exception) { }
        }
    }

    void PlayNoteSound(int index)
    {
        if (index >= 0 && index < noteClips.Length)
        {
            audioSource.PlayOneShot(noteClips[index]);
        }
    }

    void OnApplicationQuit()
    {
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }
}