/*
Copyright (c) 2014 Andrew Jones
 Based on 'Spriter2Unity' python code by Malhavok

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ThirdParty.Spriter2Unity.Editor.Spriter;

namespace Assets.ThirdParty.Spriter2Unity.Editor.Unity
{
    //Name clash between Spriter and Unity - use the Spriter version by default
    using Animation = Assets.ThirdParty.Spriter2Unity.Editor.Spriter.SpriterAnimation;

    public class PrefabBuilder
    {
        private CharacterMap charMap;

        public GameObject MakePrefab(Entity entity, GameObject root, string spriteFolder)
        {
            //Set the name (in case it changed)
            root.name = entity.Name;

            //Build the character map first
            var mapBuilder = new CharacterMapBuilder();
            charMap = mapBuilder.BuildMap(entity, root, spriteFolder);

            //Build the GameObject hierarchy
            foreach (var animation in entity.Animations)
            {
                MakePrefab(animation, root);
            }
            return root;
        }

        private void MakePrefab(Animation animation, GameObject root)
        {
            foreach(var mainKey in animation.MainlineKeys)
            {
                MakePrefab(mainKey, root);
            }
        }

        private struct RefParentInfo
        {
            public Ref Ref;
            public GameObject Root;
        }

        private void MakePrefab(MainlineKey mainKey, GameObject root)
        {
            var rootInfo = mainKey.GetChildren(null).Select(child => new RefParentInfo { Ref = child, Root = root });
            Stack<RefParentInfo> toProcess = new Stack<RefParentInfo>(rootInfo);
            while(toProcess.Count > 0)
            {
                var next = toProcess.Pop();
                var go = MakePrefab(next.Ref, next.Root);
                foreach(var child in mainKey.GetChildren(next.Ref))
                {
                    toProcess.Push(new RefParentInfo{Ref = child, Root = go});
                }
            }
        }
        
        private GameObject MakePrefab(Ref childRef, GameObject root)
        {
            var timeline = childRef.Referenced.Timeline;
            var transform = root.transform.Find(timeline.Name);
            GameObject go;
            if(transform == null)
            {
                go = MakeGameObject(childRef, root);
            }
            else
            {
                go = transform.gameObject;
            }
            return go;
        }

        private GameObject MakeGameObject(Ref childRef, GameObject parent)
        {
            TimelineKey key = childRef.Referenced;
            GameObject go = new GameObject(key.Timeline.Name);
            if (parent != null)
            {
                go.transform.parent = parent.transform;
            }

            //Any objects that show up only after t=0 begin inactive
            if(key.Time_Ms > 0)
            {
                go.SetActive(false);
            }

            var spriteKey = key as SpriteTimelineKey;
            if (spriteKey != null)
            {
                //Set initial sprite information
                var sprite = go.AddComponent<SpriteRenderer>();
                sprite.sprite = charMap.GetSprite(spriteKey.File.Folder.Id, spriteKey.File.Id);
            }

            SetTransform(childRef, go.transform);

            return go;
        }

        private void SetTransform(Ref childRef, Transform transform)
        {
            Vector3 localPosition;
            Vector3 localScale;
			Vector3 localEulerAngles;

            PrefabUtils.BakeTransforms(childRef, out localPosition, out localEulerAngles, out localScale);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
			transform.localEulerAngles = localEulerAngles;
        }
    }
}
