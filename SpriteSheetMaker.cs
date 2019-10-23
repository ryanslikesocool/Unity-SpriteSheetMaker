using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEditor;
using System.Linq;

namespace ifelse {
    public class SpriteSheetMaker : EditorWindow
    {
        public string fileName;
        public Texture2D[] textureArray;
        public int maxXSprites;

        Vector2 scrollPos;

        [MenuItem("ifelse/Sheet Maker")]
        static void Init()
        {
            SpriteSheetMaker window = (SpriteSheetMaker)EditorWindow.GetWindow<SpriteSheetMaker>();
            window.titleContent.text = "Sheet Maker";
            window.Show();
        }

        void OnGUI()
        {
            ScriptableObject scriptableObj = this;
            SerializedObject serialObj = new SerializedObject(scriptableObj);
            SerializedProperty texArray = serialObj.FindProperty("textureArray");

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.PropertyField(texArray, true);

            EditorGUILayout.EndScrollView();

            maxXSprites = EditorGUILayout.IntField("Max Horizontal Sprites", maxXSprites);

            fileName = EditorGUILayout.TextField("File Name", fileName);

            if (GUILayout.Button("Create Sheet"))
            {
                textureArray = textureArray.OrderBy(t => t.name).ToArray();

                Texture2D tex = CreateSpriteSheet(textureArray, maxXSprites);

                // Convert the new texture to a byte array
                byte[] bytes = tex.EncodeToPNG();

                // Bad workaround to get the path of the file without the actual file
                string[] paths = AssetDatabase.GetAssetPath(textureArray[0]).Split('/');
                string path = AssetDatabase.GetAssetPath(textureArray[0]).TrimEnd(paths[paths.Length - 1].ToCharArray());

                // Writes the bytes and imports the asset immediately
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(path, fileName + ".png"), bytes);
                AssetDatabase.ImportAsset(System.IO.Path.Combine(path, fileName + ".png"));
            }

            serialObj.ApplyModifiedProperties();
        }

        public static Texture2D CreateSpriteSheet(Texture2D[] inputTextures, int maxXSprites)
        {
            // Calculate width and height
            int width = inputTextures[0].width * maxXSprites;
            int height = 0;
            for (int i = 0; i < inputTextures.Length; i++)
            {
                if (i % maxXSprites == 0)
                {
                    height += inputTextures[i].height;
                }
            }

            // Create a new width * height texture Low Dynamic Range and no mipmaps
            Texture2D texture = new Texture2D(width, height, DefaultFormat.LDR, TextureCreationFlags.None);

            // Set the pixel values
            int hOffset = 0;
            int vOffset = texture.height - inputTextures[0].height;
            for (int i = 0; i < inputTextures.Length; i++)
            {
                for (int j = 0; j < inputTextures[i].width; j++)
                {
                    for (int k = 0; k < inputTextures[i].height; k++)
                    {
                        texture.SetPixel(j + hOffset, k + vOffset, inputTextures[i].GetPixel(j, k));
                    }
                }
                hOffset += inputTextures[i].width;
                if (i % maxXSprites == maxXSprites - 1 && i != 0)
                {
                    vOffset -= inputTextures[i].height;
                    hOffset = 0;
                }
            }

            // Apply all SetPixel calls
            texture.Apply();

            return texture;
        }
    }
}
