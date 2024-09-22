using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderCompiler : MonoBehaviour
{
    public RenderTexture renderTexture;
    public ComputeShader pixelateShader; // Shader para pixelar
    public ComputeShader circleShader; // Shader para dibujar círculos

    public int pixelSize = 8; // Asegúrate de que sea un solo valor para el tamaño del bloque

    void OnValidate()
    {
        pixelSize = Mathf.Clamp(pixelSize, 1, 128);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (pixelSize <= 1)
        {
            Graphics.Blit(source, destination);
            return;
        }

        // Resolucion dinamica
        if (renderTexture == null || renderTexture.width != source.width || renderTexture.height != source.height)
        {
            if (renderTexture != null)
                renderTexture.Release();

            renderTexture = new RenderTexture(source.width, source.height, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }

        // Ejecutar el shader de pixelado
        pixelateShader.SetTexture(0, "Source", source);
        pixelateShader.SetTexture(0, "Result", renderTexture);
        pixelateShader.SetInt("pixelSize", pixelSize);
        pixelateShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

        // Ejecutar el shader de círculos
        circleShader.SetTexture(0, "Source", renderTexture);
        circleShader.SetTexture(0, "Result", renderTexture);
        circleShader.SetInt("pixelSize", pixelSize);
        circleShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

        // Blit el resultado final al destino
        Graphics.Blit(renderTexture, destination);
    }
}
