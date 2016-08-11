﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPinballSlave
{
	public class FeedbackSlave
	{
		private ProPinballBridge.ProPinballFeedback _bridge;

		public void Start()
		{
			try {
				var oType = Type.GetTypeFromProgID("DirectOutput.ComObject", true);
				Console.WriteLine("COM object found.");
				if (oType != null) {
					var instance = Activator.CreateInstance(oType);
					Console.WriteLine("COM instance created.");
					var dof = (IDirectOutputCom)System.Runtime.InteropServices.Marshal.CreateWrapperOfType(instance, typeof(IDirectOutputCom));
					Console.WriteLine("Successfully loaded DOF {0}", dof.GetVersion());
				}
			} catch (Exception e) {
				Console.WriteLine("Error connecting to DOF: {0}", e.Message);
				//return;
			}

			CreateBridge();
			unsafe
			{
				_bridge.GetFeedback((flasherId, flasherName, flasherIntensity) => {
					Console.WriteLine("Flasher {0} ({1}): {2}", flasherId, new string(flasherName), flasherIntensity);

				}, (solenoidId, solenoidName, solenoidStatus) => {
					Console.WriteLine("Solenoid {0} ({1}): {2}", solenoidId, new string(solenoidName), solenoidStatus);

				}, (flipperId, flipperName, flipperStatus) => {
					Console.WriteLine("Flipper {0} ({1}): {2}", flipperId, new string(flipperName), flipperStatus);

				}, (buttonId, buttonName, buttonStatus) => {
					Console.WriteLine("Button {0} ({1}): {2}", buttonId, new string(buttonName), buttonStatus);

				}, msg => {
					Console.WriteLine("ERROR: {0}", new string(msg));

				}, () => {
					Console.WriteLine("All done!");

				});
			}
		}

		private void CreateBridge()
		{
			_bridge = new ProPinballBridge.ProPinballFeedback(392);
			if (_bridge.Status != 0) {
				unsafe
				{
					throw new Exception("Error connecting: " + new string(_bridge.Error));
				}
			}
		}

		static void Main(string[] args)
		{
			try {
				new FeedbackSlave().Start();

			} catch (Exception e) {
				Console.WriteLine("Error: {0}", e.Message);
			}
		}
	}
}
