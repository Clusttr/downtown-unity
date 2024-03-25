using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;


[CreateAssetMenu(fileName = "Prefab Brush", menuName = "Brushes/Prefab Brush")]
[CustomGridBrush(false, true, false, "Prefab Brush")]
public class PrefabBrush : GameObjectBrush
{
    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        if(brushTarget.layer == 31)
        {
            return;
        }

        Transform erased = GetObjectCell(gridLayout, brushTarget.transform, new Vector3Int(position.x, position.y, 0));
        if(erased != null)
        {
            Undo.DestroyObjectImmediate(erased.gameObject);
        }
    }

    private static Transform GetObjectCell(GridLayout gridLayout, Transform parent, Vector3Int position)
    {
        int childCount = parent.childCount;
        Vector3 min = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(position));
        Vector3 max = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(position + Vector3Int.one));

        max.y = 0.2f;

        Bounds bounds = new Bounds((max + min) * 0.5f, max - min);
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if(bounds.Contains(child.position))
            {
                return child;
            }
        }

        return null;
    }
}
