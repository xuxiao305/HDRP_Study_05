using UnityEditor.ShaderGraph;

namespace UnityEditor.Rendering.HighDefinition
{
    static class CreateFabricExShaderGraph
    {
        [MenuItem("Assets/Create/Shader/HDRP/FabricEx Graph", false, 208)]
        public static void CreateMaterialGraph()
        {
            GraphUtil.CreateNewGraph(new FabricExMasterNode());
        }
    }
}
