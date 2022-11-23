using UnityEngine;
using UnityEngine.Rendering;

public class DepthCamera : MonoBehaviour
{
    [Header("Shader Setup")]
    public Shader uberReplacementShader;

    private Camera camera;

    void Start()
    {
        // default fallbacks, if shaders are unspecified
        if (!uberReplacementShader)
            uberReplacementShader = Shader.Find("Hidden/UberReplacement");

        camera = GetComponent<Camera>();
        camera.RemoveAllCommandBuffers();
        camera.targetDisplay = 1;
        
        SetupCameraWithReplacementShader(uberReplacementShader, Color.white);
    }



    private void SetupCameraWithReplacementShader(Shader shader, Color clearColor)
    {
        var cb = new CommandBuffer();
        cb.SetGlobalFloat("_OutputMode", 2);
        camera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, cb);
        camera.AddCommandBuffer(CameraEvent.BeforeFinalPass, cb);
        camera.SetReplacementShader(shader, "");
        camera.backgroundColor = clearColor;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.allowHDR = false;
        camera.allowMSAA = false;
    }
}
