﻿using UnityEngine;

namespace Spaceshipinha.Navinha
{
    internal class NaveThrusterFlameController : MonoBehaviour
    {
		public NaveFlightConsole naveFlightConsole;
		public NaveThrusterController naveThrusterController;
        public Light light;
        public ParticleSystem particles;
		ParticleSystem.SizeOverLifetimeModule size;
		private RelativisticParticleSystem rpsController;

		private float maxRange;

        private void Start() 
        {
			naveFlightConsole.OnEnterNaveFlightConsole += OnEnterNaveFlightConsole;
			naveFlightConsole.OnExitNaveFlightConsole += OnExitNaveFlightConsole;

			rpsController = particles.gameObject.AddComponent<RelativisticParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

			size = particles.sizeOverLifetime;

			rpsController.enabled = false;
			maxRange = light.range;
			light.enabled = false;
			enabled = false;

		}
		public void OnDestroy()
		{
			naveFlightConsole.OnEnterNaveFlightConsole -= OnEnterNaveFlightConsole;
			naveFlightConsole.OnExitNaveFlightConsole -= OnExitNaveFlightConsole;
		}
		private void Update()
		{
			float input = naveThrusterController.ReadTranslationalInput().z;
			if (input > 0f)
			{
				size.sizeMultiplier = input;
				light.range = maxRange * input;
				if (!particles.isPlaying)
				{
					particles.Play();
					light.enabled = true;
					return;
				}
			}
			else if (particles.isPlaying)
			{
				particles.Stop();
				light.enabled = false;
			}
		}

		private void OnEnterNaveFlightConsole()
		{
			rpsController.enabled = true;
			enabled = true;
		}

		private void OnExitNaveFlightConsole()
		{
			particles.Stop();
			light.enabled = false;
			rpsController.enabled = false;
			enabled = false;
		}
	}
}
