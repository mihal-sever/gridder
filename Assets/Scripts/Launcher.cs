using System;
using System.Linq;
using UnityEngine;

namespace Sever.Gridder
{
    public class Launcher : MonoBehaviour
    {
        private void Awake()
        {
            Init();
            DataLoader.LoadProjects();
        }

        public void Init()
        {
            var initializables = FindObjectsOfType<MonoBehaviour>(true).OfType<IInitializable>();
            foreach (IInitializable initializable in initializables)
            {
                initializable.Init();
            }
        }
    }
}