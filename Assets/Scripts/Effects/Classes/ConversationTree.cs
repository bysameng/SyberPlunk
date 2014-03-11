using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationTree {

	public TextAsset text;
	public string choiceString;
	public SubtitleText messages;
	public List<ConversationTree> choices;
	public int end;

	public ConversationTree(TextAsset text){
		choices = new List<ConversationTree>();
		BuildTree(TextParser.SplitTextAsset(text));
	}

	public ConversationTree(string[] str, int begin){
		choices = new List<ConversationTree>();
		BuildTree(str, begin);
	}


	private void BuildTree(string[] rawCon, int begin = 0){
		messages = new SubtitleText(rawCon[begin]);
		for (int i = begin+1; i < rawCon.Length; i++){
			if (TextParser.IsEndChoice(rawCon[i])){
				end = i;
				return;
			}
			if (TextParser.IsChoice(rawCon[i])){
				while (!TextParser.IsEndChoice(rawCon[i])){
					ConversationTree t = new ConversationTree(rawCon, i+1);
					t.choiceString = TextParser.TrimChoice(rawCon[i]);
					choices.Add(t);
					i = t.end;
				}
			}
		}
	}

	public string[] GetChoices(){
		string[] choiceStringArr = new string[choices.Count];
		for (int i = 0; i < choices.Count; i++){
			choiceStringArr[i] = choices[i].choiceString;
		}
		return choiceStringArr;
	}

	public int GetChoicesCount(){
		return choices.Count;
	}

	public SubtitleText GetMessages(){
		return messages;
	}
	
	// Update is called once per frame
	public ConversationTree ChooseChoice(int i){
		return choices[i];
	}
}
