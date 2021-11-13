using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Simulation.Solvers
{
    public struct Spring
    {
        public Vector3 position;
        public Vector3 velocity;

        public Spring Update(
            Vector3 target,
            float mass = 1f,
            float dampening = 1f,
            float coefficient = 1f,
            float threshold = 0.01f)
        {
            return Update(
                Time.deltaTime,
                target,
                mass,
                dampening,
                new Vector3(coefficient, coefficient, coefficient),
                threshold
            );
        }

        public Spring Update(
            Vector3 target,
            float mass,
            float dampening,
            Vector3 coefficients,
            float threshold = 0.01f)
        {
            return Update(Time.deltaTime, target, mass, dampening, coefficients, threshold);
        }

        public Spring Update(
            float dt,
            Vector3 target,
            float mass,
            float dampening,
            Vector3 coefficients,
            float threshold = 0.01f)
        {
            var force = Vector3.zero;
            var stretch = position - target;
            var magnitude = stretch.magnitude;

            if (magnitude > threshold)
            {
                stretch = Vector3.Scale(stretch, Vector3.one / magnitude);
                stretch = Vector3.Scale(stretch, Vector3.one / stretch.magnitude);
                force += Vector3.Scale(stretch,  -coefficients * magnitude);
            }

            velocity += Vector3.Scale(force, Vector3.one / mass) * dt;
            position += velocity;
            velocity *= Mathf.Clamp01(1f - (dampening * dt));
            return this;
        }

        [DebuggerStepThrough] public static implicit operator Vector3(Spring s)
        {
            return s.position;
        }

        [DebuggerStepThrough] public override string ToString()
        {
            return position.ToString();
        }
    }
} // Hapki.Solvers