#define READABLE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * JSONObject class
 * for use with Unity
 * Copyright Matt Schoen 2010
 */

public class JSONObject : Nullable {
	const int MAX_DEPTH = 1000;
	public enum Type { NULL, STRING, NUMBER, OBJECT, ARRAY, BOOL }
	public JSONObject parent;
	public Type type = Type.NULL;
	public ArrayList list = new ArrayList();
	public ArrayList keys = new ArrayList();
	public string str;
	public double n;
	public bool b;

	public static JSONObject nullJO { get { return new JSONObject(JSONObject.Type.NULL); } }
	public static JSONObject obj { get { return new JSONObject(JSONObject.Type.OBJECT); } }
	public static JSONObject arr { get { return new JSONObject(JSONObject.Type.ARRAY); } }

	public JSONObject(JSONObject.Type t) {
		type = t;
		switch(t) {
		case Type.ARRAY:
			list = new ArrayList();
			break;
		case Type.OBJECT:
			list = new ArrayList();
			keys = new ArrayList();
			break;
		}
	}
	public JSONObject(bool b) {
		type = Type.BOOL;
		this.b = b;
	}
	public JSONObject(float f) {
		type = Type.NUMBER;
		this.n = f;
	}
	public JSONObject(Dictionary<string, string> dic) {
		type = Type.OBJECT;
		foreach(KeyValuePair<string, string> kvp in dic){
			keys.Add(kvp.Key);
			list.Add(kvp.Value);
		}
	}
	public JSONObject() { }
	public JSONObject(string str) {	//create a new JSONObject from a string (this will also create any children, and parse the whole string)
		//Debug.Log(str);
		if(str != null) {
			str = str.Trim();
			
			// TODO: take a look at this replacements again ...
//			str = rgx.Replace(str, @"");
//			str = str.Replace(@"[ ", @"[");
//			str = str.Replace(@"{ ", @"{");
//			str = str.Replace("\" ,", "\",");
//			str = str.Replace(", \"", ",\"");
#if(READABLE)
			str = str.Replace("\\n", "");
			str = str.Replace("\\t", "");
			str = str.Replace("\\r", "");
			str = str.Replace("\t", "");
			str = str.Replace("\n", "");
			str = str.Replace("\\", "");
#endif
			if(str.Length > 0) {
				if(string.Compare(str, "true", true) == 0) {
					type = Type.BOOL;
					b = true;
				} else if(string.Compare(str, "false", true) == 0) {
					type = Type.BOOL;
					b = false;
				} else if(str == "null") {
					type = Type.NULL;
				} else if(str[0] == '"') {
					type = Type.STRING;
					if(str.Length > 2) {
						this.str = str.Substring(1, str.Length - 2);
					} else {
						this.str = "";
					}
					
				} else {
					try {
						n = System.Convert.ToDouble(str);
						type = Type.NUMBER;
					} catch(System.FormatException) {
						int token_tmp = 0;
						/*
						 * Checking for the following formatting (www.json.org)
						 * object - {"field1":value,"field2":value}
						 * array - [value,value,value]
						 * value - string	- "string"
						 *		 - number	- 0.0
						 *		 - bool		- true -or- false
						 *		 - null		- null
						 */
						switch(str[0]) {
						case '{':
							type = Type.OBJECT;
							keys = new ArrayList();
							list = new ArrayList();
							break;
						case '[':
							type = JSONObject.Type.ARRAY;
							list = new ArrayList();
							break;
						default:
							type = Type.NULL;
							Debug.LogWarning("improper JSON formatting:" + str);
							return;
						}
						int depth = 0;
						bool openquote = false;
						bool inProp = false;
						for(int i = 1; i < str.Length; i++) {
							if(str[i] == ' ') {
								continue;
							}
							if(str[i] == '\\') {
								i++;
								continue;
							}
							if(str[i] == '"')
								openquote = !openquote;
							if(str[i] == '[' || str[i] == '{')
								depth++;
							if(depth == 0 && !openquote) {
								if(str[i] == ':' && !inProp) {
									inProp = true;
									try {
										int begin = token_tmp + 1;
										
										while(str[begin] == ' ') {
											begin++;
										}
										
										int tmp_i = i-1;
										while(str[tmp_i] == ' ') {
											tmp_i--;
										}
										int length = tmp_i - begin + 1;
										keys.Add(str.Substring(begin + 1, length - 2)); // -2 b/c of the ""
									} catch { Debug.Log(i + " - " + str.Length + " - " + str); }
									token_tmp = i;
								}
								if(str[i] == ',') {
									inProp = false;
									int begin = (str[token_tmp]==':' ||
												 str[token_tmp]==',' ||
												 str[token_tmp]=='{' ||
												 str[token_tmp]=='[') ? token_tmp+1 : token_tmp;
									
									while(str[begin] == ' ') {
										begin++;
									}
									
									int tmp_i = i-1;
									while(str[tmp_i] == ' ') {
										tmp_i--;
									}
									int length = tmp_i - begin + 1;
									list.Add(new JSONObject(str.Substring(begin, length)));
									token_tmp = i;
								}
								if(str[i] == ']' || str[i] == '}') {
									string s_tmp = str.Substring(token_tmp + 1, i - token_tmp - 1);
									if (s_tmp.Length > 0) {
										list.Add(new JSONObject(s_tmp));
									}
								}
							}
							if(str[i] == ']' || str[i] == '}')
								depth--;
						}
					}
				}
			}
		} else {
			type = Type.NULL;	//If the string is missing, this is a null
		}
	}
	public void AddField(bool val) { Add(new JSONObject(val)); }
	public void AddField(float val) { Add(new JSONObject(val)); }
	public void AddField(int val) { Add(new JSONObject(val)); }
	public void AddField(string val) { Add(new JSONObject(val)); }
	public void Add(JSONObject obj) {
		if(obj) {		//Don't do anything if the object is null
			if(type != JSONObject.Type.ARRAY) {
				type = JSONObject.Type.ARRAY;		//Congratulations, son, you're an ARRAY now
				Debug.LogWarning("tried to add an object to a non-array JSONObject.  We'll do it for you, but you might be doing something wrong.");
			}
			list.Add(obj);
		}
	}
	public void AddField(string name, bool val) { AddField(name, new JSONObject(val)); }
	public void AddField(string name, float val) { AddField(name, new JSONObject(val)); }
	public void AddField(string name, int val) { AddField(name, new JSONObject(val)); }
	public void AddField(string name, string val) {
		AddField(name, new JSONObject { type = JSONObject.Type.STRING, str = val });
	}
	public void AddField(string name, JSONObject obj) {
		if(obj){		//Don't do anything if the object is null
			if(type != JSONObject.Type.OBJECT){
				type = JSONObject.Type.OBJECT;		//Congratulations, son, you're an OBJECT now
				Debug.LogWarning("tried to add a field to a non-object JSONObject.  We'll do it for you, but you might be doing something wrong.");
			}
			keys.Add(name);
			list.Add(obj);
		}
	}
	public void SetField(string name, JSONObject obj) {
		if(HasField(name)) {
			list.Remove(this[name]);
			keys.Remove(name);
		}
		AddField(name, obj);
	}
	public JSONObject GetField(string name) {
		if(type == JSONObject.Type.OBJECT)
			for(int i = 0; i < keys.Count; i++)
				if((string)keys[i] == name)
					return (JSONObject)list[i];
		return null;
	}
	public bool HasField(string name) {
		if(type == JSONObject.Type.OBJECT)
			for(int i = 0; i < keys.Count; i++)
				if((string)keys[i] == name)
					return true;
		return false;
	}
	public void Clear() {
		type = JSONObject.Type.NULL;
		list.Clear();
		keys.Clear();
		str = "";
		n = 0;
		b = false;
	}
	public JSONObject Copy() {
		return new JSONObject(print());
	}
	/*
	 * The Merge function is experimental. Use at your own risk.
	 */
	public void Merge(JSONObject obj) {
		MergeRecur(this, obj);
	}
	static void MergeRecur(JSONObject left, JSONObject right) {
		if(right.type == JSONObject.Type.OBJECT) {
			for(int i = 0; i < right.list.Count; i++) {
				if(right.keys[i] != null) {
					string key = (string)right.keys[i];
					JSONObject val = (JSONObject)right.list[i];
					if(val.type == JSONObject.Type.ARRAY || val.type == JSONObject.Type.OBJECT) {
						if(left.HasField(key))
							MergeRecur(left[key], val);
						else
							left.AddField(key, val);
					} else {
						if(left.HasField(key))
							left.SetField(key, val);
						else
							left.AddField(key, val);
					}
				}
			}
		}// else left.list.Add(right.list);
	}
	public string print() {
		return print(0);
	}
	public string print(int depth) {	//Convert the JSONObject into a stiring
		if(depth++ > MAX_DEPTH) {
			Debug.Log("reached max depth!");
			return "";
		}
		string str = "";
		switch(type) {
		case Type.STRING:
			str = "\"" + this.str + "\"";
			break;
		case Type.NUMBER:
			str += n.ToString("0.00");
			break;
		case JSONObject.Type.OBJECT:
			if(list.Count > 0) {
				str = "{";
#if(READABLE)	//for a bit more readability, comment the define above to save space
				str += "\n";
				depth++;
#endif
				for(int i = 0; i < list.Count; i++) {
					string key = (string)keys[i];
					JSONObject obj = (JSONObject)list[i];
					if(obj) {
#if(READABLE)
						for(int j = 0; j < depth; j++)
							str += "\t"; //for a bit more readability
#endif
						str += "\"" + key + "\":";
						str += obj.print(depth) + ",";
#if(READABLE)
						str += "\n";
#endif
					}
				}
#if(READABLE)
				str = str.Substring(0, str.Length - 1);
#endif
				str = str.Substring(0, str.Length - 1);
				str += "}";
			} else str += "{}";
			break;
		case JSONObject.Type.ARRAY:
			str = "[";
			if(list.Count > 0) {
#if(READABLE)
				str += "\n"; //for a bit more readability
				depth++;
#endif
				foreach(JSONObject obj in list) {
					if(obj) {
#if(READABLE)
						for(int j = 0; j < depth; j++)
							str += "\t"; //for a bit more readability
#endif
						str += obj.print(depth) + ",";
#if(READABLE)
						str += "\n"; //for a bit more readability
#endif
					}
				}
#if(READABLE)
				str = str.Substring(0, str.Length - 1);
#endif
				str = str.Substring(0, str.Length - 1);
			}
			str += "]";
			break;
		case Type.BOOL:
			if(b)
				str += "true";
			else
				str += "false";
			break;
		case Type.NULL:
			str = "null";
			break;
		}
		return str;
	}
	public JSONObject this[int index] {
		get { return (JSONObject)list[index]; }
	}
	public JSONObject this[string index] {
		get { return GetField(index); }
	}
	public override string ToString() {
		return print();
	}
	public Dictionary<string, string> ToDictionary() {
		if(type == Type.OBJECT) {
			Dictionary<string, string> result = new Dictionary<string, string>();
			for(int i = 0; i < list.Count; i++) {
				JSONObject val = (JSONObject)list[i];
				switch(val.type){
				case Type.STRING:	result.Add((string)keys[i], val.str);		break;
				case Type.NUMBER:	result.Add((string)keys[i], val.n + "");	break;
				case Type.BOOL:		result.Add((string)keys[i], val.b + "");	break;
				default: Debug.LogWarning("Omitting object: " + (string)keys[i] + " in dictionary conversion"); break;
				}
			}
			return result;
		} else Debug.LogWarning("Tried to turn non-Object JSONObject into a dictionary");
		return null;
	}
}
