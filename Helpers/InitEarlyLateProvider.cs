using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Z;

//v .02 // from sceneloader 
//todo initializeonolad

public class InitEarlyLateProvider : MonoBehaviour
{

	// using System.Collections;
	// using System.Collections.Generic;
	// using UnityEngine;
	// using UnityEngine.SceneManagement;
	// using UnityEngine.UI;
	// using zUniqueness;
	// // #if NOT
	// // using OdinSerializer;
	// using System.IO;

	// public class SceneLoadHelper : MonoBehaviour
	// {

	// void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	// {
	//     if (printDebugInfo) Debug.Log("onload " + scene.name);
	//     lastLoadedSceneName = scene.name;
	//     var objectsLoadedWithThisScene = FindObjectsFromScene(scene);
	//     StartCoroutine(SpreadProcessingScene(objectsLoadedWithThisScene, scene));
	// }

	List<IRequestInitEarly> earliesProcessed = new List<IRequestInitEarly>();
	List<IRequestInitLate> latesProcessed = new List<IRequestInitLate>();
	List<IRequestShutdownInfo> shutdownListeners = new List<IRequestShutdownInfo>();

	void ProcessEarlyInits(List<IRequestInitEarly> earlies)
	{
		//        Debug.Log(Time.frameCount + " processing lis of " + earlies.Count);
		for (int j = 0; j < earlies.Count; j++)
		{
			if (!earliesProcessed.Contains(earlies[j]))
			{
				earliesProcessed.Add(earlies[j]);
				earlies[j].Init(this);
				// var shutdown=earlies[j].gameObject.GetComponent<IRequestShutdownInfo>();
				// if (shutdown!=null)
				// {
				//     Debug.Log("found shoutd");
				//     shutdownListeners.Add(shutdown);
				// }
			}
		}

	}
	IEnumerator ProcessLateInits(List<IRequestInitLate> lateInits)
	{
		// Debug.Log("processing " + lateInits.Count + " lateinits");
		yield return null;
		for (int j = 0; j < lateInits.Count; j++)
		{
			if (!latesProcessed.Contains(lateInits[j]))
			{
				latesProcessed.Add(lateInits[j]);
				lateInits[j].Init(this);
			}
		}

	}
	void OnDestroy()
	{
		for (int i = shutdownListeners.Count - 1; i >= 0; i--)
		{
			if (shutdownListeners[i] == null)
			{
				// if (printDebugInfo) Debug.Log("dead shutodwn");
			}
			else
			{
				shutdownListeners[i].OnShutdown(this);
				shutdownListeners.RemoveAt(i);
			}
		}
	}


	// [RuntimeInitializeOnLoadMethod]
	void Start()
	{
		// Debug.Log("aerlystart");
		// ProcessObjectsAlreadyPresent();
		// var c = gameObjects.Select(x => { if (x != null) return x.GetComponents(typeof(Component)); return null; });
		// EndClock("useless linq");

		var components = Resources.FindObjectsOfTypeAll<Component>() as Component[];

		List<IRequestInitEarly> earlies1 = new List<IRequestInitEarly>();
		List<IRequestInitLate> lates = new List<IRequestInitLate>();

		// shutdownListeners = new List<IRequestShutdownInfo>();
		// StartClock();
		foreach (var thisComponent in components)
		{

			var early = thisComponent as IRequestInitEarly;
			if (early != null)
			{
				earlies1.Add(early);
			}
			var late = thisComponent as IRequestInitLate;
			if (late != null) { lates.Add(late); }

			var shutdown = thisComponent as IRequestShutdownInfo;
			if (shutdown != null)
				shutdownListeners.Add(shutdown);
			// if (c is IRequestInitEarly)
			// earlies1.AddRange(g.GetComponents<IRequestInitEarly>());
			// lates.AddRange(g.GetComponents<IRequestInitLate>());
			// shutdownListeners.AddRange(g.GetComponents<IRequestShutdownInfo>());
		}
		ProcessEarlyInits(earlies1);
		StartCoroutine(ProcessLateInits(lates));
	}
	// EndClock("all getcomponents");
	// StartClock();

void OnEnable()
{
	// SceneManager.sceneLoaded += OnSceneLoaded;
	// SceneManager.sceneUnloaded += OnSceneUnloaded;
}
void OnDisable()
{
	// SceneManager.sceneLoaded -= OnSceneLoaded;
	// SceneManager.sceneUnloaded -= OnSceneUnloaded;
}
// }
// #endif

}