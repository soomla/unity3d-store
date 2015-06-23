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

using System;

namespace SoomlaWpCore.data
{
	public class RewardStorage
	{

		protected const string TAG = "SOOMLA RewardStorage"; // used for Log error messages

		public static void SetRewardStatus(string rewardId, bool give) {
            SetRewardStatus(rewardId, give, true);
		}

        public static void SetRewardStatus(string rewardId, bool give, bool notify)
        {
            SetTimesGiven(rewardId, give, notify);
		}

        public static bool IsRewardGiven(string rewardId)
        {
            return GetTimesGiven(rewardId) > 0;
		}

        public static int GetTimesGiven(string rewardId)
        {
            String key = KeyRewardTimesGiven(rewardId);
            String val = KeyValueStorage.GetValue(key);
            if (val==null)
            {
                return 0;
            }
            return int.Parse(val);
		}

        public static DateTime GetLastGivenTime(string rewardId)
        {
            long timeMillis = GetLastGivenTimeMillis(rewardId);
            if (timeMillis == 0)
            {
                return new DateTime(-1);
            }
            DateTime toReturn = new DateTime();
            toReturn.AddMilliseconds(timeMillis);
            return toReturn;
		}

        public static void SetTimesGiven(string rewardId, bool up, bool notify)
        {
            int total = GetTimesGiven(rewardId) + (up ? 1 : -1);
            ResetTimesGiven(rewardId, total);

            if (up)
            {
                SetLastGivenTimeMillis(rewardId, GetCurrentTimeStampMillis());
            }

            //TODO Notify Events
            if (notify)
            {
                if (up)
                {
                    //BusProvider.getInstance().post(new RewardGivenEvent(rewardId));
                }
                else
                {
                    //BusProvider.getInstance().post(new RewardTakenEvent(rewardId));
                }
            }
        }

        public static long GetCurrentTimeStampMillis()
        {
            return (DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / 10000;
        }

		/// <summary>
		/// Retrieves the index of the last reward given in a sequence of rewards.
		/// </summary>
		public static int GetLastSeqIdxGiven(string rewardId) {
            String key = KeyRewardIdxSeqGiven(rewardId);

            String val = KeyValueStorage.GetValue(key);

            if (val == null)
            {
                return -1;
            }
            return int.Parse(val); ;
		}

		public static void SetLastSeqIdxGiven(string rewardId, int idx) {
            String key = KeyRewardIdxSeqGiven(rewardId);
            KeyValueStorage.SetValue(key, idx.ToString());
		}


        

        public static long GetLastGivenTimeMillis(String rewardId)
        {
            String key = KeyRewardLastGiven(rewardId);
            String val = KeyValueStorage.GetValue(key);
            if (val == null)
            {
                return 0;
            }
            return long.Parse(val);

        }

        public static void SetLastGivenTimeMillis(String rewardId, long lastGiven)
        {
            String key = KeyRewardLastGiven(rewardId);
            KeyValueStorage.SetValue(key, lastGiven.ToString());
        }

        public static void ResetTimesGiven(String rewardId, int timesGiven)
        {
            String key = KeyRewardTimesGiven(rewardId);
            KeyValueStorage.SetValue(key, timesGiven.ToString());
        }
		

		/** keys **/
		private static string KeyRewards(string rewardId, string postfix) {
			return "rewards." + rewardId + "." + postfix;
		}
		
		private static string KeyRewardIdxSeqGiven(string rewardId) {
			return KeyRewards(rewardId, "seq.idx");
		}

		private static string KeyRewardTimesGiven(string rewardId) {
			return KeyRewards(rewardId, "timesGiven");
		}

		private static string KeyRewardLastGiven(string rewardId) {
			return KeyRewards(rewardId, "lastGiven");
		}
	}
}

