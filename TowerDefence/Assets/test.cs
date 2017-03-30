using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{

    public Vector3 Start = new Vector3();
    public Vector3 End = new Vector3();
    static Material lineMaterial;

    int Lines = 0;

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        Lines = GameManager.gameManager.GetMapHeight(); ;

        GL.PushMatrix();

        for (int x = 0; x < Lines; x++)
        {
            for (int y = 0; y < Lines; y++)
            {
                if (NodeGrid.nodeGrid.Grid[x, y].parent != null)
                {
                    Start.x = NodeGrid.nodeGrid.Grid[x, y].WorldPos.x;
                    Start.y = NodeGrid.nodeGrid.Grid[x, y].WorldPos.y;
                    Start.z = NodeGrid.nodeGrid.Grid[x, y].WorldPos.z;

                    End.x = NodeGrid.nodeGrid.Grid[x, y].GetDirectionVector().x / 2;
                    End.y = NodeGrid.nodeGrid.Grid[x, y].GetDirectionVector().y;
                    End.z = NodeGrid.nodeGrid.Grid[x, y].GetDirectionVector().z / 2;
                    // Draw lines
                    GL.Begin(GL.LINES);
                    // Vertex colors change from red to green
                    GL.Color(Color.black);
                    // One vertex at transform position
                    GL.Vertex3(Start.x, Start.y, Start.z);
                    // Another vertex at edge of circle
                    GL.Vertex3(Start.x + End.x * 0.5f, Start.y + End.y * 0.5f, Start.z + End.z);
                }
            }
        }
        GL.End();
        GL.PopMatrix();
    }
}
