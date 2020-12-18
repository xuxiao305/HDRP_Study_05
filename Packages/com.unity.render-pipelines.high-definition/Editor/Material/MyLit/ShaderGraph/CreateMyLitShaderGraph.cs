using UnityEditor.ShaderGraph;

namespace UnityEditor.Rendering.HighDefinition
{
    static class CreateMyLitShaderGraph
    {
        [MenuItem("Assets/Create/Shader/HDRP/MyLit Graph", false, 208)]
        public static void CreateMaterialGraph()
        {
            GraphUtil.CreateNewGraph(new MyLitMasterNode());
        }
    }
}
