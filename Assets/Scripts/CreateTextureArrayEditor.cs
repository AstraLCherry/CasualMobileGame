using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateTextureArrayEditor
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Texture 2D Array")]
    static void CreateTexture2DArrayAsset()
    {
        // Define parameters for the Texture2DArray
        int width = 64; // Adjust as needed
        int height = 64; // Adjust as needed
        int slices = 6; // Number of textures in the array
        TextureFormat format = TextureFormat.RGBA32; // Adjust format as needed
        bool mipChain = true; // Whether to generate mipmaps

        // Create the Texture2DArray instance
        Texture2DArray textureArray = new Texture2DArray(width, height, slices, format, mipChain);
        textureArray.name = "MyTextureArray"; // Name for the asset

        // Optional: Populate the texture array with data from existing textures
        // Example: Assuming you have textures named "Tex0", "Tex1", etc., in a Resources folder
        for (int i = 0; i < slices; i++)
        {
            Texture2D sourceTexture = Resources.Load<Texture2D>($"Tex{i}"); // Load from Resources folder
            if (sourceTexture != null)
            {
                Graphics.CopyTexture(sourceTexture, 0, 0, textureArray, i, 0);
            }
            else
            {
                Debug.LogWarning($"Texture Tex{i} not found in Resources folder.");
                // Optionally, fill with a default color if source texture is missing
                Color[] defaultPixels = new Color[width * height];
                for (int p = 0; p < defaultPixels.Length; p++)
                {
                    defaultPixels[p] = (i % 2 == 0) ? Color.red : Color.blue; // Example pattern
                }
                textureArray.SetPixels(defaultPixels, i);
            }
        }
        textureArray.Apply(); // Apply changes after setting pixels

        // Save the Texture2DArray as an asset
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + textureArray.name + ".asset");
        AssetDatabase.CreateAsset(textureArray, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Created Texture2DArray asset at: {assetPathAndName}");
    }
#endif
}