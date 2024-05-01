using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "WFC Module set", fileName = "module_set", order = 0)]
public class WFCModuleSet : ScriptableObject
{
    public List<WFCModule> modules;
    
    private List<TileBase> _tileset = new List<TileBase>();
    public List<TileBase> Tileset => _tileset;

    private List<WFCModule.ModuleRule> rules = new List<WFCModule.ModuleRule>();
    
    public static Vector3Int[] NeighboursTilePositions = new []
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left
    };

    public void ResetTileset()
    {
        _tileset.Clear();
        foreach (var module in modules)
        {
            if (!_tileset.Contains(module.Tile))
            {
                _tileset.Add(module.Tile);
            }
        }
    }
    
    public List<TileBase> GetTiles(WFCSlot currentSlot, Vector3Int direction)
    {

        HashSet<TileBase> tiles = new HashSet<TileBase>();
        
        var filteredModules = modules.Where(module =>
            currentSlot.Domain.Contains(module.Tile));
        
        foreach (var wfcModule in filteredModules)
        {
            List<WFCModule.ModuleRule> rules = wfcModule.Rules.Where(r => r.neighbourhoodDirection == WFCModule.VectorToEnumDirection(direction)).ToList();
            foreach (var rule in rules)
            {
                tiles.AddRange(rule.neighbours);
            }
        }

        return tiles.ToList();
    }

    public void Clear()
    {
        foreach (var module in modules)
        {
            string path = AssetDatabase.GetAssetPath(module);
            if(path != String.Empty)
                AssetDatabase.DeleteAsset(path);
        }

        var guids = AssetDatabase.FindAssets("t:WFCModule");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if(path != String.Empty)
                AssetDatabase.DeleteAsset(path);
        }
        
        modules.Clear();
        
        AssetDatabase.SaveAssets();
        
    }
    
    public void AddModule(TileBase rootTile, TileBase moduleTile, Vector3Int direction)
    {
        Debug.Log("root module tile : " + rootTile.name + " : " + moduleTile.name);
        
        string modulePath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
        if (modulePath != "")
        {
            //get if it's a new module or an already made module
            WFCModule newModule= modules.FirstOrDefault(module => module.Tile == rootTile);
            
            //if it's a new module it creat a new assest corresponding to the roottile
            if (newModule == null)
            {
                newModule = CreateInstance<WFCModule>();
                AssetDatabase.CreateAsset(newModule, modulePath + "/" + rootTile.name + ".asset");
                
                //add the module to the list of modules
                modules.Add(newModule);

                newModule.Tile = rootTile;
                newModule.Rules = new List<WFCModule.ModuleRule>();
                
                //give the rules to the module
                //newModule.AddNeighbour(rootTile, direction);
                

            }

            //add a rule if it doesn't exist already
            if (!newModule.Rules.Exists(r => 
                    r.neighbourhoodDirection == WFCModule.VectorToEnumDirection(direction) 
                    && r.neighbours.Contains(moduleTile))
                )
            {
                newModule.AddNeighbour(moduleTile, direction);
            }
        
            AssetDatabase.SaveAssets();
            AssetDatabase.SaveAssetIfDirty(this);
            
        }
    }
}
