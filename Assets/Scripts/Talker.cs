using UnityEngine;
using System.Collections;

public class Talker : MonoBehaviour {

	private GameObject textObject;
	private MLGText mlgtext;
	private CompanionText comptext;
	private SpeechBubble sbubble;
	public bool usable = true;
	public TextAsset dialoguesFile;
	public bool isTree = true;

	public string prompt = "TALK";

	private ConversationTree cTree;
	private ChoiceBubble[] choices;
	private GameObject choiceParentObject;
	private bool displayingChoices = false;

	public GameObject[] ObjectsToRemove;
	public GameObject[] ObjectsToEnable;

	public bool doneTalking = false;
	


	// Use this for initialization
	void Start () {
		choiceParentObject = new GameObject();
		choiceParentObject.AddComponent<LookAt>();
		choiceParentObject.transform.position = this.transform.position;
		textObject = new GameObject("text");
		mlgtext = textObject.AddComponent<MLGText>();
		comptext = this.gameObject.GetComponentInChildren<CompanionText>();
		sbubble = this.gameObject.GetComponentInChildren<SpeechBubble>();
		mlgtext.drawText(prompt, 50, new Vector2(0,0));
		mlgtext.displayingText = false;
		if (dialoguesFile != null){
			if (isTree) {
				cTree = new ConversationTree(dialoguesFile);
				ProcessTree();
			}
			else ProcessDialogue();
		}
	}


	public void ReProcess(TextAsset newAsset){
		Debug.Log ("Reprocessing");
		ChooseChoice(-1);
		comptext.ClearQueue();
		comptext.Clear();
		dialoguesFile = newAsset;
		if (isTree) {
			cTree = new ConversationTree(dialoguesFile);
			ProcessTree ();
			displayingChoices = false;
		}
		else ProcessDialogue();
	}


	void Load(string path){
		dialoguesFile = (TextAsset) Resources.Load(path, typeof(TextAsset));
	}


	void ProcessTree(){
		for (int i = 0; cTree.GetMessages().next; i++){
			comptext.Display(cTree.GetMessages().GetNext());
			cTree.GetMessages().Increment();
		}
		string[] choiceStringArr = GetChoices ();
		choices = new ChoiceBubble[choiceStringArr.Length];
		for (int i = 0; i < choices.Length; i++){
			GameObject c = (GameObject)Instantiate(Resources.Load("Prefabs/ChoiceObject"), 
			                                       choiceParentObject.transform.position + choiceParentObject.transform.forward, 
			                                       choiceParentObject.transform.rotation);
			c.transform.position = c.transform.position + c.transform.right * (i - (choices.Length-1)/2f);
			c.gameObject.transform.parent = choiceParentObject.transform;
			choices[i] = c.GetComponent<ChoiceBubble>();
			choices[i].talkerParent = this;
			choices[i].choiceNumber = i;
			choices[i].SetText(choiceStringArr[i]);
		}
	}


	public void ChooseChoice(int index){

		for (int i = 0; i < choices.Length; i++){
			if (index != i)
				choices[i].DestroyFade(0f);
		}
		if (index != -1){
			choices[index].DestroyFade(.5f);
		}
		else return;
		cTree = cTree.ChooseChoice(index);
		comptext.ClearQueue();
		ProcessTree ();
		comptext.Next();
		displayingChoices = false;
		string lastmsg = cTree.GetMessages().GetLastMessage();
		Debug.Log(choices.Length);
		if(choices.Length <= 0){
			LevelScript.TalkerEnd(lastmsg, comptext, this);
			for (int i = 0; i < ObjectsToRemove.Length; i++){
				ObjectsToRemove[i].SetActive(false);
			}
			for (int i = 0; i < ObjectsToEnable.Length; i++){
				ObjectsToEnable[i].SetActive(true);
			}
			doneTalking = true;
		}
	}


	public string[] GetChoices(){
		return cTree.GetChoices();
	}


	void ProcessDialogue(){
		string[] allDialogues = dialoguesFile.text.Split ('@');
		int choice = Random.Range(0, allDialogues.Length);
		string[] selectedDialogue = allDialogues[choice].Split("\n"[0]);
		for (int i = 0; i < selectedDialogue.Length; i++){
			comptext.Display(selectedDialogue[i]);
		}
	}

	
	// Update is called once per frame
	void Update () {
		if (isTree){
			if (choices.Length != 0 && !choices[0].visible) displayingChoices = false;
			if (comptext.done && !comptext.writing && !displayingChoices){
				DisplayChoicesPrompt();
			}
		}
		mlgtext.displayingText = false;
	}

	void LateUpdate(){
		comptext.usable = mlgtext.displayingText;
	}


	void Use(){
		if (!usable || comptext.writing) return;
		mlgtext.displayingText = false;
	}

	void DisplayPrompt(){
		if(comptext.IsNext())
			mlgtext.displayingText = true;
		comptext.usable = true;
	}

	void DisplayChoicesPrompt(){
		for (int i = 0; i < choices.Length; i++){
			choices[i].MakeVisible();
			choices[i].usable = true;
		}
		displayingChoices = true;
	}

	public void Die(){
		mlgtext.displayingText = false;
		this.collider.enabled = false;
	}

}
