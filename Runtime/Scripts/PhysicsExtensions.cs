using System.Collections.Generic;
using UnityEngine;

namespace UCore.Extensions
{
    public static class PhysicsExtensions
    {
        public static bool ConeCast(
            Vector3 origin,
            float maxRadius,
            Vector3 direction,
            float maxDistance,
            float coneAngle,
            out RaycastHit hit,
            LayerMask layerMask)
        {
            var didHit = Physics.SphereCast(
                origin - new Vector3(0, 0, maxRadius),
                maxRadius,
                direction,
                out var sphereCastHit,
                maxDistance,
                layerMask);

            if (!didHit) {
                hit = default;
                return false;
            }

            var hitPoint = sphereCastHit.point;
            var directionToHit = hitPoint - origin;
            var angleToHit = Vector3.Angle(direction, directionToHit);

            if (angleToHit > coneAngle) {
                hit = default;
                return false;
            }

            hit = sphereCastHit;
            return true;
        }

        public static RaycastHit[] ConeCastAll(
            Vector3 origin,
            float maxRadius,
            Vector3 direction,
            float maxDistance,
            float coneAngle,
            LayerMask layerMask)
        {
            var sphereCastHits = Physics.SphereCastAll(
                origin - new Vector3(0, 0, maxRadius),
                maxRadius,
                direction,
                maxDistance,
                layerMask);

            var coneHits = new List<RaycastHit>();

            if (sphereCastHits.Length > 0) {
                for (int i = 0; i < sphereCastHits.Length; i++) {
                    var hitPoint = sphereCastHits[i].point;
                    var directionToHit = hitPoint - origin;
                    var angleToHit = Vector3.Angle(direction, directionToHit);

                    if (angleToHit < coneAngle) {
                        coneHits.Add(sphereCastHits[i]);
                    }
                }
            }
            return coneHits.ToArray();
        }
    }
}
