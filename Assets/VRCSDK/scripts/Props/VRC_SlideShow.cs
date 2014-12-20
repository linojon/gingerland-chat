using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRC_SlideShow : MonoBehaviour 
{
	public bool autoplay = false;
	public bool shuffle = false;
	public Texture2D[] images;
	public float displayDuration = -1.0f;
	public Material imageMaterial;

	private int showingImage = 0;

	void Start () 
	{
		if(images.Length > 0)
			imageMaterial.mainTexture = images[0];

		if(autoplay && displayDuration > 0)
		{
			StartCoroutine("StartAutoplayWithDuration", displayDuration);
		}
	}
	
	void ShowNextImage( int Instigator = 0 )
	{
		Texture2D nextImage = null;

		if(images.Length > 0)
		{
			if(shuffle)
				showingImage = Random.Range(0, images.Length-1);
			else
				showingImage = (++showingImage) % images.Length;
			
			nextImage = images[showingImage];
			if(nextImage == null)
				Debug.LogError("Loaded image is null. Did you add the image to the array in the inspector?");
			imageMaterial.mainTexture = nextImage;
		}
		else
		{
			Debug.LogError("Image array length is zero.");
		}
	}

	void ShowPreviousImage( int Instigator = 0 )
	{
		Texture2D previousImage = null;
		showingImage = (--showingImage) % images.Length;
		if(showingImage < 0)
			showingImage = images.Length + showingImage;

		Debug.Log ("showing prev image: " + showingImage);
		previousImage = images[showingImage];
		if(previousImage == null)
			Debug.LogError("Loaded image is null. Did you add the image to the array in the inspector?");
		imageMaterial.mainTexture = previousImage;
	}

	IEnumerator StartAutoplayWithDuration(float duration)
	{
		while(autoplay)
		{
			yield return new WaitForSeconds(duration);
			ShowNextImage();
		}
	}

	void StopAutoplayWithDuration(float duration)
	{
		StopCoroutine("StartAutoplayWithDuration");
	}
}
