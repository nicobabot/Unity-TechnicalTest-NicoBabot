using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tangelo.Test.Components
{
    public class SpinnerComponent : MonoBehaviour, IActionComponent
    {
        // QUESTION 1: Implement here the rotation
        public float angle_of_rotation = 90.0f;
        float range_stop_calculating = 1.0f;

        public void DoAction()
        {
            throw new System.NotImplementedException();
        }

        void Update()
        {
            // QUESTION 2: This is an obsolete method. We want to do it using DOTween and out of the Update.
            //transform.RotateAround(new Vector3(0, 1, 0), 90);

            //To ensure that there are not more calculations in an update
            if (transform.rotation.eulerAngles.z < angle_of_rotation - range_stop_calculating ||
                transform.rotation.eulerAngles.z > angle_of_rotation + range_stop_calculating)
            {
                transform.DORotate(new Vector3(0, 0, angle_of_rotation), 0.25f);
            }



        }
    }
}