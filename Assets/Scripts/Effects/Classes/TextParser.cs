using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextParser{

	public static string[] SplitTextAsset(string path){
		TextAsset file = (TextAsset)Resources.Load<TextAsset> (path);
		return SplitTextAsset(file);
	}

	public static string[] SplitTextAsset(TextAsset file){
		string[] fileSplit = file.text.Split('\n');
		for (int i = 0; i < fileSplit.Length; i++){
			fileSplit[i] = RemoveExtraWhitespace(fileSplit[i]);
			System.Console.Write(fileSplit[i] + "\n");
		}
		return fileSplit;
	}

	public static string SplitFormatArticleAsset(TextAsset file, int length = 20){
		string[] arr = SplitTextAsset(file);
		string message = "";
		for (int i = 0; i < arr.Length; i++){
			message = message + Format(arr[i], length) + "\n";
		}
		return message;
	}

	//formats by wordwrap
	public static string Format(string message, int messagelength){
		if (messagelength == -1) return message;
		string formatted = RemoveExtraWhitespace(message);
		int timesinserted = 1;
		
		for (int i = messagelength; i < message.Length; i+= messagelength){
			int insertionposition = timesinserted * messagelength;
			bool inserted = false;
			for (int j = 0; j < messagelength; j++){
				if (formatted[insertionposition - j] == " "[0]){
					char[] array = formatted.ToCharArray();
					array[insertionposition - j] = '\n';
					formatted = new string(array);
					inserted = true;
					break;
				}
			}
			if (!inserted)
				formatted = formatted.Insert(insertionposition, "\n");
			timesinserted++;
		}
		
		return formatted;
	}
	

	//trims trailing and beginning whitespaces and tabs
	public static string RemoveExtraWhitespace(string str){
		int start = 0, end = 0;
		for (int i = 0; i < str.Length; i++){
			if (str[i] == '\t' || str[i] == '\r'  || str[i] == '\n'){
				
				start = i+1;
			}
			else break;
		}
		for (int i = str.Length-1; i > 0; i--){
			if (str[i] == '\t' || str[i] == '\r' || str[i] == '\n'){
				end++;
			}
			else break;
		}
		return str.Substring(start, str.Length - start - end);
	}


	public static bool IsChoice(string str){
		return (str[0] == '<' && str[str.Length-1] == '>');
	}

	public static bool IsEndChoice(string str){
		return (str == "</>");
	}
	
	public static string TrimChoice(string str){
		return str.Substring(1, str.Length - 2);
	}
	
}
