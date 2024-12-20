﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Utils;

public class Grid
{
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridArray = new int[width, height];

        if (false) // Set to true if debugging is needed
        {
            InitializeDebug();
        }
    }

    private void InitializeDebug()
    {
        TextMesh[,] debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, worldPosition + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                DrawDebugLines(worldPosition);
            }
        }
        DrawBorderLines();
        OnGridValueChanged += (sender, eventArgs) =>
        {
            debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
        };
    }

    private void DrawDebugLines(Vector3 worldPosition)
    {
        Debug.DrawLine(worldPosition, worldPosition + new Vector3(cellSize, 0, 0), Color.white, 100f);
        Debug.DrawLine(worldPosition, worldPosition + new Vector3(0, 0, cellSize), Color.white, 100f);
    }

    private void DrawBorderLines()
    {
        Vector3 topRight = GetWorldPosition(width, height);
        Vector3 bottomLeft = GetWorldPosition(0, 0);
        Debug.DrawLine(bottomLeft, new Vector3(topRight.x, bottomLeft.y, topRight.z), Color.white, 100f);
        Debug.DrawLine(new Vector3(topRight.x, bottomLeft.y, topRight.z), topRight, Color.white, 100f);
        Debug.DrawLine(topRight, new Vector3(bottomLeft.x, topRight.y, bottomLeft.z), Color.white, 100f);
        Debug.DrawLine(new Vector3(bottomLeft.x, topRight.y, bottomLeft.z), bottomLeft, Color.white, 100f);
    }

    public int GetWidth() => width;
    public int GetHeight() => height;
    public float GetCellSize() => cellSize;

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (IsInBounds(x, y))
        {
            gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y) => IsInBounds(x, y) ? gridArray[x, y] : 0;

    public int GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }

    public void AddValue(Vector3 worldPosition, int value, int fullValueRange, int totalRange)
    {
        int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));
        GetXY(worldPosition, out int originX, out int originY);

        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++)
            {
                int radius = x + y;
                int adjustedValue = CalculateAdjustedValue(value, lowerValueAmount, radius, fullValueRange);
                ApplyValueInPattern(originX, originY, x, y, adjustedValue);
            }
        }
    }

    private int CalculateAdjustedValue(int value, int lowerValueAmount, int radius, int fullValueRange)
    {
        return radius >= fullValueRange ? value - lowerValueAmount * (radius - fullValueRange) : value;
    }

    private void ApplyValueInPattern(int originX, int originY, int offsetX, int offsetY, int value)
    {
        AddValue(originX + offsetX, originY + offsetY, value);

        if (offsetX != 0) AddValue(originX - offsetX, originY + offsetY, value);
        if (offsetY != 0)
        {
            AddValue(originX + offsetX, originY - offsetY, value);
            if (offsetX != 0) AddValue(originX - offsetX, originY - offsetY, value);
        }
    }

    public void AddValue(int x, int y, int value) => SetValue(x, y, GetValue(x, y) + value);

    private bool IsInBounds(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;
}