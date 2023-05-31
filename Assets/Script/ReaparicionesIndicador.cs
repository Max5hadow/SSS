using UnityEngine;
using UnityEngine.UI;

public class ReaparicionesIndicador : MonoBehaviour
{
    public int reaparicionesRestantes;
    public Text textoReapariciones;

    void Start()
    {
        UpdateReaparicionesText();
    }

    public void UpdateReaparicionesText()
    {
        textoReapariciones.text = "Reapariciones: " + reaparicionesRestantes;
    }

    // Método para actualizar el valor de reapariciones restantes
    public void ActualizarReaparicionesRestantes(int nuevoValor)
    {
        reaparicionesRestantes = nuevoValor;
        UpdateReaparicionesText();
        Debug.Log("Reapariciones restantes: " + reaparicionesRestantes);
    }
}
