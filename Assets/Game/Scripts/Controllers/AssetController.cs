using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class AssetController : MonoBehaviour
    {
        [SerializeField] private List<Color> _colors;
        public List<Color> Colors => _colors;

        #region Singleton

        private static AssetController _instance;

        public static AssetController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AssetController>();

                    if (_instance == null)
                    {
                        Debug.LogError($"{typeof(AssetController)} is needed in the scene but it does not exist!");
                    }
                }
                return _instance;
            }
        }

        #endregion
    }
}