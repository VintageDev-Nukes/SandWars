using UnityEngine;
using System.Collections;

public enum ResourceTypeOption : byte
{
	Scene,
	Web
}

public class Load {

	public static void OnClick(string ResourceToLoad, ResourceTypeOption ResourceTypeToLoad = ResourceTypeOption.Scene)
	{
		switch (ResourceTypeToLoad)
		{
		case ResourceTypeOption.Scene:
			Application.LoadLevel(ResourceToLoad);
			break;
		case ResourceTypeOption.Web:
			Application.OpenURL(ResourceToLoad);
			break;
		}
	}

}
