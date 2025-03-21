using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard {
        public Vector3 moveToPosition;
        public bool inAlumb = false;
        public bool inBack = false;
        public bool detectPlayer = false;
        public bool alumbTrig = false;
        public Transform alumbObj = null;
        public bool fuck = false;
    }
}