using System.Collections.Generic;
using Data.Util;
using UnityEditor.Graphing;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.HighDefinition
{
    [FormerName("UnityEditor.Experimental.Rendering.HDPipeline.MyLitSubShader")]
    class MyLitSubShader : IMyLitSubShader
    {
        Pass m_PassMETA = new Pass()
        {
            Name = "META",
            LightMode = "META",
            TemplateName = "MyLitPass.template",
            MaterialName = "MyLit",
            ShaderPassName = "SHADERPASS_LIGHT_TRANSPORT",
            CullOverride = "Cull Off",
            Includes = new List<string>()
            {
                "#include \"Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassLightTransport.hlsl\"",
            },
            RequiredFields = new List<string>()
            {
                "AttributesMesh.normalOS",
                "AttributesMesh.tangentOS",     // Always present as we require it also in case of Variants lighting
                "AttributesMesh.uv0",
                "AttributesMesh.uv1",
                "AttributesMesh.color",
                "AttributesMesh.uv2",           // SHADERPASS_LIGHT_TRANSPORT always uses uv2
            },
            PixelShaderSlots = new List<int>()
            {
                MyLitMasterNode.BaseColorSlotId,
                MyLitMasterNode.NormalSlotId,
                MyLitMasterNode.BentNormalSlotId,
                MyLitMasterNode.TangentSlotId,
                MyLitMasterNode.SubsurfaceMaskSlotId,
                MyLitMasterNode.ThicknessSlotId,
                MyLitMasterNode.DiffusionProfileHashSlotId,
                MyLitMasterNode.IridescenceMaskSlotId,
                MyLitMasterNode.IridescenceThicknessSlotId,
                MyLitMasterNode.IridescenceCoatFixupTIRSlotId,
                MyLitMasterNode.IridescenceCoatFixupTIRClampSlotId,
                MyLitMasterNode.SpecularColorSlotId,
                MyLitMasterNode.DielectricIorSlotId,
                MyLitMasterNode.MetallicSlotId,
                MyLitMasterNode.EmissionSlotId,
                MyLitMasterNode.SmoothnessASlotId,
                MyLitMasterNode.SmoothnessBSlotId,
                MyLitMasterNode.AmbientOcclusionSlotId,
                MyLitMasterNode.AlphaSlotId,
                MyLitMasterNode.AlphaClipThresholdSlotId,
                MyLitMasterNode.AnisotropyASlotId,
                MyLitMasterNode.AnisotropyBSlotId,
                MyLitMasterNode.SpecularAAScreenSpaceVarianceSlotId,
                MyLitMasterNode.SpecularAAThresholdSlotId,
                MyLitMasterNode.CoatSmoothnessSlotId,
                MyLitMasterNode.CoatIorSlotId,
                MyLitMasterNode.CoatThicknessSlotId,
                MyLitMasterNode.CoatExtinctionSlotId,
                MyLitMasterNode.CoatNormalSlotId,
                MyLitMasterNode.CoatMaskSlotId,
                MyLitMasterNode.LobeMixSlotId,
                MyLitMasterNode.HazinessSlotId,
                MyLitMasterNode.HazeExtentSlotId,
                MyLitMasterNode.HazyGlossMaxDielectricF0SlotId,
                MyLitMasterNode.SpecularOcclusionSlotId,
                MyLitMasterNode.SOFixupVisibilityRatioThresholdSlotId,
                MyLitMasterNode.SOFixupStrengthFactorSlotId,
                MyLitMasterNode.SOFixupMaxAddedRoughnessSlotId,

            },
            VertexShaderSlots = new List<int>()
            {
                //MyLitMasterNode.PositionSlotId
            },
            UseInPreview = false
        };

        Pass m_PassShadowCaster = new Pass()
        {
            Name = "ShadowCaster",
            LightMode = "ShadowCaster",
            TemplateName = "MyLitPass.template",
            MaterialName = "MyLit",
            ShaderPassName = "SHADERPASS_SHADOWS",
            BlendOverride = "Blend One Zero",
            ZWriteOverride = "ZWrite On",
            ColorMaskOverride = "ColorMask 0",
            ZClipOverride = HDSubShaderUtilities.zClipShadowCaster,
            Includes = new List<string>()
            {
                "#include \"Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl\"",
            },
            PixelShaderSlots = new List<int>()
            {
                MyLitMasterNode.AlphaSlotId,
                MyLitMasterNode.AlphaClipThresholdSlotId,
                MyLitMasterNode.DepthOffsetSlotId,
            },
            VertexShaderSlots = new List<int>()
            {
                MyLitMasterNode.PositionSlotId
            },
            UseInPreview = false
        };

        Pass m_SceneSelectionPass = new Pass()
        {
            Name = "SceneSelectionPass",
            LightMode = "SceneSelectionPass",
            TemplateName = "MyLitPass.template",
            MaterialName = "MyLit",
            ShaderPassName = "SHADERPASS_DEPTH_ONLY",
            ColorMaskOverride = "ColorMask 0",
            ExtraDefines = new List<string>()
            {
                "#define SCENESELECTIONPASS",
                "#pragma editor_sync_compilation",
            },
            Includes = new List<string>()
            {
                "#include \"Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl\"",
            },
            PixelShaderSlots = new List<int>()
            {
                MyLitMasterNode.AlphaSlotId,
                MyLitMasterNode.AlphaClipThresholdSlotId,
                MyLitMasterNode.DepthOffsetSlotId,
            },
            VertexShaderSlots = new List<int>()
            {
                MyLitMasterNode.PositionSlotId,
                MyLitMasterNode.VertexNormalSlotId,
                MyLitMasterNode.VertexTangentSlotId
            },
            UseInPreview = false
        };

        Pass m_PassDepthForwardOnly = new Pass()
        {
            Name = "DepthForwardOnly",
            LightMode = "DepthForwardOnly",
            TemplateName = "MyLitPass.template",
            MaterialName = "MyLit",
            ShaderPassName = "SHADERPASS_DEPTH_ONLY",
            ZWriteOverride = "ZWrite On",
            CullOverride = HDSubShaderUtilities.defaultCullMode,

            ExtraDefines = HDSubShaderUtilities.s_ExtraDefinesForwardMaterialDepthOrMotion,
            Includes = new List<string>()
            {
                "#include \"Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl\"",
            },
            // Code path for WRITE_NORMAL_BUFFER
            //
            // See MyLit.hlsl:ConvertSurfaceDataToNormalData()
            // which ShaderPassDepthOnly uses: we need to add proper interpolators dependencies depending on WRITE_NORMAL_BUFFER.
            // In our case WRITE_NORMAL_BUFFER is always enabled here.
            // Also, we need to add PixelShaderSlots dependencies for everything potentially used there.
            // See AddPixelShaderSlotsForWriteNormalBufferPasses()
            RequiredFields = new List<string>()
            {
                "AttributesMesh.normalOS",
                "AttributesMesh.tangentOS",     // Always present as we require it also in case of Variants lighting
                "AttributesMesh.uv0",
                "AttributesMesh.uv1",
                "AttributesMesh.color",
                "AttributesMesh.uv2",           // SHADERPASS_LIGHT_TRANSPORT always uses uv2
                "AttributesMesh.uv3",           // DEBUG_DISPLAY

                "FragInputs.tangentToWorld",
                "FragInputs.positionRWS",
                "FragInputs.texCoord0",
                "FragInputs.texCoord1",
                "FragInputs.texCoord2",
                "FragInputs.texCoord3",
                "FragInputs.color",
            },
            PixelShaderSlots = new List<int>()
            {
                // see AddPixelShaderSlotsForWriteNormalBufferPasses
                MyLitMasterNode.AlphaSlotId,
                MyLitMasterNode.AlphaClipThresholdSlotId,
                MyLitMasterNode.DepthOffsetSlotId,
            },
            VertexShaderSlots = new List<int>()
            {
                MyLitMasterNode.PositionSlotId,
                MyLitMasterNode.VertexNormalSlotId,
                MyLitMasterNode.VertexTangentSlotId
            },
            UseInPreview = true,

            OnGeneratePassImpl = (IMasterNode node, ref Pass pass) =>
            {
                var masterNode = node as MyLitMasterNode;
                HDSubShaderUtilities.SetStencilStateForDepth(ref pass);

                // See comments in:
                AddPixelShaderSlotsForWriteNormalBufferPasses(masterNode, ref pass);
            }
        };

        Pass m_PassMotionVectors = new Pass()
        {
            Name = "MotionVectors",
            LightMode = "MotionVectors",
            TemplateName = "MyLitPass.template",
            MaterialName = "MyLit",
            ShaderPassName = "SHADERPASS_MOTION_VECTORS",
            CullOverride = HDSubShaderUtilities.defaultCullMode,
            ExtraDefines = HDSubShaderUtilities.s_ExtraDefinesForwardMaterialDepthOrMotion,
            Includes = new List<string>()
            {
                "#include \"Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassMotionVectors.hlsl\"",
            },
            RequiredFields = new List<string>()
            {
                "AttributesMesh.normalOS",
                "AttributesMesh.tangentOS",     // Always present as we require it also in case of Variants lighting
                "AttributesMesh.uv0",
                "AttributesMesh.uv1",
                "AttributesMesh.color",
                "AttributesMesh.uv2",           // SHADERPASS_LIGHT_TRANSPORT always uses uv2
                "AttributesMesh.uv3",           // DEBUG_DISPLAY

                "FragInputs.tangentToWorld",
                "FragInputs.positionRWS",
                "FragInputs.texCoord0",
                "FragInputs.texCoord1",
                "FragInputs.texCoord2",
                "FragInputs.texCoord3",
                "FragInputs.color",
            },

            PixelShaderSlots = new List<int>()
            {
                // see AddPixelShaderSlotsForWriteNormalBufferPasses
                MyLitMasterNode.AlphaSlotId,
                MyLitMasterNode.AlphaClipThresholdSlotId,
                MyLitMasterNode.DepthOffsetSlotId,
            },
            VertexShaderSlots = new List<int>()
            {
                MyLitMasterNode.PositionSlotId,
                MyLitMasterNode.VertexNormalSlotId,
                MyLitMasterNode.VertexTangentSlotId
            },
            UseInPreview = false,

            OnGeneratePassImpl = (IMasterNode node, ref Pass pass) =>
            {
                var masterNode = node as MyLitMasterNode;
                HDSubShaderUtilities.SetStencilStateForMotionVector(ref pass);

                // For WRITE_NORMAL_BUFFER, see comments for pass m_PassDepthForwardOnly and in:
                AddPixelShaderSlotsForWriteNormalBufferPasses(masterNode, ref pass);
            }
        };

        Pass m_PassDistortion = new Pass()
        {
            Name = "DistortionVectors",
            LightMode = "DistortionVectors",
            TemplateName = "MyLitPass.template",
            MaterialName = "MyLit",
            ShaderPassName = "SHADERPASS_DISTORTION",
            ZWriteOverride = "ZWrite Off",
            CullOverride = HDSubShaderUtilities.defaultCullMode,
            Includes = new List<string>()
            {
                "#include \"Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDistortion.hlsl\"",
            },
            StencilOverride = new List<string>()
            {
                "// Stencil setup",
                "Stencil",
                "{",
                string.Format("   WriteMask {0}", (int)StencilUsage.DistortionVectors),
                string.Format("   Ref  {0}", (int)StencilUsage.DistortionVectors),
                "   Comp Always",
                "   Pass Replace",
                "}"
            },
            PixelShaderSlots = new List<int>()
            {
                MyLitMasterNode.AlphaSlotId,
                MyLitMasterNode.AlphaClipThresholdSlotId,
                MyLitMasterNode.DistortionSlotId,
                MyLitMasterNode.DistortionBlurSlotId,
                MyLitMasterNode.DepthOffsetSlotId,
            },
            VertexShaderSlots = new List<int>()
            {
                MyLitMasterNode.PositionSlotId,
                MyLitMasterNode.VertexNormalSlotId,
                MyLitMasterNode.VertexTangentSlotId
            },
            UseInPreview = true,

            OnGeneratePassImpl = (IMasterNode node, ref Pass pass) =>
            {
                var masterNode = node as MyLitMasterNode;
                if (masterNode.distortionDepthTest.isOn)
                {
                    pass.ZTestOverride = "ZTest LEqual";
                }
                else
                {
                    pass.ZTestOverride = "ZTest Always";
                }
                if (masterNode.distortionMode == DistortionMode.Add)
                {
                    pass.BlendOverride = "Blend One One, One One";
                    pass.BlendOpOverride = "BlendOp Add, Add";
                }
                else if (masterNode.distortionMode == DistortionMode.Multiply)
                {
                    pass.BlendOverride = "Blend DstColor Zero, DstAlpha Zero";
                    pass.BlendOpOverride = "BlendOp Add, Add";
                }
                else // (masterNode.distortionMode == DistortionMode.Replace)
                {
                    pass.BlendOverride = "Blend One Zero, One Zero";
                    pass.BlendOpOverride = "BlendOp Add, Add";
                }
            }
        };

        Pass m_PassForwardOnly = new Pass()
        {
            Name = "ForwardOnly",
            LightMode = "ForwardOnly",
            TemplateName = "MyLitPass.template",
            MaterialName = "MyLit",
            ShaderPassName = "SHADERPASS_FORWARD",
            CullOverride = HDSubShaderUtilities.cullModeForward,
            ZTestOverride = HDSubShaderUtilities.zTestDepthEqualForOpaque,
            ZWriteOverride = HDSubShaderUtilities.ZWriteDefault,
            // ExtraDefines are set when the pass is generated
            Includes = new List<string>()
            {
                "#include \"Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassForward.hlsl\"",
            },
            RequiredFields = new List<string>()
            {
                "AttributesMesh.normalOS",
                "AttributesMesh.tangentOS",     // Always present as we require it also in case of Variants lighting
                "AttributesMesh.uv0",
                "AttributesMesh.uv1",
                "AttributesMesh.color",
                "AttributesMesh.uv2",           // SHADERPASS_LIGHT_TRANSPORT always uses uv2
                "AttributesMesh.uv3",           // DEBUG_DISPLAY

                "FragInputs.tangentToWorld",
                "FragInputs.positionRWS",
                "FragInputs.texCoord0",
                "FragInputs.texCoord1",
                "FragInputs.texCoord2",
                "FragInputs.texCoord3",
                "FragInputs.color",
            },
            PixelShaderSlots = new List<int>()
            {
                MyLitMasterNode.BaseColorSlotId,
                MyLitMasterNode.NormalSlotId,
                MyLitMasterNode.BentNormalSlotId,
                MyLitMasterNode.TangentSlotId,
                MyLitMasterNode.SubsurfaceMaskSlotId,
                MyLitMasterNode.ThicknessSlotId,
                MyLitMasterNode.DiffusionProfileHashSlotId,
                MyLitMasterNode.IridescenceMaskSlotId,
                MyLitMasterNode.IridescenceThicknessSlotId,
                MyLitMasterNode.IridescenceCoatFixupTIRSlotId,
                MyLitMasterNode.IridescenceCoatFixupTIRClampSlotId,
                MyLitMasterNode.SpecularColorSlotId,
                MyLitMasterNode.DielectricIorSlotId,
                MyLitMasterNode.MetallicSlotId,
                MyLitMasterNode.EmissionSlotId,
                MyLitMasterNode.SmoothnessASlotId,
                MyLitMasterNode.SmoothnessBSlotId,
                MyLitMasterNode.AmbientOcclusionSlotId,
                MyLitMasterNode.AlphaSlotId,
                MyLitMasterNode.AlphaClipThresholdSlotId,
                MyLitMasterNode.AnisotropyASlotId,
                MyLitMasterNode.AnisotropyBSlotId,
                MyLitMasterNode.SpecularAAScreenSpaceVarianceSlotId,
                MyLitMasterNode.SpecularAAThresholdSlotId,
                MyLitMasterNode.CoatSmoothnessSlotId,
                MyLitMasterNode.CoatIorSlotId,
                MyLitMasterNode.CoatThicknessSlotId,
                MyLitMasterNode.CoatExtinctionSlotId,
                MyLitMasterNode.CoatNormalSlotId,
                MyLitMasterNode.CoatMaskSlotId,
                MyLitMasterNode.LobeMixSlotId,
                MyLitMasterNode.HazinessSlotId,
                MyLitMasterNode.HazeExtentSlotId,
                MyLitMasterNode.HazyGlossMaxDielectricF0SlotId,
                MyLitMasterNode.SpecularOcclusionSlotId,
                MyLitMasterNode.SOFixupVisibilityRatioThresholdSlotId,
                MyLitMasterNode.SOFixupStrengthFactorSlotId,
                MyLitMasterNode.SOFixupMaxAddedRoughnessSlotId,
                MyLitMasterNode.LightingSlotId,
                MyLitMasterNode.BackLightingSlotId,
                MyLitMasterNode.DepthOffsetSlotId,
            },
            VertexShaderSlots = new List<int>()
            {
                MyLitMasterNode.PositionSlotId,
                MyLitMasterNode.VertexNormalSlotId,
                MyLitMasterNode.VertexTangentSlotId
            },
            UseInPreview = true,

            OnGeneratePassImpl = (IMasterNode node, ref Pass pass) =>
            {
                var masterNode = node as MyLitMasterNode;
                HDSubShaderUtilities.SetStencilStateForForward(ref pass);
                HDSubShaderUtilities.SetBlendModeForForward(ref pass);

                pass.ExtraDefines.Remove("#ifndef DEBUG_DISPLAY\n#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST\n#endif");

                if (masterNode.surfaceType == SurfaceType.Opaque && masterNode.alphaTest.isOn)
                {
                    // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                    // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                    pass.ExtraDefines.Add("#ifndef DEBUG_DISPLAY\n#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST\n#endif");
                    pass.ZTestOverride = "ZTest Equal";
                }
            }
        };

        public int GetPreviewPassIndex() { return 0; }

        private static void AddPixelShaderSlotsForWriteNormalBufferPasses(MyLitMasterNode masterNode, ref Pass pass)
        {
            // See MyLit.hlsl:ConvertSurfaceDataToNormalData()
            // Note: We remove the slots we're adding as the editor will constantly regenerate the shader but
            // without recreating the nodes thus the passes in the subshader object.
            if (masterNode.coat.isOn)
            {
                // Check ConvertSurfaceDataToNormalData, in this case, we only need those:
                pass.PixelShaderSlots.Remove(MyLitMasterNode.CoatSmoothnessSlotId);
                pass.PixelShaderSlots.Add(MyLitMasterNode.CoatSmoothnessSlotId);
                pass.PixelShaderSlots.Remove(MyLitMasterNode.CoatNormalSlotId);
                pass.PixelShaderSlots.Add(MyLitMasterNode.CoatNormalSlotId);
            }
            else
            {
                pass.PixelShaderSlots.Remove(MyLitMasterNode.NormalSlotId);
                pass.PixelShaderSlots.Add(MyLitMasterNode.NormalSlotId);
                pass.PixelShaderSlots.Remove(MyLitMasterNode.LobeMixSlotId);
                pass.PixelShaderSlots.Add(MyLitMasterNode.LobeMixSlotId);
                pass.PixelShaderSlots.Remove(MyLitMasterNode.SmoothnessASlotId);
                pass.PixelShaderSlots.Add(MyLitMasterNode.SmoothnessASlotId);
                pass.PixelShaderSlots.Remove(MyLitMasterNode.SmoothnessBSlotId);
                pass.PixelShaderSlots.Add(MyLitMasterNode.SmoothnessBSlotId);
            }
            // Also, when geometricSpecularAA.isOn, might want to add SpecularAAScreenSpaceVarianceSlotId and SpecularAAThresholdSlotId,
            // since they affect smoothnesses in surfaceData directly. This is an implicit behavior for Lit, via the GBuffer pass.
            // Versus performance, might not be important for what it is used for, but SSR uses the roughness too so for now,
            // add them.
            if (masterNode.geometricSpecularAA.isOn) // TODOTODO || Normal Map Filtering is on
            {
                pass.PixelShaderSlots.Remove(MyLitMasterNode.SpecularAAScreenSpaceVarianceSlotId);
                pass.PixelShaderSlots.Add(MyLitMasterNode.SpecularAAScreenSpaceVarianceSlotId);
                pass.PixelShaderSlots.Remove(MyLitMasterNode.SpecularAAThresholdSlotId);
                pass.PixelShaderSlots.Add(MyLitMasterNode.SpecularAAThresholdSlotId);
            }
        }

        //
        // Reference for GetActiveFieldsFromMasterNode
        // -------------------------------------------
        //
        // Properties (enables etc):
        //
        //  ok+MFD -> material feature define: means we need a predicate, because we will transform it into a #define that match the material feature, shader_feature-defined, that the rest of the shader code uses.
        //
        //  ok+MFD masterNode.baseParametrization    --> even though we can just always transfer present fields (check with $SurfaceDescription.*) like specularcolor and metallic,
        //                                               we need to translate this into the _MATERIAL_FEATURE_SPECULAR_COLOR define.
        //
        //  ok masterNode.energyConservingSpecular
        //
        //  ~~~~ ok+MFD: these are almost all material features:
        //  masterNode.anisotropy
        //  masterNode.coat
        //  masterNode.coatNormal
        //  masterNode.dualSpecularLobe
        //  masterNode.dualSpecularLobeParametrization
        //  masterNode.capHazinessWrtMetallic           -> not a material feature define, as such, we will create a combined predicate for the HazyGlossMaxDielectricF0 slot dependency
        //                                                 instead of adding a #define in the template...
        //  masterNode.iridescence
        //  masterNode.subsurfaceScattering
        //  masterNode.transmission
        //
        //  ~~~~ ...ok+MFD: these are all material features
        //
        //  ok masterNode.receiveDecals
        //  ok masterNode.receiveSSR
        //  ok masterNode.geometricSpecularAA    --> check, a way to combine predicates and/or exclude passes: TODOTODO What about WRITE_NORMAL_BUFFER passes ? (ie smoothness)
        //  ok masterNode.specularOcclusion      --> no use for it though! see comments.
        //
        //  ~~~~ ok+D: these require translation to defines also...
        //
        //  masterNode.anisotropyForAreaLights
        //  masterNode.recomputeStackPerLight
        //  masterNode.shadeBaseUsingRefractedAngles
        //  masterNode.debug

        // Inputs: Most inputs don't need a specific predicate in addition to the "present field predicate", ie the $SurfaceDescription.*,
        //         but in some special cases we check connectivity to avoid processing the default value for nothing...
        //         (see specular occlusion with _MASKMAP and _BENTNORMALMAP in LitData, or _TANGENTMAP, _BENTNORMALMAP, etc. which act a bit like that
        //         although they also avoid sampling in that case, but default tiny texture map sampling isn't a big hit since they are all cached once
        //         a default "unityTexWhite" is sampled, it is cached for everyone defaulting to white...)
        //
        // ok+ means there's a specific additional predicate
        //
        // ok masterNode.BaseColorSlotId
        // ok masterNode.NormalSlotId
        //
        // ok+ masterNode.BentNormalSlotId     --> Dependency of the predicate on IsSlotConnected avoids processing even if the slots
        // ok+ masterNode.TangentSlotId            are always there so any pass that declares its use in PixelShaderSlots will have the field in SurfaceDescription,
        //                                         but it's not necessarily useful (if slot isnt connected, waste processing on potentially static expressions if
        //                                         shader compiler cant optimize...and even then, useless to have static override value for those.)
        //
        //                                         TODOTODO: Note you could have the same argument for NormalSlot (which we dont exclude with a predicate).
        //                                         Also and anyways, the compiler is smart enough not to do the TS to WS matrix multiply on a (0,0,1) vector.
        //
        // ok+ masterNode.CoatNormalSlotId       -> we already have a "material feature" coat normal map so can use that instead, although using that former, we assume the coat normal slot
        //                                         will be there, but it's ok, we can #ifdef the code on the material feature define, and use the $SurfaceDescription.CoatNormal predicate
        //                                         for the actual assignment,
        //                                         although for that one we could again
        //                                         use the "connected" condition like for tangent and bentnormal
        //
        // The following are all ok, no need beyond present field predicate, ie $SurfaceDescription.*,
        // except special cases where noted
        //
        // ok masterNode.SubsurfaceMaskSlotId
        // ok masterNode.ThicknessSlotId
        // ok masterNode.DiffusionProfileHashSlotId
        // ok masterNode.IridescenceMaskSlotId
        // ok masterNode.IridescenceThicknessSlotId
        // ok masterNode.SpecularColorSlotId
        // ok masterNode.DielectricIorSlotId
        // ok masterNode.MetallicSlotId
        // ok masterNode.EmissionSlotId
        // ok masterNode.SmoothnessASlotId
        // ok masterNode.SmoothnessBSlotId
        // ok+ masterNode.AmbientOcclusionSlotId    -> defined a specific predicate, but not used, see MyLitData.
        // ok masterNode.AlphaSlotId
        // ok masterNode.AlphaClipThresholdSlotId
        // ok masterNode.AnisotropyASlotId
        // ok masterNode.AnisotropyBSlotId
        // ok masterNode.SpecularAAScreenSpaceVarianceSlotId
        // ok masterNode.SpecularAAThresholdSlotId
        // ok masterNode.CoatSmoothnessSlotId
        // ok masterNode.CoatIorSlotId
        // ok masterNode.CoatThicknessSlotId
        // ok masterNode.CoatExtinctionSlotId
        // ok masterNode.LobeMixSlotId
        // ok masterNode.HazinessSlotId
        // ok masterNode.HazeExtentSlotId
        // ok masterNode.HazyGlossMaxDielectricF0SlotId     -> No need for a predicate, the needed predicate is the combined (capHazinessWrtMetallic + HazyGlossMaxDielectricF0)
        //                                                     "leaking case": if the 2 are true, but we're not in metallic mode, the capHazinessWrtMetallic property is wrong,
        //                                                     that means the master node is really misconfigured, spew an error, should never happen...
        //                                                     If it happens, it's because we forgot UpdateNodeAfterDeserialization() call when modifying the capHazinessWrtMetallic or baseParametrization
        //                                                     properties, maybe through debug etc.
        //
        // ok masterNode.DistortionSlotId            -> Warning: peculiarly, instead of using $SurfaceDescription.Distortion and DistortionBlur,
        // ok masterNode.DistortionBlurSlotId           we do an #if (SHADERPASS == SHADERPASS_DISTORTION) in the template, instead of
        //                                              relying on other passed NOT to include the DistortionSlotId in their PixelShaderSlots!!

        // Other to deal with, and
        // Common between Lit and MyLit:
        //
        // doubleSidedMode, alphaTest, receiveDecals,
        // surfaceType, alphaMode, blendPreserveSpecular, transparencyFog,
        // distortion, distortionMode, distortionDepthTest,
        // sortPriority (int)
        // geometricSpecularAA, energyConservingSpecular, specularOcclusion
        //

        private static ActiveFields GetActiveFieldsFromMasterNode(AbstractMaterialNode iMasterNode, Pass pass)
        {
            var activeFields = new ActiveFields();
            var baseActiveFields = activeFields.baseInstance;

            MyLitMasterNode masterNode = iMasterNode as MyLitMasterNode;
            if (masterNode == null)
            {
                return activeFields;
            }

            if (masterNode.doubleSidedMode != DoubleSidedMode.Disabled)
            {
                if (pass.ShaderPassName != "SHADERPASS_MOTION_VECTORS")   // HACK to get around lack of a good interpolator dependency system
                {                                                   // we need to be able to build interpolators using multiple input structs
                                                                    // also: should only require isFrontFace if Normals are required...
                    // Important: the following is used in SharedCode.template.hlsl for determining the normal flip mode
                    baseActiveFields.Add("FragInputs.isFrontFace");
                }
            }

            if (masterNode.alphaTest.isOn)
            {
                if (pass.PixelShaderUsesSlot(MyLitMasterNode.AlphaClipThresholdSlotId))
                {
                    baseActiveFields.Add("AlphaTest");
                }
            }

            if (masterNode.surfaceType != SurfaceType.Opaque)
            {
                if (masterNode.transparencyFog.isOn)
                {
                    baseActiveFields.Add("AlphaFog");
                }

                if (masterNode.blendPreserveSpecular.isOn)
                {
                    baseActiveFields.Add("BlendMode.PreserveSpecular");
                }
            }

            switch(masterNode.normalDropOffSpace)
            {
                case NormalDropOffSpace.Tangent:
                    baseActiveFields.AddAll("NormalDropOffTS");
                    break;
                case NormalDropOffSpace.Object:
                    baseActiveFields.AddAll("NormalDropOffOS");
                    break;
                case NormalDropOffSpace.World:
                    baseActiveFields.AddAll("NormalDropOffWS");
                    break;
                default:
                    UnityEngine.Debug.LogError("Unknown normal drop off space: " + masterNode.normalDropOffSpace);
                    break;
            }

            //
            // Predicates to change into defines:
            //

            // Even though we can just always transfer the present (check with $SurfaceDescription.*) fields like specularcolor
            // and metallic, we still need to know the baseParametrization in the template to translate into the
            // _MATERIAL_FEATURE_SPECULAR_COLOR define:
            if (masterNode.baseParametrization == MyLit.BaseParametrization.SpecularColor)
            {
                baseActiveFields.Add("BaseParametrization.SpecularColor");
            }
            if (masterNode.energyConservingSpecular.isOn) // No defines, suboption of BaseParametrization.SpecularColor
            {
                baseActiveFields.Add("EnergyConservingSpecular");
            }
            if (masterNode.anisotropy.isOn)
            {
                baseActiveFields.Add("Material.Anisotropy");
            }
            if (masterNode.coat.isOn)
            {
                baseActiveFields.Add("Material.Coat");
                if (pass.PixelShaderUsesSlot(MyLitMasterNode.CoatMaskSlotId))
                {
                    var coatMaskSlot = masterNode.FindSlot<Vector1MaterialSlot>(MyLitMasterNode.CoatMaskSlotId);
                    bool connected = masterNode.IsSlotConnected(MyLitMasterNode.CoatMaskSlotId);

                    if (connected || (coatMaskSlot.value != 0.0f && coatMaskSlot.value != 1.0f))
                    {
                        baseActiveFields.Add("CoatMask");
                    }
                    else if (coatMaskSlot.value == 0.0f)
                    {
                        baseActiveFields.Add("CoatMaskZero");
                    }
                    else if (coatMaskSlot.value == 1.0f)
                    {
                        baseActiveFields.Add("CoatMaskOne");
                    }
                }
            }
            if (masterNode.coatNormal.isOn)
            {
                baseActiveFields.Add("Material.CoatNormal");
            }
            if (masterNode.dualSpecularLobe.isOn)
            {
                baseActiveFields.Add("Material.DualSpecularLobe");
                if (masterNode.dualSpecularLobeParametrization == MyLit.DualSpecularLobeParametrization.HazyGloss)
                {
                    baseActiveFields.Add("DualSpecularLobeParametrization.HazyGloss");
                    // Option for baseParametrization == Metallic && DualSpecularLobeParametrization == HazyGloss:
                    if (masterNode.capHazinessWrtMetallic.isOn && pass.PixelShaderUsesSlot(MyLitMasterNode.HazyGlossMaxDielectricF0SlotId))
                    {
                        // check the supporting slot is there (although masternode should deal with having a consistent property config)
                        var maxDielectricF0Slot = masterNode.FindSlot<Vector1MaterialSlot>(MyLitMasterNode.HazyGlossMaxDielectricF0SlotId);

                        if (maxDielectricF0Slot != null)
                        {
                            // Again we assume masternode has HazyGlossMaxDielectricF0 which should always be the case
                            // if capHazinessWrtMetallic.isOn.
                            baseActiveFields.Add("CapHazinessIfNotMetallic");
                        }
                    }
                }
            }
            if (masterNode.iridescence.isOn)
            {
                baseActiveFields.Add("Material.Iridescence");
            }
            if (masterNode.subsurfaceScattering.isOn && masterNode.surfaceType != SurfaceType.Transparent)
            {
                baseActiveFields.Add("Material.SubsurfaceScattering");
            }
            if (masterNode.transmission.isOn)
            {
                baseActiveFields.Add("Material.Transmission");
            }

            // Advanced:
            if (masterNode.anisotropyForAreaLights.isOn)
            {
                baseActiveFields.Add("AnisotropyForAreaLights");
            }
            if (masterNode.recomputeStackPerLight.isOn)
            {
                baseActiveFields.Add("RecomputeStackPerLight");
            }
            if (masterNode.honorPerLightMinRoughness.isOn)
            {
                baseActiveFields.Add("HonorPerLightMinRoughness");
            }
            if (masterNode.shadeBaseUsingRefractedAngles.isOn)
            {
                baseActiveFields.Add("ShadeBaseUsingRefractedAngles");
            }
            if (masterNode.debug.isOn)
            {
                baseActiveFields.Add("MyLitDebug");
            }

            //
            // Other property predicates:
            //

            if (!masterNode.receiveDecals.isOn)
            {
                baseActiveFields.Add("DisableDecals");
            }

            if (!masterNode.receiveSSR.isOn)
            {
                baseActiveFields.Add("DisableSSR");
            }

            if (masterNode.addPrecomputedVelocity.isOn)
            {
                baseActiveFields.Add("AddPrecomputedVelocity");
            }

            // Note here we combine an "enable"-like predicate and the $SurfaceDescription.(slotname) predicate
            // into a single $GeometricSpecularAA pedicate.
            //
            // ($SurfaceDescription.* predicates are useful to make sure the field is present in the struct in the template.
            // The field will be present if both the master node and pass have the slotid, see this set intersection we make
            // in GenerateSurfaceDescriptionStruct(), with HDSubShaderUtilities.FindMaterialSlotsOnNode().)
            //
            // Normally, since the feature enable adds the required slots, only the $SurfaceDescription.* would be required,
            // but some passes might not need it and not declare the PixelShaderSlot, or, inversely, the pass might not
            // declare it as a way to avoid it.
            //
            // IE this has also the side effect to disable geometricSpecularAA - even if "on" - for passes that don't explicitly
            // advertise these slots(eg for a general feature, with separate "enable" and "field present" predicates, the
            // template could take a default value and process it anyway if a feature is "on").
            //
            // (Note we can achieve the same results in the template on just single predicates by making defines out of them,
            // and using #if defined() && etc)
            bool haveSomeSpecularAA = false; // TODOTODO in prevision of normal texture filtering
            if (masterNode.geometricSpecularAA.isOn
                && pass.PixelShaderUsesSlot(MyLitMasterNode.SpecularAAThresholdSlotId)
                && pass.PixelShaderUsesSlot(MyLitMasterNode.SpecularAAScreenSpaceVarianceSlotId))
            {
                haveSomeSpecularAA = true;
                baseActiveFields.Add("GeometricSpecularAA");
            }
            if (haveSomeSpecularAA)
            {
                baseActiveFields.Add("SpecularAA");
            }

            if (masterNode.screenSpaceSpecularOcclusionBaseMode != MyLitMasterNode.SpecularOcclusionBaseMode.Off
                || masterNode.dataBasedSpecularOcclusionBaseMode != MyLitMasterNode.SpecularOcclusionBaseMode.Off)
            {
                // activates main define
                baseActiveFields.Add("SpecularOcclusion");
            }

            baseActiveFields.Add("ScreenSpaceSpecularOcclusionBaseMode." + masterNode.screenSpaceSpecularOcclusionBaseMode.ToString());
            if (MyLitMasterNode.SpecularOcclusionModeUsesVisibilityCone(masterNode.screenSpaceSpecularOcclusionBaseMode))
            {
                baseActiveFields.Add("ScreenSpaceSpecularOcclusionAOConeSize." + masterNode.screenSpaceSpecularOcclusionAOConeSize.ToString());
                baseActiveFields.Add("ScreenSpaceSpecularOcclusionAOConeDir." + masterNode.screenSpaceSpecularOcclusionAOConeDir.ToString());
            }

            baseActiveFields.Add("DataBasedSpecularOcclusionBaseMode." + masterNode.dataBasedSpecularOcclusionBaseMode.ToString());
            if (MyLitMasterNode.SpecularOcclusionModeUsesVisibilityCone(masterNode.dataBasedSpecularOcclusionBaseMode))
            {
                baseActiveFields.Add("DataBasedSpecularOcclusionAOConeSize." + masterNode.dataBasedSpecularOcclusionAOConeSize.ToString());
            }

            // Set bent normal fixup predicate if needed:
            if (masterNode.SpecularOcclusionUsesBentNormal())
            {
                baseActiveFields.Add("SpecularOcclusionConeFixupMethod." + masterNode.specularOcclusionConeFixupMethod.ToString());
            }

            //
            // Input special-casing predicates:
            //

            if (masterNode.IsSlotConnected(MyLitMasterNode.BentNormalSlotId) && pass.PixelShaderUsesSlot(MyLitMasterNode.BentNormalSlotId))
            {
                baseActiveFields.Add("BentNormal");
            }

            if (masterNode.IsSlotConnected(MyLitMasterNode.TangentSlotId) && pass.PixelShaderUsesSlot(MyLitMasterNode.TangentSlotId))
            {
                baseActiveFields.Add("Tangent");
            }

            // The following idiom enables an optimization on feature ports that don't have an enable switch in the settings
            // view, where the default value might not produce a visual result and incur a processing cost we want to avoid.
            // For ambient occlusion, this is the case for the SpecularOcclusion calculations which also depend on it,
            // where a value of 1 will produce no results.
            // See SpecularOcclusion, we don't optimize out this case...
            if (pass.PixelShaderUsesSlot(MyLitMasterNode.AmbientOcclusionSlotId))
            {
                bool connected = masterNode.IsSlotConnected(MyLitMasterNode.AmbientOcclusionSlotId);
                var ambientOcclusionSlot = masterNode.FindSlot<Vector1MaterialSlot>(MyLitMasterNode.AmbientOcclusionSlotId);
                // master node always has it, assert ambientOcclusionSlot != null
                if (connected || ambientOcclusionSlot.value != ambientOcclusionSlot.defaultValue)
                {
                    baseActiveFields.Add("AmbientOcclusion");
                }
            }

            if (masterNode.IsSlotConnected(MyLitMasterNode.CoatNormalSlotId) && pass.PixelShaderUsesSlot(MyLitMasterNode.CoatNormalSlotId))
            {
                baseActiveFields.Add("CoatNormal");
            }

            if (masterNode.IsSlotConnected(MyLitMasterNode.LightingSlotId)&& pass.PixelShaderUsesSlot(MyLitMasterNode.LightingSlotId))
            {
                baseActiveFields.Add("LightingGI");
            }
            if (masterNode.IsSlotConnected(MyLitMasterNode.BackLightingSlotId)&& pass.PixelShaderUsesSlot(MyLitMasterNode.BackLightingSlotId))
            {
                baseActiveFields.Add("BackLightingGI");
            }

            if (masterNode.depthOffset.isOn && pass.PixelShaderUsesSlot(MyLitMasterNode.DepthOffsetSlotId))
                baseActiveFields.Add("DepthOffset");

            if (masterNode.supportLodCrossFade.isOn)
                baseActiveFields.AddAll("LodCrossFade");

            return activeFields;
        }

        private static bool GenerateShaderPassLit(MyLitMasterNode masterNode, Pass pass, GenerationMode mode, ShaderGenerator result, List<string> sourceAssetDependencyPaths)
        {
            if (mode == GenerationMode.ForReals || pass.UseInPreview)
            {
                pass.OnGeneratePass(masterNode);

                // apply master node options to active fields
                var activeFields = GetActiveFieldsFromMasterNode(masterNode, pass);

                // use standard shader pass generation
                bool vertexActive = false;
                if (masterNode.IsSlotConnected(MyLitMasterNode.PositionSlotId) ||
                    masterNode.IsSlotConnected(MyLitMasterNode.VertexNormalSlotId) ||
                    masterNode.IsSlotConnected(MyLitMasterNode.VertexTangentSlotId) )
                {
                    vertexActive = true;
                }
                return HDSubShaderUtilities.GenerateShaderPass(masterNode, pass, mode, activeFields, result, sourceAssetDependencyPaths, vertexActive);
            }
            else
            {
                return false;
            }
        }

        public string GetSubshader(IMasterNode iMasterNode, GenerationMode mode, List<string> sourceAssetDependencyPaths = null)
        {
            if (sourceAssetDependencyPaths != null)
            {
                // MyLitSubShader.cs
                sourceAssetDependencyPaths.Add(AssetDatabase.GUIDToAssetPath("9649efe3e0e8e2941a983bb0f3a034ad"));
                // HDSubShaderUtilities.cs
                sourceAssetDependencyPaths.Add(AssetDatabase.GUIDToAssetPath("713ced4e6eef4a44799a4dd59041484b"));
            }

            var masterNode = iMasterNode as MyLitMasterNode;

            var subShader = new ShaderGenerator();
            subShader.AddShaderChunk("SubShader", true);
            subShader.AddShaderChunk("{", true);
            subShader.Indent();
            {
                // Add tags at the SubShader level
                var renderingPass = masterNode.surfaceType == SurfaceType.Opaque ? HDRenderQueue.RenderQueueType.Opaque : HDRenderQueue.RenderQueueType.Transparent;
                int queue = HDRenderQueue.ChangeType(renderingPass, masterNode.sortPriority, masterNode.alphaTest.isOn);
                HDSubShaderUtilities.AddTags(subShader, HDRenderPipeline.k_ShaderTagName, HDRenderTypeTags.HDLitShader, queue);

                // generate the necessary shader passes
                bool opaque = (masterNode.surfaceType == SurfaceType.Opaque);
                bool transparent = !opaque;

                bool distortionActive = transparent && masterNode.distortion.isOn;

                GenerateShaderPassLit(masterNode, m_PassShadowCaster, mode, subShader, sourceAssetDependencyPaths);
                GenerateShaderPassLit(masterNode, m_PassMETA, mode, subShader, sourceAssetDependencyPaths);
                GenerateShaderPassLit(masterNode, m_SceneSelectionPass, mode, subShader, sourceAssetDependencyPaths);

                GenerateShaderPassLit(masterNode, m_PassDepthForwardOnly, mode, subShader, sourceAssetDependencyPaths);
                GenerateShaderPassLit(masterNode, m_PassMotionVectors, mode, subShader, sourceAssetDependencyPaths);

                if (distortionActive)
                {
                    GenerateShaderPassLit(masterNode, m_PassDistortion, mode, subShader, sourceAssetDependencyPaths);
                }

                // Assign define here based on opaque or transparent to save some variant
                m_PassForwardOnly.ExtraDefines = opaque ? HDSubShaderUtilities.s_ExtraDefinesForwardOpaque : HDSubShaderUtilities.s_ExtraDefinesForwardTransparent;
                GenerateShaderPassLit(masterNode, m_PassForwardOnly, mode, subShader, sourceAssetDependencyPaths);
            }

            subShader.Deindent();
            subShader.AddShaderChunk("}", true);

            if (!masterNode.OverrideEnabled)
            {
                subShader.AddShaderChunk(@"CustomEditor ""UnityEditor.Rendering.HighDefinition.MyLitGUI""");
            }

            return subShader.GetShaderString(0);
        }

        public bool IsPipelineCompatible(RenderPipelineAsset renderPipelineAsset)
        {
            return renderPipelineAsset is HDRenderPipelineAsset;
        }
    }
}
