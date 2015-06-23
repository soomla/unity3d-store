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
using SoomlaWpCore.util;
using SoomlaWpCore.data;

namespace SoomlaWpCore.rewards
{	

	/// <summary>
	/// A specific type of <code>Reward</code> that represents a badge
	/// with an icon. For example: when the user achieves a top score,
	/// the user can earn a "Highest Score" badge reward.
	/// </summary>
	public class BadgeReward : Reward {
		public string IconUrl;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">see parent</param>
		/// <param name="name">see parent</param>
		public BadgeReward(string id, string name)
			: base(id, name)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">see parent</param>
		/// <param name="name">see parent</param>
		/// <param name="iconUrl">A url to the icon of this Badge on the device.</param>
		public BadgeReward(string id, string name, string iconUrl)
			: base(id, name)
		{
			IconUrl = iconUrl;
		}

		/// <summary>
		/// see parent.
		/// </summary>
		public BadgeReward(JSONObject jsonReward)
			: base(jsonReward)
		{
			IconUrl = jsonReward[JSONConsts.SOOM_REWARD_ICONURL].str;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_REWARD_ICONURL, IconUrl);
			
			return obj;
		}

		protected override bool giveInner() {
			
			// nothing to do here... the parent Reward gives in storage
			return true;
		}

		protected override bool takeInner() {
			// nothing to do here... the parent Reward takes in storage
			return true;
		}

	}
}
