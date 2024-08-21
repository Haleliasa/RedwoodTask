#nullable enable

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Injector {
    private readonly IServiceProvider serviceProvider;
    private readonly List<GameObject> gameObjects = new();
    private readonly List<IInjectComponent> components = new();

    public Injector(IServiceProvider serviceProvider) {
        this.serviceProvider = serviceProvider;
    }

    public void Inject(Scene scene, bool includeAssets = true) {
        scene.GetRootGameObjects(this.gameObjects);
        Inject(this.gameObjects, deep: true);

        if (includeAssets) {
            ScriptableObject[] assets = Resources.FindObjectsOfTypeAll<ScriptableObject>();
            Inject(assets);
        }
    }

    public void Inject(IEnumerable<GameObject> gameObjects, bool deep = true) {
        foreach (GameObject gameObject in gameObjects) {
            Inject(gameObject, deep);
        }
    }

    public void Inject(GameObject gameObject, bool deep = true) {
        if (deep) {
            gameObject.GetComponentsInChildren(includeInactive: true, this.components);
        } else {
            gameObject.GetComponents(this.components);
        }
        foreach (IInjectComponent component in this.components) {
            Inject(component);
        }
        this.components.Clear();
    }

    public void Inject(IEnumerable targets) {
        foreach (object target in targets) {
            Inject(target);
        }
    }

    public void Inject(object target) {
        List<MethodInfo> methods = target.GetType()
            .GetMethods(BindingFlags.Instance
                | BindingFlags.Static
                | BindingFlags.Public
                | BindingFlags.NonPublic)
            .Where(m => Attribute.IsDefined(m, typeof(InjectAttribute)))
            .ToList();
        foreach (MethodInfo method in methods) {
            object[] parameters = method.GetParameters().Where(p => !p.IsOut).Select(p => {
                if (p.ParameterType == typeof(Injector)) {
                    return this;
                }

                InjectKeyAttribute? keyAttr = p.GetCustomAttribute<InjectKeyAttribute>();
                if (keyAttr != null) {
                    return this.serviceProvider.GetRequiredKeyedService(p.ParameterType, keyAttr.Key);
                }

                return this.serviceProvider.GetRequiredService(p.ParameterType);
            }).ToArray();
            method.Invoke(!method.IsStatic ? target : null, parameters);
        }
    }
}
