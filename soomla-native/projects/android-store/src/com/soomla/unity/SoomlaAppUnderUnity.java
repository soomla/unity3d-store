package com.soomla.unity;

import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;

import android.app.Activity;
import android.content.Context;
import android.util.Log;

import com.soomla.store.SoomlaApp;

public class SoomlaAppUnderUnity extends SoomlaApp {
	private Class<?> _unityPlayerClass;
	private Field _unityPlayerActivityField;
	private Method _unitySendMessageMethod;
	
    @Override
    public void onCreate()
    {
        super.onCreate();
        try {
	        _unityPlayerClass = Class.forName("com.unity3d.player.UnityPlayer");
	        _unityPlayerActivityField = _unityPlayerClass.getField("currentActivity");
	        _unitySendMessageMethod = _unityPlayerClass.getMethod("UnitySendMessage", new Class[] { String.class, String.class, String.class });
        } catch (ClassNotFoundException e) {
			Log.i("Soomla", "could not find UnityPlayer class: " + e.getMessage());
		} catch (NoSuchFieldException e) {
			Log.i("Soomla", "could not find currentActivity field: " + e.getMessage());
		} catch (Exception e) {
			Log.i("Soomla", "unkown exception occurred locating getActivity(): " + e.getMessage());
		}
        SoomlaApp.mInstance = this;
    }

    @Override
    protected Context _getAppContext() {
		if (this._unityPlayerActivityField != null)
		{
			try {
				return (Activity) this._unityPlayerActivityField.get(this._unityPlayerClass);
			} catch (Exception e) {
				return null;
			}
		}
		return null;
    }

	public void UnitySendMessage(String m, String p) {
		if (p == null) {
			p = "";
		}

		if (this._unitySendMessageMethod != null) {
			try {
				this._unitySendMessageMethod.invoke(null, new Object[] {"Soomla", m, p });
			} catch (IllegalArgumentException e) {
				Log.i("Soomla", "could not find UnitySendMessage method: " + e.getMessage());
			} catch (IllegalAccessException e) {
				Log.i("Soomla",	"could not find UnitySendMessage method: " + e.getMessage());
			} catch (InvocationTargetException e) {
				Log.i("Soomla", "could not find UnitySendMessage method: " + e.getMessage());
			}
		} else {
			Log.i("Soomla", "UnitySendMessage: Soomla, " + m + ", " + p);
		}
	}
}