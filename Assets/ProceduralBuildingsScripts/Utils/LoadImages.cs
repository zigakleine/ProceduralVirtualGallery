using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System;
 

public class LoadImages
{

    int[] imageGroups = {1,1,2,1,1,0,1,1,1,1,0,1,0,1,0,1,1,0,0,0,1,0,0,0,1,1,0,1,1,1,1,1,1,1,1,0,0,1,1,2,0,1,0,0,1,1,1,2,1,1,0,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,0,0,1,0,0,0,1,2,0,0,2,0,0,0,1,0,0,0,0,0,1,0,1,0,0,0,0,2,0,0,0,2,2,0,0,2,0,0,0,0,0,0,2,0,0,0,1,0,0,0,0,0,2,2,0,0,0,0,2,0,0,2,0,2,2,2,2,0,2,2,2,2,2,2,0,2,2,2,2,2,0,0,2,2,2,0,2,2,0,2,0,2,0,2,1,0,2,2,2,2,2,2,0,0,2,2,0,0,2,2,2,2,2,0,2,2,2,0,0,2,0,2,2,2,2,2};

    List<Texture2D>[] images;
    public List<Texture2D>[] Images{ get => images; }

    int[] imageArrayCounter;
    int[] imageAmounts;
    public int[] ImageAmounts{get => imageAmounts; }



    public LoadImages() {


        UnityEngine.Object[] textures = Resources.LoadAll("artwork", typeof(Texture2D));

        this.images = new List<Texture2D>[3];
        for(int i = 0; i<3; i++ ){ 
            this.images[i] = new List<Texture2D>();
        }
    
        this.imageArrayCounter = new int[3];
        this.imageAmounts = new int[3];

        for(int i = 0; i < textures.Length; i++) {

            //Debug.Log(textures[i].name);
            Texture2D img = (Texture2D) textures[i];
            Texture2D newTex = new Texture2D(img.width, img.height, TextureFormat.ARGB32, false);
            newTex.SetPixels(img.GetPixels());
            newTex.Apply();
            this.images[imageGroups[i]].Add(newTex);
      
        }

        for(int i = 0; i<3; i++ ){
            this.imageAmounts[i] = this.images[i].Count;
            this.imageArrayCounter[i] = 0;
        }


    }


    // public Images ClassifyImages() {
    //     Images images = new Images();

    //     images.images = new string[imageAmount];

    //     for (int i = 0; i < imageAmount; i++) {
             
    //         byte[] bytes = this.images[i].EncodeToJPG();
          
    //         string enc = Convert.ToBase64String(bytes);
    //         images.images[i] = enc;

    //     }
    //     return images;
    // }

    public Texture2D GetNextTexture(int cluster) {
        Texture2D toReturn = this.images[cluster][this.imageArrayCounter[cluster]];

        if(imageArrayCounter[cluster] + 1 >= this.imageAmounts[cluster]) {
            this.imageArrayCounter[cluster] = 0;
        }
        else {
            this.imageArrayCounter[cluster]++;
        }

        return toReturn;
    }


}
