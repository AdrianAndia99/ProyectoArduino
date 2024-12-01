using System;
using System.IO.Ports;
using UnityEngine;
using System.Collections; // Necesario para las corutinas

public class ArduinoController : MonoBehaviour
{
    public AudioClip[] notas; // Array de clips de audio (do, re, mi, etc.)
    public AudioSource audioSource;

    private SerialPort arduinoPort;
    private bool canPressButton = true; // Indica si el botón puede ser presionado
    private float waitTime = 0.5f; // Tiempo de espera en segundos

    void Start()
    {
        // Configura el puerto COM que usa tu Arduino
        arduinoPort = new SerialPort("COM4", 9600); // Asegúrate de usar el puerto correcto
        arduinoPort.Open();
        arduinoPort.ReadTimeout = 100; // Tiempo de espera en milisegundos
    }

    void Update()
    {
        if (arduinoPort.IsOpen && canPressButton)
        {
            try
            {
                string data = arduinoPort.ReadLine(); // Lee el mensaje del Arduino
                int buttonIndex;

                if (int.TryParse(data, out buttonIndex))
                {
                    Debug.Log($"Botón presionado: {buttonIndex}"); // Muestra el índice en la consola
                    if (buttonIndex >= 0 && buttonIndex < notas.Length)
                    {
                        PlayNote(buttonIndex);
                        StartCoroutine(WaitForNextButtonPress()); // Inicia la corutina para esperar antes de permitir otro botón
                    }
                }
            }
            catch (TimeoutException)
            {
                // No hacer nada si no hay datos
            }
        }
    }

    private void PlayNote(int index)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Detiene la reproducción actual
        }

        audioSource.clip = notas[index];
        audioSource.Play();
    }

    // Corutina que implementa el tiempo de espera antes de permitir otra pulsación de botón
    private IEnumerator WaitForNextButtonPress()
    {
        canPressButton = false; // Evita que se presione otro botón inmediatamente
        yield return new WaitForSeconds(waitTime); // Espera 0.5 segundos
        canPressButton = true; // Permite presionar otro botón
    }

    private void OnApplicationQuit()
    {
        // Cierra el puerto serie al salir del juego
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }
}