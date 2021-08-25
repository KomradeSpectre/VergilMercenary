using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static VergilMercenary.VergilMerc;

namespace VergilMercenary
{
	public class VergilController : MonoBehaviour
	{
		public uint btlPlayID;

		public uint dtPlayID;

		private bool dtPlayed;

		private bool dtPlaying;

		// Token: 0x0600000A RID: 10 RVA: 0x0000302C File Offset: 0x0000122C
		public void ToggleMusic()
		{
			bool flag = this.dtPlaying;
			if (flag)
			{
				this.dtPlayID = Util.PlaySound("Devil_Trigger_Pause", base.gameObject);
				this.dtPlaying = false;
			}
			bool flag2 = this.btlPlayID == 0U;
			if (flag2)
			{
				musicActive++;
				this.btlPlayID = Util.PlaySound("Bury_The_Light_System", base.gameObject);
				bool flag3 = NetworkClient.active && !NetworkServer.active;
				if (flag3)
				{
					AkSoundEngine.SetState("Ranking", "SSS");
				}
			}
			else
			{
				musicActive--;
				AkSoundEngine.StopPlayingID(this.btlPlayID);
				this.btlPlayID = 0U;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000030E0 File Offset: 0x000012E0
		public void ToggleAltMusic()
		{
			bool flag = this.dtPlayed;
			if (flag)
			{
				bool flag2 = this.dtPlaying;
				if (flag2)
				{
					musicActive--;
					this.dtPlayID = Util.PlaySound("Devil_Trigger_Pause", base.gameObject);
				}
				else
				{
					bool flag3 = this.btlPlayID > 0U;
					if (flag3)
					{
						AkSoundEngine.StopPlayingID(this.btlPlayID);
						this.btlPlayID = 0U;
					}
					else
					{
						musicActive++;
					}
					this.dtPlayID = Util.PlaySound("Devil_Trigger_Resume", base.gameObject);
					bool flag4 = NetworkClient.active && !NetworkServer.active;
					if (flag4)
					{
						AkSoundEngine.SetState("Style_Rank", "SSS");
					}
				}
				this.dtPlaying = !this.dtPlaying;
			}
			else
			{
				bool flag5 = this.btlPlayID > 0U;
				if (flag5)
				{
					AkSoundEngine.StopPlayingID(this.btlPlayID);
					this.btlPlayID = 0U;
				}
				else
				{
					musicActive++;
				}
				this.dtPlayed = true;
				this.dtPlaying = true;
				this.dtPlayID = Util.PlaySound("Devil_Trigger", base.gameObject);
				bool flag6 = NetworkClient.active && !NetworkServer.active;
				if (flag6)
				{
					AkSoundEngine.SetState("Style_Rank", "SSS");
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00003234 File Offset: 0x00001434
		private void OnDestroy()
		{
			bool flag = this.btlPlayID > 0U;
			if (flag)
			{
				musicActive--;
				AkSoundEngine.StopPlayingID(this.btlPlayID);
			}
			bool flag2 = this.dtPlaying;
			if (flag2)
			{
				musicActive--;
				this.dtPlayID = Util.PlaySound("Devil_Trigger_Pause", base.gameObject);
				AkSoundEngine.StopPlayingID(this.dtPlayID);
			}
		}
	}
}
