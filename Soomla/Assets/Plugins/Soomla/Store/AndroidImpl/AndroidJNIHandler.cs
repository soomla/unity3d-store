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

using UnityEngine;
using System;

namespace Soomla.Store {

	/// <summary>
	/// This class uses JNI and provides functions that call SOOMLA's android-store.
	/// </summary>
	public static class AndroidJNIHandler {

#if UNITY_ANDROID && !UNITY_EDITOR

		/// <summary>
		/// Calls android-store function that returns void and receives a string argument. 
		/// </summary>
		/// <param name="jniObject">A type-less instance of any Java class.</param>
		/// <param name="method">The method to call in android-store.</param>
		/// <param name="arg0">The method's string argument.</param>
		public static void CallVoid(AndroidJavaObject jniObject, string method, string arg0) {
			if(!Application.isEditor){
				jniObject.Call(method, arg0);
				
				checkExceptions();
			}
		}

		/// <summary>
		/// Calls android-store function that returns void and receives an object argument, and a string argument. 
		/// </summary>
		/// <param name="jniObject">A type-less instance of any Java class.</param>
		/// <param name="method">The method to call in android-store.</param>
		/// <param name="arg0">The method's AndroidJavaObject argument.</param>
		/// <param name="arg1">The method's string argument.</param>
		public static void CallVoid(AndroidJavaObject jniObject, string method, AndroidJavaObject arg0, string arg1) {
			if(!Application.isEditor){
				jniObject.Call(method, arg0, arg1);
				
				checkExceptions();
			}
		}

		/// <summary>
		/// Calls android-store static function that returns void and receives a string argument. 
		/// </summary>
		/// <param name="jniObject">A type-less instance of any Java class.</param>
		/// <param name="method">The method to call in android-store.</param>
		/// <param name="arg0">The method's string argument.</param>
		public static void CallStaticVoid(AndroidJavaClass jniObject, string method, string arg0) {
			if(!Application.isEditor){
				jniObject.CallStatic(method, arg0);

				checkExceptions();
			}
		}

		/// <summary>
		/// Calls android-store static function that returns void and receives 2 string arguments. 
		/// </summary>
		/// <param name="jniObject">A type-less instance of any Java class.</param>
		/// <param name="method">The method to call in android-store.</param>
		/// <param name="arg0">The method's string argument.</param>
		/// <param name="arg1">The method's string argument.</param>
		public static void CallStaticVoid(AndroidJavaClass jniObject, string method, string arg0, string arg1) {
			if(!Application.isEditor){
				jniObject.CallStatic(method, arg0, arg1);

				checkExceptions();
			}
		}

		/// <summary>
		/// Calls android-store static function that returns void and receives a string argument, and an int argument. 
		/// </summary>
		/// <param name="jniObject">A type-less instance of any Java class.</param>
		/// <param name="method">The method to call in android-store.</param>
		/// <param name="arg0">The method's string argument.</param>
		/// <param name="arg1">The method's int argument.</param>
		public static void CallStaticVoid(AndroidJavaClass jniObject, string method, string arg0, int arg1) {
			if(!Application.isEditor){
				jniObject.CallStatic(method, arg0, arg1);

				checkExceptions();
			}
		}

		/// <summary>
		/// Calls android-store static function that has a return value and receives a string argument. 
		/// </summary>
		/// <param name="jniObject">A type-less instance of any Java class.</param>
		/// <param name="method">The method to call in android-store.</param>
		/// <param name="arg0">The method's string argument.</param>
		/// <returns>Return value of the function called.</returns>
		public static T CallStatic<T>(AndroidJavaClass jniObject, string method, string arg0) {
			if (!Application.isEditor) {
				T retVal = jniObject.CallStatic<T>(method, arg0);

				checkExceptions();
				
				if (retVal is AndroidJavaObject) {
					if ((retVal as AndroidJavaObject).GetRawObject() == IntPtr.Zero) {
						throw new VirtualItemNotFoundException();
					}
				}

				return retVal;
			}
			
			return default(T);
		}

		/// <summary>
		/// Calls android-store static function that has a return value and receives a string argument, and an int 
		/// argument. 
		/// </summary>
		/// <param name="jniObject">A type-less instance of any Java class.</param>
		/// <param name="method">The method to call in android-store.</param>
		/// <param name="arg0">The method's string argument.</param>
		/// <param name="arg1">The method's int argument.</param>
		/// <returns>Return value of the function called.</returns>
		public static T CallStatic<T>(AndroidJavaClass jniObject, string method, string arg0, int arg1) {
			if (!Application.isEditor) {
				T retVal = jniObject.CallStatic<T>(method, arg0, arg1);

				checkExceptions();

				if (retVal is AndroidJavaObject) {
					if ((retVal as AndroidJavaObject).GetRawObject() == IntPtr.Zero) {
						throw new VirtualItemNotFoundException();
					}
				}

				return retVal;
			}

			return default(T);
		}

		/// <summary>
		/// Calls android-store static function that has a return value and receives an int argument. 
		/// </summary>
		/// <param name="jniObject">A type-less instance of any Java class.</param>
		/// <param name="method">The method to call in android-store.</param>
		/// <param name="arg0">The method's int argument.</param>
		/// <returns>Return value of the function called.</returns>
		public static T CallStatic<T>(AndroidJavaClass jniObject, string method, int arg0) {
			if (!Application.isEditor) {
				T retVal = jniObject.CallStatic<T>(method, arg0);
				
				checkExceptions();
				
				if (retVal is AndroidJavaObject) {
					if ((retVal as AndroidJavaObject).GetRawObject() == IntPtr.Zero) {
						throw new VirtualItemNotFoundException();
					}
				}
				
				return retVal;
			}
			
			return default(T);
		}

		/// <summary>
		/// Throws one of the exceptions (<c>InsufficientFundsException</c>, <c>VirtualItemNotFoundException</c>, or 
		/// <c>NotEnoughGoodsException</c>) if needed. 
		/// </summary>
		public static void checkExceptions ()
		{
			IntPtr jException = AndroidJNI.ExceptionOccurred();
			if (jException != IntPtr.Zero) {
				AndroidJNI.ExceptionClear();
				
				AndroidJavaClass jniExceptionClass = new AndroidJavaClass("com.soomla.store.exceptions.InsufficientFundsException");
				if (AndroidJNI.IsInstanceOf(jException, jniExceptionClass.GetRawClass())) {
					Debug.Log("SOOMLA/UNITY Caught InsufficientFundsException!");
					
					throw new InsufficientFundsException();
				}
				
				jniExceptionClass.Dispose();
				jniExceptionClass = new AndroidJavaClass("com.soomla.store.exceptions.VirtualItemNotFoundException");
				if (AndroidJNI.IsInstanceOf(jException, jniExceptionClass.GetRawClass())) {
					Debug.Log("SOOMLA/UNITY Caught VirtualItemNotFoundException!");
					
					throw new VirtualItemNotFoundException();
				}
				
				jniExceptionClass.Dispose();
				jniExceptionClass = new AndroidJavaClass("com.soomla.store.exceptions.NotEnoughGoodsException");
				if (AndroidJNI.IsInstanceOf(jException, jniExceptionClass.GetRawClass())) {
					Debug.Log("SOOMLA/UNITY Caught NotEnoughGoodsException!");
					
					throw new NotEnoughGoodsException();
				}
				
				jniExceptionClass.Dispose();
				
				Debug.Log("SOOMLA/UNITY Got an exception but can't identify it!");
			}
		}
#endif
	}
}

