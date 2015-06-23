/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using System.Collections;
using System;
using System.Collections.Generic;
using SoomlaWpCore.util;
using SoomlaWpCore.data;

namespace SoomlaWpCore.rewards
{

	/// <summary>
	/// A reward is an entity which can be earned by the user for meeting certain
	/// criteria in game progress.  For example - a user can earn a badge for completing
	/// a mission. Dealing with <code>Reward</code>s is very similar to dealing with
	/// <code>VirtualItem</code>s: grant a reward by giving it and recall a
	/// reward by taking it.
	///
	/// In the Profile module, rewards can be attached to various actions. For example:
	/// You can give your user 100 coins for logging in through Facebook.
	/// </summary>
	public abstract class Reward : SoomlaEntity<Reward> {
		private static string TAG = "SOOMLA Reward";

		public Schedule Schedule;

		public bool Owned {
			get {
				return RewardStorage.IsRewardGiven(mID);
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">the reward's ID (unique id to distinguish between rewards).</param>
		/// <param name="name">the reward's name.</param>
		public Reward(string id, string name)
			: base(name, "", id)
		{
			Schedule = Schedule.AnyTimeOnce();

            if (RewardsMap.ContainsKey(this.mID))
            {
                RewardsMap[this.mID] = this;
            }
            else
            {
                RewardsMap.Add(this.mID, this);
            }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonReward">A JSONObject representation of <c>Reward</c>.</param>
		public Reward(JSONObject jsonReward) :
			base(jsonReward)
		{
			JSONObject scheduleObj = jsonReward[JSONConsts.SOOM_SCHEDULE];
			if (scheduleObj) {
				Schedule = new Schedule(scheduleObj);
			} else {
				Schedule = null;
			}

            if (RewardsMap.ContainsKey(this.mID))
            {
                RewardsMap[this.mID] = this;
            }
            else
            {
                RewardsMap.Add(this.mID, this);
            }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>A JSONObject representation of <c>Reward</c>.</return>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			if (Schedule != null) {
				obj.AddField(JSONConsts.SOOM_SCHEDULE, Schedule.toJSONObject());
			} else {
				obj.AddField(JSONConsts.SOOM_SCHEDULE, Schedule.AnyTimeOnce().toJSONObject());
			}

			return obj;
		}

		/// <summary>
		/// Factory method to easily create a reward according to the given JSONObject.
		/// </summary>
		/// <returns>A JSONObject representation of <c>Reward</c>.</returns>
		/// <param name="rewardObj">The actual reward according to the given JSONObject.</param>
		public static Reward fromJSONObject(JSONObject rewardObj) {
			string className = rewardObj[JSONConsts.SOOM_CLASSNAME].str;

			Reward reward = (Reward) Activator.CreateInstance(Type.GetType("Soomla." + className), new object[] { rewardObj });

            if (RewardsMap.ContainsKey(reward.mID))
            {
                RewardsMap[reward.mID] = reward;
            }
            else
            {
                RewardsMap.Add(reward.mID, reward);
            }

			return reward;
		}

		public bool Take() {

			if (!RewardStorage.IsRewardGiven(mID)) {
				SoomlaUtils.LogDebug(TAG, "Reward not given. id: " + mID);
				return false;
			}

			if (takeInner()) {
				RewardStorage.SetRewardStatus(mID, false);
				return true;
			}

			return false;
		}

		public bool CanGive() {
			return Schedule.Approve(RewardStorage.GetTimesGiven(mID));
		}

		public bool Give() {

			if (!CanGive()) {
				SoomlaUtils.LogDebug(TAG, "(Give) Reward is not approved by Schedule. id: " + mID);
				return false;
			}

			if (giveInner()) {
				RewardStorage.SetRewardStatus(mID, true);
				return true;
			}

			return false;
		}

		protected abstract bool giveInner();

		protected abstract bool takeInner();


		private static Dictionary<string, Reward> RewardsMap = new Dictionary<string, Reward>();
		public static Reward GetReward(string rewardID) {
			Reward reward = null;
			RewardsMap.TryGetValue(rewardID, out reward);

			return reward;
		}

		public static List<Reward> GetRewards(){
			List<Reward> rewards = new List<Reward> ();
			foreach(Reward reward in RewardsMap.Values)
				rewards.Add(reward);

			return rewards;
		}
	}
}
