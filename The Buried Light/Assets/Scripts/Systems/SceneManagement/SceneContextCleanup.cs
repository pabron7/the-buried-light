using System.Collections.Generic;
using UnityEngine;
using Zenject;

public static class SceneContextCleanup
{
    public static void CleanupAllSceneContexts()
    {
        // Access the SceneContextRegistry instance
        var sceneContextRegistry = ProjectContext.Instance.Container.Resolve<SceneContextRegistry>();
        var activeContexts = new List<SceneContext>(sceneContextRegistry.SceneContexts);

        foreach (var context in activeContexts)
        {
            if (context != null)
            {
                Debug.Log($"Cleaning up SceneContext: {context.name}");
                // Destroy the SceneContext GameObject
                Object.Destroy(context.gameObject);
            }
        }

        Debug.Log("All active SceneContexts have been cleaned up.");
    }
}
