using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color exploredColor = Color.yellow;
    [SerializeField] private Color pathColor = new Color(1.0f, 0.5f, 0.0f);
    [SerializeField] private Color blockedColor = Color.gray;

    private TextMeshPro label = null;
    private Vector2Int coordinates = Vector2Int.zero;
    private Node node = null;
    private GridManager gridManager = null;

    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;
    }

    private void Update()
    {
        gridManager = FindObjectOfType<GridManager>();
        DisplayCoordinates();

        if (!Application.isPlaying)
        {    
            UpdateObjectName();
            label.enabled = true;
        }

        SetLabelColor();
        ToggleLabels();
    }

    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.enabled;
        }
    }

    private void SetLabelColor()
    {
        if (gridManager == null) { return; }

        node = gridManager.GetNode(coordinates);
        if (node == null) { return; }

        if (node.isPath)
        {
            label.color = pathColor;
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
        else if (node.isWalkable)
        {
            label.color = blockedColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }

    private void DisplayCoordinates()
    {
        if (gridManager == null) { return; }

        coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

        label.text = coordinates.ToString();
    }

    private void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
}
