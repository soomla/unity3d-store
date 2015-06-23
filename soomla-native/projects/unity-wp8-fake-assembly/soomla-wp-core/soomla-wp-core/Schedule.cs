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
using System;
using SoomlaWpCore.util;
using SoomlaWpCore.data;


namespace SoomlaWpCore
{	

	public class Schedule {

		private static string TAG = "SOOMLA Schedule";

		public enum Recurrence {
			EVERY_MONTH,
			EVERY_WEEK,
			EVERY_DAY,
			EVERY_HOUR,
			NONE
		}

		public class DateTimeRange {
			public DateTime Start;
			public DateTime End;
			public DateTimeRange(DateTime start, DateTime end) {
				Start = start;
				End = end;
			}
		}

		public Recurrence RequiredRecurrence;
		public List<DateTimeRange> TimeRanges;
		public int ActivationLimit;

		public static Schedule AnyTimeOnce() {
			return new Schedule(1);
		}

		public static Schedule AnyTimeLimited(int activationLimit) {
			return new Schedule(activationLimit);
		}

		public static Schedule AnyTimeUnLimited() {
			return new Schedule(0);
		}

		public Schedule(int activationLimit) :
			this(null, Recurrence.NONE, activationLimit)
		{
		}

		public Schedule(DateTime startTime, DateTime endTime, Recurrence recurrence, int activationLimit) :
			this(new List<DateTimeRange> { new DateTimeRange(startTime, endTime) }, recurrence, activationLimit)
		{
		}

		public Schedule(List<DateTimeRange> timeRanges, Recurrence recurrence, int activationLimit)
		{
			TimeRanges = timeRanges;
			
			RequiredRecurrence = recurrence;
			ActivationLimit = activationLimit;
		}

		public Schedule(JSONObject jsonSched)
		{
			if(jsonSched[JSONConsts.SOOM_SCHE_REC]) {
				RequiredRecurrence = (Recurrence) jsonSched[JSONConsts.SOOM_SCHE_REC].n;
			} else {
				RequiredRecurrence = Recurrence.NONE;
			}

			ActivationLimit = (int)Math.Ceiling(jsonSched[JSONConsts.SOOM_SCHE_APPROVALS].n);

			TimeRanges = new List<DateTimeRange>();
			if (jsonSched[JSONConsts.SOOM_SCHE_RANGES]) {
				List<JSONObject> RangesObjs = jsonSched[JSONConsts.SOOM_SCHE_RANGES].list;
				foreach(JSONObject RangeObj in RangesObjs) {
					TimeSpan tmpTime = TimeSpan.FromMilliseconds((long)RangeObj[JSONConsts.SOOM_SCHE_RANGE_START].n);
					DateTime start = new DateTime(tmpTime.Ticks);
					tmpTime = TimeSpan.FromMilliseconds((long)RangeObj[JSONConsts.SOOM_SCHE_RANGE_END].n);
					DateTime end = new DateTime(tmpTime.Ticks);

					TimeRanges.Add(new DateTimeRange(start, end));
				}
			}
		}

		public JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);

			obj.AddField(JSONConsts.SOOM_CLASSNAME, SoomlaUtils.GetClassName(this));
			obj.AddField(JSONConsts.SOOM_SCHE_REC, (int)RequiredRecurrence);
			obj.AddField(JSONConsts.SOOM_SCHE_APPROVALS, ActivationLimit);

			JSONObject rangesObj = new JSONObject(JSONObject.Type.ARRAY);
			if (TimeRanges != null)
			{
				foreach(DateTimeRange dtr in TimeRanges)
				{
					long startMillis = dtr.Start.Ticks / TimeSpan.TicksPerMillisecond;
					long endMillis = dtr.End.Ticks / TimeSpan.TicksPerMillisecond;
					JSONObject singleRange = new JSONObject(JSONObject.Type.OBJECT);
					singleRange.AddField(JSONConsts.SOOM_CLASSNAME, SoomlaUtils.GetClassName(dtr));
					singleRange.AddField(JSONConsts.SOOM_SCHE_RANGE_START, startMillis);
					singleRange.AddField(JSONConsts.SOOM_SCHE_RANGE_END, endMillis);

					rangesObj.Add(singleRange);
				}
			}

			obj.AddField(JSONConsts.SOOM_SCHE_RANGES, rangesObj);
			
			return obj;
		}

		public bool Approve(int activationTimes) {

			DateTime now = DateTime.Now;

			if (ActivationLimit < 1 && (TimeRanges == null || TimeRanges.Count == 0)) {
				SoomlaUtils.LogDebug(TAG, "There's no activation limit and no TimeRanges. APPROVED!");
				return true;
			}

			if (ActivationLimit>0 && activationTimes >= ActivationLimit) {
				SoomlaUtils.LogDebug(TAG, "Activation limit exceeded.");
				return false;
			}

			if ((TimeRanges == null || TimeRanges.Count == 0)) {
				SoomlaUtils.LogDebug(TAG, "We have an activation limit that was not reached. Also, we don't have any time ranges. APPROVED!");
				return true;
			}


			// NOTE: From this point on ... we know that we didn't reach the activation limit AND we have TimeRanges.
			//		 We'll just make sure the time ranges and the Recurrence copmlies.


			foreach(DateTimeRange dtr in TimeRanges) {
				if (now >= dtr.Start && now <= dtr.End) {
					SoomlaUtils.LogDebug(TAG, "We are just in one of the time spans, it can't get any better then that. APPROVED!");
					return true;
				}
			}

			// we don't need to continue if RequiredRecurrence is NONE
			if (RequiredRecurrence == Recurrence.NONE) {
				return false;
			}

			foreach(DateTimeRange dtr in TimeRanges) {
				if (now.Minute >= dtr.Start.Minute && now.Minute <= dtr.End.Minute) {
					SoomlaUtils.LogDebug(TAG, "Now is in one of the time ranges' minutes span.");

					if (RequiredRecurrence == Recurrence.EVERY_HOUR) {
						SoomlaUtils.LogDebug(TAG, "It's a EVERY_HOUR recurrence. APPROVED!");
						return true;
					}

					if (now.Hour >= dtr.Start.Hour && now.Hour <= dtr.End.Hour) {
						SoomlaUtils.LogDebug(TAG, "Now is in one of the time ranges' hours span.");

						if (RequiredRecurrence == Recurrence.EVERY_DAY) {
							SoomlaUtils.LogDebug(TAG, "It's a EVERY_DAY recurrence. APPROVED!");
							return true;
						}

						if (now.DayOfWeek >= dtr.Start.DayOfWeek && now.DayOfWeek <= dtr.End.DayOfWeek) {
							SoomlaUtils.LogDebug(TAG, "Now is in one of the time ranges' day-of-week span.");
							
							if (RequiredRecurrence == Recurrence.EVERY_WEEK) {
								SoomlaUtils.LogDebug(TAG, "It's a EVERY_WEEK recurrence. APPROVED!");
								return true;
							}

							if (now.Day >= dtr.Start.Day && now.Day <= dtr.End.Day) {
								SoomlaUtils.LogDebug(TAG, "Now is in one of the time ranges' days span.");
								
								if (RequiredRecurrence == Recurrence.EVERY_MONTH) {
									SoomlaUtils.LogDebug(TAG, "It's a EVERY_MONTH recurrence. APPROVED!");
									return true;
								}
							}
						}
					}
				}

			}

			return false;
		}

	}
}
