using UnityEngine;
using System;

namespace Soomla
{
	public static class AndroidJNIHandler
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		public static void CallVoid(AndroidJavaObject jniObject, string method, string arg0) {
			if(!Application.isEditor){
				jniObject.Call(method, arg0);
				
				checkExceptions();
			}
		}
		
		public static void CallVoid(AndroidJavaObject jniObject, string method, AndroidJavaObject arg0, string arg1) {
			if(!Application.isEditor){
				jniObject.Call(method, arg0, arg1);
				
				checkExceptions();
			}
		}
		
		public static void CallStaticVoid(AndroidJavaClass jniObject, string method, string arg0) {
			if(!Application.isEditor){
				jniObject.CallStatic(method, arg0);

				checkExceptions();
			}
		}
		
		public static void CallStaticVoid(AndroidJavaClass jniObject, string method, string arg0, string arg1) {
			if(!Application.isEditor){
				jniObject.CallStatic(method, arg0, arg1);

				checkExceptions();
			}
		}
		
		public static void CallStaticVoid(AndroidJavaClass jniObject, string method, string arg0, int arg1) {
			if(!Application.isEditor){
				jniObject.CallStatic(method, arg0, arg1);

				checkExceptions();
			}
		}

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

		public static void checkExceptions ()
		{
			IntPtr jException = AndroidJNI.ExceptionOccurred();
			if (jException != IntPtr.Zero) {
				AndroidJNI.ExceptionClear();
				
				AndroidJavaClass jniExceptionClass = new AndroidJavaClass("com.soomla.store.exceptions.InsufficientFundsException");
				if (AndroidJNI.IsInstanceOf(jException, jniExceptionClass.GetRawClass())) {
					Debug.Log("SOOMLA/UNITY Cought InsufficientFundsException!");
					
					throw new InsufficientFundsException();
				}
				
				jniExceptionClass.Dispose();
				jniExceptionClass = new AndroidJavaClass("com.soomla.store.exceptions.VirtualItemNotFoundException");
				if (AndroidJNI.IsInstanceOf(jException, jniExceptionClass.GetRawClass())) {
					Debug.Log("SOOMLA/UNITY Cought VirtualItemNotFoundException!");
					
					throw new VirtualItemNotFoundException();
				}
				
				jniExceptionClass.Dispose();
				jniExceptionClass = new AndroidJavaClass("com.soomla.store.exceptions.NotEnoughGoodsException");
				if (AndroidJNI.IsInstanceOf(jException, jniExceptionClass.GetRawClass())) {
					Debug.Log("SOOMLA/UNITY Cought NotEnoughGoodsException!");
					
					throw new NotEnoughGoodsException();
				}
				
				jniExceptionClass.Dispose();
				
				Debug.Log("SOOMLA/UNITY Got an exception but can't identify it!");
			}
		}
#endif
	}
}

