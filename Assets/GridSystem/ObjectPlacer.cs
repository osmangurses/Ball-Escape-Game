using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ObjectPlacer : EditorWindow
{
    private GameObject selectedPrefab;
    private List<GameObject> availablePrefabs = new List<GameObject>();
    private Vector2 scrollPosition;
    private int selectedPrefabIndex = -1;
    private float gridSize = 0.5f;
    private GUIStyle headerStyle;
    private GUIStyle listItemStyle;
    private GUIStyle selectedStyle;
    private Color defaultColor;
    private GameObject lastPlacedObject;

    [MenuItem("Tools/Object Placer")]
    public static void ShowWindow()
    {
        GetWindow<ObjectPlacer>("Object Placer");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        defaultColor = GUI.backgroundColor;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void InitializeStyles()
    {
        if (headerStyle == null)
        {
            headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 14;
            headerStyle.margin = new RectOffset(4, 4, 10, 8);
        }

        if (listItemStyle == null)
        {
            listItemStyle = new GUIStyle(EditorStyles.helpBox);
            listItemStyle.margin = new RectOffset(4, 4, 4, 4);
            listItemStyle.padding = new RectOffset(8, 8, 8, 8);
        }

        if (selectedStyle == null)
        {
            selectedStyle = new GUIStyle(listItemStyle);
            selectedStyle.normal.background = CreateColoredTexture(new Color(0.7f, 0.9f, 1.0f, 0.3f));
        }
    }

    private Texture2D CreateColoredTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    private void OnGUI()
    {
        InitializeStyles();

        EditorGUILayout.Space(5);
        GUILayout.Label("Object Placer Tool", headerStyle, GUILayout.Height(30));

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.4f));

        GUILayout.Label("Prefab Ekle:", EditorStyles.boldLabel);

        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 60.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Prefableri buraya sürükleyin", EditorStyles.helpBox);

        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        GameObject prefab = draggedObject as GameObject;
                        if (prefab != null && PrefabUtility.IsPartOfPrefabAsset(prefab) && !availablePrefabs.Contains(prefab))
                        {
                            availablePrefabs.Add(prefab);
                        }
                    }
                }

                evt.Use();
                break;
        }

        EditorGUILayout.Space(10);
        GUILayout.Label("Izgara Ayarlarý:", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Izgara Boyutu:", GUILayout.Width(100));
        gridSize = EditorGUILayout.FloatField(gridSize, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("Nesneler " + gridSize + " birimlik ýzgara noktalarýna yerleþtirilecek.", MessageType.Info);

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Tüm Prefableri Temizle", GUILayout.Height(30)))
        {
            availablePrefabs.Clear();
            selectedPrefab = null;
            selectedPrefabIndex = -1;
        }

        EditorGUILayout.Space(15);
        GUILayout.Label("Kullaným:", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. Prefableri yukarýdaki alana sürükleyin\n" +
            "2. Saðdaki listeden bir prefab seçin\n" +
            "3. Scene görünümünde sol týklayarak prefabý yerleþtirin\n" +
            "4. Q ve E tuþlarý ile son yerleþtirilen nesneyi döndürün", MessageType.Info);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));

        GUILayout.Label("Prefab Listesi:", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (availablePrefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("Henüz prefab eklenmedi. Prefableri sol taraftaki alana sürükleyip býrakýn.", MessageType.Info);
        }

        for (int i = 0; i < availablePrefabs.Count; i++)
        {
            GameObject prefab = availablePrefabs[i];
            if (prefab == null)
            {
                availablePrefabs.RemoveAt(i);
                i--;
                continue;
            }

            bool isSelected = (selectedPrefabIndex == i);
            GUIStyle style = isSelected ? selectedStyle : listItemStyle;

            EditorGUILayout.BeginVertical(style);
            EditorGUILayout.BeginHorizontal();

            if (isSelected)
            {
                GUI.backgroundColor = new Color(0.8f, 0.9f, 1f);
            }

            bool newIsSelected = GUILayout.Toggle(isSelected, "", GUILayout.Width(20));
            if (newIsSelected != isSelected)
            {
                selectedPrefabIndex = newIsSelected ? i : -1;
                selectedPrefab = newIsSelected ? prefab : null;
            }

            if (GUILayout.Button(prefab.name, EditorStyles.label))
            {
                selectedPrefabIndex = isSelected ? -1 : i;
                selectedPrefab = isSelected ? null : prefab;
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Kaldýr", GUILayout.Width(60)))
            {
                availablePrefabs.RemoveAt(i);
                if (selectedPrefabIndex == i)
                {
                    selectedPrefabIndex = -1;
                    selectedPrefab = null;
                }
                else if (selectedPrefabIndex > i)
                {
                    selectedPrefabIndex--;
                }
                i--;
            }

            GUI.backgroundColor = defaultColor;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(2);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        if (selectedPrefab != null)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label("Seçili Prefab:", EditorStyles.boldLabel, GUILayout.Width(100));
            GUILayout.Label(selectedPrefab.name);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        if (evt.type == EventType.MouseDown)
        {
            Repaint();
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event evt = Event.current;

        if (evt.type == EventType.KeyDown && lastPlacedObject != null)
        {
            if (evt.keyCode == KeyCode.Q)
            {
                RotateLastPlacedObject(-90f);
                evt.Use();
            }
            else if (evt.keyCode == KeyCode.E)
            {
                RotateLastPlacedObject(90f);
                evt.Use();
            }
        }

        if (selectedPrefab == null)
            return;

        if (evt.type == EventType.Repaint || evt.type == EventType.MouseMove)
        {
            Vector3 mousePosition = evt.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

            float distanceToDrawPlane = Mathf.Abs(sceneView.camera.transform.position.z);
            Vector3 worldPosition = ray.GetPoint(distanceToDrawPlane);

            Vector3 snappedPosition = new Vector3(
                Mathf.Round(worldPosition.x / gridSize) * gridSize,
                Mathf.Round(worldPosition.y / gridSize) * gridSize,
                0
            );

            Handles.color = new Color(0, 1, 0, 0.5f);
            Handles.DrawWireCube(snappedPosition, new Vector3(gridSize, gridSize, 0.1f));

            Handles.BeginGUI();
            GUI.contentColor = Color.white;
            GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
            GUI.Label(new Rect(evt.mousePosition.x + 15, evt.mousePosition.y - 30, 150, 40),
                     $"({snappedPosition.x}, {snappedPosition.y})\nQ/E: Döndür", EditorStyles.helpBox);
            Handles.EndGUI();

            SceneView.RepaintAll();
        }

        if (evt.type == EventType.MouseDown && evt.button == 0)
        {
            Vector3 mousePosition = evt.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

            float distanceToDrawPlane = Mathf.Abs(sceneView.camera.transform.position.z);
            Vector3 worldPosition = ray.GetPoint(distanceToDrawPlane);

            Vector3 snappedPosition = new Vector3(
                Mathf.Round(worldPosition.x / gridSize) * gridSize,
                Mathf.Round(worldPosition.y / gridSize) * gridSize,
                0
            );

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
            instance.transform.position = snappedPosition;

            Undo.RegisterCreatedObjectUndo(instance, "Place Object");

            Selection.activeGameObject = instance;

            lastPlacedObject = instance;

            evt.Use();
        }
    }

    private void RotateLastPlacedObject(float rotationAmount)
    {
        if (lastPlacedObject == null)
            return;

        Vector3 currentRotation = lastPlacedObject.transform.eulerAngles;

        lastPlacedObject.transform.eulerAngles = new Vector3(
            currentRotation.x,
            currentRotation.y,
            currentRotation.z + rotationAmount
        );

        Undo.RecordObject(lastPlacedObject.transform, "Rotate Object");

        SceneView.RepaintAll();
    }
}