using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlotTrigger : MonoBehaviour {

	public string playername = "AdvancedPlayer";
	//private GameObject player = LevelScript.player;

	public SubtitleText[] messageArray;

	public float messageduration = 3f;
	public float delay = .2f;

	public TextAsset messageFile;
	public string message;

	public string companionMessage;
	public TextAsset companionMessageAsset;

	public GameObject[] TalkersToUpdate;
	public TextAsset[] TalkerFiles;

	public GameObject[] ObjectsToRemove;
	public GameObject[] ObjectsToEnable;

	public bool destroyOnUse = true;

	public string MessageToLevelScript;



	// Use this for initialization
	void Start () {
		if (messageFile == null) return;
		string[] messages = messageFile.text.Split ("\n"[0]);
		messageArray = new SubtitleText[messages.Length];
		for (int i = 0; i < messages.Length; i++){
			messageArray[i] = new SubtitleText(messages[i]);
		}
	}


	public void LoadFile(string path){
		messageFile = (TextAsset) Resources.Load(path, typeof(TextAsset));
		this.Start();
	}


	void OnTriggerEnter(Collider other){
		if (other.name == "AdvancedPlayer"){
			Destroy(this.collider);
			StartCoroutine("Activate");
		}
	}


	public void ShowMessage(){
		StartCoroutine("Activate");
		Destroy(this.collider);
	}

	public void PlayPlot(){
		if (TalkersToUpdate.Length == TalkerFiles.Length){
			for (int i = 0; i < TalkersToUpdate.Length; i++){
				Talker t = TalkersToUpdate[i].GetComponent<Talker>();
				t.ReProcess(TalkerFiles[i]);
			}
		}
		for (int i = 0; i < ObjectsToRemove.Length; i++){
			ObjectsToRemove[i].SetActive(false);
		}
		for (int i = 0; i < ObjectsToEnable.Length; i++){
			ObjectsToEnable[i].SetActive(true);
		}
		if(companionMessage != ""){
			LevelScript.SendComptextMessage(companionMessage);
		}
		else if (companionMessageAsset != null)
			LevelScript.SendComptextMessage(companionMessageAsset);

	}


	IEnumerator Activate(){
		if(MessageToLevelScript != null && MessageToLevelScript != ""){
			LevelScript.CobbledTogetherEvent(MessageToLevelScript);
		}
		if (messageArray != null){
			int messageIndex = Random.Range(0, messageArray.Length);
			SubtitleText msg = messageArray[messageIndex];
			while (msg.next){
				LevelScript.subtitleQueue.Enqueue(TextParser.Format(msg.GetNext(), 40));
				msg.Increment();
			}
		}
		else LevelScript.subtitleQueue.Enqueue(message);
		PlayPlot ();
		if (destroyOnUse) Destroy(this.gameObject);
		yield return null;
	}


}
