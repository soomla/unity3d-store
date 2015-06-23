/// Copyright (C) 2012-2015 Soomla Inc.
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
/// 
/// Based on https://github.com/snotyak/EventBus

using System;
using System.Collections.Generic;
using System.Reflection;

namespace SoomlaWpCore.util
{
	public class EventBus
	{
		private List<object> _subscribers = new List<object>();

		public EventBus ()
		{
		}

		public void Register(object subscriber){
			if (!_subscribers.Contains (subscriber)) {
                //SoomlaUtils.LogDebug("EventBus", "Register : "+subscriber.ToString());
				_subscribers.Add (subscriber);
			}
		}

		public void Unregister(object subscriber){
			if (_subscribers.Contains (subscriber)) {
				_subscribers.Remove (subscriber);
			}
		}

		public void Post(object e){
			/*
            foreach(object instance in _subscribers){
				foreach(MethodInfo method in GetSubscribedMethods(instance.GetType (), e)){
					try{
						method.Invoke (instance, new object[]{e});
					}catch(TargetInvocationException){}
				}
			}
            */
		}

		private List<MethodInfo> GetSubscribedMethods (Type type, object obj)
		{
			List<MethodInfo> subscribedMethods = new List<MethodInfo> ();
            /*
			var methods = type.GetRuntimeMethods ();
			foreach(MethodInfo info in methods){
				foreach(Attribute attr in info.GetCustomAttributes ()){
					if (attr.GetType () == typeof(Subscribe)) {
						var paramInfo = info.GetParameters ();
						if(paramInfo.Length == 1 && paramInfo[0].ParameterType == obj.GetType ()){
							subscribedMethods.Add (info);
						}
					}
				}
			}
            */
			return subscribedMethods;
		}
	}
}

