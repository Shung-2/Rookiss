using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

public class MapEditor
{
#if UNITY_EDITOR
    
    // % : Ctrl, # : Shift, & : Alt
    // [MenuItem("Tools/GenerateMap %#g")]
    // private static void HelloWorld()
    // {
    //     if (EditorUtility.DisplayDialog("Hello World", "Create?", "Create", "Cancel"))
    //     {
    //         new GameObject("Hello World");
    //     }
    // }
    
    [MenuItem("Tools/GenerateMap %#g")]
    private static void GenerateMap()
    {
        GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach (GameObject go in gameObjects)
        {
            Tilemap tm =  Util.FindChild<Tilemap>(go, "Tilemap_Collision", true);
        
            // 파일을 만듭니다.
            using (var writer = File.CreateText($"Assets/Resources/Map/{go.name}.txt"))
            {
                // MinX, MinY, MaxX, MaxY
                writer.WriteLine(tm.cellBounds.xMin);
                writer.WriteLine(tm.cellBounds.xMax);
                writer.WriteLine(tm.cellBounds.yMin);
                writer.WriteLine(tm.cellBounds.yMax);
            
                // 왼쪽 최상단부터 시작한다.
                for (int y = tm.cellBounds.yMax; y >= tm.cellBounds.yMin; y--)
                {
                    for (int x = tm.cellBounds.xMin; x <= tm.cellBounds.xMax; x++)
                    {
                        TileBase tile = tm.GetTile(new Vector3Int(x, y, 0));
                    
                        // 타일이 있는 경우 "1" 작성
                        if (tile != null)
                        {
                            writer.Write("1");
                        }
                        else
                        {
                            writer.Write("0");
                        }
                    }
                
                    writer.WriteLine();
                }
            }
        }
    }
    
#endif
    
}
