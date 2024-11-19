using UnityEngine;

public class SenalHover : MonoBehaviour
{
    public float escalaX = 1.2f; // La escala en X cuando el cursor está encima
    public float velocidad = 5f; // La velocidad para hacer el cambio de escala
    private Vector3 escalaInicial;
    private bool cursorEncima = false;
    private bool escaladoCompletado = false;

    void Start()
    {
        // Guarda la escala inicial del objeto
        escalaInicial = transform.localScale;
    }

    void OnMouseEnter()
    {
        if (!escaladoCompletado)
        {
            cursorEncima = true;
        }
    }

    void OnMouseExit()
    {
        cursorEncima = false;
    }

    void Update()
    {
        // Si el cursor está encima y la animación aún no se ha completado
        if (cursorEncima && !escaladoCompletado)
        {
            // Escala solo en el eje X hacia el valor de `escalaX`
            Vector3 escalaObjetivo = new Vector3(escalaInicial.x * escalaX, escalaInicial.y, escalaInicial.z);
            transform.localScale = Vector3.Lerp(transform.localScale, escalaObjetivo, velocidad * Time.deltaTime);

            // Marca el escalado como completado cuando esté suficientemente cerca del objetivo
            if (Mathf.Abs(transform.localScale.x - escalaObjetivo.x) < 0.01f)
            {
                transform.localScale = escalaObjetivo; // Asegura que llegue exactamente al objetivo
                escaladoCompletado = true;
            }
        }
        else if (!cursorEncima && escaladoCompletado)
        {
            // Vuelve a la escala inicial cuando el cursor se va (opcional, puedes omitir esto)
            transform.localScale = Vector3.Lerp(transform.localScale, escalaInicial, velocidad * Time.deltaTime);
            if (Mathf.Abs(transform.localScale.x - escalaInicial.x) < 0.01f)
            {
                transform.localScale = escalaInicial;
                escaladoCompletado = false;
            }
        }
    }
}
