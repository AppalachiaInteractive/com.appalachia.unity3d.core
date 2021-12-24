#region

using Appalachia.Core.Attributes;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Logging;

#endregion

namespace Appalachia.Core.Aspects.Profiling
{
    [CallStaticConstructorInEditor]
    public static class PROFILING
    {
        #region MENU

#if UNITY_EDITOR

        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.RootTools.Base + "Profiling/Enabled" + SHC.CTRL_ALT_SHFT_P,
            true
        )]
        private static bool Profiling_ToggleProfilerValidate()
        {
            UnityEditor.Menu.SetChecked("Profiling/Enabled", UnityEditorInternal.ProfilerDriver.enabled);
            return true;
        }

        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.RootTools.Base + "Profiling/Enabled" + SHC.CTRL_ALT_SHFT_P,
            priority = 0
        )]
        public static void Profiling_ToggleProfiler()
        {
            if (UnityEditorInternal.ProfilerDriver.enabled)
            {
                UnityEditor.EditorApplication.delayCall += Profiling_Disable;
            }
            else
            {
                UnityEditor.EditorApplication.delayCall += Profiling_Enable;
            }
        }

        public static void Profiling_Enable()
        {
            AppaLog.Context.Utility.Warn("Profiling starting...");
            UnityEditorInternal.ProfilerDriver.enabled = true;

            //Profiler.enabled = true;
        }

        public static void Profiling_Disable()
        {
            AppaLog.Context.Utility.Warn("Profiling ending...");
            UnityEditorInternal.ProfilerDriver.enabled = false;

            //Profiler.enabled = false;
        }

        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.RootTools.Base + "Profiling/Clear" + SHC.CTRL_ALT_SHFT_O,
            priority = 0
        )]
        public static void Profiling_Clear()
        {
            UnityEditorInternal.ProfilerDriver.ClearAllFrames();
            AppaLog.Context.Utility.Warn("Profiling cleared.");
        }

#endif

        #endregion
    }
}

/*private static ProfilerMarker META_PRF = new ProfilerMarker("PROFILING.Profile");
private static ProfilerMarker META_PRF_NullChecks = new ProfilerMarker("PROFILING.Profile.NullChecks");
private static ProfilerMarker META_PRF_Hashing = new ProfilerMarker("PROFILING.Profile.Hashing");
private static ProfilerMarker META_PRF_Lookups = new ProfilerMarker("PROFILING.Profile.Lookups");
private static ProfilerMarker META_PRF_Lookups_Instantiation = new ProfilerMarker("PROFILING.Profile.Lookups.Instantiation");
private static ProfilerMarker META_PRF_Lookups_Add = new ProfilerMarker("PROFILING.Profile.Lookups.Add");
private static ProfilerMarker META_PRF_Lookups_Get = new ProfilerMarker("PROFILING.Profile.Lookups.Get");


private static Dictionary<int, ProfilerMarkerWrapper> _markers = new Dictionary<int, ProfilerMarkerWrapper>();

internal static bool InternalDisable = false;

[BurstDiscard]
public static IProfile Profile(
    string additive = null,
    bool ignore = false,
    [System.Runtime.CompilerServices.CallerMemberName]
    string memberName = "",
    [System.Runtime.CompilerServices.CallerFilePath]
    string sourceFilePath = "",
    [System.Runtime.CompilerServices.CallerLineNumber]
    int sourceLineNumber = 0)
{
    if (ignore || InternalDisable) return _dummy;

    ProfilerMarkerWrapper marker;
    using (META_PRF.Auto())
    {
        using (META_PRF_NullChecks.Auto())
        {
            if (_markers == null)
            {
                _markers = new Dictionary<int, ProfilerMarkerWrapper>();
            }
        }

        var hashCode = 0;

        using (META_PRF_Hashing.Auto())
        {
            unchecked
            {
                hashCode = additive?.GetHashCode() ?? 79;
                hashCode ^= memberName?.GetHashCode() ?? 61;
                hashCode ^= sourceFilePath?.GetHashCode() ?? 127;
                if (sourceLineNumber != 0) hashCode ^= sourceLineNumber;
            }
        }

        using (META_PRF_Lookups.Auto())
        {
            if (!_markers.ContainsKey(hashCode))
            {
                using (META_PRF_Lookups_Instantiation.Auto())
                {
                    marker = new ProfilerMarkerWrapper(memberName, sourceFilePath, sourceLineNumber);
                }

                using (META_PRF_Lookups_Add.Auto())
                {
                    _markers.Add(hashCode, marker);
                }
            }
            else
            {
                using (META_PRF_Lookups_Get.Auto())
                {
                    marker = _markers[hashCode];
                }
            }
        }
    }

    marker.Start();

    return marker;
}

private static readonly DummyDisposable _dummy = new DummyDisposable();
*/

#region Old

/*
public static class MESH_BURY_STATE
{
    private const string CLASS = "MeshBurialState" + ".";
    public static ProfilerMarker GetMeshAsset = new ProfilerMarker(CLASS + "GetMeshAsset");
    public static ProfilerMarker ProcessMesh = new ProfilerMarker(CLASS + "ProcessMesh");
    public static ProfilerMarker ReprocessMesh = new ProfilerMarker(CLASS + "ReprocessMesh");
}

public static class MESH_BURY_STATE_MGR
{
    private const string CLASS = "MeshBurialSharedStateManager" + ".";
    public static ProfilerMarker Get = new ProfilerMarker(CLASS + "Get");
    public static ProfilerMarker GetByPrefab = new ProfilerMarker(CLASS + "GetByPrefab");
    public static ProfilerMarker GetByPrefabInstance = new ProfilerMarker(CLASS + "GetByPrefabInstance");
    public static ProfilerMarker GetByGameObject = new ProfilerMarker(CLASS + "GetByGameObject");
    public static ProfilerMarker GetByHashCode = new ProfilerMarker(CLASS + "GetByHashCode");
}

public static class MESH_BURY
{
    private const string CLASS = "MeshBury" + ".";
    public static ProfilerMarker OnDrawGizmosSelected = new ProfilerMarker(CLASS + "OnDrawGizmosSelected");
    public static ProfilerMarker ProcessMesh = new ProfilerMarker(CLASS + "ProcessMesh");
    public static ProfilerMarker ReprocessMesh = new ProfilerMarker(CLASS + "ReprocessMesh");
}

public static class MESH_OBJ
{
    private const string CLASS = "MeshObject" + ".";
    public static ProfilerMarker Populate = new ProfilerMarker(CLASS + "Populate");
    public static ProfilerMarker Refresh = new ProfilerMarker(CLASS + "Refresh");
    public static ProfilerMarker RepopulateNonSerializedFields = new ProfilerMarker(CLASS + "RepopulateNonSerializedFields");
    public static ProfilerMarker RepopulateTriangleData = new ProfilerMarker(CLASS + "RepopulateTriangleData");
    public static ProfilerMarker RepopulateEdgeData = new ProfilerMarker(CLASS + "RepopulateEdgeData");
    public static ProfilerMarker RepopulateVertexData = new ProfilerMarker(CLASS + "RepopulateVertexData");
    public static ProfilerMarker GetEdge = new ProfilerMarker(CLASS + "GetEdge");
    public static ProfilerMarker CalculateBorderEdges = new ProfilerMarker(CLASS + "CalculateBorderEdges");
    public static ProfilerMarker DrawBorders = new ProfilerMarker(CLASS + "DrawBorders");
    public static ProfilerMarker DrawTerrain = new ProfilerMarker(CLASS + "DrawTerrain");
    public static ProfilerMarker DrawVertexStatus = new ProfilerMarker(CLASS + "DrawVertexStatus");
    public static ProfilerMarker DrawTriangle = new ProfilerMarker(CLASS + "DrawTriangle");
    public static ProfilerMarker ProcessVertices = new ProfilerMarker(CLASS + "ProcessVertices");
    public static ProfilerMarker ProcessTriangles = new ProfilerMarker(CLASS + "ProcessTriangles");
    public static ProfilerMarker ProcessTriangles_Array = new ProfilerMarker(CLASS + "ProcessTriangles.Array");
    public static ProfilerMarker ProcessTriangles_Indices = new ProfilerMarker(CLASS + "ProcessTriangles.Indices");
    public static ProfilerMarker ProcessTriangles_Iterate = new ProfilerMarker(CLASS + "ProcessTriangles.Iterate");
    public static ProfilerMarker ProcessTriangles_Instantiate = new ProfilerMarker(CLASS + "ProcessTriangles.Instantiate");
}

public static class SAFE_NATIVE
{
    private const string CLASS = "SafeNative" + ".";
    public static ProfilerMarker SafeDispose_ARRAY = new ProfilerMarker(CLASS + "SafeDispose (Native Array)");
    public static ProfilerMarker SafeDispose_LIST = new ProfilerMarker(CLASS + "SafeDispose (Native List)");
    public static ProfilerMarker SafeDispose_IDISP = new ProfilerMarker(CLASS + "SafeDispose (IDisposable)");
    public static ProfilerMarker IsDisposed = new ProfilerMarker(CLASS + "IsDisposed");
}


public static class GLBL_REND_OPTS
{
    private const string CLASS = "GlobalRenderingOptions" + ".";
    public static ProfilerMarker ApplyTo = new ProfilerMarker(CLASS + "ApplyTo");
    public static ProfilerMarker MarkDirty = new ProfilerMarker(CLASS + "MarkDirty");
}

public static class RNDR_PASS_SETT
{
    private const string CLASS = "RenderPassSettings" + ".";
    public static ProfilerMarker CopyTo = new ProfilerMarker(CLASS + "CopyTo");
    public static ProfilerMarker GetOverrideLightingSettings = new ProfilerMarker(CLASS + "GetOverrideLightingSettings");
}

public static class PRFB_RNDR_MGR
{
    private const string CLASS = "PrefabRenderingManager" + ".";
    public static ProfilerMarker OnEnable = new ProfilerMarker(CLASS + "OnEnable");
    public static ProfilerMarker OnDisable = new ProfilerMarker(CLASS + "OnDisable");
    public static ProfilerMarker Update = new ProfilerMarker(CLASS + "Update");
    public static ProfilerMarker PrepareToExecuteUpdateLoop = new ProfilerMarker(CLASS + "PrepareToExecuteUpdateLoop");
    public static ProfilerMarker InitializeStructureInPlace = new ProfilerMarker(CLASS + "InitializeStructureInPlace");
    public static ProfilerMarker GetInstanceRootForPrefab = new ProfilerMarker(CLASS + "GetInstanceRootForPrefab");
    public static ProfilerMarker InitializePrefabRenderSettings = new ProfilerMarker(CLASS + "InitializePrefabRenderSettings");
    public static ProfilerMarker StartEditorSimulation = new ProfilerMarker(CLASS + "StartEditorSimulation");
    public static ProfilerMarker StopEditorSimulation = new ProfilerMarker(CLASS + "StopEditorSimulation");
    public static ProfilerMarker OnDrawGizmos = new ProfilerMarker(CLASS + "OnDrawGizmos");
    public static ProfilerMarker ReBuryMeshes = new ProfilerMarker(CLASS + "ReBuryMeshes");
    public static ProfilerMarker Bounce = new ProfilerMarker(CLASS + "Bounce");
    public static ProfilerMarker ScheduleBatchedJobs = new ProfilerMarker(CLASS + "ScheduleBatchedJobs");
    public static ProfilerMarker CompleteJobs = new ProfilerMarker(CLASS + "CompleteJobs");
    public static ProfilerMarker Options_Get = new ProfilerMarker(CLASS + "Options_Get");
    public static ProfilerMarker Options_Set = new ProfilerMarker(CLASS + "Options_Set");
    public static ProfilerMarker OnAwake = new ProfilerMarker(CLASS + "OnAwake");
    public static ProfilerMarker LateUpdate = new ProfilerMarker(CLASS + "LateUpdate");
    public static ProfilerMarker RunOnce = new ProfilerMarker(CLASS + "RunOnce");
    public static ProfilerMarker UpdateReferencePositions = new ProfilerMarker(CLASS + "UpdateReferencePositions");
    public static ProfilerMarker ResetRenderingSets = new ProfilerMarker(CLASS + "ResetRenderingSets");
    public static ProfilerMarker AddDistanceReferenceObject = new ProfilerMarker(CLASS + "AddDistanceReferenceObject");
    public static ProfilerMarker RemoveDistanceRefenceObject = new ProfilerMarker(CLASS + "RemoveDistanceRefenceObject");
    public static ProfilerMarker GetRuntimePrefabRenderingElement = new ProfilerMarker(CLASS + "GetRuntimePrefabRenderingElement");
    public static ProfilerMarker SetSceneDirty = new ProfilerMarker(CLASS + "SetSceneDirty");
    public static ProfilerMarker ConfirmExecutionState = new ProfilerMarker(CLASS + "ConfirmExecutionState");
    public static ProfilerMarker ExecuteNecessaryStateChanges = new ProfilerMarker(CLASS + "ExecuteNecessaryStateChanges");
    public static ProfilerMarker ChangeRuntimeRenderingState = new ProfilerMarker(CLASS + "ChangeRuntimeRenderingState");
    public static ProfilerMarker ChangeEditorSimulationState = new ProfilerMarker(CLASS + "ChangeEditorSimulationState");
    public static ProfilerMarker IsEditorSimulating = new ProfilerMarker(CLASS + "IsEditorSimulating");
}

public static class PRFB_RNDR_MGR_INIT
{
    private const string CLASS = "PrefabRenderingManagerInitializer" + ".";
    public static ProfilerMarker ExecuteInitialization = new ProfilerMarker(CLASS + "ExecuteInitialization");
    public static ProfilerMarker CheckNulls = new ProfilerMarker(CLASS + "CheckNulls");
    public static ProfilerMarker InitializeTransform = new ProfilerMarker(CLASS + "InitializeTransform");
    public static ProfilerMarker InitializeStructure = new ProfilerMarker(CLASS + "InitializeStructure");
    public static ProfilerMarker RecalculateRenderingBounds = new ProfilerMarker(CLASS + "RecalculateRenderingBounds");
    public static ProfilerMarker InitializeGPUIInitializationTracking = new ProfilerMarker(CLASS + "InitializeGPUIInitializationTracking");
    public static ProfilerMarker InitializeRuntimePrefabRenderingDataSets =
        new ProfilerMarker(CLASS + "InitializeRuntimePrefabRenderingDataSets");

    public static ProfilerMarker WarmUpShaders = new ProfilerMarker(CLASS + "WarmUpShaders");
    public static ProfilerMarker InitializeAllPrefabRenderingSets = new ProfilerMarker(CLASS + "InitializeAllPrefabRenderingSets");
    public static ProfilerMarker InitializeOptions = new ProfilerMarker(CLASS + "InitializeOptions");
    public static ProfilerMarker OnEnable = new ProfilerMarker(CLASS + "OnEnable");
    public static ProfilerMarker Update = new ProfilerMarker(CLASS + "Update");
}

public static class PRFB_RNDR_MGR_MOD
{
    private const string CLASS = "PrefabRenderingManagerModifier" + ".";
    public static ProfilerMarker UpdateVegetationItemsRendering_PKG = new ProfilerMarker(CLASS + "UpdateVegetationItemsRendering (Package)");
    public static ProfilerMarker UpdateVegetationItemsRendering_ID =
        new ProfilerMarker(CLASS + "UpdateVegetationItemsRendering (Individual)");
    public static ProfilerMarker UpdateVegetationItemsRendering_IDS = new ProfilerMarker(CLASS + "UpdateVegetationItemsRendering (Many)");
}

public static class PRFB_RNDR_MGR_DEST
{
    private const string CLASS = "PrefabRenderingManagerDestroyer" + ".";
    public static ProfilerMarker ExecuteDataSetTeardown = new ProfilerMarker(CLASS + "UpdateVegetationItemsRendering (Package)");
    public static ProfilerMarker TeardownRuntimePrefabRenderingDataSets =
        new ProfilerMarker(CLASS + "TeardownRuntimePrefabRenderingDataSets");
    public static ProfilerMarker TeardownRuntimePrefabRenderingDataSet = new ProfilerMarker(CLASS + "TeardownRuntimePrefabRenderingDataSet");
    public static ProfilerMarker Dispose = new ProfilerMarker(CLASS + "Dispose");
}

public static class RNTM_PRFB_RENDR_ELE
{
    private const string CLASS = "RuntimePrefabRenderingElement" + ".";
    public static ProfilerMarker Initialize = new ProfilerMarker(CLASS + "Initialize");
    public static ProfilerMarker ResizeUninitialized = new ProfilerMarker(CLASS + "ResizeUninitialized");
    public static ProfilerMarker Dispose = new ProfilerMarker(CLASS + "Dispose");
    public static ProfilerMarker Clear = new ProfilerMarker(CLASS + "Clear");
    public static ProfilerMarker CompactMemory = new ProfilerMarker(CLASS + "CompactMemory");
    public static ProfilerMarker SetCapacity = new ProfilerMarker(CLASS + "SetCapacity");
}

public static class PRFB_LOC_SOURCE
{
    private const string CLASS = "PrefabLocationSource" + ".";
    public static ProfilerMarker GetRenderingParameters = new ProfilerMarker(CLASS + "GetRenderingParameters");
    public static ProfilerMarker GetRenderingParametersFromPrefabCollection =
        new ProfilerMarker(CLASS + "GetRenderingParametersFromPrefabCollection");
    public static ProfilerMarker GetRenderingParametersFromVegetationStudio =
        new ProfilerMarker(CLASS + "GetRenderingParametersFromVegetationStudio");
    public static ProfilerMarker CreateParametersForPrefab = new ProfilerMarker(CLASS + "CreateParametersForPrefab");
    public static ProfilerMarker InitializeVegetationItemIndexLookup = new ProfilerMarker(CLASS + "InitializeVegetationItemIndexLookup");
    public static ProfilerMarker UpdateRuntimeRenderingParameters = new ProfilerMarker(CLASS + "UpdateRuntimeRenderingParameters");
    public static ProfilerMarker GetMatricesFromVegetationSystem = new ProfilerMarker(CLASS + "GetMatricesFromVegetationSystem");
    public static ProfilerMarker GetMatricesFromVegetationSystem_NullChecks =
        new ProfilerMarker(CLASS + "GetMatricesFromVegetationSystem.NullChecks");
    public static ProfilerMarker GetMatricesFromVegetationSystem_CompleteCellLoading =
        new ProfilerMarker(CLASS + "GetMatricesFromVegetationSystem.CompleteCellLoading");
    public static ProfilerMarker GetMatricesFromVegetationSystem_AddToCollections =
        new ProfilerMarker(CLASS + "GetMatricesFromVegetationSystem.AddToCollections");
    public static ProfilerMarker GetMatricesFromVegetationSystem_DisableRuntimeRendering =
        new ProfilerMarker(CLASS + "GetMatricesFromVegetationSystem.DisableRuntimeRendering");
    public static ProfilerMarker GetTRSFromVegetationStorage = new ProfilerMarker(CLASS + "GetTRSFromVegetationStorage");
}

public static class RUNTM_PRFB_RNDR_SET
{
    private const string CLASS = "RuntimePrefabRenderingState" + ".";
    public static ProfilerMarker UpdateElementInstanceData = new ProfilerMarker(CLASS + "UpdateElementInstanceData");
    public static ProfilerMarker InstantiateElementInstanceData = new ProfilerMarker(CLASS + "InstantiateElementInstanceData");
    public static ProfilerMarker UpdateInstanceData = new ProfilerMarker(CLASS + "UpdateInstanceData");
    public static ProfilerMarker UpdateExternalParameterEnabled = new ProfilerMarker(CLASS + "UpdateExternalParameterEnabled");
    public static ProfilerMarker ScheduleInitializationJobs = new ProfilerMarker(CLASS + "ScheduleInitializationJobs");
    public static ProfilerMarker InstantiateNewInstances = new ProfilerMarker(CLASS + "InstantiateNewInstances");
    public static ProfilerMarker UpdateMaximumDistances = new ProfilerMarker(CLASS + "UpdateMaximumDistances");
    public static ProfilerMarker ValidateExternalInitializationBeforeUpdate =
        new ProfilerMarker(CLASS + "ValidateExternalInitializationBeforeUpdate");
    public static ProfilerMarker InstantiateInstanceData = new ProfilerMarker(CLASS + "InstantiateInstanceData");
    public static ProfilerMarker FinalizeInitialization = new ProfilerMarker(CLASS + "FinalizeInitialization");
    public static ProfilerMarker ValidateAllInstances = new ProfilerMarker(CLASS + "ValidateAllInstances");
    public static ProfilerMarker ValidateAllElementInstances = new ProfilerMarker(CLASS + "ValidateAllElementInstances");
    public static ProfilerMarker ExamineStateChange = new ProfilerMarker(CLASS + "ExamineStateChange");
    public static ProfilerMarker RemoveExistingInstances = new ProfilerMarker(CLASS + "RemoveExistingInstances");
    public static ProfilerMarker RemoveExistingElementInstances = new ProfilerMarker(CLASS + "RemoveExistingElementInstances");
    public static ProfilerMarker GetPreparedSubtree = new ProfilerMarker(CLASS + "GetPreparedSubtree");
    public static ProfilerMarker GPUInstancerAPI_InitializeWithMatrix4x4Array =
        new ProfilerMarker(CLASS + "GPUInstancerAPI.InitializeWithMatrix4x4Array");
}

public static class PRFB_RNDR_INSTC_DATA
{
    private const string CLASS = "PrefabRenderingInstanceData" + ".";
    public static ProfilerMarker InitializeInstanceState = new ProfilerMarker(CLASS + "InitializeInstanceState");
    public static ProfilerMarker InitializePendingState = new ProfilerMarker(CLASS + "InitializePendingState");
    public static ProfilerMarker UpdatePendingState = new ProfilerMarker(CLASS + "UpdatePendingState");
    public static ProfilerMarker ValidateInstanceState = new ProfilerMarker(CLASS + "ValidateInstanceState");
    public static ProfilerMarker DestroyImmediate = new ProfilerMarker(CLASS + "DestroyImmediate");
    public static ProfilerMarker ResetComponents = new ProfilerMarker(CLASS + "ResetComponents");
    public static ProfilerMarker InitializeComponents = new ProfilerMarker(CLASS + "InitializeComponents");
    public static ProfilerMarker InitializeComponents_LOD = new ProfilerMarker(CLASS + "InitializeComponents_LOD");
    public static ProfilerMarker InitializeComponents_Renderers = new ProfilerMarker(CLASS + "InitializeComponents_Renderers");
    public static ProfilerMarker InitializeComponents_Lighting = new ProfilerMarker(CLASS + "InitializeComponents_Lighting");
    public static ProfilerMarker InitializeComponents_Collisions = new ProfilerMarker(CLASS + "InitializeComponents_Collisions");
    public static ProfilerMarker InitializeComponents_Rigidbody = new ProfilerMarker(CLASS + "InitializeComponents_Rigidbody");
    public static ProfilerMarker ShouldDelayTogglingComponents = new ProfilerMarker(CLASS + "ShouldDelayTogglingComponents");
    public static ProfilerMarker ToggleComponents = new ProfilerMarker(CLASS + "ToggleComponents");
    public static ProfilerMarker UpdateMatrices = new ProfilerMarker(CLASS + "UpdateMatrices");
    public static ProfilerMarker UpdateMatrices_Original = new ProfilerMarker(CLASS + "UpdateMatrices_Original");
    public static ProfilerMarker UpdateMatrices_GameObject = new ProfilerMarker(CLASS + "UpdateMatrices_GameObject");
    public static ProfilerMarker UpdateMatrices_GPUI = new ProfilerMarker(CLASS + "UpdateMatrices_GPUI");
    public static ProfilerMarker UpdateMatrices_GO_Matrix = new ProfilerMarker(CLASS + "UpdateMatrices_GO_Matrix");
    public static ProfilerMarker UpdateMatrices_NGO_Matrix = new ProfilerMarker(CLASS + "UpdateMatrices_NGO_Matrix");
    public static ProfilerMarker CheckDisablingConditions = new ProfilerMarker(CLASS + "CheckDisablingConditions");
    public static ProfilerMarker SetDistanceInternal = new ProfilerMarker(CLASS + "SetDistanceInternal");
    public static ProfilerMarker SetDistanceInternal_GetPositionFromMatrix =
        new ProfilerMarker(CLASS + "SetDistanceAppalachia.GetPositionFromMatrix");
    public static ProfilerMarker SetDistanceInternal_Vector3_Distance = new ProfilerMarker(CLASS + "SetDistanceAppalachia.Vector3.Distance");
    public static ProfilerMarker CheckBasicConditions = new ProfilerMarker(CLASS + "CheckBasicConditions");
    public static ProfilerMarker Teardown = new ProfilerMarker(CLASS + "Teardown");
    public static ProfilerMarker RemovePrefabInstance = new ProfilerMarker(CLASS + "RemovePrefabInstance");
    public static ProfilerMarker Destroy = new ProfilerMarker(CLASS + "Destroy");
    public static ProfilerMarker DisableInstancingForInstance = new ProfilerMarker(CLASS + "DisableInstancingForInstance");
    public static ProfilerMarker DisableInstancingForNoGameObjectInstance =
        new ProfilerMarker(CLASS + "DisableInstancingForNoGameObjectInstance");
    public static ProfilerMarker InstantiateGPUI = new ProfilerMarker(CLASS + "InstantiateGPUI");
    public static ProfilerMarker AddPrefabInstance = new ProfilerMarker(CLASS + "AddPrefabInstance");
    public static ProfilerMarker EnableInstancingForNoGameObjectInstance =
        new ProfilerMarker(CLASS + "EnableInstancingForNoGameObjectInstance");
    public static ProfilerMarker EnableInstancingForInstance = new ProfilerMarker(CLASS + "EnableInstancingForInstance");
}

public static class GPU_INSTC_PROTO_PAIR_COLL
{
    private const string CLASS = "GPUInstancerPrototypePairCollection" + ".";
    public static ProfilerMarker UpdateCollection = new ProfilerMarker(CLASS + "UpdateCollection");
    public static ProfilerMarker FindOrCreate = new ProfilerMarker(CLASS + "FindOrCreate");
    public static ProfilerMarker ModifyGameObject = new ProfilerMarker(CLASS + "ModifyGameObject");
    public static ProfilerMarker AssignExistingPrototypes = new ProfilerMarker(CLASS + "AssignExistingPrototypes");
    public static ProfilerMarker CreateNewPrototypesIfNecessary = new ProfilerMarker(CLASS + "CreateNewPrototypesIfNecessary");
    public static ProfilerMarker UpdatePrototype = new ProfilerMarker(CLASS + "UpdatePrototype");
}

public static class FAST_LIST
{
    private const string CLASS = "FastList" + ".";
    public static ProfilerMarker IncreaseCapacity = new ProfilerMarker(CLASS + "IncreaseCapacity");
    public static ProfilerMarker SetCapacity = new ProfilerMarker(CLASS + "SetCapacity");
    public static ProfilerMarker SetCount = new ProfilerMarker(CLASS + "SetCount");
    public static ProfilerMarker EnsureCount = new ProfilerMarker(CLASS + "EnsureCount");
    public static ProfilerMarker SetArray = new ProfilerMarker(CLASS + "SetArray");
    public static ProfilerMarker AddUnique = new ProfilerMarker(CLASS + "AddUnique");
    public static ProfilerMarker Contains = new ProfilerMarker(CLASS + "Contains");
    public static ProfilerMarker IndexOf = new ProfilerMarker(CLASS + "IndexOf");
    public static ProfilerMarker GetIndex = new ProfilerMarker(CLASS + "GetIndex");
    public static ProfilerMarker Add = new ProfilerMarker(CLASS + "Add");
    public static ProfilerMarker Add2 = new ProfilerMarker(CLASS + "Add2");
    public static ProfilerMarker Add3 = new ProfilerMarker(CLASS + "Add3");
    public static ProfilerMarker Add4 = new ProfilerMarker(CLASS + "Add4");
    public static ProfilerMarker Add5 = new ProfilerMarker(CLASS + "Add5");
    public static ProfilerMarker AddThreadSafe = new ProfilerMarker(CLASS + "AddThreadSafe");
    public static ProfilerMarker Insert = new ProfilerMarker(CLASS + "Insert");
    public static ProfilerMarker AddRange = new ProfilerMarker(CLASS + "AddRange");
    public static ProfilerMarker GrabListThreadSafe = new ProfilerMarker(CLASS + "GrabListThreadSafe");
    public static ProfilerMarker ChangeRange = new ProfilerMarker(CLASS + "ChangeRange");
    public static ProfilerMarker Remove = new ProfilerMarker(CLASS + "Remove");
    public static ProfilerMarker RemoveAt = new ProfilerMarker(CLASS + "RemoveAt");
    public static ProfilerMarker RemoveLast = new ProfilerMarker(CLASS + "RemoveLast");
    public static ProfilerMarker RemoveRange = new ProfilerMarker(CLASS + "RemoveRange");
    public static ProfilerMarker Dequeue = new ProfilerMarker(CLASS + "Dequeue");
    public static ProfilerMarker Clear = new ProfilerMarker(CLASS + "Clear");
    public static ProfilerMarker ClearThreadSafe = new ProfilerMarker(CLASS + "ClearThreadSafe");
    public static ProfilerMarker ClearRange = new ProfilerMarker(CLASS + "ClearRange");
    public static ProfilerMarker FastClear = new ProfilerMarker(CLASS + "FastClear");
    public static ProfilerMarker ToArray = new ProfilerMarker(CLASS + "ToArray");
}

public static class OCTREE
{
    private const string CLASS = "Octree" + ".";
    public static ProfilerMarker GetAppropriateChildIndexFromVector = new ProfilerMarker(CLASS + "GetAppropriateChildIndexFromVector");
    public static ProfilerMarker CreateChildren = new ProfilerMarker(CLASS + "CreateChildren");
    public static ProfilerMarker Add_Node = new ProfilerMarker(CLASS + "Add (Node)");
    public static ProfilerMarker Add_KeyValue = new ProfilerMarker("Octree.Add (Key/Value)");
    public static ProfilerMarker Add_Internal_Node = new ProfilerMarker(CLASS + "Add Internal (Node)");
    public static ProfilerMarker Add_Internal_Node_ContainedInTree = new ProfilerMarker(CLASS + "Add Internal (Node).ContainedInTree");
    public static ProfilerMarker Add_Internal_Node_AddToChildren = new ProfilerMarker(CLASS + "Add Internal (Node).AddToChildren");
    public static ProfilerMarker Add_Internal_Node_InstantiateList = new ProfilerMarker(CLASS + "Add Internal (Node).InstantiateList");
    public static ProfilerMarker Add_Internal_Node_InstantiateList_Keys =
        new ProfilerMarker(CLASS + "Add Internal (Node).InstantiateList.Keys");
    public static ProfilerMarker Add_Internal_Node_InstantiateList_Values =
        new ProfilerMarker(CLASS + "Add Internal (Node).InstantiateList.Values");
    public static ProfilerMarker Add_Internal_Node_AddNodesToCollection =
        new ProfilerMarker(CLASS + "Add Internal (Node).AddNodesToCollection");
    public static ProfilerMarker Add_Internal_Node_AddNodesToCollection_Keys =
        new ProfilerMarker(CLASS + "Add Internal (Node).AddNodesToCollection.Keys");
    public static ProfilerMarker Add_Internal_Node_AddNodesToCollection_Values =
        new ProfilerMarker(CLASS + "Add Internal (Node).AddNodesToCollection.Values");
    public static ProfilerMarker Remove_Key = new ProfilerMarker(CLASS + "Remove (Key)");
    public static ProfilerMarker Remove_KeyValue = new ProfilerMarker("Octree.Remove (Key/Value)");
    public static ProfilerMarker Remove_Node = new ProfilerMarker(CLASS + "Remove (Node)");
    public static ProfilerMarker RemoveIntersecting = new ProfilerMarker(CLASS + "RemoveIntersecting");
    public static ProfilerMarker Clear = new ProfilerMarker(CLASS + "Clear");
    public static ProfilerMarker Reorganize = new ProfilerMarker(CLASS + "Reorganize");
    public static ProfilerMarker GetNodeByKey = new ProfilerMarker(CLASS + "GetNodeByKey");
    public static ProfilerMarker HasAny = new ProfilerMarker(CLASS + "HasAny");
    public static ProfilerMarker GetAll = new ProfilerMarker(CLASS + "GetAll");
    public static ProfilerMarker GetAllWhere = new ProfilerMarker(CLASS + "GetAllWhere");
    public static ProfilerMarker GetNearestWhere = new ProfilerMarker(CLASS + "GetNearestWhere");
    public static ProfilerMarker DrawGizmos = new ProfilerMarker(CLASS + "DrawGizmos");
    public static ProfilerMarker YieldAll = new ProfilerMarker(CLASS + "YieldAll");
    public static ProfilerMarker YieldAllWhere = new ProfilerMarker(CLASS + "YieldAllWhere");
}

public static class BOX_OCTREE
{
    private const string CLASS = "BoundsOctree" + ".";
    public static ProfilerMarker GetAppropriateChildIndex = new ProfilerMarker(CLASS + "GetAppropriateChildIndex");
    public static ProfilerMarker GetRayHits = new ProfilerMarker(CLASS + "GetRayHits");
    public static ProfilerMarker GetRayHitsWhere = new ProfilerMarker(CLASS + "GetRayHitsWhere");
    public static ProfilerMarker CreateFromVectors = new ProfilerMarker(CLASS + "CreateFromVectors");
    public static ProfilerMarker ContainedInTree = new ProfilerMarker(CLASS + "ContainedInTree");
    public static ProfilerMarker NodeIsEligible = new ProfilerMarker(CLASS + "NodeIsEligible");
    public static ProfilerMarker Magnitude = new ProfilerMarker(CLASS + "Magnitude");
    public static ProfilerMarker MagnitudeSquared = new ProfilerMarker(CLASS + "MagnitudeSquared");
}

public static class SPHERE_OCTREE
{
    private const string CLASS = "SphereBoundsOctree" + ".";
    public static ProfilerMarker GetAppropriateChildIndex = new ProfilerMarker(CLASS + "GetAppropriateChildIndex");
    public static ProfilerMarker CreateFromVectors = new ProfilerMarker(CLASS + "CreateFromVectors");
    public static ProfilerMarker ContainedInTree = new ProfilerMarker(CLASS + "ContainedInTree");
    public static ProfilerMarker NodeIsEligible = new ProfilerMarker(CLASS + "NodeIsEligible");
    public static ProfilerMarker Magnitude = new ProfilerMarker(CLASS + "Magnitude");
    public static ProfilerMarker MagnitudeSquared = new ProfilerMarker(CLASS + "MagnitudeSquared");
}

public static class POINT_OCTREE
{
    private const string CLASS = "PointOctree" + ".";
    public static ProfilerMarker GetAppropriateChildIndex = new ProfilerMarker(CLASS + "GetAppropriateChildIndex");
    public static ProfilerMarker CreateFromVectors = new ProfilerMarker(CLASS + "CreateFromVectors");
    public static ProfilerMarker ContainedInTree = new ProfilerMarker(CLASS + "ContainedInTree");
    public static ProfilerMarker NodeIsEligible = new ProfilerMarker(CLASS + "NodeIsEligible");
    public static ProfilerMarker Magnitude = new ProfilerMarker(CLASS + "Magnitude");
    public static ProfilerMarker MagnitudeSquared = new ProfilerMarker(CLASS + "MagnitudeSquared");
}

public static class RNTM_PRFB_RNDR_DATA_ELE
{
    private const string CLASS = "SafeNative" + ".";
    public static ProfilerMarker CountByState = new ProfilerMarker(CLASS + "CountByState");
}

public static class INDEX_LIST
{
    private const string CLASS = "AppaList" + ".";
    public static ProfilerMarker Initialize = new ProfilerMarker(CLASS + "Initialize");
    public static ProfilerMarker RequiresRebuild = new ProfilerMarker(CLASS + "RequiresRebuild");
    public static ProfilerMarker Rebuild = new ProfilerMarker(CLASS + "Rebuild");
    public static ProfilerMarker AddIfKeyNotPresent = new ProfilerMarker(CLASS + "AddIfKeyNotPresent");
    public static ProfilerMarker AddOrUpdate = new ProfilerMarker(CLASS + "AddOrUpdate");
    public static ProfilerMarker Add = new ProfilerMarker(CLASS + "Add");
    public static ProfilerMarker Update = new ProfilerMarker(CLASS + "Update");
    public static ProfilerMarker RemoveAt = new ProfilerMarker(CLASS + "RemoveAt");
    public static ProfilerMarker AddOrUpdateRange = new ProfilerMarker(CLASS + "AddOrUpdateRange");
    public static ProfilerMarker Clear = new ProfilerMarker(CLASS + "Clear");
    public static ProfilerMarker ContainsKey = new ProfilerMarker(CLASS + "ContainsKey");
    public static ProfilerMarker Get = new ProfilerMarker(CLASS + "Get");
    public static ProfilerMarker GetByIndex = new ProfilerMarker(CLASS + "GetByIndex");
    public static ProfilerMarker GetKeyByIndex = new ProfilerMarker(CLASS + "GetKeyByIndex");
    public static ProfilerMarker IfPresent = new ProfilerMarker(CLASS + "IfPresent");
    public static ProfilerMarker TryGet = new ProfilerMarker(CLASS + "TryGet");
    public static ProfilerMarker AppaList = new ProfilerMarker(CLASS + "AppaList");
    public static ProfilerMarker RemoveByKey = new ProfilerMarker(CLASS + "RemoveByKey");
    public static ProfilerMarker Lookup_GET = new ProfilerMarker(CLASS + "Lookup.Get");
    public static ProfilerMarker Values_GET = new ProfilerMarker(CLASS + "Values.Get");
    public static ProfilerMarker Keys_GET = new ProfilerMarker(CLASS + "Keys.Get");
    public static ProfilerMarker Count_GET = new ProfilerMarker(CLASS + "Count.Get");
    public static ProfilerMarker Indexer_INT_GET = new ProfilerMarker(CLASS + "Indexer.ByIndex.Get");
    public static ProfilerMarker Indexer_INT_SET = new ProfilerMarker(CLASS + "Indexer.ByIndex.Set");
    public static ProfilerMarker Indexer_KEY_GET = new ProfilerMarker(CLASS + "Indexer.ByKey.Get");
    public static ProfilerMarker Indexer_KEY_SET = new ProfilerMarker(CLASS + "Indexer.ByKey.Set");
    public static ProfilerMarker AddOrUpdateIf_FUNC_PRED = new ProfilerMarker(CLASS + "AddOrUpdateIf (Function+Predicate)");
    public static ProfilerMarker AddOrUpdateIf_PRED = new ProfilerMarker(CLASS + "AddOrUpdateIf (Predicate)");
}

public static class RNDM_PRFB_SPAWNER
{
    private const string CLASS = "RandomPrefabSpawner" + ".";
    public static ProfilerMarker OnEnable = new ProfilerMarker(CLASS + "OnEnable");
    public static ProfilerMarker CreateNewSettings = new ProfilerMarker(CLASS + "CreateNewSettings");
    public static ProfilerMarker CreateNewState = new ProfilerMarker(CLASS + "CreateNewState");
    public static ProfilerMarker EnableSpawning = new ProfilerMarker(CLASS + "EnableSpawning");
    public static ProfilerMarker DisableSpawning = new ProfilerMarker(CLASS + "DisableSpawning");
    public static ProfilerMarker ExecuteUpdate = new ProfilerMarker(CLASS + "ExecuteUpdate");
    public static ProfilerMarker OnDrawGizmos = new ProfilerMarker(CLASS + "OnDrawGizmos");
}

public static class PRFB_RGDBDY_MGR
{
    private const string CLASS = "PrefabSpawnerRigidbodyManager" + ".";
    public static ProfilerMarker Enqueue = new ProfilerMarker(CLASS + "Enqueue");
    public static ProfilerMarker HandleRigidbodyRemoval = new ProfilerMarker(CLASS + "HandleRigidbodyRemoval");
    public static ProfilerMarker EligibleForActivation = new ProfilerMarker(CLASS + "EligibleForActivation");
}

public static class PRFB_SPWN_PNT_ST
{
    private const string CLASS = "PrefabSpawnPointState" + ".";
    public static ProfilerMarker EligibleForActivation = new ProfilerMarker(CLASS + "EligibleForActivation");
    public static ProfilerMarker CanFinalize = new ProfilerMarker(CLASS + "CanFinalize");
    public static ProfilerMarker CheckForSpawnOrphans = new ProfilerMarker(CLASS + "CheckForSpawnOrphans");
    public static ProfilerMarker RemoveNulls = new ProfilerMarker(CLASS + "RemoveNulls");
    public static ProfilerMarker DestroyInstances = new ProfilerMarker(CLASS + "DestroyInstances");
    public static ProfilerMarker Spawn = new ProfilerMarker(CLASS + "Spawn");
    public static ProfilerMarker SetMatrix4x4ToTransform = new ProfilerMarker(CLASS + "SetMatrix4x4ToTransform");
}

public static class PRFB_SPWN_PNT_COL
{
    private const string CLASS = "PrefabSpawnPointCollection" + ".";
    public static ProfilerMarker FullCheck = new ProfilerMarker(CLASS + "FullCheck");
    public static ProfilerMarker CheckForSpawnOrphans = new ProfilerMarker(CLASS + "CheckForSpawnOrphans");
    public static ProfilerMarker RemoveNulls = new ProfilerMarker(CLASS + "RemoveNulls");
    public static ProfilerMarker DestroyInstances = new ProfilerMarker(CLASS + "DestroyInstances");
    public static ProfilerMarker AddSpawnPoint = new ProfilerMarker(CLASS + "AddSpawnPoint");
    public static ProfilerMarker UpdateSpawnPoints = new ProfilerMarker(CLASS + "UpdateSpawnPoints");
    public static ProfilerMarker EligibleForActivation = new ProfilerMarker(CLASS + "EligibleForActivation");
    public static ProfilerMarker TryGetActiveSpawnPoint = new ProfilerMarker(CLASS + "TryGetActiveSpawnPoint");
    public static ProfilerMarker SpawnMany = new ProfilerMarker(CLASS + "SpawnMany");
    public static ProfilerMarker CanFinalize = new ProfilerMarker(CLASS + "CanFinalize");
    public static ProfilerMarker ExecutePreFinalization = new ProfilerMarker(CLASS + "ExecutePreFinalization");
    public static ProfilerMarker ReadyToFinalize = new ProfilerMarker(CLASS + "ReadyToFinalize");
    public static ProfilerMarker FinalizeSpawning = new ProfilerMarker(CLASS + "FinalizeSpawning");
}

public static class PRFB_RNDR_SET
{
    private const string CLASS = "PrefabRenderingSet" + ".";
    public static ProfilerMarker Initialize = new ProfilerMarker(CLASS + "Initialize");
    public static ProfilerMarker Refresh = new ProfilerMarker(CLASS + "Refresh");
    public static ProfilerMarker GetPhysical = new ProfilerMarker(CLASS + "GetPhysical");
    public static ProfilerMarker GetInstancing = new ProfilerMarker(CLASS + "GetInstancing");
    public static ProfilerMarker GetGraphics = new ProfilerMarker(CLASS + "GetGraphics");
    public static ProfilerMarker UpdatePrototypeGeneralSettings = new ProfilerMarker(CLASS + "UpdatePrototypeGeneralSettings");
}

public static class RNTM_RNDR_GFX_OPTS
{
    private const string CLASS = "RuntimeRenderingGraphicsOptions" + ".";
    public static ProfilerMarker EnsureRangeSafe = new ProfilerMarker(CLASS + "EnsureRangeSafe");
    public static ProfilerMarker MarkDirty = new ProfilerMarker(CLASS + "MarkDirty");
    public static ProfilerMarker CopyFrom = new ProfilerMarker(CLASS + "CopyFrom");
    public static ProfilerMarker ApplyTo = new ProfilerMarker(CLASS + "ApplyTo");
    public static ProfilerMarker UpdatePrototypeShadowMap = new ProfilerMarker(CLASS + "UpdatePrototypeShadowMap");
}

public static class MESH_BURY_MGR_PRCSR
{
    private const string CLASS = "MeshBurialManagementProcessor" + ".";
    public static ProfilerMarker Reset = new ProfilerMarker(CLASS + "Reset");
    public static ProfilerMarker InitializeVSP = new ProfilerMarker(CLASS + "InitializeVSP");
    public static ProfilerMarker Initialize = new ProfilerMarker(CLASS + "Initialize");
    public static ProfilerMarker RefreshVSP = new ProfilerMarker(CLASS + "RefreshVSP");
    public static ProfilerMarker RefreshPrefabRenderingSets = new ProfilerMarker(CLASS + "RefreshPrefabRenderingSets");
    public static ProfilerMarker EnqueueCell = new ProfilerMarker(CLASS + "EnqueueCell");
    public static ProfilerMarker EnqueuePrefabSpawnPointCollection = new ProfilerMarker(CLASS + "EnqueuePrefabSpawnPointCollection");
    public static ProfilerMarker EnqueueRuntimePrefabRenderingSet = new ProfilerMarker(CLASS + "EnqueueRuntimePrefabRenderingSet");
    public static ProfilerMarker ShouldAdoptTerrainNormal = new ProfilerMarker(CLASS + "ShouldAdoptTerrainNormal");
    public static ProfilerMarker PrepareAndEnqueue = new ProfilerMarker(CLASS + "PrepareAndEnqueue");
    public static ProfilerMarker InitializeMultipassBurial = new ProfilerMarker(CLASS + "InitializeMultipassBurial");
    public static ProfilerMarker ProcessFrame = new ProfilerMarker(CLASS + "ProcessFrame");
    public static ProfilerMarker ProcessVegetationBurials = new ProfilerMarker(CLASS + "ProcessVegetationBurials");
    public static ProfilerMarker ProcessGenericQueue = new ProfilerMarker(CLASS + "ProcessGenericQueue");
    public static ProfilerMarker ExecuteProcessingLoop = new ProfilerMarker(CLASS + "ExecuteProcessingLoop");
}

public static class MESH_BURY_MGR
{
    private const string CLASS = "MeshBurialManager" + ".";
    public static ProfilerMarker GetInitializedState = new ProfilerMarker(CLASS + "GetInitializedState");
    public static ProfilerMarker PrepareAndApplyAdjustmentParameters = new ProfilerMarker(CLASS + "PrepareAndApplyAdjustmentParameters");
    public static ProfilerMarker PrepareAndApplyAdjustmentParameters_Matrix =
        new ProfilerMarker(CLASS + "PrepareAndApplyAdjustmentParameters (Matrix)");
    public static ProfilerMarker ImproveAdjustment = new ProfilerMarker(CLASS + "ImproveAdjustment");
    public static ProfilerMarker ImproveAdjustment_Matrix = new ProfilerMarker(CLASS + "ImproveAdjustment (Matrix)");
}

public static class MESH_BURY_EXE_MGR
{
    private const string CLASS = "SafeNative" + ".";
    public static ProfilerMarker ProcessFrame = new ProfilerMarker(CLASS + "ProcessFrame");
    public static ProfilerMarker FinalizeResultData = new ProfilerMarker(CLASS + "FinalizeResultData");
    public static ProfilerMarker ApplyFinalizedResults = new ProfilerMarker(CLASS + "ApplyFinalizedResults");
    public static ProfilerMarker CheckFinalizedResult = new ProfilerMarker(CLASS + "CheckFinalizedResult");
    public static ProfilerMarker ProcessGenericQueue = new ProfilerMarker(CLASS + "ProcessGenericQueue");
}

public static class MESH_BURY_MGR_EXT
{
    private const string CLASS = "MeshBurialStateExtensions" + ".";
    public static ProfilerMarker Bury = new ProfilerMarker(CLASS + "Bury");
    public static ProfilerMarker AdjustNormal = new ProfilerMarker(CLASS + "AdjustNormal");
    public static ProfilerMarker PrepareAdjustmentParameters = new ProfilerMarker(CLASS + "PrepareAdjustmentParameters");
    public static ProfilerMarker ApplyAdjustmentParameters = new ProfilerMarker(CLASS + "ApplyAdjustmentParameters");
    public static ProfilerMarker CalculateBurialAdjustment = new ProfilerMarker(CLASS + "CalculateBurialAdjustment");
    public static ProfilerMarker CalculateInitialError = new ProfilerMarker(CLASS + "CalculateInitialError");
    public static ProfilerMarker CalculateError = new ProfilerMarker(CLASS + "CalculateError");
}

public static class JOB_MESH_OBJ
{
    private const string CLASS = "MeshObject" + ".";
    public static ProfilerMarker CalculateBurialAdjustment = new ProfilerMarker(CLASS + "CalculateBurialAdjustment");
}

public static class MESH_BURY_ADJ_STATE
{
    private const string CLASS = "MeshBurialAdjustmentState" + ".";
    public static ProfilerMarker InitializeLookupStorage = new ProfilerMarker(CLASS + "InitializeLookupStorage");
    public static ProfilerMarker InitializeLookup = new ProfilerMarker(CLASS + "InitializeLookup");
    public static ProfilerMarker TryGetValue = new ProfilerMarker(CLASS + "TryGetValue");
    public static ProfilerMarker Contains = new ProfilerMarker(CLASS + "Contains");
    public static ProfilerMarker AddOrUpdate = new ProfilerMarker(CLASS + "AddOrUpdate");
    public static ProfilerMarker Reset = new ProfilerMarker(CLASS + "Reset");
    public static ProfilerMarker Consume = new ProfilerMarker(CLASS + "Consume");
    public static ProfilerMarker GetNative = new ProfilerMarker(CLASS + "GetNative");
    public static ProfilerMarker OnEnable = new ProfilerMarker(CLASS + "OnEnable");
    public static ProfilerMarker OnDisable = new ProfilerMarker(CLASS + "OnDisable");
}

public static class MESH_BURY_INST
{
    private const string CLASS = "MeshBurialInstanceData" + ".";
    public static ProfilerMarker Dispose = new ProfilerMarker(CLASS + "Dispose");
    public static ProfilerMarker Check = new ProfilerMarker(CLASS + "Check");
    public static ProfilerMarker Update = new ProfilerMarker(CLASS + "Update");
    public static ProfilerMarker Update_Instances = new ProfilerMarker(CLASS + "Update_Instances");
    public static ProfilerMarker Update_Lookup = new ProfilerMarker(CLASS + "Update_Lookup");
    public static ProfilerMarker Update_Results = new ProfilerMarker(CLASS + "Update_Results");
    public static ProfilerMarker Update_Parameters = new ProfilerMarker(CLASS + "Update_Parameters");
    public static ProfilerMarker Update_Instances_Iterations = new ProfilerMarker(CLASS + "Update_Instances_Iterations");
    public static ProfilerMarker Update_Instances_Iterations_Matrices = new ProfilerMarker(CLASS + "Update_Instances_Iterations_Matrices");
    public static ProfilerMarker Update_Instances_Iterations_Init = new ProfilerMarker(CLASS + "Update_Instances_Iterations_Init");
    public static ProfilerMarker Update_Results_Iterations = new ProfilerMarker(CLASS + "Update_Results_Iterations");
    public static ProfilerMarker Update_Parameters_Iterations = new ProfilerMarker(CLASS + "Update_Parameters_Iterations");
    public static ProfilerMarker UPI_Retrieve = new ProfilerMarker(CLASS + "UPI_Retrieve");
    public static ProfilerMarker UPI_Check = new ProfilerMarker(CLASS + "UPI_Check");
    public static ProfilerMarker UPI_Create = new ProfilerMarker(CLASS + "UPI_Create");
    public static ProfilerMarker UPI_DisposeCreate = new ProfilerMarker(CLASS + "UPI_DisposeCreate");
    public static ProfilerMarker UPI_Reassign = new ProfilerMarker(CLASS + "UPI_Reassign");
}

public static class MESH_BURY_JOB
{
    private const string CLASS = "MeshBurialJobs" + ".";
    public static ProfilerMarker ScheduleBurial = new ProfilerMarker(CLASS + "ScheduleBurial");
    public static ProfilerMarker ScheduleBurialSpecificJobs = new ProfilerMarker(CLASS + "ScheduleBurialSpecificJobs");
    public static ProfilerMarker CombineDependencies = new ProfilerMarker(CLASS + "CombineDependencies");
    public static ProfilerMarker CallScheduleOptimizationJobs = new ProfilerMarker(CLASS + "CallScheduleOptimizationJobs");
    public static ProfilerMarker ScheduleBatchedJobs = new ProfilerMarker(CLASS + "ScheduleBatchedJobs");
    public static ProfilerMarker Dispose = new ProfilerMarker(CLASS + "Dispose");
    public static ProfilerMarker ExecuteScheduleBurialSpecificJobs = new ProfilerMarker(CLASS + "ExecuteScheduleBurialSpecificJobs");
    public static ProfilerMarker InstantiateInstanceData = new ProfilerMarker(CLASS + "InstantiateInstanceData");
    public static ProfilerMarker InstantiateJob = new ProfilerMarker(CLASS + "InstantiateJob");
    public static ProfilerMarker ScheduleMatrixInitializationJob = new ProfilerMarker(CLASS + "ScheduleMatrixInitializationJob");
}

public static class OPTIMIZE
{
    private const string CLASS = "Optimizer" + ".";
    public static ProfilerMarker ScheduleOptimizationJobs = new ProfilerMarker(CLASS + "ScheduleOptimizationJobs");
    public static ProfilerMarker CombineDependencies = new ProfilerMarker(CLASS + "CombineDependencies");
    public static ProfilerMarker ScheduleParamJobs = new ProfilerMarker(CLASS + "ScheduleParamJobs");
    public static ProfilerMarker CallDelegateCreator = new ProfilerMarker(CLASS + "CallDelegateCreator");
    public static ProfilerMarker ScheduleCalculationJobs = new ProfilerMarker(CLASS + "ScheduleCalculationJobs");
}

public static class CACHE
{
    private const string CLASS = "Cached<T>";
    public static ProfilerMarker ShouldInvalidate = new ProfilerMarker("Cached<T>.ShouldInvalidate");
    public static ProfilerMarker RecordCacheValidData = new ProfilerMarker("Cached<T>.RecordCacheValidData");
    public static ProfilerMarker RecordCacheInvalidationData = new ProfilerMarker("Cached<T>.RecordCacheInvalidationData");
    public static ProfilerMarker Get = new ProfilerMarker("Cached<T>.Get");
}*/

#endregion

/*
public class DummyDisposable : IProfile
{
    public IProfile Start()
    {
        return this;
    }

    public void Dispose()
    {
    }
}

public interface IProfile : IDisposable
{
    IProfile Start();
}

public class ProfilerMarkerWrapper : IDisposable, IProfile
{
    public ProfilerMarkerWrapper(string memberName, string sourceFilePath, int sourceLineNumber)
    {
        var fileName = AppaPath.GetFileNameWithoutExtension(sourceFilePath);
        this.marker = new ProfilerMarker($"{fileName}.{memberName} [{sourceLineNumber}]");
    }

    private ProfilerMarker marker;

    public IProfile Start()
    {
        marker.Begin();
        return this;
    }

    public IProfile Start(UnityEngine.Object context)
    {
        marker.Begin(context);
        return this;
    }

    void IDisposable.Dispose()
    {
        marker.End();
    }
}

public static class MARKERS
{
    
}
*/
