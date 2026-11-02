using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class SetupWardrobePipeline
{
    private const string FBX_PATH = "Assets/_Project/Models/wardrobe-wooden/source/Wardrobe_Operable.fbx";
    private const string DIFFUSE_PATH = "Assets/_Project/Models/wardrobe-wooden/textures/WardrobeDiffuse.png";
    private const string MAT_DIR = "Assets/_Project/Models/wardrobe-wooden/Materials";
    private const string MAT_WARDROBE_PATH = MAT_DIR + "/M_Wardrobe.mat";
    private const string MAT_HANDLE_PATH = MAT_DIR + "/M_WardrobeHandle.mat";
    private const string PREFAB_DIR = "Assets/_Project/Prefabs/Furniture";
    private const string PREFAB_PATH = PREFAB_DIR + "/Wardrobe_Wooden.prefab";
    private const string PREFAB_ALT_PATH = "Assets/_Project/Models/wardrobe-wooden/Wardrobe_Wooden.prefab";
    private const string SCENE_PATH = "Assets/Scenes/SampleScene.unity";

    public static void Execute()
    {
        Debug.Log("=== STARTING WARDROBE PIPELINE ===");

        // 1. Ensure Directories exist
        if (!Directory.Exists(MAT_DIR))
            Directory.CreateDirectory(MAT_DIR);
        if (!Directory.Exists(PREFAB_DIR))
            Directory.CreateDirectory(PREFAB_DIR);
        AssetDatabase.Refresh();

        // 2. Setup Materials
        Material matWardrobe = AssetDatabase.LoadAssetAtPath<Material>(MAT_WARDROBE_PATH);
        if (matWardrobe == null)
        {
            Shader litShader = Shader.Find("Universal Render Pipeline/Lit");
            if (litShader == null)
                litShader = Shader.Find("Standard");
            matWardrobe = new Material(litShader);
            matWardrobe.name = "M_Wardrobe";
            Texture2D diffuseTex = AssetDatabase.LoadAssetAtPath<Texture2D>(DIFFUSE_PATH);
            if (diffuseTex != null)
            {
                matWardrobe.SetTexture("_BaseMap", diffuseTex);
                matWardrobe.SetTexture("_MainTex", diffuseTex);
            }
            matWardrobe.SetColor("_BaseColor", Color.white);
            matWardrobe.SetFloat("_Smoothness", 0.25f);
            AssetDatabase.CreateAsset(matWardrobe, MAT_WARDROBE_PATH);
            Debug.Log($"Created material {MAT_WARDROBE_PATH}");
        }

        Material matHandle = AssetDatabase.LoadAssetAtPath<Material>(MAT_HANDLE_PATH);
        if (matHandle == null)
        {
            Shader litShader = Shader.Find("Universal Render Pipeline/Lit");
            if (litShader == null)
                litShader = Shader.Find("Standard");
            matHandle = new Material(litShader);
            matHandle.name = "M_WardrobeHandle";
            matHandle.SetColor("_BaseColor", new Color(0.538f, 0.538f, 0.538f, 1f));
            matHandle.SetFloat("_Metallic", 0.85f);
            matHandle.SetFloat("_Smoothness", 0.65f);
            AssetDatabase.CreateAsset(matHandle, MAT_HANDLE_PATH);
            Debug.Log($"Created material {MAT_HANDLE_PATH}");
        }

        // 3. Load Operable FBX
        GameObject fbxAsset = AssetDatabase.LoadAssetAtPath<GameObject>(FBX_PATH);
        if (fbxAsset == null)
        {
            Debug.LogError($"Could not find FBX at {FBX_PATH}");
            return;
        }

        // 4. Construct Prefab GameObject
        GameObject wardrobeRoot = new GameObject("Wardrobe_Wooden");
        wardrobeRoot.layer = 0; // Default

        // Instantiate FBX parts
        GameObject fbxInstance = Object.Instantiate(fbxAsset, wardrobeRoot.transform);
        fbxInstance.name = "Model";

        // Find child objects
        Transform frameT = null;
        Transform doorLeftT = null;
        Transform doorRightT = null;

        // Traverse fbxInstance children
        foreach (Transform t in fbxInstance.GetComponentsInChildren<Transform>(true))
        {
            if (t.name.Contains("Wardrobe_Frame"))
                frameT = t;
            else if (t.name.Contains("Wardrobe_Door_Left"))
                doorLeftT = t;
            else if (t.name.Contains("Wardrobe_Door_Right"))
                doorRightT = t;
        }

        if (frameT == null || doorLeftT == null || doorRightT == null)
        {
            Debug.LogError("Could not locate all 3 parts in FBX instance!");
            return;
        }

        // Assign clean materials to all renderers
        Material[] cleanMats = new Material[] { matWardrobe, matHandle };
        foreach (var mr in wardrobeRoot.GetComponentsInChildren<MeshRenderer>(true))
        {
            mr.sharedMaterials = cleanMats;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            mr.receiveShadows = true;
        }

        // Assign layers: Frame = Layer 0 (Default), Doors = Layer 7 (Interactable)
        frameT.gameObject.layer = 0;
        doorLeftT.gameObject.layer = 7;
        doorRightT.gameObject.layer = 7;

        // Setup BoxColliders for Frame:
        // Left Wall: center=(-1.098, 0.018, 0.048), size=(0.055, 2.411, 0.876)
        var colLeft = frameT.gameObject.AddComponent<BoxCollider>();
        colLeft.center = new Vector3(-1.098f, 0.018f, 0.048f);
        colLeft.size = new Vector3(0.055f, 2.411f, 0.876f);

        // Right Wall: center=(1.098, 0.018, 0.048), size=(0.055, 2.411, 0.876)
        var colRight = frameT.gameObject.AddComponent<BoxCollider>();
        colRight.center = new Vector3(1.098f, 0.018f, 0.048f);
        colRight.size = new Vector3(0.055f, 2.411f, 0.876f);

        // Back Wall: center=(0, 0.018, -0.375), size=(2.252, 2.411, 0.030)
        var colBack = frameT.gameObject.AddComponent<BoxCollider>();
        colBack.center = new Vector3(0f, 0.018f, -0.375f);
        colBack.size = new Vector3(2.252f, 2.411f, 0.030f);

        // Top Wall: center=(0, 1.200, 0.048), size=(2.252, 0.050, 0.876)
        var colTop = frameT.gameObject.AddComponent<BoxCollider>();
        colTop.center = new Vector3(0f, 1.200f, 0.048f);
        colTop.size = new Vector3(2.252f, 0.050f, 0.876f);

        // Bottom Floor: center=(0, -1.160, 0.048), size=(2.252, 0.050, 0.876)
        var colBottom = frameT.gameObject.AddComponent<BoxCollider>();
        colBottom.center = new Vector3(0f, -1.160f, 0.048f);
        colBottom.size = new Vector3(2.252f, 0.050f, 0.876f);

        // Shelf: center=(0, 0.3188, 0.0225), size=(2.141, 0.0375, 0.765)
        var colShelf = frameT.gameObject.AddComponent<BoxCollider>();
        colShelf.center = new Vector3(0f, 0.3188f, 0.0225f);
        colShelf.size = new Vector3(2.141f, 0.0375f, 0.765f);

        // Setup BoxColliders for Doors:
        // Left Door: mesh goes from X=0 to +1.06905, Z from -0.03435 to 0. center=(0.5345, 0, -0.0172), size=(1.069, 2.3532, 0.036)
        var colDoorL = doorLeftT.gameObject.AddComponent<BoxCollider>();
        colDoorL.center = new Vector3(0.5345f, 0f, -0.0172f);
        colDoorL.size = new Vector3(1.069f, 2.3532f, 0.036f);

        var interactL = doorLeftT.gameObject.AddComponent<WardrobeDoorInteractable>();
        var soL = new SerializedObject(interactL);
        soL.FindProperty("doorTransform").objectReferenceValue = doorLeftT;
        soL.FindProperty("openAngleY").floatValue = -90f;
        soL.FindProperty("closedAngleY").floatValue = 0f;
        soL.ApplyModifiedPropertiesWithoutUndo();

        // Right Door: mesh goes from X=-1.06905 to 0, Z from -0.03435 to 0. center=(-0.5345, 0, -0.0172), size=(1.069, 2.3532, 0.036)
        var colDoorR = doorRightT.gameObject.AddComponent<BoxCollider>();
        colDoorR.center = new Vector3(-0.5345f, 0f, -0.0172f);
        colDoorR.size = new Vector3(1.069f, 2.3532f, 0.036f);

        var interactR = doorRightT.gameObject.AddComponent<WardrobeDoorInteractable>();
        var soR = new SerializedObject(interactR);
        soR.FindProperty("doorTransform").objectReferenceValue = doorRightT;
        soR.FindProperty("openAngleY").floatValue = 90f;
        soR.FindProperty("closedAngleY").floatValue = 0f;
        soR.ApplyModifiedPropertiesWithoutUndo();

        // Save Prefabs cleanly
        GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(wardrobeRoot, PREFAB_PATH);
        Debug.Log($"Saved clean prefab to {PREFAB_PATH}");

        // Also save clean copy to PREFAB_ALT_PATH
        PrefabUtility.SaveAsPrefabAsset(wardrobeRoot, PREFAB_ALT_PATH);
        Debug.Log($"Saved clean prefab copy to {PREFAB_ALT_PATH}");

        Object.DestroyImmediate(wardrobeRoot);

        // 5. Update SampleScene.unity
        var scene = EditorSceneManager.OpenScene(SCENE_PATH);
        GameObject existingWardrobe = GameObject.Find("Wardrobe");
        Transform furnitureParent = null;

        Vector3 targetPos = new Vector3(4.3860f, 1.26475f, -3.1112f);
        Quaternion targetRot = Quaternion.identity;
        Vector3 targetScale = Vector3.one;

        if (existingWardrobe != null)
        {
            furnitureParent = existingWardrobe.transform.parent;
            Debug.Log($"Found existing Wardrobe in scene under '{furnitureParent?.name}'. Replacing...");
            Object.DestroyImmediate(existingWardrobe);
        }
        else
        {
            GameObject furnObj = GameObject.Find("FURNITURE ");
            if (furnObj == null)
                furnObj = GameObject.Find("FURNITURE");
            if (furnObj != null)
                furnitureParent = furnObj.transform;
        }

        GameObject sceneInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
        sceneInstance.name = "Wardrobe";
        if (furnitureParent != null)
            sceneInstance.transform.SetParent(furnitureParent, false);

        sceneInstance.transform.localPosition = targetPos;
        sceneInstance.transform.localRotation = targetRot;
        sceneInstance.transform.localScale = targetScale;

        // Verify scene instance bounds
        var renderers = sceneInstance.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds totalBounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                totalBounds.Encapsulate(renderers[i].bounds);
            Debug.Log($"New Wardrobe Scene Instance Bounds center: {totalBounds.center.ToString("F4")}, size: {totalBounds.size.ToString("F4")}, min: {totalBounds.min.ToString("F4")}, max: {totalBounds.max.ToString("F4")}");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log($"Successfully updated {SCENE_PATH}");

        // 6. Verification: Test Door Rotation & Render Verification Screenshots
        VerifyAndRender(sceneInstance);

        AssetDatabase.SaveAssets();
        Debug.Log("=== WARDROBE PIPELINE COMPLETE ===");
    }

    private static void VerifyAndRender(GameObject wardrobe)
    {
        Transform doorLeft = null;
        Transform doorRight = null;
        foreach (Transform t in wardrobe.GetComponentsInChildren<Transform>())
        {
            if (t.name.Contains("Wardrobe_Door_Left"))
                doorLeft = t;
            else if (t.name.Contains("Wardrobe_Door_Right"))
                doorRight = t;
        }

        if (doorLeft == null || doorRight == null)
        {
            Debug.LogError("Could not find doors for verification!");
            return;
        }

        // Test Door Angles:
        // Closed: (0, 0, 0)
        doorLeft.localEulerAngles = Vector3.zero;
        doorRight.localEulerAngles = Vector3.zero;
        Debug.Log($"Closed doors verified: Left={doorLeft.localEulerAngles}, Right={doorRight.localEulerAngles}");

        // Create temporary Camera and cleanly calibrated lights
        GameObject camObj = new GameObject("VerificationCam");
        Camera cam = camObj.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.12f, 0.12f, 0.14f, 1f);
        cam.fieldOfView = 75f;
        cam.nearClipPlane = 0.05f;

        Vector3 wardrobeCenter = wardrobe.transform.position + new Vector3(0f, 0.05f, 0f);

        // Headlight directional light (aligned with camera view, no room-wall shadow artifacts)
        GameObject lightObj = new GameObject("VerificationHeadlight");
        Light dirLight = lightObj.AddComponent<Light>();
        dirLight.type = LightType.Directional;
        dirLight.intensity = 1.0f;
        dirLight.shadows = LightShadows.None;
        dirLight.transform.rotation = Quaternion.Euler(5f, 0f, 0f);

        // Soft interior point light to illuminate the cavity, shelf, rod, and backboard
        GameObject pointLightObj = new GameObject("VerificationFillLight");
        Light fillLight = pointLightObj.AddComponent<Light>();
        fillLight.type = LightType.Point;
        fillLight.intensity = 1.2f;
        fillLight.range = 3.5f;
        fillLight.shadows = LightShadows.None;
        fillLight.transform.position = wardrobeCenter + new Vector3(0f, 0.2f, 0.2f);

        RenderTexture rt = new RenderTexture(1280, 720, 24);
        cam.targetTexture = rt;

        // --- 1. Render Closed State (Front View from room walkway) ---
        cam.transform.position = wardrobeCenter + new Vector3(0f, 0.05f, 1.60f);
        cam.transform.LookAt(wardrobeCenter);
        RenderAndSave(cam, rt, "E:/Unity/GUT FEELING/wardrobe_doors_closed.png");
        Debug.Log("Rendered wardrobe_doors_closed.png");

        // --- 2. Render Open State (Front View, Left=-90, Right=+90) ---
        doorLeft.localEulerAngles = new Vector3(0f, -90f, 0f);
        doorRight.localEulerAngles = new Vector3(0f, 90f, 0f);
        Debug.Log($"Open doors (-90, +90) verified: Left={doorLeft.localEulerAngles}, Right={doorRight.localEulerAngles}");

        cam.transform.position = wardrobeCenter + new Vector3(0f, 0.05f, 1.60f);
        cam.transform.LookAt(wardrobeCenter);
        RenderAndSave(cam, rt, "E:/Unity/GUT FEELING/wardrobe_doors_open.png");
        Debug.Log("Rendered wardrobe_doors_open.png");

        // --- 3. Render Open State (Perspective Isometric Angle View from room walkway) ---
        cam.transform.position = wardrobeCenter + new Vector3(-1.05f, 0.20f, 1.55f);
        cam.transform.LookAt(wardrobeCenter + new Vector3(0.05f, 0.0f, 0.0f));
        RenderAndSave(cam, rt, "E:/Unity/GUT FEELING/wardrobe_doors_iso.png");
        Debug.Log("Rendered wardrobe_doors_iso.png");

        // Reset doors to closed for final scene state
        doorLeft.localEulerAngles = Vector3.zero;
        doorRight.localEulerAngles = Vector3.zero;

        // Clean up temp camera and lights
        cam.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();
        Object.DestroyImmediate(rt);
        Object.DestroyImmediate(camObj);
        Object.DestroyImmediate(lightObj);
        Object.DestroyImmediate(pointLightObj);
    }

    private static void RenderAndSave(Camera cam, RenderTexture rt, string savePath)
    {
        RenderTexture.active = rt;
        cam.Render();
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(savePath, bytes);
        Object.DestroyImmediate(tex);
    }
}
