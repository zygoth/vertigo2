using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelPerfectQuad : PixelPerfectBase {

	Texture checkTexture;
	
	new public void LateUpdate() {
		if (checkTexture!=renderer.sharedMaterial.mainTexture) {
			offset=new Vector3(
				(((int)renderer.sharedMaterial.mainTexture.width)%2==0)?0:PixelPerfect.unitsPerPixel*0.5f,
				(((int)renderer.sharedMaterial.mainTexture.height)%2==0)?0:PixelPerfect.unitsPerPixel*0.5f,
				0);
			UpdateScale();
			checkTexture=renderer.sharedMaterial.mainTexture;
		}
		base.LateUpdate();
	}

	override public void UpdateScale() {
		if (checkPixelScale!=pixelScale || checkTexture!=renderer.sharedMaterial.mainTexture) {
			Transform saveParent=transform.parent;
			transform.parent=null;
			transform.localScale=new Vector3(
				Mathf.Sign(transform.localScale.x)*pixelScale*PixelPerfect.unitsPerPixel*renderer.sharedMaterial.mainTexture.width,
				Mathf.Sign(transform.localScale.y)*pixelScale*PixelPerfect.unitsPerPixel*renderer.sharedMaterial.mainTexture.height,
				transform.localScale.z);
			transform.parent=saveParent;
			checkPixelScale=pixelScale;
		}
	}

	override protected float GetImageWidth () {return renderer.sharedMaterial.mainTexture.width;}
	override protected float GetImageHeight() {return renderer.sharedMaterial.mainTexture.height;}
}
