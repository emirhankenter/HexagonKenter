﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class HexagonLevelBehaviour : MonoBehaviour
    {
        private List<HexagonBehaviour> _hexagonBehaviours;
        public List<HexagonBehaviour> HexagonBehaviours => _hexagonBehaviours;
    }
}