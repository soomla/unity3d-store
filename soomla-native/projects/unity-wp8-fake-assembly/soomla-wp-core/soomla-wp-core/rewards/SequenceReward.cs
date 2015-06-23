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
using System.Collections.Generic;
using SoomlaWpCore.util;
using SoomlaWpCore.data;


namespace SoomlaWpCore.rewards
{	

	/// <summary>
	/// A specific type of <code>Reward</code> that holds of list of other rewards
	/// in a certain sequence.  The rewards are given in ascending order.  For example,
	/// in a Karate game the user can progress between belts and can be rewarded a
	///	sequence of: blue belt, yellow belt, green belt, brown belt, black belt
	/// </summary>
	public class SequenceReward : Reward {

		private static string TAG = "SOOMLA SequenceReward";

		public List<Reward> Rewards;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">see parent.</param>
		/// <param name="name">see parent.</param>
		/// <param name="rewards">The list of rewards in the sequence.</param>
		public SequenceReward(string id, string name, List<Reward> rewards)
			: base(id, name)
		{
			if ((rewards == null || rewards.Count == 0)) {
				SoomlaUtils.LogError(TAG, "This reward doesn't make sense without items");
			}
			Rewards = rewards;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonReward">see parent.</param>
		public SequenceReward(JSONObject jsonReward)
			: base(jsonReward)
		{
			List<JSONObject> rewardsObj = jsonReward[JSONConsts.SOOM_REWARDS].list;
			if ((rewardsObj == null || rewardsObj.Count == 0)) {
				SoomlaUtils.LogWarning(TAG, "Reward has no meaning without children");
				rewardsObj = new List<JSONObject>();
			}

			Rewards = new List<Reward>();
			foreach(JSONObject rewardObj in rewardsObj) {
				Rewards.Add(Reward.fromJSONObject(rewardObj));
			}
		}

		/// <summary>
		/// see parent.
		/// </summary>
		/// <returns>see parent.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			
			JSONObject rewardsObj = new JSONObject(JSONObject.Type.ARRAY);
			foreach(Reward r in Rewards) {
				rewardsObj.Add(r.toJSONObject());
			}
			obj.AddField(JSONConsts.SOOM_REWARDS, rewardsObj);
			
			return obj;
		}


		public Reward GetLastGivenReward() {
			int idx = RewardStorage.GetLastSeqIdxGiven(mID);
			if (idx < 0) {
				return null;
			}
			return Rewards[idx];
		}

		public bool HasMoreToGive() {
			return RewardStorage.GetLastSeqIdxGiven(mID) < Rewards.Count ;
		}

		public bool ForceNextRewardToGive(Reward reward) {
			for (int i = 0; i < Rewards.Count; i++) {
				if (Rewards[i].GetId() == reward.GetId()) {
					RewardStorage.SetLastSeqIdxGiven(mID, i - 1);
					return true;
				}
			}
			return false;
		}




		protected override bool giveInner() {
			int idx = RewardStorage.GetLastSeqIdxGiven(mID);
			if (idx >= Rewards.Count) {
				return false; // all rewards in the sequence were given
			}
			RewardStorage.SetLastSeqIdxGiven(mID, ++idx);
			return true;
		}

		protected override bool takeInner() {
			int idx = RewardStorage.GetLastSeqIdxGiven(mID);
			if (idx <= 0) {
				return false; // all rewards in the sequence were taken
			}
			RewardStorage.SetLastSeqIdxGiven(mID, --idx);
			return true;
		}

	}
}
