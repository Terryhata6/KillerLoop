
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilits
{
    public class CollectableSpray : MonoBehaviour
    {
        public static CollectableSpray Instance;

        #region PrivateFields

        private float _tempRad;
        private Vector3 temp;
        private List<Transform> _tempTF = new List<Transform>();

        #endregion

        private void Awake()
        {
            Instance = this;
        }

        #region PublicMethods

        public void SprayCollectables(List<Transform> transforms, float radius, float height)
        {
            StartCoroutine(Spray(transforms, radius, height));
        }

        public void SprayCollectable(Transform transform, float radius, float height)
        {
            _tempTF.Clear();
            _tempTF.Add(transform);
            SprayCollectables(_tempTF, radius, height);
        }

        #endregion

        #region PrivateMethods

        private IEnumerator Spray(List<Transform> transforms, float radius, float height)
        {
            Transform[] objects = new Transform[transforms.Count];
            transforms.CopyTo(objects);
            Vector3[] basePos = new Vector3[objects.Length];
            Vector3[] nextPos = new Vector3[objects.Length];
            float iterator = 0f;
            float x = -1f;
            bool count = true;


            for (int i = 0; i < objects.Length; i++)
            {
                basePos[i] = objects[i].position;
                _tempRad = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                nextPos[i] = (new Vector3(Mathf.Cos(_tempRad), 0, Mathf.Sin(_tempRad)) * radius) + objects[i].position;
            }


            while (count)
            {
                for (int j = 0; j < objects.Length; j++)
                {
                    temp = Vector3.Lerp(basePos[j], nextPos[j], iterator);
                    temp.y = -1.8f * ((x) * (x)) + height; // Менять параболу тут
                    if (objects[j])
                    {
                        objects[j].position = temp;
                    }
                }
                iterator += Time.deltaTime;
                x += Time.deltaTime * 2f;



                if (iterator >= 1)
                {
                    count = false;
                }
                yield return null;
            }

        }

        #endregion
    }
}