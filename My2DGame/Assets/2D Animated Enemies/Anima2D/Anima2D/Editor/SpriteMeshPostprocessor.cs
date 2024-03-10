using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;
using System.Collections.Generic;
using System.Linq;

namespace Anima2D
{
	[InitializeOnLoad]
	public class SpriteMeshPostprocessor : AssetPostprocessor
	{
		static Dictionary<string,string> s_SpriteMeshToTextureCache = new Dictionary<string, string>();
		
		static SpriteMeshPostprocessor()
		{
			if(!Application.isPlaying)
			{
				EditorApplication.delayCall += Initialize;
			}
		}

		static void Initialize() 
		{
			s_SpriteMeshToTextureCache.Clear();
			
			var spriteMeshGUIDs = AssetDatabase.FindAssets("t:SpriteMesh");
			
			foreach(var guid in spriteMeshGUIDs)
			{
				var spriteMesh = LoadSpriteMesh(AssetDatabase.GUIDToAssetPath(guid));
				
				if(spriteMesh)
				{
					UpdateCachedSpriteMesh(spriteMesh);
				}
			}		
		}

		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{	
			foreach(var importetAssetPath in importedAssets)
			{
				var spriteMesh = LoadSpriteMesh(importetAssetPath);
				
				if(spriteMesh != null)
				{
					UpdateCachedSpriteMesh(spriteMesh);
					UpgradeSpriteMesh(spriteMesh); 
				}
			}
		}
		
		static void UpgradeSpriteMesh(SpriteMesh spriteMesh)
		{
			if(spriteMesh)
			{
				SerializedObject spriteMeshSO = new SerializedObject(spriteMesh);
				SerializedProperty apiVersionProp = spriteMeshSO.FindProperty("m_ApiVersion");
				
				if(apiVersionProp.intValue < SpriteMesh.api_version)
				{
					if(apiVersionProp.intValue < 2)
					{
						Debug.LogError("SpriteMesh " + spriteMesh + " was created using an ancient version of Anima2D which can't be upgraded anymore.\n" +
							"The last version that can upgrade this asset is Anima2D 1.1.5");
					}
					
					if(apiVersionProp.intValue < 3)
					{
						Upgrade_003(spriteMeshSO);
					}

					if(apiVersionProp.intValue < 4)
					{
						Upgrade_004(spriteMeshSO);
					}

					if(apiVersionProp.intValue < 5)
					{
						Upgrade_005(spriteMeshSO);
					}
					
					spriteMeshSO.Update();
					apiVersionProp.intValue = SpriteMesh.api_version;
					spriteMeshSO.ApplyModifiedProperties();
					
					AssetDatabase.SaveAssets();
				}
			}
		}
		
		static void Upgrade_003(SerializedObject spriteMeshSO)
		{
			SpriteMesh spriteMesh = spriteMeshSO.targetObject as SpriteMesh;
			SpriteMeshData spriteMeshData = SpriteMeshUtils.LoadSpriteMeshData(spriteMesh);
			
			if(spriteMesh.sprite && spriteMeshData)
			{
				TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(spriteMesh.sprite)) as TextureImporter;
				
				float maxImporterSize = textureImporter.maxTextureSize;
				
				SpriteMeshUtils.GetWidthAndHeight(textureImporter, out var width, out var height);
				
				int maxSize = Mathf.Max(width,height);
				
				float factor = maxSize / maxImporterSize;
				
				if(factor > 1f)
				{
					SerializedObject spriteMeshDataSO = new SerializedObject(spriteMeshData);
					SerializedProperty smdPivotPointProp = spriteMeshDataSO.FindProperty("m_PivotPoint");
					SerializedProperty smdVerticesProp = spriteMeshDataSO.FindProperty("m_Vertices");
					SerializedProperty smdHolesProp = spriteMeshDataSO.FindProperty("m_Holes");
					
					spriteMeshDataSO.Update();
					
					smdPivotPointProp.vector2Value = spriteMeshData.pivotPoint * factor;
					
					for(int i = 0; i < spriteMeshData.vertices.Length; ++i)
					{
						smdVerticesProp.GetArrayElementAtIndex(i).vector2Value = spriteMeshData.vertices[i] * factor;
					}
					
					for(int i = 0; i < spriteMeshData.holes.Length; ++i)
					{
						smdHolesProp.GetArrayElementAtIndex(i).vector2Value = spriteMeshData.holes[i] * factor;
					}
					
					spriteMeshDataSO.ApplyModifiedProperties();
					
					EditorUtility.SetDirty(spriteMeshData);
				}
			}
		}

		static void Upgrade_004(SerializedObject spriteMeshSO)
		{
			SerializedProperty materialsProp = spriteMeshSO.FindProperty("m_SharedMaterials");

			for(int i = 0; i < materialsProp.arraySize; ++i)
			{
				SerializedProperty materialProp = materialsProp.GetArrayElementAtIndex(i);
				Material material = materialProp.objectReferenceValue as Material;

				if(material)
				{
					GameObject.DestroyImmediate(material, true);
				}
			}

			spriteMeshSO.Update();
			materialsProp.arraySize = 0;
			spriteMeshSO.ApplyModifiedProperties();
		}

		static void Upgrade_005(SerializedObject spriteMeshSO)
		{
			var spriteMesh = spriteMeshSO.targetObject as SpriteMesh;
			var spriteMeshData = SpriteMeshUtils.LoadSpriteMeshData(spriteMesh);

			spriteMeshData.ApplyToSprite(spriteMesh.sprite);
		}
		
		static void UpdateCachedSpriteMesh(SpriteMesh spriteMesh)
		{
			if(spriteMesh)
			{
				string key = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(spriteMesh));
				
				if(spriteMesh.sprite)
				{
					SpriteMesh spriteMeshFromSprite = GetSpriteMeshFromSprite(spriteMesh.sprite);
					
					if(!spriteMeshFromSprite || spriteMesh == spriteMeshFromSprite)
					{
						string value = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(SpriteUtility.GetSpriteTexture(spriteMesh.sprite,false)));
						
						s_SpriteMeshToTextureCache[key] = value;
					}else{
						Debug.LogWarning("Anima2D: SpriteMesh " + spriteMesh.name + " uses the same Sprite as " + spriteMeshFromSprite.name + ". Use only one SpriteMesh per Sprite.");
					}
					
				}else if(s_SpriteMeshToTextureCache.ContainsKey(key))
				{
					s_SpriteMeshToTextureCache.Remove(key);
				}
			}
		}

		public static SpriteMesh GetSpriteMeshFromSprite(Sprite sprite)
		{
			var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(sprite));
			
			if(s_SpriteMeshToTextureCache.ContainsValue(guid))
			{
				foreach(var pair in s_SpriteMeshToTextureCache)
				{
					if(pair.Value.Equals(guid))
					{
						var spriteMesh = LoadSpriteMesh(AssetDatabase.GUIDToAssetPath(pair.Key));
						
						if(spriteMesh && spriteMesh.sprite == sprite)
						{
							return spriteMesh;
						}
					}
				}
			}
			
			return null;
		}
		
		static SpriteMesh LoadSpriteMesh(string assetPath)
		{
			return AssetDatabase.LoadAssetAtPath(assetPath,typeof(SpriteMesh)) as SpriteMesh;
		}
	}
}
